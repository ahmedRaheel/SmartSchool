# Schema/backend synchronization – 2026-08-24

## Corrected in this build
- `communication.notification` now matches `NotificationEntity`/Dapper: recipient_user_id, type, message, related entity fields, action_url, priority, read state and occurred_at.
- Existing legacy notification rows are backfilled by the migration; legacy NOT NULL columns no longer block EF inserts.
- Notification unread query is implemented (removed `NotImplementedException`).
- Admin dashboard unread notification count now uses `is_read = false`, not the obsolete queue `status`.
- Unhandled API exceptions are persisted to `observability.application_log` with exception text, path, trace ID, correlation ID and useful JSON properties.
- Client/UI errors are persisted to the same table and failure of the telemetry store does not break the portal.
- Added authenticated telemetry log list/detail API at `/api/telemetry/logs` and `/api/telemetry/logs/{id}`.
- Added indexes for notification inbox and observability UI queries.

## Database migration
Run `database/migrations/20260824_sync_notification_observability.sql` against an existing PostgreSQL database.
For a new bootstrap database, `database.sql` contains the corrected notification definition.
`database/SmartSchoolComplete.synced.sql` is the supplied full dump plus the synchronization migration.

## Validation limitation
The source was statically inspected in this environment. The .NET SDK is not installed here, so a `dotnet build` could not be executed. Build/run the solution locally after applying the migration; any remaining runtime SQL mismatch should now be captured with its exact PostgreSQL exception in `observability.application_log` rather than hidden behind only `INTERNAL_SERVER_ERROR`.
