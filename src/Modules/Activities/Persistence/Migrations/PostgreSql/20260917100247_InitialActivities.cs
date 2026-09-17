using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Activities.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "activity");

            migrationBuilder.CreateTable(
                name: "activity",
                schema: "activity",
                columns: table => new
                {
                    activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    coordinator_employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    activity_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    venue = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    max_participants = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.activity_id);
                });

            migrationBuilder.CreateTable(
                name: "student_award",
                schema: "activity",
                columns: table => new
                {
                    student_award_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    award_type_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    award_date = table.Column<DateOnly>(type: "date", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    generated_document_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_award", x => x.student_award_id);
                });

            migrationBuilder.CreateTable(
                name: "student_of_month",
                schema: "activity",
                columns: table => new
                {
                    student_of_month_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    award_month = table.Column<int>(type: "integer", nullable: true),
                    award_year = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    MetadataJson = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_of_month", x => x.student_of_month_id);
                });

            migrationBuilder.CreateTable(
                name: "student_activity",
                schema: "activity",
                columns: table => new
                {
                    student_activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    joined_at = table.Column<DateOnly>(type: "date", nullable: false),
                    left_at = table.Column<DateOnly>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_activity", x => x.student_activity_id);
                    table.ForeignKey(
                        name: "FK_student_activity_activity_activity_id",
                        column: x => x.activity_id,
                        principalSchema: "activity",
                        principalTable: "activity",
                        principalColumn: "activity_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_tenant_id_activity_date_status",
                schema: "activity",
                table: "activity",
                columns: new[] { "tenant_id", "activity_date", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_activity_tenant_id_code",
                schema: "activity",
                table: "activity",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_activity_activity_id",
                schema: "activity",
                table: "student_activity",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_activity_tenant_id_activity_id_student_id",
                schema: "activity",
                table: "student_activity",
                columns: new[] { "tenant_id", "activity_id", "student_id" },
                unique: true,
                filter: "is_active = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_student_award_tenant_id_student_id_award_date",
                schema: "activity",
                table: "student_award",
                columns: new[] { "tenant_id", "student_id", "award_date" });

            migrationBuilder.CreateIndex(
                name: "IX_student_of_month_tenant_id",
                schema: "activity",
                table: "student_of_month",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_of_month_tenant_id_code",
                schema: "activity",
                table: "student_of_month",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_activity",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "student_award",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "student_of_month",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "activity",
                schema: "activity");
        }
    }
}
