using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Learning.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialLearning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "lms");

            migrationBuilder.CreateTable(
                name: "academic_assignment",
                schema: "lms",
                columns: table => new
                {
                    academic_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teaching_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assignment_type_code = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    instructions = table.Column<string>(type: "text", nullable: true),
                    assigned_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    total_marks = table.Column<decimal>(type: "numeric", nullable: true),
                    allow_late_submission = table.Column<bool>(type: "boolean", nullable: false),
                    max_attempts = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_academic_assignment", x => x.academic_assignment_id);
                });

            migrationBuilder.CreateTable(
                name: "learningresource",
                schema: "lms",
                columns: table => new
                {
                    learning_resource_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_learningresource", x => x.learning_resource_id);
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                schema: "lms",
                columns: table => new
                {
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_lesson", x => x.lesson_id);
                });

            migrationBuilder.CreateTable(
                name: "assignment_student",
                schema: "lms",
                columns: table => new
                {
                    assignment_student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment_student", x => x.assignment_student_id);
                    table.ForeignKey(
                        name: "FK_assignment_student_academic_assignment_academic_assignment_~",
                        column: x => x.academic_assignment_id,
                        principalSchema: "lms",
                        principalTable: "academic_assignment",
                        principalColumn: "academic_assignment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_assignment_submission",
                schema: "lms",
                columns: table => new
                {
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_no = table.Column<int>(type: "integer", nullable: false),
                    submitted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    submission_text = table.Column<string>(type: "text", nullable: true),
                    marks_obtained = table.Column<decimal>(type: "numeric", nullable: true),
                    teacher_feedback = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    file_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    content_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    file_content = table.Column<byte[]>(type: "bytea", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_assignment_submission", x => x.submission_id);
                    table.ForeignKey(
                        name: "FK_student_assignment_submission_academic_assignment_academic_~",
                        column: x => x.academic_assignment_id,
                        principalSchema: "lms",
                        principalTable: "academic_assignment",
                        principalColumn: "academic_assignment_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_academic_assignment_tenant_id",
                schema: "lms",
                table: "academic_assignment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_assignment_tenant_id_code",
                schema: "lms",
                table: "academic_assignment",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignment_student_academic_assignment_id",
                schema: "lms",
                table: "assignment_student",
                column: "academic_assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_student_tenant_id_academic_assignment_id_student~",
                schema: "lms",
                table: "assignment_student",
                columns: new[] { "tenant_id", "academic_assignment_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_learningresource_tenant_id",
                schema: "lms",
                table: "learningresource",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_learningresource_tenant_id_code",
                schema: "lms",
                table: "learningresource",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_tenant_id",
                schema: "lms",
                table: "lesson",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_tenant_id_code",
                schema: "lms",
                table: "lesson",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_submission_academic_assignment_id",
                schema: "lms",
                table: "student_assignment_submission",
                column: "academic_assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_submission_tenant_id",
                schema: "lms",
                table: "student_assignment_submission",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_submission_tenant_id_academic_assignment~",
                schema: "lms",
                table: "student_assignment_submission",
                columns: new[] { "tenant_id", "academic_assignment_id", "student_id", "attempt_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_submission_tenant_id_code",
                schema: "lms",
                table: "student_assignment_submission",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignment_student",
                schema: "lms");

            migrationBuilder.DropTable(
                name: "learningresource",
                schema: "lms");

            migrationBuilder.DropTable(
                name: "lesson",
                schema: "lms");

            migrationBuilder.DropTable(
                name: "student_assignment_submission",
                schema: "lms");

            migrationBuilder.DropTable(
                name: "academic_assignment",
                schema: "lms");
        }
    }
}
