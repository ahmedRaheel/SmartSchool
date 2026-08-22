# v61 Tenant Provisioning + Notification Authorization

- Notification read/list/unread endpoints require authentication, not SuperAdminOnly.
- Tenant creation now creates the tenant and its master Admin identity account as one application operation.
- Tenant request collects organization code/name plus administrator first name, last name, email and phone.
- Identity creates a strong temporary password, sets MustChangePassword=true, and returns the password once.
- If account provisioning fails, the newly created tenant is compensated/deleted so a tenant is not left without an administrator.
- Tenant codes are checked globally.
- Portal Add Tenant is a human business form and shows one-time credentials after successful creation.
- Result<T> remains the only API response contract.
