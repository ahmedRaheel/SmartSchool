using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.AITutor.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAITutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai_tutor");

            migrationBuilder.CreateTable(
                name: "learning_recommendation",
                schema: "ai_tutor",
                columns: table => new
                {
                    learning_recommendation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    topic = table.Column<string>(type: "text", nullable: true),
                    recommendation_type = table.Column<string>(type: "text", nullable: false),
                    recommendation_text = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_learning_recommendation", x => x.learning_recommendation_id);
                });

            migrationBuilder.CreateTable(
                name: "student_topic_mastery",
                schema: "ai_tutor",
                columns: table => new
                {
                    student_topic_mastery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic = table.Column<string>(type: "text", nullable: false),
                    mastery_score = table.Column<decimal>(type: "numeric", nullable: true),
                    confidence_score = table.Column<decimal>(type: "numeric", nullable: true),
                    evidence_count = table.Column<int>(type: "integer", nullable: false),
                    last_assessed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_student_topic_mastery", x => x.student_topic_mastery_id);
                });

            migrationBuilder.CreateTable(
                name: "tutor_conversation",
                schema: "ai_tutor",
                columns: table => new
                {
                    tutor_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: true),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_tutor_conversation", x => x.tutor_conversation_id);
                });

            migrationBuilder.CreateTable(
                name: "generated_quiz",
                schema: "ai_tutor",
                columns: table => new
                {
                    generated_quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_conversation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    topic = table.Column<string>(type: "text", nullable: true),
                    difficulty = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_generated_quiz", x => x.generated_quiz_id);
                    table.ForeignKey(
                        name: "FK_generated_quiz_tutor_conversation_tutor_conversation_id",
                        column: x => x.tutor_conversation_id,
                        principalSchema: "ai_tutor",
                        principalTable: "tutor_conversation",
                        principalColumn: "tutor_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tutor_message",
                schema: "ai_tutor",
                columns: table => new
                {
                    tutor_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_tutor_message", x => x.tutor_message_id);
                    table.ForeignKey(
                        name: "FK_tutor_message_tutor_conversation_tutor_conversation_id",
                        column: x => x.tutor_conversation_id,
                        principalSchema: "ai_tutor",
                        principalTable: "tutor_conversation",
                        principalColumn: "tutor_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tutor_session",
                schema: "ai_tutor",
                columns: table => new
                {
                    tutor_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic = table.Column<string>(type: "text", nullable: true),
                    learning_objective = table.Column<string>(type: "text", nullable: true),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    session_summary = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_tutor_session", x => x.tutor_session_id);
                    table.ForeignKey(
                        name: "FK_tutor_session_tutor_conversation_tutor_conversation_id",
                        column: x => x.tutor_conversation_id,
                        principalSchema: "ai_tutor",
                        principalTable: "tutor_conversation",
                        principalColumn: "tutor_conversation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_quiz_attempt",
                schema: "ai_tutor",
                columns: table => new
                {
                    student_quiz_attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    generated_quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    score = table.Column<decimal>(type: "numeric", nullable: true),
                    answers = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_student_quiz_attempt", x => x.student_quiz_attempt_id);
                    table.ForeignKey(
                        name: "FK_student_quiz_attempt_generated_quiz_generated_quiz_id",
                        column: x => x.generated_quiz_id,
                        principalSchema: "ai_tutor",
                        principalTable: "generated_quiz",
                        principalColumn: "generated_quiz_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_generated_quiz_tenant_id",
                schema: "ai_tutor",
                table: "generated_quiz",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_generated_quiz_tenant_id_code",
                schema: "ai_tutor",
                table: "generated_quiz",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_generated_quiz_tutor_conversation_id",
                schema: "ai_tutor",
                table: "generated_quiz",
                column: "tutor_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_learning_recommendation_tenant_id",
                schema: "ai_tutor",
                table: "learning_recommendation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_learning_recommendation_tenant_id_code",
                schema: "ai_tutor",
                table: "learning_recommendation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_quiz_attempt_generated_quiz_id",
                schema: "ai_tutor",
                table: "student_quiz_attempt",
                column: "generated_quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_quiz_attempt_tenant_id",
                schema: "ai_tutor",
                table: "student_quiz_attempt",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_quiz_attempt_tenant_id_code",
                schema: "ai_tutor",
                table: "student_quiz_attempt",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_topic_mastery_tenant_id",
                schema: "ai_tutor",
                table: "student_topic_mastery",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_topic_mastery_tenant_id_code",
                schema: "ai_tutor",
                table: "student_topic_mastery",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_conversation_tenant_id",
                schema: "ai_tutor",
                table: "tutor_conversation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_conversation_tenant_id_code",
                schema: "ai_tutor",
                table: "tutor_conversation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_message_tenant_id",
                schema: "ai_tutor",
                table: "tutor_message",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_message_tenant_id_code",
                schema: "ai_tutor",
                table: "tutor_message",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_message_tutor_conversation_id",
                schema: "ai_tutor",
                table: "tutor_message",
                column: "tutor_conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_session_tenant_id",
                schema: "ai_tutor",
                table: "tutor_session",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_session_tenant_id_code",
                schema: "ai_tutor",
                table: "tutor_session",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_session_tutor_conversation_id",
                schema: "ai_tutor",
                table: "tutor_session",
                column: "tutor_conversation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "learning_recommendation",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "student_quiz_attempt",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "student_topic_mastery",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "tutor_message",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "tutor_session",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "generated_quiz",
                schema: "ai_tutor");

            migrationBuilder.DropTable(
                name: "tutor_conversation",
                schema: "ai_tutor");
        }
    }
}
