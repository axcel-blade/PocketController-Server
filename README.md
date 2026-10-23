# PocketController-Server

Windows server for [PocketController](https://github.com/axcel-blade/PocketController). The phone app sends UDP input; this server turns each phone into a virtual Xbox 360 (XInput) controller through the [ViGEmBus](https://github.com/nefarius/ViGEmBus) driver.

The wire format is documented in the app repo at `docs/PROTOCOL.md`.

## Requirements

- Windows 10/11
- [ViGEmBus driver](https://github.com/nefarius/ViGEmBus/releases)
- .NET 10 runtime
- To build: .NET 10 SDK, and Visual Studio 2022 17.13+ or Visual Studio 2026 (the solution is `.slnx`)

## Getting Started

1. Install the ViGEmBus driver.
2. Open `server/PocketControllerServer.slnx`, set **PocketControllerServer** as the startup project, and run it (F5).
3. Click **Start** — the server listens on UDP port `5555` by default.
4. If Windows Firewall asks, allow PocketController Server on private networks (UDP `5555` for play, `5556`–`5557` for discovery).

## Development

```sh
cd server
dotnet build PocketControllerServer.slnx
dotnet test Tests
```
