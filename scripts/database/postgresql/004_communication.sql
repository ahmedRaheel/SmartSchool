CREATE SCHEMA IF NOT EXISTS "Communication";

CREATE TABLE IF NOT EXISTS "Communication"."ChatConversations" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "Title" varchar(200) NOT NULL,
    "ConversationType" varchar(50) NOT NULL, "CreatedByUserId" uuid NOT NULL,
    "RelatedEntityId" uuid NULL, "RelatedEntityType" varchar(100) NULL,
    "IsClosed" boolean NOT NULL DEFAULT false, "ClosedAt" timestamptz NULL
);
CREATE TABLE IF NOT EXISTS "Communication"."ChatParticipants" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "ConversationId" uuid NOT NULL,
    "UserId" uuid NOT NULL, "Role" varchar(50) NOT NULL, "JoinedAt" timestamptz NOT NULL,
    "LastReadAt" timestamptz NULL, "IsMuted" boolean NOT NULL DEFAULT false,
    CONSTRAINT "FK_ChatParticipants_Conversation" FOREIGN KEY ("ConversationId")
      REFERENCES "Communication"."ChatConversations"("Id") ON DELETE CASCADE,
    CONSTRAINT "UQ_ChatParticipant" UNIQUE ("TenantId","ConversationId","UserId")
);
CREATE TABLE IF NOT EXISTS "Communication"."ChatMessages" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "ConversationId" uuid NOT NULL,
    "SenderUserId" uuid NOT NULL, "MessageType" varchar(30) NOT NULL,
    "Message" varchar(4000) NOT NULL, "ReplyToMessageId" uuid NULL,
    "SentAt" timestamptz NOT NULL, "EditedAt" timestamptz NULL,
    "IsDeleted" boolean NOT NULL DEFAULT false,
    CONSTRAINT "FK_ChatMessages_Conversation" FOREIGN KEY ("ConversationId")
      REFERENCES "Communication"."ChatConversations"("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Communication"."ChatAttachments" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "MessageId" uuid NOT NULL,
    "FileName" varchar(255) NOT NULL, "ContentType" varchar(150) NOT NULL,
    "FileSizeBytes" bigint NOT NULL, "StorageKey" varchar(500) NOT NULL,
    CONSTRAINT "FK_ChatAttachments_Message" FOREIGN KEY ("MessageId")
      REFERENCES "Communication"."ChatMessages"("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Communication"."Notifications" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "RecipientUserId" uuid NOT NULL,
    "Type" varchar(80) NOT NULL, "Title" varchar(200) NOT NULL,
    "Message" varchar(2000) NOT NULL, "RelatedEntityId" uuid NULL,
    "RelatedEntityType" varchar(100) NULL, "ActionUrl" varchar(500) NULL,
    "Priority" varchar(20) NOT NULL, "IsRead" boolean NOT NULL DEFAULT false,
    "ReadAt" timestamptz NULL, "OccurredAt" timestamptz NOT NULL
);
CREATE TABLE IF NOT EXISTS "Communication"."NotificationPreferences" (
    "Id" uuid PRIMARY KEY, "TenantId" uuid NOT NULL, "UserId" uuid NOT NULL,
    "NotificationType" varchar(80) NOT NULL, "InAppEnabled" boolean NOT NULL DEFAULT true,
    "PushEnabled" boolean NOT NULL DEFAULT true, "EmailEnabled" boolean NOT NULL DEFAULT false,
    "SmsEnabled" boolean NOT NULL DEFAULT false,
    CONSTRAINT "UQ_NotificationPreference" UNIQUE ("TenantId","UserId","NotificationType")
);
