# Changelog

Released history is not recorded in this repository. There are no git tags and no release notes.

The version in `PocketControllerServer.csproj` is **1.2.2** (`Version`, `AssemblyVersion` `1.2.2.0`, `FileVersion` `1.2.2.0`).

## [Unreleased]

### Added
- GitHub Actions workflows for release publishing, CodeQL, dependency review, secret scanning, changelog checks, stale issue cleanup, coverage, release drafting, Conventional Commit / PR title checks, license checks, and SBOM generation

### Changed
- Moved the .NET project to the repository root so `dotnet build` / `dotnet test` work from the clone directory (removed the nested `server/` folder)
