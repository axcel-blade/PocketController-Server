# Support

Help for PocketController Server on Windows.

## Getting Help

- **Bug reports & feature requests** — open an [issue](https://github.com/axcel-blade/PocketController-Server/issues).
- **Questions** — open a [discussion](https://github.com/axcel-blade/PocketController-Server/discussions).
- **Security issues** — see [SECURITY.md](SECURITY.md).

## Before Opening an Issue

1. Check the [README](README.md) for setup: ViGEmBus, .NET 10, and UDP `5555`, `5556`, and `5557`.
2. Install or update the [ViGEmBus driver](https://github.com/nefarius/ViGEmBus/releases). A missing or outdated driver is the usual reason a virtual controller never appears.
3. Confirm **Start** was clicked and Windows Firewall allows PocketController Server on private networks.
4. Search [existing issues](https://github.com/axcel-blade/PocketController-Server/issues).

## Providing Good Bug Reports

Please include:

- Windows version (for example, Windows 11 22H2)
- .NET runtime version (`dotnet --version`)
- ViGEmBus driver version
- PocketController Server version (shown in the window header)
- Steps to reproduce
- Expected vs actual behaviour
- Relevant lines from the server event log
