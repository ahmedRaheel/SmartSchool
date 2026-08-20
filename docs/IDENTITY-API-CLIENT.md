# Identity API / SmartSchool API integration

Identity host maps only `MapIdentityServerEndpoints()`. Legacy CQRS profile/role-assignment
endpoints are not mapped there, eliminating the unregistered mediator startup error.

SmartSchool.Api validates user access tokens against `https://localhost:7101` and uses the
`smartschool-api-service` client with client-credentials scope `smartschool.identity.manage`
to call `/api/internal/accounts`.

Development secrets are placeholders. Move the client secret to user-secrets/environment
variables before deployment.
