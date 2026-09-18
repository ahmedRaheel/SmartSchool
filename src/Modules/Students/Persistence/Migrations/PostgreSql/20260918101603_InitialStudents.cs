using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Students.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "student");

            migrationBuilder.EnsureSchema(
                name: "document");

            migrationBuilder.CreateTable(
                name: "attendance",
                schema: "student",
                columns: table => new
                {
                    attendance_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_attendance", x => x.attendance_id);
                });

            migrationBuilder.CreateTable(
                name: "guardian",
                schema: "student",
                columns: table => new
                {
                    guardian_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cnic_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guardian", x => x.guardian_id);
                });

            migrationBuilder.CreateTable(
                name: "parentdocument",
                schema: "document",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    parentid = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_parentdocument", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student",
                schema: "student",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    school_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_number = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    photo_content_type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    photo_file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    admission_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "admission_placement",
                schema: "student",
                columns: table => new
                {
                    admission_placement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    approved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admission_placement", x => x.admission_placement_id);
                    table.ForeignKey(
                        name: "FK_admission_placement_student_student_id",
                        column: x => x.student_id,
                        principalSchema: "student",
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_enrollment",
                schema: "student",
                columns: table => new
                {
                    student_enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_number = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_enrollment", x => x.student_enrollment_id);
                    table.ForeignKey(
                        name: "FK_student_enrollment_student_student_id",
                        column: x => x.student_id,
                        principalSchema: "student",
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_guardian",
                schema: "student",
                columns: table => new
                {
                    student_guardian_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guardian_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relationship = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    can_view_academics = table.Column<bool>(type: "boolean", nullable: false),
                    can_view_finance = table.Column<bool>(type: "boolean", nullable: false),
                    can_pickup = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_guardian", x => x.student_guardian_id);
                    table.ForeignKey(
                        name: "FK_student_guardian_guardian_guardian_id",
                        column: x => x.guardian_id,
                        principalSchema: "student",
                        principalTable: "guardian",
                        principalColumn: "guardian_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_guardian_student_student_id",
                        column: x => x.student_id,
                        principalSchema: "student",
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admission_placement_student_id",
                schema: "student",
                table: "admission_placement",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_tenant_id",
                schema: "student",
                table: "attendance",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_tenant_id_code",
                schema: "student",
                table: "attendance",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_guardian_tenant_id_cnic_number",
                schema: "student",
                table: "guardian",
                columns: new[] { "tenant_id", "cnic_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parentdocument_tenantid_parentid_documenttypeid",
                schema: "document",
                table: "parentdocument",
                columns: new[] { "tenantid", "parentid", "documenttypeid" });

            migrationBuilder.CreateIndex(
                name: "IX_parentdocument_tenantid_sha256hash",
                schema: "document",
                table: "parentdocument",
                columns: new[] { "tenantid", "sha256hash" });

            migrationBuilder.CreateIndex(
                name: "IX_parentdocument_tenantid_storageprovider_storagekey",
                schema: "document",
                table: "parentdocument",
                columns: new[] { "tenantid", "storageprovider", "storagekey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_tenant_id_student_number",
                schema: "student",
                table: "student",
                columns: new[] { "tenant_id", "student_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_enrollment_student_id",
                schema: "student",
                table: "student_enrollment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_enrollment_tenant_id",
                schema: "student",
                table: "student_enrollment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_guardian_guardian_id",
                schema: "student",
                table: "student_guardian",
                column: "guardian_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_guardian_student_id_guardian_id",
                schema: "student",
                table: "student_guardian",
                columns: new[] { "student_id", "guardian_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admission_placement",
                schema: "student");

            migrationBuilder.DropTable(
                name: "attendance",
                schema: "student");

            migrationBuilder.DropTable(
                name: "parentdocument",
                schema: "document");

            migrationBuilder.DropTable(
                name: "student_enrollment",
                schema: "student");

            migrationBuilder.DropTable(
                name: "student_guardian",
                schema: "student");

            migrationBuilder.DropTable(
                name: "guardian",
                schema: "student");

            migrationBuilder.DropTable(
                name: "student",
                schema: "student");
        }
    }
}
