using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Examinations.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialExaminations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exam");

            migrationBuilder.CreateTable(
                name: "exam",
                schema: "exam",
                columns: table => new
                {
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: true),
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_type_code = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    result_publish_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam", x => x.exam_id);
                });

            migrationBuilder.CreateTable(
                name: "grade_scale",
                schema: "exam",
                columns: table => new
                {
                    grade_scale_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    minimum_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    maximum_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    grade_point = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_grade_scale", x => x.grade_scale_id);
                });

            migrationBuilder.CreateTable(
                name: "exam_subject",
                schema: "exam",
                columns: table => new
                {
                    exam_subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_date = table.Column<DateOnly>(type: "date", nullable: true),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    total_marks = table.Column<decimal>(type: "numeric", nullable: false),
                    passing_marks = table.Column<decimal>(type: "numeric", nullable: true),
                    room_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_exam_subject", x => x.exam_subject_id);
                    table.ForeignKey(
                        name: "FK_exam_subject_exam_exam_id",
                        column: x => x.exam_id,
                        principalSchema: "exam",
                        principalTable: "exam",
                        principalColumn: "exam_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exam_task",
                schema: "exam",
                columns: table => new
                {
                    exam_task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_course_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    instructions = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    assigned_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    submitted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completed_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    submission_notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    submission_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    submission_content_type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    submission_file_data = table.Column<byte[]>(type: "bytea", nullable: true),
                    assignment_notified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    reminder_24_hours_sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    reminder_2_hours_sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    overdue_reminder_sent_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_task", x => x.exam_task_id);
                    table.ForeignKey(
                        name: "FK_exam_task_exam_exam_id",
                        column: x => x.exam_id,
                        principalSchema: "exam",
                        principalTable: "exam",
                        principalColumn: "exam_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exam_task_exam_subject_exam_subject_id",
                        column: x => x.exam_subject_id,
                        principalSchema: "exam",
                        principalTable: "exam_subject",
                        principalColumn: "exam_subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_exam_result",
                schema: "exam",
                columns: table => new
                {
                    student_exam_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    marks_obtained = table.Column<decimal>(type: "numeric", nullable: true),
                    percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    grade = table.Column<string>(type: "text", nullable: true),
                    is_absent = table.Column<bool>(type: "boolean", nullable: false),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    entered_by = table.Column<Guid>(type: "uuid", nullable: true),
                    verified_by = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_student_exam_result", x => x.student_exam_result_id);
                    table.ForeignKey(
                        name: "FK_student_exam_result_exam_subject_exam_subject_id",
                        column: x => x.exam_subject_id,
                        principalSchema: "exam",
                        principalTable: "exam_subject",
                        principalColumn: "exam_subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_tenant_id",
                schema: "exam",
                table: "exam",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_tenant_id_code",
                schema: "exam",
                table: "exam",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_subject_exam_id",
                schema: "exam",
                table: "exam_subject",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_subject_tenant_id",
                schema: "exam",
                table: "exam_subject",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_subject_tenant_id_code",
                schema: "exam",
                table: "exam_subject",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_exam_id",
                schema: "exam",
                table: "exam_task",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_exam_subject_id",
                schema: "exam",
                table: "exam_task",
                column: "exam_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_tenant_id_due_at_status",
                schema: "exam",
                table: "exam_task",
                columns: new[] { "tenant_id", "due_at", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_tenant_id_exam_id_exam_subject_id",
                schema: "exam",
                table: "exam_task",
                columns: new[] { "tenant_id", "exam_id", "exam_subject_id" });

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_tenant_id_exam_subject_id_teacher_employee_id_tas~",
                schema: "exam",
                table: "exam_task",
                columns: new[] { "tenant_id", "exam_subject_id", "teacher_employee_id", "task_type", "is_active" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_task_tenant_id_teacher_user_id_status",
                schema: "exam",
                table: "exam_task",
                columns: new[] { "tenant_id", "teacher_user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_grade_scale_tenant_id",
                schema: "exam",
                table: "grade_scale",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_grade_scale_tenant_id_code",
                schema: "exam",
                table: "grade_scale",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_result_exam_subject_id",
                schema: "exam",
                table: "student_exam_result",
                column: "exam_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_result_tenant_id",
                schema: "exam",
                table: "student_exam_result",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_exam_result_tenant_id_code",
                schema: "exam",
                table: "student_exam_result",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_task",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "grade_scale",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "student_exam_result",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "exam_subject",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "exam",
                schema: "exam");
        }
    }
}
