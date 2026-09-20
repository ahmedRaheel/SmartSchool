# API Smoke Test - Environment Token Fallback

The self-contained Identity/business fixture has been removed from the active smoke-test path.

## Authentication

Set a valid SmartSchool API access token:

```powershell
$env:SMARTSCHOOL_API_TOKEN = "<access-token>"
```

Then run:

```powershell
.\scripts\Test-AllApiEndpoints.ps1 `
  -BaseUrl http://localhost:7001 `
  -Mode All
```

The runner:

- requires `SMARTSCHOOL_API_TOKEN` (or `--token` when invoking the C# tool directly),
- performs an authentication preflight against `/api/testing/auth-probe` when available,
- decodes tenant/school/branch/campus/user and actor IDs from JWT claims for request generation,
- uses numeric paging values (`page=1`, `pageSize=25`, etc.),
- does not create or clean up Identity users, tenants, schools, campuses, or actors,
- writes JSON, CSV, and Markdown reports under `artifacts/api-smoke`.

Because `Mode All` can execute POST/PUT/PATCH/DELETE endpoints against existing data, run it only against a disposable development/test database. Use `-Mode ReadOnly` for a non-mutating scan.
