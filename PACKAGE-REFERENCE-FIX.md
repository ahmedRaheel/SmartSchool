# Package Reference Build Fix

The package restore/build problem came from putting `PackageReference` items in `Directory.Build.targets`.
NuGet package references need to be present during restore evaluation, so centralized conditional references now live in `Directory.Build.props`.

## Structure

- `Directory.Packages.props` — package versions only (Central Package Management).
- `Directory.Build.props` — shared/conditional package and project references, imported early.
- `Directory.Build.targets` — build targets only; no package references.
- Individual `.csproj` files remain minimal.

## Additional fixes

- Added `SmartSchool.Application` explicitly to `SmartSchool.slnx`.
- Removed unused EF Core/OpenTelemetry/compact formatter references from Infrastructure until code actually uses them.
- Removed `Serilog.Enrichers.Span`; trace/correlation enrichment is already handled through `LogContext`.
- Aligned `Serilog.AspNetCore` with .NET 10.
- Updated Hangfire PostgreSQL and Confluent.Kafka to available stable versions.

## Verification limitation

The generation container does not contain the .NET SDK, so `dotnet restore/build` could not be executed here. The MSBuild/NuGet project structure has been corrected for restore-time package evaluation.
