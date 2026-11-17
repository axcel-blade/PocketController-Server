# Changelog

The version in `PocketControllerServer.csproj` is **1.2.3** (`Version`, `AssemblyVersion` `1.2.3.0`, `FileVersion` `1.2.3.0`).

## [Unreleased]

## [1.2.3] - 2026-11-17

### Added
- GitHub Actions workflows for CI (build/test/publish) and tagged release publishing

### Changed
- Moved the .NET project to the repository root so `dotnet build` / `dotnet test` work from the clone directory (removed the nested `server/` folder)
- Tagged releases publish Windows installers (`.exe` setup bootstrapper and `.msi`) instead of a zip — self-contained, no separate .NET runtime install
