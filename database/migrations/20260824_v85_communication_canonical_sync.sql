BEGIN;

CREATE SCHEMA IF NOT EXISTS communication;

-- Canonical generated CRUD tables: align entity-specific keys, tenant scope and lifecycle fields.
ALTER TABLE communication.conversation
    ADD COLUMN IF NOT EXISTS code varchar(100),
    ADD COLUMN IF NOT EXISTS name varchar(250),
    ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE communication.conversation SET code = COALESCE(code, conversation_id::text), name = COALESCE(name, title, 'Conversation') WHERE code IS NULL OR name IS NULL;
ALTER TABLE communication.conversation ALTER COLUMN code SET NOT NULL, ALTER COLUMN name SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_conversation_tenant_code ON communication.conversation(tenant_id, code);

ALTER TABLE communication.conversation_participant
    ADD COLUMN IF NOT EXISTS conversation_participant_id uuid DEFAULT gen_random_uuid(),
    ADD COLUMN IF NOT EXISTS tenant_id uuid,
    ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true NOT NULL,
    ADD COLUMN IF NOT EXISTS created_at timestamptz DEFAULT now() NOT NULL,
    ADD COLUMN IF NOT EXISTS updated_at timestamptz,
    ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    ADD COLUMN IF NOT EXISTS code varchar(100),
    ADD COLUMN IF NOT EXISTS name varchar(250),
    ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE communication.conversation_participant cp
SET tenant_id = c.tenant_id
FROM communication.conversation c
WHERE cp.conversation_id = c.conversation_id AND cp.tenant_id IS NULL;
UPDATE communication.conversation_participant SET code = COALESCE(code, conversation_participant_id::text), name = COALESCE(name, 'Participant') WHERE code IS NULL OR name IS NULL;
ALTER TABLE communication.conversation_participant ALTER COLUMN conversation_participant_id SET NOT NULL, ALTER COLUMN tenant_id SET NOT NULL, ALTER COLUMN code SET NOT NULL, ALTER COLUMN name SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_conversation_participant_id ON communication.conversation_participant(conversation_participant_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_conversation_participant_tenant_code ON communication.conversation_participant(tenant_id, code);

ALTER TABLE communication.message
    ADD COLUMN IF NOT EXISTS tenant_id uuid,
    ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true NOT NULL,
    ADD COLUMN IF NOT EXISTS created_at timestamptz DEFAULT now() NOT NULL,
    ADD COLUMN IF NOT EXISTS updated_at timestamptz,
    ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    ADD COLUMN IF NOT EXISTS code varchar(100),
    ADD COLUMN IF NOT EXISTS name varchar(250),
    ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE communication.message m SET tenant_id = c.tenant_id FROM communication.conversation c WHERE m.conversation_id = c.conversation_id AND m.tenant_id IS NULL;
UPDATE communication.message SET code = COALESCE(code, message_id::text), name = COALESCE(name, left(COALESCE(body, 'Message'), 250)) WHERE code IS NULL OR name IS NULL;
ALTER TABLE communication.message ALTER COLUMN tenant_id SET NOT NULL, ALTER COLUMN code SET NOT NULL, ALTER COLUMN name SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_message_tenant_code ON communication.message(tenant_id, code);

ALTER TABLE communication.message_receipt
    ADD COLUMN IF NOT EXISTS message_receipt_id uuid DEFAULT gen_random_uuid(),
    ADD COLUMN IF NOT EXISTS tenant_id uuid,
    ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT true NOT NULL,
    ADD COLUMN IF NOT EXISTS created_at timestamptz DEFAULT now() NOT NULL,
    ADD COLUMN IF NOT EXISTS updated_at timestamptz,
    ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    ADD COLUMN IF NOT EXISTS code varchar(100),
    ADD COLUMN IF NOT EXISTS name varchar(250),
    ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE communication.message_receipt r SET tenant_id = m.tenant_id FROM communication.message m WHERE r.message_id = m.message_id AND r.tenant_id IS NULL;
UPDATE communication.message_receipt SET code = COALESCE(code, message_receipt_id::text), name = COALESCE(name, 'Message receipt') WHERE code IS NULL OR name IS NULL;
ALTER TABLE communication.message_receipt ALTER COLUMN message_receipt_id SET NOT NULL, ALTER COLUMN tenant_id SET NOT NULL, ALTER COLUMN code SET NOT NULL, ALTER COLUMN name SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_message_receipt_id ON communication.message_receipt(message_receipt_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_message_receipt_tenant_code ON communication.message_receipt(tenant_id, code);

-- Notification: retain delivery fields while adding the richer application notification contract.
ALTER TABLE communication.notification
    ADD COLUMN IF NOT EXISTS recipient_user_id uuid,
    ADD COLUMN IF NOT EXISTS type varchar(80),
    ADD COLUMN IF NOT EXISTS message text,
    ADD COLUMN IF NOT EXISTS related_entity_id uuid,
    ADD COLUMN IF NOT EXISTS related_entity_type varchar(100),
    ADD COLUMN IF NOT EXISTS action_url varchar(500),
    ADD COLUMN IF NOT EXISTS priority varchar(20) DEFAULT 'Normal' NOT NULL,
    ADD COLUMN IF NOT EXISTS is_read boolean DEFAULT false NOT NULL,
    ADD COLUMN IF NOT EXISTS read_at timestamptz,
    ADD COLUMN IF NOT EXISTS occurred_at timestamptz;
UPDATE communication.notification
SET recipient_user_id = COALESCE(recipient_user_id, user_id),
    type = COALESCE(type, channel_code, 'General'),
    message = COALESCE(message, body, ''),
    occurred_at = COALESCE(occurred_at, created_at);
ALTER TABLE communication.notification ALTER COLUMN recipient_user_id SET NOT NULL, ALTER COLUMN type SET NOT NULL, ALTER COLUMN occurred_at SET NOT NULL;
CREATE INDEX IF NOT EXISTS ix_notification_recipient_unread ON communication.notification(tenant_id, recipient_user_id, is_read, occurred_at DESC);

-- Canonical chat tables use snake_case and entity-specific primary keys.
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'Id')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'chat_conversation_id') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "Id" TO chat_conversation_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'TenantId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'tenant_id') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "TenantId" TO tenant_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'Title')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'title') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "Title" TO title;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'ConversationType')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'conversation_type') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "ConversationType" TO conversation_type;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'CreatedByUserId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'created_by_user_id') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "CreatedByUserId" TO created_by_user_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'RelatedEntityId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'related_entity_id') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "RelatedEntityId" TO related_entity_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'RelatedEntityType')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'related_entity_type') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "RelatedEntityType" TO related_entity_type;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'IsClosed')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'is_closed') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "IsClosed" TO is_closed;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'ClosedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'closed_at') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "ClosedAt" TO closed_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'IsActive')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'is_active') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "IsActive" TO is_active;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'CreatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'created_at') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'UpdatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'updated_at') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'RowVersion')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_conversation' AND column_name = 'row_version') THEN
        ALTER TABLE communication.chat_conversation RENAME COLUMN "RowVersion" TO row_version;
    END IF;
