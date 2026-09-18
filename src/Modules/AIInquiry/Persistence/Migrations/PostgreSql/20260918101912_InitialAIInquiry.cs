using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.AIInquiry.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAIInquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai_core");

            migrationBuilder.CreateTable(
                name: "inquiry_conversation",
                schema: "ai_core",
                columns: table => new
                {
                    inquiry_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    visitor_session_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    visitor_name = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    interested_program_id = table.Column<Guid>(type: "uuid", nullable: true),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_inquiry_conversation", x => x.inquiry_conversation_id);
                });

            migrationBuilder.CreateTable(
                name: "human_handoff",
                schema: "ai_core",
                columns: table => new
                {
                    human_handoff_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inquiry_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    assigned_to_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    accepted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    resolved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_human_handoff", x => x.human_handoff_id);
                    table.ForeignKey(
                        name: "FK_human_handoff_inquiry_conversation_inquiry_conversation_id",
                        column: x => x.inquiry_conversation_id,
                        principalSchema: "ai_core",
                        principalTable: "inquiry_conversation",
                        principalColumn: "inquiry_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inquiry_message",
                schema: "ai_core",
                columns: table => new
                {
                    inquiry_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inquiry_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_inquiry_message", x => x.inquiry_message_id);
                    table.ForeignKey(
                        name: "FK_inquiry_message_inquiry_conversation_inquiry_conversation_id",
                        column: x => x.inquiry_conversation_id,
                        principalSchema: "ai_core",
                        principalTable: "inquiry_conversation",
                        principalColumn: "inquiry_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lead_capture",
                schema: "ai_core",
                columns: table => new
                {
                    lead_capture_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inquiry_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    interested_campus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    interested_program_id = table.Column<Guid>(type: "uuid", nullable: true),
                    interested_grade_id = table.Column<Guid>(type: "uuid", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    captured_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    converted_inquiry_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_lead_capture", x => x.lead_capture_id);
                    table.ForeignKey(
                        name: "FK_lead_capture_inquiry_conversation_inquiry_conversation_id",
                        column: x => x.inquiry_conversation_id,
                        principalSchema: "ai_core",
                        principalTable: "inquiry_conversation",
                        principalColumn: "inquiry_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_human_handoff_inquiry_conversation_id",
                schema: "ai_core",
                table: "human_handoff",
                column: "inquiry_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_human_handoff_tenant_id",
                schema: "ai_core",
                table: "human_handoff",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_human_handoff_tenant_id_code",
                schema: "ai_core",
                table: "human_handoff",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_conversation_tenant_id",
                schema: "ai_core",
                table: "inquiry_conversation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_conversation_tenant_id_code",
                schema: "ai_core",
                table: "inquiry_conversation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_message_inquiry_conversation_id",
                schema: "ai_core",
                table: "inquiry_message",
                column: "inquiry_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_message_tenant_id",
                schema: "ai_core",
                table: "inquiry_message",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_message_tenant_id_code",
                schema: "ai_core",
                table: "inquiry_message",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lead_capture_inquiry_conversation_id",
                schema: "ai_core",
                table: "lead_capture",
                column: "inquiry_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_lead_capture_tenant_id",
                schema: "ai_core",
                table: "lead_capture",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_lead_capture_tenant_id_code",
                schema: "ai_core",
                table: "lead_capture",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "human_handoff",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "inquiry_message",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "lead_capture",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "inquiry_conversation",
                schema: "ai_core");
        }
    }
}
