IF SCHEMA_ID('Communication') IS NULL EXEC('CREATE SCHEMA Communication');
GO
CREATE TABLE Communication.ChatConversations (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 Title nvarchar(200) NOT NULL, ConversationType varchar(50) NOT NULL,
 CreatedByUserId uniqueidentifier NOT NULL, RelatedEntityId uniqueidentifier NULL,
 RelatedEntityType varchar(100) NULL, IsClosed bit NOT NULL DEFAULT 0, ClosedAt datetimeoffset NULL);
CREATE TABLE Communication.ChatParticipants (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 ConversationId uniqueidentifier NOT NULL, UserId uniqueidentifier NOT NULL,
 Role varchar(50) NOT NULL, JoinedAt datetimeoffset NOT NULL, LastReadAt datetimeoffset NULL,
 IsMuted bit NOT NULL DEFAULT 0,
 CONSTRAINT FK_ChatParticipants_Conversation FOREIGN KEY(ConversationId)
 REFERENCES Communication.ChatConversations(Id) ON DELETE CASCADE,
 CONSTRAINT UQ_ChatParticipant UNIQUE(TenantId,ConversationId,UserId));
CREATE TABLE Communication.ChatMessages (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 ConversationId uniqueidentifier NOT NULL, SenderUserId uniqueidentifier NOT NULL,
 MessageType varchar(30) NOT NULL, Message nvarchar(4000) NOT NULL,
 ReplyToMessageId uniqueidentifier NULL, SentAt datetimeoffset NOT NULL,
 EditedAt datetimeoffset NULL, IsDeleted bit NOT NULL DEFAULT 0,
 CONSTRAINT FK_ChatMessages_Conversation FOREIGN KEY(ConversationId)
 REFERENCES Communication.ChatConversations(Id) ON DELETE CASCADE);
CREATE TABLE Communication.ChatAttachments (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 MessageId uniqueidentifier NOT NULL, FileName nvarchar(255) NOT NULL,
 ContentType varchar(150) NOT NULL, FileSizeBytes bigint NOT NULL,
 StorageKey nvarchar(500) NOT NULL,
 CONSTRAINT FK_ChatAttachments_Message FOREIGN KEY(MessageId)
 REFERENCES Communication.ChatMessages(Id) ON DELETE CASCADE);
CREATE TABLE Communication.Notifications (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 RecipientUserId uniqueidentifier NOT NULL, Type varchar(80) NOT NULL,
 Title nvarchar(200) NOT NULL, Message nvarchar(2000) NOT NULL,
 RelatedEntityId uniqueidentifier NULL, RelatedEntityType varchar(100) NULL,
 ActionUrl nvarchar(500) NULL, Priority varchar(20) NOT NULL,
 IsRead bit NOT NULL DEFAULT 0, ReadAt datetimeoffset NULL, OccurredAt datetimeoffset NOT NULL);
CREATE TABLE Communication.NotificationPreferences (
 Id uniqueidentifier NOT NULL PRIMARY KEY, TenantId uniqueidentifier NOT NULL,
 UserId uniqueidentifier NOT NULL, NotificationType varchar(80) NOT NULL,
 InAppEnabled bit NOT NULL DEFAULT 1, PushEnabled bit NOT NULL DEFAULT 1,
 EmailEnabled bit NOT NULL DEFAULT 0, SmsEnabled bit NOT NULL DEFAULT 0,
 CONSTRAINT UQ_NotificationPreference UNIQUE(TenantId,UserId,NotificationType));
GO
