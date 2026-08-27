BEGIN;

-- Align communication.notification with NotificationEntity and Dapper queries.
ALTER TABLE communication.notification
    ADD COLUMN IF NOT EXISTS recipient_user_id uuid,
    ADD COLUMN IF NOT EXISTS type varchar(100),
    ADD COLUMN IF NOT EXISTS message text,
    ADD COLUMN IF NOT EXISTS related_entity_id uuid,
    ADD COLUMN IF NOT EXISTS related_entity_type varchar(100),
    ADD COLUMN IF NOT EXISTS action_url varchar(500),
    ADD COLUMN IF NOT EXISTS priority varchar(50) NOT NULL DEFAULT 'Normal',
    ADD COLUMN IF NOT EXISTS is_read boolean NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS read_at timestamptz,
    ADD COLUMN IF NOT EXISTS occurred_at timestamptz;

-- Preserve data from the previous notification shape when upgrading an existing DB.
UPDATE communication.notification
SET recipient_user_id = COALESCE(recipient_user_id, user_id),
    type = COALESCE(type, channel_code),
    message = COALESCE(message, body, ''),
    occurred_at = COALESCE(occurred_at, created_at)
WHERE recipient_user_id IS NULL
   OR type IS NULL
   OR message IS NULL
   OR occurred_at IS NULL;

ALTER TABLE communication.notification
    ALTER COLUMN recipient_user_id SET NOT NULL,
    ALTER COLUMN type SET NOT NULL,
    ALTER COLUMN message SET NOT NULL,
    ALTER COLUMN occurred_at SET NOT NULL,
    ALTER COLUMN user_id DROP NOT NULL,
    ALTER COLUMN channel_code DROP NOT NULL;

CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient
    ON communication.notification (tenant_id, recipient_user_id);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient_unread
    ON communication.notification (tenant_id, recipient_user_id, is_read);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient_occurred
    ON communication.notification (tenant_id, recipient_user_id, occurred_at DESC);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_type
    ON communication.notification (tenant_id, type);

-- Operations UI reads newest logs and commonly filters by trace/correlation id.
CREATE INDEX IF NOT EXISTS ix_application_log_timestamp_utc
    ON observability.application_log (timestamp_utc DESC);
CREATE INDEX IF NOT EXISTS ix_application_log_level_timestamp
    ON observability.application_log (level, timestamp_utc DESC);
CREATE INDEX IF NOT EXISTS ix_application_log_trace_id
    ON observability.application_log (trace_id);
CREATE INDEX IF NOT EXISTS ix_application_log_correlation_id
    ON observability.application_log (correlation_id);

COMMIT;
