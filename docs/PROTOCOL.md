# PocketController Wire Protocol

This file describes the packets this Windows server parses and the replies it sends. The implementation is `server/Protocol/`. Phones connect from [PocketController-App](https://github.com/axcel-blade/PocketController-App).

## Transport

| Item         | Value |
|--------------|-------|
| Transport    | UDP, unicast |
| Default port | `5555` (`Constants.DefaultPort`, configurable in the server) |
| Packet size  | 48 bytes (`MessageSerializer.PacketSize`), every controller packet, both directions |
| Byte order   | Little-endian |
| Keep-alive   | A `Ping` refreshes the session. The server drops it after 5 s with no packet (`Constants.ClientTimeoutMs`). |

## Packet layout

`MessageSerializer` writes and reads the fields in this order.

| Offset | Size | Type | Field | Notes |
|-------:|-----:|------|-------|-------|
| 0 | 1 | `uint8` | `type` | See message types below |
| 1 | 2 | `uint16` | `buttons` | Bitmask, see below |
| 3 | 4 | `float32` | `leftStickX` | -1.0 (left) … 1.0 (right) |
| 7 | 4 | `float32` | `leftStickY` | -1.0 (down) … 1.0 (up) |
| 11 | 4 | `float32` | `rightStickX` | -1.0 … 1.0 |
| 15 | 4 | `float32` | `rightStickY` | -1.0 … 1.0 (up positive) |
| 19 | 4 | `float32` | `leftTrigger` | 0.0 … 1.0 |
| 23 | 4 | `float32` | `rightTrigger` | 0.0 … 1.0 |
| 27 | 1 | `uint8` | `dpad` | Bitmask, see below |
| 28 | 12 | `float32`×3 | `gyroX/Y/Z` | Read by the serializer. `GamepadMapper` does not apply them. |
| 40 | 8 | `int64` | `timestampMs` | Echoed in replies |

A datagram shorter than this layout fails deserialization.

### Buttons bitmask

`GamepadMapper` maps these bits onto the virtual Xbox 360 controller.

| Bit | Button | Bit | Button |
|----:|--------|----:|--------|
| 0 | A | 6 | Start |
| 1 | B | 7 | Back |
| 2 | X | 8 | Left stick click |
| 3 | Y | 9 | Right stick click |
| 4 | LB | 10 | Guide |
| 5 | RB | | |

### D-pad bitmask

`Up = 1`, `Down = 2`, `Left = 4`, `Right = 8`. Each bit is applied on its own, so two bits is a diagonal.

## Message types

The server accepts:

| Value | Name | What the server does |
|------:|------|----------------------|
| 0 | `Connect` | Opens a session and a virtual controller when a slot is free. |
| 1 | `Disconnect` | Removes that session and its controller. |
| 2 | `Input` | Applies buttons, sticks, triggers and D-pad to that session's controller. |
| 3 | `Ping` | Refreshes the idle timer. |

The server replies (only `type` and the echoed `timestampMs` are set):

| Value | Name | When the server sends it |
|------:|------|--------------------------|
| 4 | `ConnectAck` | `Connect` created or refreshed a session. |
| 5 | `Pong` | Reply to `Ping` from an endpoint that has a session. |
| 6 | `ServerFull` | `Connect` was refused. All 4 slots are in use, or the virtual controller could not be created. |
| 7 | `NotConnected` | `Ping` arrived from an endpoint with no session. |

`Input` and `Disconnect` from an endpoint with no session are ignored. `Disconnect` has no reply.

## LAN discovery

Discovery uses UTF-8 text on its own ports, so it never mixes with the 48-byte controller packets. `DiscoveryMessage` builds and parses it.

| Port | What the server does |
|-----:|----------------------|
| 5556 | Reads a probe and replies to the sender. |
| 5557 | Broadcasts an announcement every 2 s while running. |

| Packet | Format |
|--------|--------|
| Probe | `PCTRL?1\|<device name>` |
| Announcement | `PCTRL!1\|{"name":"GAMING-PC","port":5555,"clients":1,"max":4,"id":"3f9c0a1b2c4d","ips":["192.168.0.5","172.20.0.1"]}` |

* `name` is the machine name, `port` is the controller port, and `clients` / `max` are the sessions in use and the limit of 4.
* `id` is chosen when the server starts. `ips` lists the machine's IPv4 addresses.
* A datagram that is not a `PCTRL?1|` probe, or is longer than 512 bytes, is ignored. A device name is truncated to 64 characters.
* If UDP `5556` is already in use, the controller port still runs and discovery stays off.

### Auto-connect

The server does not choose a peer and does not retry a connection. It waits for `Connect` on the controller port, answers probes, and broadcasts announcements while it is running.

## Connection state

* **No session** — `Connect` is answered with `ConnectAck` or `ServerFull`. `Ping` is answered with `NotConnected`.
* **Session** — `Input` updates the virtual controller, `Ping` is answered with `Pong`, and `Disconnect` removes the session.
* **Timed out** — `HeartbeatMonitor` removes a session after 5 s with no packet. The next `Ping` from that endpoint gets `NotConnected`.
