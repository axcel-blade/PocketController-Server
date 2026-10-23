# PocketController-Server

<p align="center">
  <img src="server/Assets/app.ico" alt="PocketController Server icon" width="128">
</p>

<!-- Dynamic badges (live data from GitHub) -->
[![CI](https://img.shields.io/github/actions/workflow/status/axcel-blade/PocketController-Server/ci-server.yml?branch=main&label=CI&logo=dotnet)](https://github.com/axcel-blade/PocketController-Server/actions/workflows/ci-server.yml)
[![Latest release](https://img.shields.io/github/v/release/axcel-blade/PocketController-Server?include_prereleases&sort=semver)](https://github.com/axcel-blade/PocketController-Server/releases)
[![Open issues](https://img.shields.io/github/issues/axcel-blade/PocketController-Server)](https://github.com/axcel-blade/PocketController-Server/issues)
[![Last commit](https://img.shields.io/github/last-commit/axcel-blade/PocketController-Server/main)](https://github.com/axcel-blade/PocketController-Server/commits/main)
[![Stars](https://img.shields.io/github/stars/axcel-blade/PocketController-Server?style=flat)](https://github.com/axcel-blade/PocketController-Server/stargazers)

<!-- Static badges -->
[![License: MIT](https://img.shields.io/badge/license-MIT-c4f82a)](LICENSE.md)
[![Version](https://img.shields.io/badge/version-1.2.2-c4f82a)](CHANGELOG.md)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Platforms](https://img.shields.io/badge/platform-Windows%2010%2F11-0078D4?logo=windows&logoColor=white)](docs/ARCHITECTURE.md)
[![Protocol](https://img.shields.io/badge/protocol-UDP%20%C2%B7%2048%20bytes-5cd6e6)](docs/PROTOCOL.md)

This repository is the Windows server. It turns UDP input into a virtual Xbox 360 (XInput) controller through the [ViGEmBus](https://github.com/nefarius/ViGEmBus) driver. Phones connect from [PocketController-App](https://github.com/axcel-blade/PocketController-App).

## What this project does

```
server/
├── Protocol/       (48-byte packets and discovery text)
├── NetworkLayer/   (UDP listen, sessions, discovery)
├── GamepadDriver/  (virtual Xbox 360 via ViGEm)
└── MainForm        (server window)
```

- Listens for 48-byte little-endian UDP controller packets. The default port is `5555`.
- Answers discovery probes on UDP `5556` and broadcasts announcements on UDP `5557`.
- Keeps up to 4 sessions. Each session gets one virtual Xbox 360 controller.
- Drops a session after 5 seconds with no packets.
- The window has Start/Stop, the listen address, the session list, and an event log.

| Setting        | Value |
|----------------|-------|
| UDP port       | 5555  |
| Discovery      | UDP 5556 (probes) / 5557 (announcements) |
| Announcements  | every 2 s while running |
| Sessions       | 4     |
| Idle timeout   | 5 s   |

## Requirements

- Windows 10/11
- [ViGEmBus driver](https://github.com/nefarius/ViGEmBus/releases)
- .NET 10 runtime
- To build: .NET 10 SDK, and Visual Studio 2022 17.13+ or Visual Studio 2026 (the solution is `.slnx`)

## Getting Started

1. Install the ViGEmBus driver.
2. Open `server/PocketControllerServer.slnx`, set **PocketControllerServer** as the startup project, and run it (F5).
3. Click **Start**. The server listens on UDP port `5555` by default.
4. If Windows Firewall asks, allow PocketController Server on private networks (UDP `5555` for play, `5556`–`5557` for discovery).

Having trouble? See [SUPPORT.md](SUPPORT.md).

> The packet layout this server parses and the replies it sends are documented in [docs/PROTOCOL.md](docs/PROTOCOL.md).

## Development

```sh
cd server
dotnet build PocketControllerServer.slnx
dotnet test Tests
```

See [CONTRIBUTING.md](CONTRIBUTING.md).

## Version

| Project | Version |
|---------|---------|
| PocketController Server | **1.2.2** |

`server/PocketControllerServer.csproj` sets `Version`, `AssemblyVersion` (`1.2.2.0`), and `FileVersion` (`1.2.2.0`). The window header in `server/MainForm.Designer.cs` shows the same number. See the [changelog](CHANGELOG.md).

## Further reading

- [CONTRIBUTING.md](CONTRIBUTING.md)
- [SUPPORT.md](SUPPORT.md)
- [SECURITY.md](SECURITY.md)
- [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)
- [CHANGELOG.md](CHANGELOG.md)
- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)
- [docs/PROTOCOL.md](docs/PROTOCOL.md)
- [LICENSE.md](LICENSE.md)
