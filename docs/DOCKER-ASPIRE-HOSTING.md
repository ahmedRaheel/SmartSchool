# SmartSchool local container hosting

## Recommendation
Keep Dockerfiles on both deployable APIs. Use Aspire AppHost for local orchestration/observability.
Dockerfiles are the deployment artifact; Aspire is the developer/deployment orchestration model.

Existing infrastructure containers:
- PostgreSQL: host port 5432
- Redis: host port 6379
- Kafka: host port 9092
- Ollama: host port 11434

## Option A - Run both APIs in Docker
Copy `.env.example` to `.env`, change all secrets, then:

```powershell
docker compose -f docker-compose.app.yml up --build -d
docker compose -f docker-compose.app.yml ps
docker compose -f docker-compose.app.yml logs -f identity-api
docker compose -f docker-compose.app.yml logs -f smartschool-api
```

Identity API: http://localhost:7101
SmartSchool API: http://localhost:7001

Containers reach the already-running infrastructure through `host.docker.internal`.

## Option B - Aspire for development
The AppHost runs the two .NET API projects and leaves PostgreSQL/Redis/Kafka/Ollama in the
existing Docker containers. This avoids duplicate infrastructure containers.

```powershell
dotnet restore
dotnet run --project src/SmartSchool.AppHost/SmartSchool.AppHost.csproj
```

In this mode the APIs use their Development/localhost configuration.

## Secrets
Do not commit real PostgreSQL passwords, Duende client secrets, or SuperAdmin passwords.
The checked-in Docker settings are overridden by Compose environment variables.

## Azure
Keep the same Dockerfiles. Aspire can later target Azure Container Apps or Kubernetes without
changing the business modules. Production should replace local PostgreSQL/Redis/Kafka/Ollama
endpoints with managed/private endpoints and store secrets in Key Vault or equivalent.
