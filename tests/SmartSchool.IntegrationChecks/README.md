# Workflow integration checks

Requirements: .NET 10 SDK and Node.js 22 or newer. From this directory:

```sh
npm ci
node run-checks.mjs
node check-upgrade.mjs
```

The runner creates an isolated PGlite PostgreSQL database, installs the fresh schema, applies V123 again to check repeatability, and starts the real feature endpoints on `127.0.0.1:5487`. It tests real PostgreSQL queries, EF writes, file bytes, workflow decisions, and production authorization policies. Each HTTP request has a fresh dependency injection scope. No user database is modified.

`test-results/workflow-checks.json` is written only after the complete suite passes. Use `DOTNET_EXECUTABLE` if the SDK executable is not on PATH. Ports 5433 and 5487 must be free.

Only the test project accepts `X-Test-Role`. Production code does not register this authentication handler. The account provisioning service is a test double; this suite does not certify login, token issuance, or live Identity account creation. Embedded PostgreSQL also does not certify production connection pooling, network behavior, or concurrent transaction throughput.

For native PostgreSQL verification, first create a disposable database with pgcrypto and pgvector available, install `database/SmartSchool.FreshInstall.sql`, build the test project, and run its DLL with `SMARTSCHOOL_TEST_DATABASE` set to that database's Npgsql connection string. Never point fixture checks at a real school database. To inspect EF mappings without a connection:

```sh
dotnet run --project . --no-build -- --export-model
```
