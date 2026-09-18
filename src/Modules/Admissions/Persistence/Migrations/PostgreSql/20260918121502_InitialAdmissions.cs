using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Admissions.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAdmissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admission");

            migrationBuilder.CreateTable(
                name: "admission_criteria",
                schema: "admission",
                columns: table => new
                {
                    admission_criteria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    minimum_marks = table.Column<decimal>(type: "numeric", nullable: false),
                    entrance_test_minimum = table.Column<decimal>(type: "numeric", nullable: true),
                    minimum_age = table.Column<int>(type: "integer", nullable: true),
                    maximum_age = table.Column<int>(type: "integer", nullable: true),
                    interview_required = table.Column<bool>(type: "boolean", nullable: false),
                    required_documents = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admission_criteria", x => x.admission_criteria_id);
                });

            migrationBuilder.CreateTable(
                name: "admissiondecision",
                schema: "admission",
                columns: table => new
                {
                    admission_decision_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_admissiondecision", x => x.admission_decision_id);
                });

            migrationBuilder.CreateTable(
                name: "applicant",
                schema: "admission",
                columns: table => new
                {
                    applicant_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_applicant", x => x.applicant_id);
                });

            migrationBuilder.CreateTable(
                name: "application",
                schema: "admission",
                columns: table => new
                {
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_application", x => x.application_id);
                });

            migrationBuilder.CreateTable(
                name: "inquiry",
                schema: "admission",
                columns: table => new
                {
                    inquiry_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_inquiry", x => x.inquiry_id);
                });

            migrationBuilder.CreateTable(
                name: "student_application",
                schema: "admission",
                columns: table => new
                {
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    guardian_name = table.Column<string>(type: "text", nullable: false),
                    guardian_cnic = table.Column<string>(type: "text", nullable: true),
                    guardian_email = table.Column<string>(type: "text", nullable: true),
                    guardian_phone = table.Column<string>(type: "text", nullable: true),
                    relationship = table.Column<string>(type: "text", nullable: true),
                    previous_school = table.Column<string>(type: "text", nullable: true),
                    previous_marks = table.Column<decimal>(type: "numeric", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entrance_test_marks = table.Column<decimal>(type: "numeric", nullable: true),
                    interview_passed = table.Column<bool>(type: "boolean", nullable: true),
                    decision_notes = table.Column<string>(type: "text", nullable: true),
                    decided_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    submitted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_application", x => x.application_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admissiondecision_tenant_id",
                schema: "admission",
                table: "admissiondecision",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_admissiondecision_tenant_id_code",
                schema: "admission",
                table: "admissiondecision",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applicant_tenant_id",
                schema: "admission",
                table: "applicant",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_applicant_tenant_id_code",
                schema: "admission",
                table: "applicant",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_application_tenant_id",
                schema: "admission",
                table: "application",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_tenant_id_code",
                schema: "admission",
                table: "application",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_tenant_id",
                schema: "admission",
                table: "inquiry",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_inquiry_tenant_id_code",
                schema: "admission",
                table: "inquiry",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admission_criteria",
                schema: "admission");

            migrationBuilder.DropTable(
                name: "admissiondecision",
                schema: "admission");

            migrationBuilder.DropTable(
                name: "applicant",
                schema: "admission");

            migrationBuilder.DropTable(
                name: "application",
                schema: "admission");

            migrationBuilder.DropTable(
                name: "inquiry",
                schema: "admission");

            migrationBuilder.DropTable(
                name: "student_application",
                schema: "admission");
        }
    }
}
