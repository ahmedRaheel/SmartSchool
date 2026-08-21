# Portal direct Duende login (v49)

The React portal no longer calls `/api/account/login`, so Identity.Api no longer performs an HTTP call to itself.

Portal token request:
- POST `http://localhost:7101/connect/token`
- content type `application/x-www-form-urlencoded`
- grant_type=password
- client_id=smartschool-login-api
- username/password
- scope=`openid profile email smartschool.profile smartschool.api offline_access`

The browser client is public (`RequireClientSecret=false`) and has the configured portal origin in `AllowedCorsOrigins`.
After receiving the token, the portal calls `/api/account/me` with Bearer authentication to build its session.
Refresh tokens are exchanged directly at `/connect/token`.

The development Duende seeder replaces the persisted `smartschool-login-api` client so an older confidential-client row is upgraded automatically.

Security note: Resource Owner Password is retained only because direct JSON/form login was explicitly requested. Authorization Code + PKCE remains the preferred browser flow for production.
