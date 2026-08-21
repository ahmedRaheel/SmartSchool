# v47 Identity Docker login fix

The Identity API JSON login endpoint calls Duende `/connect/token` internally. Docker host port `7101` is not the port on which Kestrel listens inside the Identity container. Compose now explicitly sets `LoginApiClient__TokenEndpoint=http://127.0.0.1:8080/connect/token`, the client id and secret, and the Identity pipeline explicitly enables authentication. Login now returns HTTP 503 with the attempted token endpoint if the internal token service cannot be reached instead of an unhandled 500.

After updating `.env`, recreate/rebuild Identity:

```powershell
docker compose -f docker-compose.app.yml build --no-cache identity-api
docker compose -f docker-compose.app.yml up -d --force-recreate identity-api
docker exec smartschool-identity-api printenv | findstr LoginApiClient
```