END $$;

DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'Id')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'chat_message_id') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "Id" TO chat_message_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'TenantId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'tenant_id') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "TenantId" TO tenant_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'ConversationId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'conversation_id') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "ConversationId" TO conversation_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'SenderUserId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'sender_user_id') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "SenderUserId" TO sender_user_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'MessageType')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'message_type') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "MessageType" TO message_type;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'Message')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'message') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "Message" TO message;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'ReplyToMessageId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'reply_to_message_id') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "ReplyToMessageId" TO reply_to_message_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'SentAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'sent_at') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "SentAt" TO sent_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'EditedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'edited_at') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "EditedAt" TO edited_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'IsDeleted')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'is_deleted') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "IsDeleted" TO is_deleted;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'IsActive')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'is_active') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "IsActive" TO is_active;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'CreatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'created_at') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'UpdatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'updated_at') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'RowVersion')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_message' AND column_name = 'row_version') THEN
        ALTER TABLE communication.chat_message RENAME COLUMN "RowVersion" TO row_version;
    END IF;
END $$;

DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'Id')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'chat_participant_id') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "Id" TO chat_participant_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'TenantId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'tenant_id') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "TenantId" TO tenant_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'ConversationId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'conversation_id') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "ConversationId" TO conversation_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'UserId')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'user_id') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "UserId" TO user_id;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'Role')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'role') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "Role" TO role;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'JoinedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'joined_at') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "JoinedAt" TO joined_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'LastReadAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'last_read_at') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "LastReadAt" TO last_read_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'IsMuted')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'is_muted') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "IsMuted" TO is_muted;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'IsActive')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'is_active') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "IsActive" TO is_active;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'CreatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'created_at') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "CreatedAt" TO created_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'UpdatedAt')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'updated_at') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "UpdatedAt" TO updated_at;
    END IF;
END $$;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'RowVersion')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'communication' AND table_name = 'chat_participant' AND column_name = 'row_version') THEN
        ALTER TABLE communication.chat_participant RENAME COLUMN "RowVersion" TO row_version;
    END IF;
END $$;

CREATE TABLE IF NOT EXISTS communication.chat_attachment (
    chat_attachment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    message_id uuid NOT NULL,
    file_name varchar(255) NOT NULL,
    content_type varchar(150) NOT NULL,
    file_size_bytes bigint NOT NULL,
    storage_key varchar(500) NOT NULL,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);

CREATE TABLE IF NOT EXISTS communication.notification_preference (
    notification_preference_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL,
    user_id uuid NOT NULL,
    notification_type varchar(80) NOT NULL,
    in_app_enabled boolean NOT NULL DEFAULT true,
    push_enabled boolean NOT NULL DEFAULT true,
    email_enabled boolean NOT NULL DEFAULT false,
    sms_enabled boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_notification_preference_user_type ON communication.notification_preference(tenant_id, user_id, notification_type);

COMMIT;
