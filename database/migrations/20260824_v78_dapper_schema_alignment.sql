-- SmartSchool v78 Dapper/schema alignment
-- Apply v71 comprehensive schema sync first. This migration is intentionally idempotent.
-- The runtime error `assignment_id does not exist` is a query defect, not a missing
-- lms.academic_assignment column. Its real PK is academic_assignment_id, so no alias
-- column is added here.

-- Notification/application model columns that are valid in the backend and absent
-- from the original database are retained as database columns.
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS recipient_user_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS type varchar(100);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS message text;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_type varchar(150);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS action_url text;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS priority varchar(30);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS is_read boolean NOT NULL DEFAULT false;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS read_at timestamptz;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS occurred_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP;

-- v71 contains the complete generated model-to-schema additions for the remaining modules.
-- Keep this migration small so it is safe to apply after v71 without duplicating all DDL.
