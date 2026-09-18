using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.HR.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialHR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "document");

            migrationBuilder.CreateTable(
                name: "candidate",
                schema: "hr",
                columns: table => new
                {
                    candidate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    current_job_title = table.Column<string>(type: "text", nullable: true),
                    current_employer = table.Column<string>(type: "text", nullable: true),
                    total_experience_years = table.Column<decimal>(type: "numeric", nullable: true),
                    highest_qualification = table.Column<string>(type: "text", nullable: true),
                    expected_salary = table.Column<decimal>(type: "numeric", nullable: true),
                    notice_period_days = table.Column<int>(type: "integer", nullable: true),
                    status_code = table.Column<string>(type: "text", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_candidate", x => x.candidate_id);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                schema: "hr",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    staff_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    designation = table.Column<short>(type: "smallint", nullable: false),
                    employee_number = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cnic_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    job_title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    photo_content_type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    photo_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    alternate_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    emergency_contact_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    emergency_contact_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    employment_type_code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    source_candidate_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "employee_education",
                schema: "hr",
                columns: table => new
                {
                    employee_education_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qualification = table.Column<string>(type: "text", nullable: false),
                    institute = table.Column<string>(type: "text", nullable: true),
                    field_of_study = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    grade = table.Column<string>(type: "text", nullable: true),
                    is_highest = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_education", x => x.employee_education_id);
                });

            migrationBuilder.CreateTable(
                name: "employmenthistory",
                schema: "hr",
                columns: table => new
                {
                    employment_history_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_employmenthistory", x => x.employment_history_id);
                });

            migrationBuilder.CreateTable(
                name: "interview",
                schema: "hr",
                columns: table => new
                {
                    interview_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    interview_type_code = table.Column<string>(type: "text", nullable: false),
                    round_number = table.Column<int>(type: "integer", nullable: false),
                    scheduled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    meeting_url = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    overall_score = table.Column<decimal>(type: "numeric", nullable: true),
                    recommendation = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_interview", x => x.interview_id);
                });

            migrationBuilder.CreateTable(
                name: "job",
                schema: "hr",
                columns: table => new
                {
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    job_family_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    responsibilities = table.Column<string>(type: "text", nullable: true),
                    minimum_qualification = table.Column<string>(type: "text", nullable: true),
                    minimum_experience_years = table.Column<decimal>(type: "numeric", nullable: true),
                    is_teaching_position = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_job", x => x.job_id);
                });

            migrationBuilder.CreateTable(
                name: "job_grade",
                schema: "hr",
                columns: table => new
                {
                    job_grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_level = table.Column<int>(type: "integer", nullable: true),
                    minimum_salary = table.Column<decimal>(type: "numeric", nullable: true),
                    midpoint_salary = table.Column<decimal>(type: "numeric", nullable: true),
                    maximum_salary = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_code = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_job_grade", x => x.job_grade_id);
                });

            migrationBuilder.CreateTable(
                name: "resume",
                schema: "hr",
                columns: table => new
                {
                    resume_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_resume", x => x.resume_id);
                });

            migrationBuilder.CreateTable(
                name: "teacher_teaching_assignment",
                schema: "hr",
                columns: table => new
                {
                    teacher_teaching_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    periods_per_week = table.Column<int>(type: "integer", nullable: true),
                    is_class_teacher = table.Column<bool>(type: "boolean", nullable: false),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: true),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_teaching_assignment", x => x.teacher_teaching_assignment_id);
                });

            migrationBuilder.CreateTable(
                name: "teacherdocument",
                schema: "document",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacherid = table.Column<Guid>(type: "uuid", nullable: false),
                    documenttypeid = table.Column<Guid>(type: "uuid", nullable: false),
                    originalfilename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    contenttype = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    filesizebytes = table.Column<long>(type: "bigint", nullable: false),
                    storageprovider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    storagekey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    sha256hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    documentnumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    issuedon = table.Column<DateOnly>(type: "date", nullable: true),
                    expireson = table.Column<DateOnly>(type: "date", nullable: true),
                    isverified = table.Column<bool>(type: "boolean", nullable: false),
                    verifiedbyuserid = table.Column<Guid>(type: "uuid", nullable: true),
                    verifiedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    rowversion = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherdocument", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee_experience",
                schema: "hr",
                columns: table => new
                {
                    employee_experience_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employer = table.Column<string>(type: "text", nullable: false),
                    job_title = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    responsibilities = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_experience", x => x.employee_experience_id);
                    table.ForeignKey(
                        name: "FK_employee_experience_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "leave_request",
                schema: "hr",
                columns: table => new
                {
                    leave_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    leave_type = table.Column<string>(type: "text", nullable: false),
                    from_date = table.Column<DateOnly>(type: "date", nullable: false),
                    to_date = table.Column<DateOnly>(type: "date", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    decision_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    decision_note = table.Column<string>(type: "text", nullable: true),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_leave_request", x => x.leave_request_id);
                    table.ForeignKey(
                        name: "FK_leave_request_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "position",
                schema: "hr",
                columns: table => new
                {
                    position_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: true),
                    job_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_grade_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reports_to_position_id = table.Column<Guid>(type: "uuid", nullable: true),
                    position_code = table.Column<string>(type: "text", nullable: false),
                    headcount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_position", x => x.position_id);
                    table.ForeignKey(
                        name: "FK_position_job_grade_job_grade_id",
                        column: x => x.job_grade_id,
                        principalSchema: "hr",
                        principalTable: "job_grade",
                        principalColumn: "job_grade_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_position_job_job_id",
                        column: x => x.job_id,
                        principalSchema: "hr",
                        principalTable: "job",
                        principalColumn: "job_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_candidate_tenant_id_branch_id",
                schema: "hr",
                table: "candidate",
                columns: new[] { "tenant_id", "branch_id" });

            migrationBuilder.CreateIndex(
                name: "IX_candidate_tenant_id_code",
                schema: "hr",
                table: "candidate",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_tenant_id_cnic_number",
                schema: "hr",
                table: "employee",
                columns: new[] { "tenant_id", "cnic_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_tenant_id_employee_number",
                schema: "hr",
                table: "employee",
                columns: new[] { "tenant_id", "employee_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_experience_employee_id",
                schema: "hr",
                table: "employee_experience",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employmenthistory_tenant_id",
                schema: "hr",
                table: "employmenthistory",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_employmenthistory_tenant_id_code",
                schema: "hr",
                table: "employmenthistory",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interview_tenant_id",
                schema: "hr",
                table: "interview",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_interview_tenant_id_code",
                schema: "hr",
                table: "interview",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_tenant_id",
                schema: "hr",
                table: "job",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_tenant_id_code",
                schema: "hr",
                table: "job",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_grade_tenant_id",
                schema: "hr",
                table: "job_grade",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_grade_tenant_id_code",
                schema: "hr",
                table: "job_grade",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_employee_id",
                schema: "hr",
                table: "leave_request",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_tenant_id_branch_id",
                schema: "hr",
                table: "leave_request",
                columns: new[] { "tenant_id", "branch_id" });

            migrationBuilder.CreateIndex(
                name: "IX_leave_request_tenant_id_code",
                schema: "hr",
                table: "leave_request",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_position_job_grade_id",
                schema: "hr",
                table: "position",
                column: "job_grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_position_job_id",
                schema: "hr",
                table: "position",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_position_tenant_id",
                schema: "hr",
                table: "position",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_position_tenant_id_code",
                schema: "hr",
                table: "position",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_resume_tenant_id",
                schema: "hr",
                table: "resume",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_resume_tenant_id_code",
                schema: "hr",
                table: "resume",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teacherdocument_tenantid_sha256hash",
                schema: "document",
                table: "teacherdocument",
                columns: new[] { "tenantid", "sha256hash" });

            migrationBuilder.CreateIndex(
                name: "IX_teacherdocument_tenantid_storageprovider_storagekey",
                schema: "document",
                table: "teacherdocument",
                columns: new[] { "tenantid", "storageprovider", "storagekey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teacherdocument_tenantid_teacherid_documenttypeid",
                schema: "document",
                table: "teacherdocument",
                columns: new[] { "tenantid", "teacherid", "documenttypeid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidate",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_education",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_experience",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employmenthistory",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "interview",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "leave_request",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "position",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "resume",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "teacher_teaching_assignment",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "teacherdocument",
                schema: "document");

            migrationBuilder.DropTable(
                name: "employee",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "job_grade",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "job",
                schema: "hr");
        }
    }
}
