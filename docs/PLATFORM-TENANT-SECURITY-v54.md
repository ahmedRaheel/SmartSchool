# Platform / tenant security v54

Implemented:
- SuperAdmin-only tenant create/update/delete authorization.
- SuperAdmin can enable/disable all Identity users in a tenant.
- SuperAdmin can delete tenant Identity users.
- SchoolAdmin/Admin can manage users only inside their own tenant.
- New accounts use a generated temporary password when password is omitted and `MustChangePassword=true`.
- Password lifecycle fields: SchoolId, MustChangePassword, PasswordChangedAt.
- Token claims: school_id and must_change_password.
- Portal Platform Management page supports tenant context, School Master Admin creation, tenant enable/disable and user listing.
- SuperAdmin dashboard uses `selected_tenant_id`; normal users remain bound to their token tenant.
- Impersonation start is audited and returns a support intent. A real target-user access token is NOT forged by the portal. Complete token exchange requires a dedicated IdentityServer extension/custom grant and must be added before the UI actually switches identity.

Security:
- Tenant deletion is destructive. The API deletes Identity users only in the current implementation; business-data purge must remain a separate controlled workflow.
- Passwords/tokens are never written to telemetry.
