# Changelog

The version in `PocketControllerServer.csproj` is **1.2.3** (`Version`, `AssemblyVersion` `1.2.3.0`, `FileVersion` `1.2.3.0`).

## [Unreleased]

### Changed
- Tagged releases (`vX.Y.Z`, numeric SemVer) now publish a self-contained Windows zip — unzip and run `PocketControllerServer.exe` without installing the .NET runtime

## [1.2.3] - 2026-11-17

### Added
- GitHub Actions workflows for CI (build/test/publish) and tagged release publishing

### Changed
- Moved the .NET project to the repository root so `dotnet build` / `dotnet test` work from the clone directory (removed the nested `server/` folder)
