using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.AIParent.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAIParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai_core");

            migrationBuilder.CreateTable(
                name: "parent_conversation",
                schema: "ai_core",
                columns: table => new
                {
                    parent_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guardian_id = table.Column<Guid>(type: "uuid", nullable: false),
                    selected_student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_parent_conversation", x => x.parent_conversation_id);
                });

            migrationBuilder.CreateTable(
                name: "parent_message",
                schema: "ai_core",
                columns: table => new
                {
                    parent_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_parent_message", x => x.parent_message_id);
                    table.ForeignKey(
                        name: "FK_parent_message_parent_conversation_parent_conversation_id",
                        column: x => x.parent_conversation_id,
                        principalSchema: "ai_core",
                        principalTable: "parent_conversation",
                        principalColumn: "parent_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "parent_tool_execution",
                schema: "ai_core",
                columns: table => new
                {
                    parent_tool_execution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tool_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    input_payload = table.Column<string>(type: "text", nullable: true),
                    output_payload = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    executed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_parent_tool_execution", x => x.parent_tool_execution_id);
                    table.ForeignKey(
                        name: "FK_parent_tool_execution_parent_conversation_parent_conversati~",
                        column: x => x.parent_conversation_id,
                        principalSchema: "ai_core",
                        principalTable: "parent_conversation",
                        principalColumn: "parent_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parent_conversation_tenant_id",
                schema: "ai_core",
                table: "parent_conversation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_conversation_tenant_id_code",
                schema: "ai_core",
                table: "parent_conversation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parent_message_parent_conversation_id",
                schema: "ai_core",
                table: "parent_message",
                column: "parent_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_message_tenant_id",
                schema: "ai_core",
                table: "parent_message",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_message_tenant_id_code",
                schema: "ai_core",
                table: "parent_message",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parent_tool_execution_parent_conversation_id",
                schema: "ai_core",
                table: "parent_tool_execution",
                column: "parent_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_tool_execution_tenant_id",
                schema: "ai_core",
                table: "parent_tool_execution",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_tool_execution_tenant_id_code",
                schema: "ai_core",
                table: "parent_tool_execution",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parent_message",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "parent_tool_execution",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "parent_conversation",
                schema: "ai_core");
        }
    }
}
