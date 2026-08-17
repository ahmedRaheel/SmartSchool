# SmartSchool Build Fix v8

Changes:
- Removed direct Newtonsoft.Json references and migrated direct serialization calls to System.Text.Json.
- Added Microsoft.AspNetCore.App framework reference centrally for non-Web projects that expose Minimal API endpoint mappings.
- Added shared ASP.NET Core global usings for IResult, IEndpointRouteBuilder, IServiceCollection and ILogger.
- Added model aliases in generated feature namespaces to address namespace/type collisions such as AiExecutionLog, KnowledgeChunk and similar generated entities.
- Preserved clean module csproj files and central package management.
- Preserved IdentityServer/RBAC/sample actor work from v7.

Remaining direct Newtonsoft references: 0

Note: Newtonsoft.Json can still appear as a transitive dependency of a third-party NuGet package.
If a restore still reports Newtonsoft.Json 11.0.1, identify the parent package with:
`dotnet list package --include-transitive`
and upgrade/remove that parent package rather than adding Newtonsoft directly.
