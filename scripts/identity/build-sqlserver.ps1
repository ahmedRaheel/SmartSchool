$ErrorActionPreference = "Stop"
$env:Persistence__Provider = "SqlServer"
$env:Persistence__ConnectionStringName = "SmartSchoolSqlServer"
dotnet ef migrations add InitialAspNetIdentity -c SmartSchoolIdentityDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/SqlServer/AspNetIdentity
dotnet ef migrations add InitialDuendeConfiguration -c ConfigurationDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/SqlServer/DuendeConfiguration
dotnet ef migrations add InitialDuendeOperational -c PersistedGrantDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj -o Persistence/Migrations/SqlServer/DuendeOperational
dotnet ef database update -c SmartSchoolIdentityDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
dotnet ef database update -c ConfigurationDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
dotnet ef database update -c PersistedGrantDbContext -p ../../src/Modules/Identity/SmartSchool.Modules.Identity.csproj -s ../../src/SmartSchool.Api/SmartSchool.Api.csproj
