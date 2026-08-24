-- SmartSchool v86 full application synchronization
-- Apply migrations in the canonical order below to an existing database.
\i 20260824_v71_comprehensive_schema_sync.sql
\i 20260824_v78_dapper_schema_alignment.sql
\i 20260824_v85_communication_canonical_sync.sql
