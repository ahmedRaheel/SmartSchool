using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Communication.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialCommunication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "communication");

            migrationBuilder.CreateTable(
                name: "chat_conversation",
                schema: "communication",
                columns: table => new
                {
                    chat_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    conversation_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    related_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false),
                    closed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_conversation", x => x.chat_conversation_id);
                });

            migrationBuilder.CreateTable(
                name: "conversation",
                schema: "communication",
                columns: table => new
                {
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    conversation_type_code = table.Column<string>(type: "text", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversation", x => x.conversation_id);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                schema: "communication",
                columns: table => new
                {
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipient_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    related_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    action_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.notification_id);
                });

            migrationBuilder.CreateTable(
                name: "notification_preference",
                schema: "communication",
                columns: table => new
                {
                    notification_preference_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    in_app_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    push_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    email_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    sms_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_preference", x => x.notification_preference_id);
                });

            migrationBuilder.CreateTable(
                name: "chat_message",
                schema: "communication",
                columns: table => new
                {
                    chat_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    message = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    reply_to_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    edited_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_message", x => x.chat_message_id);
                    table.ForeignKey(
                        name: "FK_chat_message_chat_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "communication",
                        principalTable: "chat_conversation",
                        principalColumn: "chat_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "chat_participant",
                schema: "communication",
                columns: table => new
                {
                    chat_participant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    joined_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_muted = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_participant", x => x.chat_participant_id);
                    table.ForeignKey(
                        name: "FK_chat_participant_chat_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "communication",
                        principalTable: "chat_conversation",
                        principalColumn: "chat_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "conversation_participant",
                schema: "communication",
                columns: table => new
                {
                    conversation_participant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    joined_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    left_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversation_participant", x => x.conversation_participant_id);
                    table.ForeignKey(
                        name: "FK_conversation_participant_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "communication",
                        principalTable: "conversation",
                        principalColumn: "conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "message",
                schema: "communication",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reply_to_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    message_type_code = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: true),
                    sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    edited_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_message_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "communication",
                        principalTable: "conversation",
                        principalColumn: "conversation_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_message_message_reply_to_message_id",
                        column: x => x.reply_to_message_id,
                        principalSchema: "communication",
                        principalTable: "message",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "chat_attachment",
                schema: "communication",
                columns: table => new
                {
                    chat_attachment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content_type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    file_size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    storage_key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_attachment", x => x.chat_attachment_id);
                    table.ForeignKey(
                        name: "FK_chat_attachment_chat_message_message_id",
                        column: x => x.message_id,
                        principalSchema: "communication",
                        principalTable: "chat_message",
                        principalColumn: "chat_message_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "message_receipt",
                schema: "communication",
                columns: table => new
                {
                    message_receipt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_receipt", x => x.message_receipt_id);
                    table.ForeignKey(
                        name: "FK_message_receipt_message_message_id",
                        column: x => x.message_id,
                        principalSchema: "communication",
                        principalTable: "message",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_attachment_message_id",
                schema: "communication",
                table: "chat_attachment",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_attachment_tenant_id_message_id",
                schema: "communication",
                table: "chat_attachment",
                columns: new[] { "tenant_id", "message_id" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_conversation_tenant_id_created_by_user_id",
                schema: "communication",
                table: "chat_conversation",
                columns: new[] { "tenant_id", "created_by_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_conversation_id",
                schema: "communication",
                table: "chat_message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_tenant_id_conversation_id_sent_at",
                schema: "communication",
                table: "chat_message",
                columns: new[] { "tenant_id", "conversation_id", "sent_at" });

            migrationBuilder.CreateIndex(
                name: "IX_chat_participant_conversation_id",
                schema: "communication",
                table: "chat_participant",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_participant_tenant_id_conversation_id_user_id",
                schema: "communication",
                table: "chat_participant",
                columns: new[] { "tenant_id", "conversation_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_conversation_tenant_id",
                schema: "communication",
                table: "conversation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_tenant_id_code",
                schema: "communication",
                table: "conversation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_conversation_participant_conversation_id",
                schema: "communication",
                table: "conversation_participant",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_participant_tenant_id",
                schema: "communication",
                table: "conversation_participant",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_participant_tenant_id_code",
                schema: "communication",
                table: "conversation_participant",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_conversation_id",
                schema: "communication",
                table: "message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_reply_to_message_id",
                schema: "communication",
                table: "message",
                column: "reply_to_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_tenant_id",
                schema: "communication",
                table: "message",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_tenant_id_code",
                schema: "communication",
                table: "message",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_receipt_message_id",
                schema: "communication",
                table: "message_receipt",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_receipt_tenant_id",
                schema: "communication",
                table: "message_receipt",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_receipt_tenant_id_code",
                schema: "communication",
                table: "message_receipt",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id",
                schema: "communication",
                table: "notification",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id_recipient_user_id",
                schema: "communication",
                table: "notification",
                columns: new[] { "tenant_id", "recipient_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id_recipient_user_id_is_read",
                schema: "communication",
                table: "notification",
                columns: new[] { "tenant_id", "recipient_user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id_recipient_user_id_is_read_occurred_at",
                schema: "communication",
                table: "notification",
                columns: new[] { "tenant_id", "recipient_user_id", "is_read", "occurred_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id_recipient_user_id_occurred_at",
                schema: "communication",
                table: "notification",
                columns: new[] { "tenant_id", "recipient_user_id", "occurred_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_tenant_id_type",
                schema: "communication",
                table: "notification",
                columns: new[] { "tenant_id", "type" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_preference_tenant_id_user_id_notification_type",
                schema: "communication",
                table: "notification_preference",
                columns: new[] { "tenant_id", "user_id", "notification_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_attachment",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "chat_participant",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "conversation_participant",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "message_receipt",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "notification",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "notification_preference",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "chat_message",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "message",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "chat_conversation",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "conversation",
                schema: "communication");
        }
    }
}
