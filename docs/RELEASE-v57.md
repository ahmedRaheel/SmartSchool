# v57 Consistent API Contract + Real UI Notifications

Canonical response contract:
{
  "success": true|false,
  "data": <payload or null>,
  "error": null | { "code": "...", "message": "..." },
  "traceId": "...",
  "timestampUtc": "..."
}

- Result<T> success and failure now use the same envelope.
- Result success without data returns HTTP 200 with data=null instead of a shape-changing 204.
- Validation/not-found/conflict/unauthorized retain proper HTTP status codes while returning the envelope.
- Unhandled SmartSchool.Api exceptions use the same failure envelope.
- React apiClient unwraps success data and converts failure envelopes into SmartSchoolApiError.
- AppShell notification bell now loads real Communication notifications, unread count, marks one/all read, polls every 30 seconds and navigates through ActionUrl.
- Removed seeded/mock notifications from AppShell.

Note: framework-generated 401/403 responses that occur before an endpoint/middleware body is produced may still need an authentication/authorization result handler if a JSON envelope is required for absolutely every pipeline rejection.
