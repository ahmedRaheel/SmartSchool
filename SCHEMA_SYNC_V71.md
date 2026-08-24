# SmartSchool v71 Comprehensive Synchronization

This pass is solution-wide, not notification-only.

## Applied

- Replaced **116** `excludingId` predicates with the requested explicit nullable guard form.
- Audited **130** EF entity configurations against the canonical PostgreSQL schema.
- Synchronized **218** Dapper `FROM` clauses with their EF schema/table mappings.
- Added **562** missing database-backed properties to domain entities where the database already contained fields not represented by the model.
- Added corresponding EF column mappings for synchronized fields.
- Added an idempotent PostgreSQL migration that creates model-backed tables missing from the supplied database and adds model-backed columns missing from existing tables.
- Corrected `MlPredictionResultEntity` so it no longer shares `ai.prediction_model` with `PredictionModelEntity`; it now maps to `ai.ml_prediction_result`.
- Preserved the v70 notification and observability fixes.

## Important predicate note

The original expression `!excludingId.HasValue || entity.Id != excludingId.Value` and the requested expression are logically equivalent in C#. The requested form has nevertheless been applied consistently throughout the solution as requested.

## Database deployment

Run `database/migrations/20260824_v71_comprehensive_schema_sync.sql` against the existing PostgreSQL database. For a new database, the synchronization is also appended to `database.sql` and `database/SmartSchoolComplete.synced.sql`.
