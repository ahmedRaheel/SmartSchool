using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace SmartSchool.Modules.AICore.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAICore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai_core");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "knowledge_collection",
                schema: "ai_core",
                columns: table => new
                {
                    knowledge_collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    access_scope = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_knowledge_collection", x => x.knowledge_collection_id);
                });

            migrationBuilder.CreateTable(
                name: "model_configuration",
                schema: "ai_core",
                columns: table => new
                {
                    model_configuration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    model_name = table.Column<string>(type: "text", nullable: false),
                    configuration = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_model_configuration", x => x.model_configuration_id);
                });

            migrationBuilder.CreateTable(
                name: "prompt_template",
                schema: "ai_core",
                columns: table => new
                {
                    prompt_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assistant_type = table.Column<string>(type: "text", nullable: false),
                    prompt_type = table.Column<string>(type: "text", nullable: false),
                    prompt_text = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_prompt_template", x => x.prompt_template_id);
                });

            migrationBuilder.CreateTable(
                name: "rag_knowledge_chunk",
                schema: "ai_core",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    collection = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    document_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    embedding_v384 = table.Column<Vector>(type: "vector(384)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rag_knowledge_chunk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_definition",
                schema: "ai_core",
                columns: table => new
                {
                    tool_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    handler_key = table.Column<string>(type: "text", nullable: false),
                    requires_user_authorization = table.Column<bool>(type: "boolean", nullable: false),
                    requires_human_approval = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_tool_definition", x => x.tool_definition_id);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_document",
                schema: "ai_core",
                columns: table => new
                {
                    knowledge_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    knowledge_collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    document_type = table.Column<string>(type: "text", nullable: true),
                    source_url = table.Column<string>(type: "text", nullable: true),
                    metadata = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_knowledge_document", x => x.knowledge_document_id);
                    table.ForeignKey(
                        name: "FK_knowledge_document_knowledge_collection_knowledge_collectio~",
                        column: x => x.knowledge_collection_id,
                        principalSchema: "ai_core",
                        principalTable: "knowledge_collection",
                        principalColumn: "knowledge_collection_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ai_execution_log",
                schema: "ai_core",
                columns: table => new
                {
                    ai_execution_log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assistant_type = table.Column<string>(type: "text", nullable: false),
                    conversation_reference_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    model_configuration_id = table.Column<Guid>(type: "uuid", nullable: true),
                    prompt_tokens = table.Column<int>(type: "integer", nullable: true),
                    completion_tokens = table.Column<int>(type: "integer", nullable: true),
                    total_tokens = table.Column<int>(type: "integer", nullable: true),
                    estimated_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    latency_ms = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_ai_execution_log", x => x.ai_execution_log_id);
                    table.ForeignKey(
                        name: "FK_ai_execution_log_model_configuration_model_configuration_id",
                        column: x => x.model_configuration_id,
                        principalSchema: "ai_core",
                        principalTable: "model_configuration",
                        principalColumn: "model_configuration_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_chunk",
                schema: "ai_core",
                columns: table => new
                {
                    knowledge_chunk_id = table.Column<Guid>(type: "uuid", nullable: false),
                    knowledge_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chunk_index = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "text", nullable: true),
                    embedding_reference = table.Column<string>(type: "text", nullable: true),
                    embedding = table.Column<Vector>(type: "vector(384)", nullable: true),
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
                    table.PrimaryKey("PK_knowledge_chunk", x => x.knowledge_chunk_id);
                    table.ForeignKey(
                        name: "FK_knowledge_chunk_knowledge_document_knowledge_document_id",
                        column: x => x.knowledge_document_id,
                        principalSchema: "ai_core",
                        principalTable: "knowledge_document",
                        principalColumn: "knowledge_document_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_execution_log_model_configuration_id",
                schema: "ai_core",
                table: "ai_execution_log",
                column: "model_configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_execution_log_tenant_id",
                schema: "ai_core",
                table: "ai_execution_log",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_execution_log_tenant_id_code",
                schema: "ai_core",
                table: "ai_execution_log",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_chunk_knowledge_document_id",
                schema: "ai_core",
                table: "knowledge_chunk",
                column: "knowledge_document_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_chunk_tenant_id",
                schema: "ai_core",
                table: "knowledge_chunk",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_chunk_tenant_id_code",
                schema: "ai_core",
                table: "knowledge_chunk",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_collection_tenant_id",
                schema: "ai_core",
                table: "knowledge_collection",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_collection_tenant_id_code",
                schema: "ai_core",
                table: "knowledge_collection",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_document_knowledge_collection_id",
                schema: "ai_core",
                table: "knowledge_document",
                column: "knowledge_collection_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_document_tenant_id",
                schema: "ai_core",
                table: "knowledge_document",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_document_tenant_id_code",
                schema: "ai_core",
                table: "knowledge_document",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_model_configuration_tenant_id",
                schema: "ai_core",
                table: "model_configuration",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_configuration_tenant_id_code",
                schema: "ai_core",
                table: "model_configuration",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prompt_template_tenant_id",
                schema: "ai_core",
                table: "prompt_template",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_prompt_template_tenant_id_code",
                schema: "ai_core",
                table: "prompt_template",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tool_definition_tenant_id",
                schema: "ai_core",
                table: "tool_definition",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_definition_tenant_id_code",
                schema: "ai_core",
                table: "tool_definition",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_execution_log",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "knowledge_chunk",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "prompt_template",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "rag_knowledge_chunk",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "tool_definition",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "model_configuration",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "knowledge_document",
                schema: "ai_core");

            migrationBuilder.DropTable(
                name: "knowledge_collection",
                schema: "ai_core");
        }
    }
}
