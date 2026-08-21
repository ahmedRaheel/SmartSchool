# Identity Login API

`POST /api/account/login` accepts JSON email/password and returns a Duende-issued access token plus user summary.
The endpoint validates the ASP.NET Identity account first, including `IsActive` and lockout behavior, then requests the token from Duende's `/connect/token` endpoint using the internal confidential `smartschool-login-api` password-grant client.

This direct password API is intended for SmartSchool first-party clients/testing. Authorization Code + PKCE remains preferred for browser clients because OAuth 2.1 deprecates the password grant.

Docker request:
```json
{
  "email": "superadmin@smartschool.local",
  "password": "<SUPERADMIN_PASSWORD>"
}
```

URL: `http://localhost:7101/api/account/login`
