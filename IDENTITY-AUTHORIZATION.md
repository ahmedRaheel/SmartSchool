# SmartSchool IdentityServer, RBAC and sample actors

The API validates IdentityServer-issued JWT access tokens.

Required claims:
- `sub`
- `tenant_id`
- `school_id`
- `role`

`ICurrentUser` resolves tenant, school and user identity from the authenticated token.
Production business handlers should use this context instead of trusting a tenant id
provided by the caller.

Sample development actors are supplied for SchoolAdmin, Teacher, Student and Parent.

No default password is committed. IdentityServer accounts should be provisioned through
the selected IdentityServer administration/provisioning API. SmartSchool must not write
directly to IdentityServer persistence tables.

Named authorization policies are supplied for school administration, academics,
student self-service, parent self-service, finance and HR.
