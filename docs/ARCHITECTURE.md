# PocketController Architecture

How the Windows server in this repository is put together. Packet layout: [PROTOCOL.md](PROTOCOL.md).

The server reads UDP packets, keeps one session per sender, and drives a virtual Xbox 360 controller for each session through ViGEmBus.

## Modules

| Path | Responsibility |
|------|----------------|
| `server/Protocol/GamepadMessage.cs` | Fields in one 48-byte packet. |
| `server/Protocol/MessageType.cs` | Packet type byte: `Connect`, `Disconnect`, `Input`, `Ping`, and the replies. |
| `server/Protocol/MessageSerializer.cs` | Little-endian serialize and deserialize. Every packet is 48 bytes. |
| `server/Protocol/DiscoveryMessage.cs` | Probe and announcement text, separate from the 48-byte packet. |
| `server/Protocol/Constants.cs` | Ports `5555`, `5556`, `5557`, four sessions, 5 s timeout, 2 s announcements. |
| `server/NetworkLayer/UdpServer.cs` | Receive loop and `Send` for replies. |
| `server/NetworkLayer/ClientManager.cs` | One session per endpoint, up to 4. |
| `server/NetworkLayer/ClientSession.cs` | One endpoint and the time of its last packet. |
| `server/NetworkLayer/DiscoveryService.cs` | Answers probes on UDP 5556 and broadcasts announcements on UDP 5557. |
| `server/NetworkLayer/HeartbeatMonitor.cs` | Drops a session that has been quiet for 5 s. Checks every 3 s. |
| `server/GamepadDriver/VirtualGamepad.cs` | One ViGEm Xbox 360 controller. |
| `server/GamepadDriver/VirtualGamepadManager.cs` | Creates and removes one controller per session. |
| `server/GamepadDriver/GamepadMapper.cs` | Maps packet fields onto Xbox 360 buttons, sticks and triggers. |
| `server/ServerController.cs` | Connects the network layer to the gamepad driver and sends replies. |
| `server/MainForm.cs` | Server window. `LayoutCards()` places the controls on a DPI-scaled grid. |
| `server/MainForm.Designer.cs` | Window controls, including the version label. |
| `server/Theme.cs` | Window colours. |
| `server/ThemedControls.cs` | Drawn controls used by the window. |
| `server/TrayManager.cs` | Notification-area icon. |
| `server/SettingsManager.cs` | Saved settings, including the listen port. |
| `server/Assets/app.ico` | Icon embedded for the window and tray. |

The window shows Start/Stop, the address to use, the list of sessions, and an event log.

### Message flow

`UdpServer` raises each datagram. `ServerController.HandleMessage` deserializes it and:

| Incoming | Session | What the server does |
|----------|---------|----------------------|
| `Connect` | none, and a slot is free | create a session and a virtual controller, reply `ConnectAck` |
| `Connect` | none, because the controller could not be created or every slot is in use | reply `ServerFull` |
| `Input` | yes | `GamepadMapper` updates that session's Xbox 360 controller |
| `Ping` | yes | reply `Pong`, echoing `timestampMs` |
| `Ping` | no | reply `NotConnected` |
| `Disconnect` | yes | remove the session and its controller |
| anything else | no | ignore |

Replies are a 48-byte packet with `type` and the echoed `timestampMs` set. The other fields are zero.

### Session lifecycle

A session starts when `Connect` is accepted and ends when `Disconnect` arrives, the heartbeat finds 5 s of silence, or the server stops. Stopping releases every session and its virtual controller. `GetOrAdd` does not keep a session whose virtual controller failed to create.

### Discovery

While the controller port is open, `DiscoveryService` listens on UDP `5556` for `PCTRL?1|<name>` and broadcasts `PCTRL!1|{...}` on UDP `5557` every 2 s. The announcement carries the machine name, controller port, sessions in use, the maximum of 4, a per-run id, and the machine's IPv4 addresses. See [PROTOCOL.md](PROTOCOL.md#lan-discovery).

## Testing

- `server/Tests/Protocol/MessageSerializerTests.cs` — serializer round-trip.
- `server/Tests/Protocol/GamepadMessageTests.cs` — button-bit checks.
- `server/Tests/NetworkLayer/ClientManagerTests.cs` — session limits and lookup.
- `server/Tests/NetworkLayer/ClientSessionTests.cs` — session state.
- `server/Tests/NetworkLayer/DiscoveryTests.cs` — discovery packets.
