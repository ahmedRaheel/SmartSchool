using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Organization.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academic");

            migrationBuilder.EnsureSchema(
                name: "org");

            migrationBuilder.EnsureSchema(
                name: "saas");

            migrationBuilder.EnsureSchema(
                name: "student");

            migrationBuilder.CreateTable(
                name: "academic_system",
                schema: "academic",
                columns: table => new
                {
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    system_type_code = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_academic_system", x => x.academic_system_id);
                });

            migrationBuilder.CreateTable(
                name: "campus_education_level",
                schema: "org",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    education_level_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campus_education_level", x => new { x.tenant_id, x.campus_id, x.education_level_id });
                });

            migrationBuilder.CreateTable(
                name: "school_branding",
                schema: "saas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    logo_content_type = table.Column<string>(type: "text", nullable: true),
                    logo_file_name = table.Column<string>(type: "text", nullable: true),
                    small_logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    small_logo_content_type = table.Column<string>(type: "text", nullable: true),
                    small_logo_file_name = table.Column<string>(type: "text", nullable: true),
                    favicon = table.Column<byte[]>(type: "bytea", nullable: true),
                    favicon_content_type = table.Column<string>(type: "text", nullable: true),
                    favicon_file_name = table.Column<string>(type: "text", nullable: true),
                    certificate_logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    certificate_logo_content_type = table.Column<string>(type: "text", nullable: true),
                    certificate_logo_file_name = table.Column<string>(type: "text", nullable: true),
                    letterhead = table.Column<byte[]>(type: "bytea", nullable: true),
                    letterhead_content_type = table.Column<string>(type: "text", nullable: true),
                    letterhead_file_name = table.Column<string>(type: "text", nullable: true),
                    watermark = table.Column<byte[]>(type: "bytea", nullable: true),
                    watermark_content_type = table.Column<string>(type: "text", nullable: true),
                    watermark_file_name = table.Column<string>(type: "text", nullable: true),
                    primary_color = table.Column<string>(type: "text", nullable: true),
                    secondary_color = table.Column<string>(type: "text", nullable: true),
                    accent_color = table.Column<string>(type: "text", nullable: true),
                    footer_text = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_school_branding", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "section",
                schema: "academic",
                columns: table => new
                {
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.section_id);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                schema: "academic",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    short_name = table.Column<string>(type: "text", nullable: true),
                    is_practical = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_subject", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "subscription",
                schema: "org",
                columns: table => new
                {
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_subscription", x => x.subscription_id);
                });

            migrationBuilder.CreateTable(
                name: "tenant",
                schema: "saas",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    status_code = table.Column<short>(type: "smallint", nullable: false),
                    default_language = table.Column<string>(type: "text", nullable: false),
                    timezone = table.Column<string>(type: "text", nullable: false),
                    currency_code = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant", x => x.tenant_id);
                });

            migrationBuilder.CreateTable(
                name: "tenant_settings",
                schema: "saas",
                columns: table => new
                {
                    tenant_settings_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_start_month = table.Column<short>(type: "smallint", nullable: false),
                    default_language = table.Column<short>(type: "smallint", nullable: false),
                    date_format = table.Column<short>(type: "smallint", nullable: false),
                    time_zone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    week_start = table.Column<short>(type: "smallint", nullable: false),
                    fee_warning_days = table.Column<short>(type: "smallint", nullable: false),
                    ai_rag_assistant = table.Column<bool>(type: "boolean", nullable: false),
                    ai_tutor = table.Column<bool>(type: "boolean", nullable: false),
                    ai_quiz = table.Column<bool>(type: "boolean", nullable: false),
                    ai_predictions = table.Column<bool>(type: "boolean", nullable: false),
                    ai_agent = table.Column<bool>(type: "boolean", nullable: false),
                    ai_parent_chatbot = table.Column<bool>(type: "boolean", nullable: false),
                    internal_chat = table.Column<bool>(type: "boolean", nullable: false),
                    notifications = table.Column<bool>(type: "boolean", nullable: false),
                    broadcast = table.Column<bool>(type: "boolean", nullable: false),
                    parent_portal = table.Column<bool>(type: "boolean", nullable: false),
                    assignments = table.Column<bool>(type: "boolean", nullable: false),
                    student_leave_apply = table.Column<bool>(type: "boolean", nullable: false),
                    library_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    online_payment = table.Column<bool>(type: "boolean", nullable: false),
                    fee_reminders = table.Column<bool>(type: "boolean", nullable: false),
                    digital_receipts = table.Column<bool>(type: "boolean", nullable: false),
                    staff_self_leave = table.Column<bool>(type: "boolean", nullable: false),
                    biometric_attendance = table.Column<bool>(type: "boolean", nullable: false),
                    qr_attendance = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor = table.Column<bool>(type: "boolean", nullable: false),
                    session_timeout = table.Column<bool>(type: "boolean", nullable: false),
                    ip_restriction = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant_settings", x => x.tenant_settings_id);
                });

            migrationBuilder.CreateTable(
                name: "program",
                schema: "academic",
                columns: table => new
                {
                    program_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_program", x => x.program_id);
                    table.ForeignKey(
                        name: "FK_program_academic_system_academic_system_id",
                        column: x => x.academic_system_id,
                        principalSchema: "academic",
                        principalTable: "academic_system",
                        principalColumn: "academic_system_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "school",
                schema: "org",
                columns: table => new
                {
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    registration_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fax = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    website = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    province = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    country = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school", x => x.school_id);
                    table.ForeignKey(
                        name: "FK_school_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "saas",
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tenant_contact",
                schema: "saas",
                columns: table => new
                {
                    tenant_contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type = table.Column<short>(type: "smallint", nullable: false),
                    contact_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant_contact", x => x.tenant_contact_id);
                    table.ForeignKey(
                        name: "FK_tenant_contact_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "saas",
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "campus",
                schema: "org",
                columns: table => new
                {
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    branch_type = table.Column<short>(type: "smallint", nullable: false),
                    branch_gender_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    province = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    country = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fax = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mobile = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campus", x => x.campus_id);
                    table.ForeignKey(
                        name: "FK_campus_academic_system_academic_system_id",
                        column: x => x.academic_system_id,
                        principalSchema: "academic",
                        principalTable: "academic_system",
                        principalColumn: "academic_system_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_campus_school_school_id",
                        column: x => x.school_id,
                        principalSchema: "org",
                        principalTable: "school",
                        principalColumn: "school_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "academic_year",
                schema: "academic",
                columns: table => new
                {
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_current = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_academic_year", x => x.academic_year_id);
                    table.ForeignKey(
                        name: "FK_academic_year_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "department",
                schema: "org",
                columns: table => new
                {
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    head_of_department_employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    telephone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.department_id);
                    table.ForeignKey(
                        name: "FK_department_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grade_level",
                schema: "academic",
                columns: table => new
                {
                    grade_level_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_system_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    education_level_id = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_grade_level", x => x.grade_level_id);
                    table.ForeignKey(
                        name: "FK_grade_level_academic_system_academic_system_id",
                        column: x => x.academic_system_id,
                        principalSchema: "academic",
                        principalTable: "academic_system",
                        principalColumn: "academic_system_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_grade_level_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room",
                schema: "org",
                columns: table => new
                {
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    room_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.room_id);
                    table.ForeignKey(
                        name: "FK_room_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "term",
                schema: "academic",
                columns: table => new
                {
                    term_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
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
                    table.PrimaryKey("PK_term", x => x.term_id);
                    table.ForeignKey(
                        name: "FK_term_academic_year_academic_year_id",
                        column: x => x.academic_year_id,
                        principalSchema: "academic",
                        principalTable: "academic_year",
                        principalColumn: "academic_year_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "class_section",
                schema: "academic",
                columns: table => new
                {
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    program_grade_id = table.Column<Guid>(type: "uuid", nullable: true),
                    grade_level_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    room_id = table.Column<Guid>(type: "uuid", nullable: true),
                    room_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_class_section", x => x.class_section_id);
                    table.ForeignKey(
                        name: "FK_class_section_academic_year_academic_year_id",
                        column: x => x.academic_year_id,
                        principalSchema: "academic",
                        principalTable: "academic_year",
                        principalColumn: "academic_year_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_class_section_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_class_section_grade_level_grade_level_id",
                        column: x => x.grade_level_id,
                        principalSchema: "academic",
                        principalTable: "grade_level",
                        principalColumn: "grade_level_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_class_section_room_room_id",
                        column: x => x.room_id,
                        principalSchema: "org",
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "course_offering",
                schema: "academic",
                columns: table => new
                {
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: true),
                    program_subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_course_offering", x => x.course_offering_id);
                    table.ForeignKey(
                        name: "FK_course_offering_academic_year_academic_year_id",
                        column: x => x.academic_year_id,
                        principalSchema: "academic",
                        principalTable: "academic_year",
                        principalColumn: "academic_year_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_course_offering_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_course_offering_term_term_id",
                        column: x => x.term_id,
                        principalSchema: "academic",
                        principalTable: "term",
                        principalColumn: "term_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "timetable",
                schema: "academic",
                columns: table => new
                {
                    timetable_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: true),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_timetable", x => x.timetable_id);
                    table.ForeignKey(
                        name: "FK_timetable_academic_year_academic_year_id",
                        column: x => x.academic_year_id,
                        principalSchema: "academic",
                        principalTable: "academic_year",
                        principalColumn: "academic_year_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_timetable_campus_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "org",
                        principalTable: "campus",
                        principalColumn: "campus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timetable_term_term_id",
                        column: x => x.term_id,
                        principalSchema: "academic",
                        principalTable: "term",
                        principalColumn: "term_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_course_enrollment",
                schema: "student",
                columns: table => new
                {
                    student_course_enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_type_code = table.Column<string>(type: "text", nullable: false),
                    selected_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_student_course_enrollment", x => x.student_course_enrollment_id);
                    table.ForeignKey(
                        name: "FK_student_course_enrollment_course_offering_course_offering_id",
                        column: x => x.course_offering_id,
                        principalSchema: "academic",
                        principalTable: "course_offering",
                        principalColumn: "course_offering_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacher_course_assignment",
                schema: "academic",
                columns: table => new
                {
                    teacher_course_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teaching_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    assignment_role = table.Column<string>(type: "text", nullable: false),
                    periods_per_week = table.Column<int>(type: "integer", nullable: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: true),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_teacher_course_assignment", x => x.teacher_course_assignment_id);
                    table.ForeignKey(
                        name: "FK_teacher_course_assignment_class_section_class_section_id",
                        column: x => x.class_section_id,
                        principalSchema: "academic",
                        principalTable: "class_section",
                        principalColumn: "class_section_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teacher_course_assignment_course_offering_course_offering_id",
                        column: x => x.course_offering_id,
                        principalSchema: "academic",
                        principalTable: "course_offering",
                        principalColumn: "course_offering_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "timetable_entry",
                schema: "academic",
                columns: table => new
                {
                    timetable_entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timetable_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    timetable_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teaching_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teacher_course_assignment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    room_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entry_type = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_timetable_entry", x => x.timetable_entry_id);
                    table.ForeignKey(
                        name: "FK_timetable_entry_class_section_class_section_id",
                        column: x => x.class_section_id,
                        principalSchema: "academic",
                        principalTable: "class_section",
                        principalColumn: "class_section_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_timetable_entry_course_offering_course_offering_id",
                        column: x => x.course_offering_id,
                        principalSchema: "academic",
                        principalTable: "course_offering",
                        principalColumn: "course_offering_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_timetable_entry_room_room_id",
                        column: x => x.room_id,
                        principalSchema: "org",
                        principalTable: "room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_timetable_entry_teacher_course_assignment_teacher_course_as~",
                        column: x => x.teacher_course_assignment_id,
                        principalSchema: "academic",
                        principalTable: "teacher_course_assignment",
                        principalColumn: "teacher_course_assignment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_timetable_entry_timetable_timetable_id",
                        column: x => x.timetable_id,
                        principalSchema: "academic",
                        principalTable: "timetable",
                        principalColumn: "timetable_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_academic_system_tenant_id",
                schema: "academic",
                table: "academic_system",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_system_tenant_id_code",
                schema: "academic",
                table: "academic_system",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_academic_year_campus_id",
                schema: "academic",
                table: "academic_year",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_year_tenant_id",
                schema: "academic",
                table: "academic_year",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_year_tenant_id_code",
                schema: "academic",
                table: "academic_year",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_campus_academic_system_id",
                schema: "org",
                table: "campus",
                column: "academic_system_id");

            migrationBuilder.CreateIndex(
                name: "IX_campus_school_id",
                schema: "org",
                table: "campus",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_campus_tenant_id_code",
                schema: "org",
                table: "campus",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_campus_tenant_id_school_id",
                schema: "org",
                table: "campus",
                columns: new[] { "tenant_id", "school_id" });

            migrationBuilder.CreateIndex(
                name: "IX_class_section_academic_year_id",
                schema: "academic",
                table: "class_section",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_section_campus_id",
                schema: "academic",
                table: "class_section",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_section_grade_level_id",
                schema: "academic",
                table: "class_section",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_section_room_id",
                schema: "academic",
                table: "class_section",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_section_tenant_id",
                schema: "academic",
                table: "class_section",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_section_tenant_id_code",
                schema: "academic",
                table: "class_section",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_offering_academic_year_id",
                schema: "academic",
                table: "course_offering",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_offering_campus_id",
                schema: "academic",
                table: "course_offering",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_offering_tenant_id",
                schema: "academic",
                table: "course_offering",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_offering_tenant_id_code",
                schema: "academic",
                table: "course_offering",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_offering_term_id",
                schema: "academic",
                table: "course_offering",
                column: "term_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_campus_id",
                schema: "org",
                table: "department",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_tenant_id",
                schema: "org",
                table: "department",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_tenant_id_code",
                schema: "org",
                table: "department",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grade_level_academic_system_id",
                schema: "academic",
                table: "grade_level",
                column: "academic_system_id");

            migrationBuilder.CreateIndex(
                name: "IX_grade_level_campus_id",
                schema: "academic",
                table: "grade_level",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_grade_level_tenant_id_campus_id",
                schema: "academic",
                table: "grade_level",
                columns: new[] { "tenant_id", "campus_id" });

            migrationBuilder.CreateIndex(
                name: "IX_grade_level_tenant_id_campus_id_code",
                schema: "academic",
                table: "grade_level",
                columns: new[] { "tenant_id", "campus_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_academic_system_id",
                schema: "academic",
                table: "program",
                column: "academic_system_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_tenant_id",
                schema: "academic",
                table: "program",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_tenant_id_code",
                schema: "academic",
                table: "program",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_campus_id_code",
                schema: "org",
                table: "room",
                columns: new[] { "campus_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_tenant_id",
                schema: "org",
                table: "room",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_school_tenant_id_code",
                schema: "org",
                table: "school",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_school_branding_tenant_id",
                schema: "saas",
                table: "school_branding",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_school_branding_tenant_id_code",
                schema: "saas",
                table: "school_branding",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_tenant_id_code",
                schema: "academic",
                table: "section",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_course_enrollment_course_offering_id",
                schema: "student",
                table: "student_course_enrollment",
                column: "course_offering_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_course_enrollment_tenant_id",
                schema: "student",
                table: "student_course_enrollment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_course_enrollment_tenant_id_code",
                schema: "student",
                table: "student_course_enrollment",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subject_tenant_id",
                schema: "academic",
                table: "subject",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_tenant_id_code",
                schema: "academic",
                table: "subject",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscription_tenant_id",
                schema: "org",
                table: "subscription",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_tenant_id_code",
                schema: "org",
                table: "subscription",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teacher_course_assignment_class_section_id",
                schema: "academic",
                table: "teacher_course_assignment",
                column: "class_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_course_assignment_course_offering_id",
                schema: "academic",
                table: "teacher_course_assignment",
                column: "course_offering_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_course_assignment_tenant_id",
                schema: "academic",
                table: "teacher_course_assignment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_course_assignment_tenant_id_code",
                schema: "academic",
                table: "teacher_course_assignment",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tenant_tenant_id",
                schema: "saas",
                table: "tenant",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tenant_tenant_id_code",
                schema: "saas",
                table: "tenant",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tenant_contact_tenant_id",
                schema: "saas",
                table: "tenant_contact",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tenant_settings_tenant_id",
                schema: "saas",
                table: "tenant_settings",
                column: "tenant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_term_academic_year_id",
                schema: "academic",
                table: "term",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_term_tenant_id",
                schema: "academic",
                table: "term",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_term_tenant_id_code",
                schema: "academic",
                table: "term",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_timetable_academic_year_id",
                schema: "academic",
                table: "timetable",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_campus_id",
                schema: "academic",
                table: "timetable",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_tenant_id",
                schema: "academic",
                table: "timetable",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_tenant_id_code",
                schema: "academic",
                table: "timetable",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_timetable_term_id",
                schema: "academic",
                table: "timetable",
                column: "term_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_class_section_id",
                schema: "academic",
                table: "timetable_entry",
                column: "class_section_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_course_offering_id",
                schema: "academic",
                table: "timetable_entry",
                column: "course_offering_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_room_id",
                schema: "academic",
                table: "timetable_entry",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_teacher_course_assignment_id",
                schema: "academic",
                table: "timetable_entry",
                column: "teacher_course_assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_tenant_id",
                schema: "academic",
                table: "timetable_entry",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_tenant_id_code",
                schema: "academic",
                table: "timetable_entry",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_timetable_entry_timetable_id",
                schema: "academic",
                table: "timetable_entry",
                column: "timetable_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campus_education_level",
                schema: "org");

            migrationBuilder.DropTable(
                name: "department",
                schema: "org");

            migrationBuilder.DropTable(
                name: "program",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "school_branding",
                schema: "saas");

            migrationBuilder.DropTable(
                name: "section",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "student_course_enrollment",
                schema: "student");

            migrationBuilder.DropTable(
                name: "subject",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "subscription",
                schema: "org");

            migrationBuilder.DropTable(
                name: "tenant_contact",
                schema: "saas");

            migrationBuilder.DropTable(
                name: "tenant_settings",
                schema: "saas");

            migrationBuilder.DropTable(
                name: "timetable_entry",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "teacher_course_assignment",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "timetable",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "class_section",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "course_offering",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "grade_level",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "room",
                schema: "org");

            migrationBuilder.DropTable(
                name: "term",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "academic_year",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "campus",
                schema: "org");

            migrationBuilder.DropTable(
                name: "academic_system",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "school",
                schema: "org");

            migrationBuilder.DropTable(
                name: "tenant",
                schema: "saas");
        }
    }
}
