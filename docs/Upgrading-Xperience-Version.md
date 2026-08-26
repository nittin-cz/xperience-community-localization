# Upgrading the Xperience by Kentico version

This document describes the steps a maintainer follows to move this repository (the library
and the `DancingGoat` sample project) to a newer Xperience by Kentico version, whenever Kentico
ships a new refresh. Follow it whenever the [Xperience changelog](https://docs.kentico.com/changelog/)
lists a new refresh you want to support, or when a bug report (like [#28](https://github.com/nittin-cz/xperience-community-localization/issues/28))
turns out to be caused by a binary-compatibility change in a Kentico package.

## Background

The library ships two NuGet packages (`XperienceCommunity.Localization` and
`XperienceCommunity.Localization.Base`) that are compiled against a specific version of the
`Kentico.Xperience.*` packages. Because Xperience by Kentico is a continuously delivered
product, Kentico occasionally makes small binary-compatibility breaking changes to admin/API
extension methods between refreshes (adding an optional parameter changes the compiled call
site's method signature even though the C# source still compiles unchanged). When that happens,
a package built against an older refresh throws a `MissingMethodException` (surfaced as a 500
error in the admin UI) when loaded into a project running a newer refresh.

The fix is always the same: recompile the library against the newer `Kentico.Xperience.*`
packages and release a new NuGet version.

## Steps

1. **Check the target version.** Decide which Xperience version to target — normally the latest
   available release, unless there's a reason to pin to something specific (e.g. matching what
   the reporting customer runs). All `Kentico.Xperience.*` packages used in this repo
   (`Admin`, `WebApp`, `AzureStorage`, `Core`, `ImageProcessing`) are released in lockstep, so
   they should all be bumped to the same version.

2. **Update the central package versions.** Edit [`Directory.Packages.props`](../Directory.Packages.props)
   and bump every `Kentico.Xperience.*` `PackageVersion` entry to the target version. Because the
   repo uses [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management),
   this single file drives both the library projects under `src/` and the `examples/DancingGoat`
   sample project.

3. **Restore and rebuild.**

   ```bash
   dotnet restore XperienceCommunity.Localization.sln --force-evaluate
   dotnet build XperienceCommunity.Localization.sln --configuration Release --no-restore
   ```

   `--force-evaluate` regenerates the `packages.lock.json` files (the repo has
   `RestorePackagesWithLockFile` enabled) — commit the updated lock files together with the
   `Directory.Packages.props` change. Fix any compile errors caused by API changes in the new
   Kentico packages (check the [Xperience changelog](https://docs.kentico.com/changelog/) for
   breaking-change notes).

4. **Update the sample project's database and files.** This step requires a running Xperience
   database for `examples/DancingGoat` (see [Contributing-Setup.md](Contributing-Setup.md) for
   how to create one). It cannot be skipped when validating a real upgrade end-to-end, only when
   just fixing a compile-time issue.

   ```bash
   cd examples/DancingGoat
   dotnet run --no-build -- --kxp-update
   ```

   Before running this, disable Continuous Integration (CI) in the Xperience **Settings**
   application (or via `--kxp-ci-disable`, available since refresh 31.6.0), and re-enable it
   (`--kxp-ci-enable`) once the update finishes. See
   [Update Xperience by Kentico projects](https://docs.kentico.com/documentation/developers-and-admins/installation/update-xperience-by-kentico-projects)
   for full details, including the SaaS deployment flow.

5. **Verify the fix manually.** Run `examples/DancingGoat`, sign in to the administration, and
   open the **Localizations** application (the module this library adds) to confirm it loads
   without a 500 error. Exercise create/edit/delete of a localization key.

6. **Update the version matrix.** Add a row to the *Library Version Matrix* table in
   [`README.md`](../README.md) documenting the minimum Xperience version the new library release
   requires, and add a short note if the change fixes a specific compatibility issue (see the
   note added for [#28](https://github.com/nittin-cz/xperience-community-localization/issues/28)
   as an example).

7. **Bump the library version and release.** Follow [Releasing-NuGet-Packages.md](Releasing-NuGet-Packages.md)
   to version and publish the updated packages.

## Notes

- The library targets `net8.0` regardless of the Xperience refresh; only the `Kentico.Xperience.*`
  package versions change between refreshes within the same Xperience major version.
- If Kentico ships a genuinely breaking API change (not just a binary-compat shift), the library
  code itself will also need updating — check the compiler errors from step 3 first.
