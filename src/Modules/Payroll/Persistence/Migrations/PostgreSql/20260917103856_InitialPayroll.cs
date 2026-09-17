using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Payroll.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialPayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "payroll");

            migrationBuilder.CreateTable(
                name: "employee_compensation",
                schema: "hr",
                columns: table => new
                {
                    employee_compensation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_grade_id = table.Column<Guid>(type: "uuid", nullable: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: false),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    basic_salary = table.Column<decimal>(type: "numeric", nullable: false),
                    gross_salary = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_code = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_employee_compensation", x => x.employee_compensation_id);
                });

            migrationBuilder.CreateTable(
                name: "payroll_run",
                schema: "payroll",
                columns: table => new
                {
                    payroll_run_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payroll_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_code = table.Column<string>(type: "text", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_payroll_run", x => x.payroll_run_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensation_tenant_id",
                schema: "hr",
                table: "employee_compensation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensation_tenant_id_code",
                schema: "hr",
                table: "employee_compensation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payroll_run_tenant_id",
                schema: "payroll",
                table: "payroll_run",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_payroll_run_tenant_id_code",
                schema: "payroll",
                table: "payroll_run",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_compensation",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "payroll_run",
                schema: "payroll");
        }
    }
}
