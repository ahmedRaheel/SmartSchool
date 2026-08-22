# SmartSchool.IdentityServer4

This project is deliberately isolated from the .NET 10 SmartSchool API.

IdentityServer4 4.1.2 targets the legacy ASP.NET Core/.NET Core 3.1 generation.
Do not reference it from the .NET 10 API or BuildingBlocks projects.

The SmartSchool API remains a standards-based JWT/OIDC resource server and can
validate tokens from this host through Authority/Audience configuration.

Security warning: IdentityServer4 is end-of-life and has known security issues.
This project is included only because the solution explicitly requested the
legacy free IdentityServer4 4.1.2 stack. For production, use a maintained OIDC
provider or a supported IdentityServer edition.
