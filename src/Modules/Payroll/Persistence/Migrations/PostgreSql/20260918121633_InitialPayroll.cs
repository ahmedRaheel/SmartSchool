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
                name: "payroll");

            migrationBuilder.CreateTable(
                name: "employee_compensation",
                schema: "payroll",
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
                name: "employee_projection",
                schema: "payroll",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_number = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_projection", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "increment",
                schema: "payroll",
                columns: table => new
                {
                    increment_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_increment", x => x.increment_id);
                });

            migrationBuilder.CreateTable(
                name: "job_grade_projection",
                schema: "payroll",
                columns: table => new
                {
                    job_grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_grade_projection", x => x.job_grade_id);
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

            migrationBuilder.CreateTable(
                name: "payslip",
                schema: "payroll",
                columns: table => new
                {
                    payslip_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_payslip", x => x.payslip_id);
                });

            migrationBuilder.CreateTable(
                name: "salarystructure",
                schema: "payroll",
                columns: table => new
                {
                    salary_structure_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_salarystructure", x => x.salary_structure_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensation_tenant_id",
                schema: "payroll",
                table: "employee_compensation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensation_tenant_id_code",
                schema: "payroll",
                table: "employee_compensation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_projection_tenant_id_employee_id",
                schema: "payroll",
                table: "employee_projection",
                columns: new[] { "tenant_id", "employee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_increment_tenant_id",
                schema: "payroll",
                table: "increment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_increment_tenant_id_code",
                schema: "payroll",
                table: "increment",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_grade_projection_tenant_id_job_grade_id",
                schema: "payroll",
                table: "job_grade_projection",
                columns: new[] { "tenant_id", "job_grade_id" },
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

            migrationBuilder.CreateIndex(
                name: "IX_payslip_tenant_id",
                schema: "payroll",
                table: "payslip",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_payslip_tenant_id_code",
                schema: "payroll",
                table: "payslip",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_salarystructure_tenant_id",
                schema: "payroll",
                table: "salarystructure",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_salarystructure_tenant_id_code",
                schema: "payroll",
                table: "salarystructure",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_compensation",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "employee_projection",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "increment",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "job_grade_projection",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "payroll_run",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "payslip",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "salarystructure",
                schema: "payroll");
        }
    }
}
