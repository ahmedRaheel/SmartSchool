# SmartSchool Communication Persistence Review — v85

## Scope

The Communication module was reviewed against `database/SmartSchoolComplete.sql` as the physical PostgreSQL source supplied with the solution.

## Corrections

- Resolved committed C# merge-conflict markers across `src` by retaining the current HEAD branch implementation.
- Removed duplicate EF configuration classes that configured the same Communication entity more than once.
- Standardized Communication EF mappings on the `communication` schema and snake_case PostgreSQL columns.
- Kept entity-specific primary key properties (`ConversationId`, `MessageId`, `NotificationId`, `ChatConversationId`, etc.).
- Corrected Dapper aliases so entity-specific identifiers are hydrated into the correct C# properties instead of an obsolete `Id` property.
- Corrected `conversation_participant` and `message_receipt` Dapper queries that referenced a non-existent `id` column.
- Added explicit string conversion for `NotificationType` enum persistence.
- Removed duplicate legacy Notification properties that represented the same concepts as the richer notification contract.
- Added an idempotent PostgreSQL migration to synchronize Communication tables required by the current mappings.
- Added missing tenant/lifecycle/entity-specific-key fields for normalized participant/message/receipt tables.
- Added canonical `chat_attachment` and `notification_preference` tables.
- Standardized chat table columns to snake_case and entity-specific PK names.

## Database migration

Apply:

`database/migrations/20260824_v85_communication_canonical_sync.sql`

For a new database, `database/SmartSchoolComplete.v85.sql` contains the supplied full dump followed by the v85 synchronization migration.

## Static gates run

- No unresolved `<<<<<<<` / `>>>>>>>` markers remain under `src`.
- No `AS "Id"` Dapper aliases remain in Communication persistence.
- No duplicate EF configuration target remains in Communication.
- No duplicate physical-column mapping was detected inside Communication entity configurations.
- No Communication EF configuration references a property absent from its configured entity/base lifecycle model.

## Build limitation

The execution container does not have the .NET SDK installed, so `dotnet build` could not be executed here. Static source/schema validation was run instead. A local `dotnet build` remains the final compiler gate.
