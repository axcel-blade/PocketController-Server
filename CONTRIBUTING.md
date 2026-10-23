# Contributing to PocketController-Server

Thank you for your interest in contributing to the Windows server.

## Getting Started

1. Fork the repository and create your branch from `develop`.
2. Follow the [Git Flow](#git-flow) branching model.
3. Make sure the server builds and tests cleanly before opening a pull request.

## Git Flow

This is the required workflow. Only `main` exists in this repository today.

| Branch | Purpose |
|--------|---------|
| `main` | Stable releases only |
| `develop` | Integration branch for ongoing work |
| `feature/*` | New features branched from `develop` |
| `release/*` | Release preparation branched from `develop` |
| `hotfix/*` | Critical fixes branched from `main` |

## Prerequisites

- Windows 10/11
- .NET 10 SDK
- [ViGEmBus driver](https://github.com/nefarius/ViGEmBus/releases), to run the server locally
- Visual Studio 2022 17.13+ or Visual Studio 2026, if you open the `.slnx` solution

## Building

```
cd server
dotnet build PocketControllerServer.slnx
dotnet test Tests
```

## Protocol Changes

The packets this server parses and the replies it sends are documented in [docs/PROTOCOL.md](docs/PROTOCOL.md).
If you change them, update `server/Protocol/`, `server/Tests`, and that document.

## Pull Request Guidelines

- Keep PRs focused — one feature or fix per PR.
- Update `CHANGELOG.md` under `[Unreleased]` with a summary of your change.
- When releasing the server, update:
  - `CHANGELOG.md` — move `[Unreleased]` entries into a dated `## [X.Y.Z] - YYYY-MM-DD` section
  - `README.md` — the version badge and the **Version** section
  - `server/PocketControllerServer.csproj` — `Version`, `AssemblyVersion`, `FileVersion`
  - `server/MainForm.Designer.cs` — the `lblVersion` text
- Do not include build outputs (`bin/`, `obj/`) or IDE files (`.vs/`) in your commit.

## Code Style

- C#: follow standard C# naming conventions and keep each class in its own file, named after the class.
- No commented-out code or `TODO` left in production files — open an issue instead.
