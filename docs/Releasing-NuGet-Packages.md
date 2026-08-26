# Releasing NuGet packages

This document describes how a maintainer publishes a new version of the
`XperienceCommunity.Localization` / `XperienceCommunity.Localization.Base` NuGet packages from
this repository. It is written so it can be pasted into the YouTrack knowledge base as-is.

## Overview

Both packages share a single version number, taken from the `<VersionPrefix>` in
[`Directory.Build.props`](../Directory.Build.props). Publishing is done through a manually
triggered GitHub Actions workflow — there is no automatic release-on-merge.

## Steps

1. **Merge all changes for the release into `main`.**
   Make sure the change(s) you want to ship (bug fix, dependency bump, new feature) are already
   merged via a reviewed PR.

2. **Bump the version number.**
   Edit [`Directory.Build.props`](../Directory.Build.props) and update `<VersionPrefix>` following
   [Semantic Versioning](https://semver.org/):

   - **Patch** (`2.0.1` → `2.0.2`) — bug fixes, dependency/compatibility updates that don't change
     the public API.
   - **Minor** (`2.0.x` → `2.1.0`) — backwards-compatible new functionality.
   - **Major** (`2.x` → `3.0.0`) — breaking API changes.

   Commit this change (a PR titled e.g. `chore: bump version to X.Y.Z`, see recent history such
   as PR #27) and merge it to `main`.

3. **Run the "Build and push nuget to registry" workflow.**
   This is defined in [`.github/workflows/build_and_publish.yml`](../.github/workflows/build_and_publish.yml)
   and is triggered manually (`workflow_dispatch`):

   - Go to the repository on GitHub → **Actions** tab.
   - Select **Build and push nuget to registry** in the left sidebar.
   - Click **Run workflow**, choose the `main` branch, and confirm.

   The workflow runs two jobs in parallel — one per package (`XperienceCommunity.Localization.Base`
   and `XperienceCommunity.Localization`) — each of which:

   - Reads the version from `Directory.Build.props`.
   - Runs `dotnet pack` for the package's `.csproj` in `Release` configuration.
   - Publishes the resulting `.nupkg` to [nuget.org](https://www.nuget.org) using the
     `NUGET_API_KEY` repository secret.

   Only repository owner `nittin-cz` can trigger this workflow (enforced by an `if:` condition in
   the workflow), and it requires the `NUGET_API_KEY` secret to be configured on the repository.

4. **Verify the published packages.**
   Check [nuget.org/packages/XperienceCommunity.Localization](https://www.nuget.org/packages/XperienceCommunity.Localization)
   and [nuget.org/packages/XperienceCommunity.Localization.Base](https://www.nuget.org/packages/XperienceCommunity.Localization.Base)
   to confirm the new version appears (indexing on nuget.org can take a few minutes).

5. **Tag the release and update GitHub Releases.**
   Create a Git tag matching the version (e.g. `v2.0.2`) and publish a corresponding GitHub
   Release with the notable changes — the package metadata's `PackageReleaseNotes` field points
   consumers to the repository's [Releases page](https://github.com/nittin-cz/xperience-community-localization/releases).

6. **Update the README version matrix if needed.**
   If the release changes the minimum supported Xperience by Kentico version (e.g. after
   following [Upgrading-Xperience-Version.md](Upgrading-Xperience-Version.md)), make sure the
   *Library Version Matrix* table in [`README.md`](../README.md) reflects it before/with this
   release.

## Local pack (dry run, optional)

To sanity-check that a version packs correctly before running the GitHub Actions workflow, you
can pack locally without publishing:

```bash
dotnet pack "src/XperienceCommunity.Localization.Base/XperienceCommunity.Localization.Base.csproj" -p:Version=X.Y.Z --configuration Release
dotnet pack "src/XperienceCommunity.Localization/XperienceCommunity.Localization.csproj" -p:Version=X.Y.Z --configuration Release
```

The resulting `.nupkg`/`.snupkg` files are written to each project's `bin/Release` folder. This
does **not** publish anything — it's only useful to catch packaging errors early.

## Troubleshooting

- **Workflow fails at the "Get version" step** — `Directory.Build.props` must contain a
  `<VersionPrefix>X.Y.Z</VersionPrefix>` line; the workflow extracts it with a regex.
- **`dotnet nuget push` fails with 403** — the `NUGET_API_KEY` secret is missing, expired, or
  lacks push permission for these package IDs on nuget.org.
- **Version already exists on nuget.org** — nuget.org does not allow re-publishing the same
  version; bump `VersionPrefix` again and re-run the workflow.
