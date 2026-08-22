$ErrorActionPreference = "Stop"
$env:Persistence__Provider = "PostgreSql"
$env:Persistence__ConnectionStringName = "SmartSchool"
dotnet ef migrations add InitialAspNetIdentity -c SmartSchoolIdentityDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/PostgreSql/AspNetIdentity
dotnet ef migrations add InitialDuendeConfiguration -c ConfigurationDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/PostgreSql/DuendeConfiguration
dotnet ef migrations add InitialDuendeOperational -c PersistedGrantDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/PostgreSql/DuendeOperational
dotnet ef database update -c SmartSchoolIdentityDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
dotnet ef database update -c ConfigurationDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
dotnet ef database update -c PersistedGrantDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
