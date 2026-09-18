using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Finance.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialFinance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "payroll");

            migrationBuilder.CreateTable(
                name: "discount",
                schema: "finance",
                columns: table => new
                {
                    discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_discount", x => x.discount_id);
                });

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
                name: "fee_type",
                schema: "finance",
                columns: table => new
                {
                    fee_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    frequency = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fee_type", x => x.fee_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Increment",
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
                    table.PrimaryKey("PK_Increment", x => x.increment_id);
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
                name: "Payslip",
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
                    table.PrimaryKey("PK_Payslip", x => x.payslip_id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryStructure",
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
                    table.PrimaryKey("PK_SalaryStructure", x => x.salary_structure_id);
                });

            migrationBuilder.CreateTable(
                name: "scholarship",
                schema: "finance",
                columns: table => new
                {
                    scholarship_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_scholarship", x => x.scholarship_id);
                });

            migrationBuilder.CreateTable(
                name: "student_invoice",
                schema: "finance",
                columns: table => new
                {
                    student_invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: true),
                    invoice_number = table.Column<string>(type: "text", nullable: false),
                    invoice_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    balance_amount = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_student_invoice", x => x.student_invoice_id);
                });

            migrationBuilder.CreateTable(
                name: "student_payment",
                schema: "finance",
                columns: table => new
                {
                    student_payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_number = table.Column<string>(type: "text", nullable: false),
                    payment_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_method = table.Column<string>(type: "text", nullable: false),
                    reference_no = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_student_payment", x => x.student_payment_id);
                });

            migrationBuilder.CreateTable(
                name: "studentfee",
                schema: "finance",
                columns: table => new
                {
                    student_fee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_studentfee", x => x.student_fee_id);
                });

            migrationBuilder.CreateTable(
                name: "fee_structure",
                schema: "finance",
                columns: table => new
                {
                    fee_structure_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_level_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fee_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    frequency = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: true),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_fee_structure", x => x.fee_structure_id);
                    table.ForeignKey(
                        name: "FK_fee_structure_fee_type_fee_type_id",
                        column: x => x.fee_type_id,
                        principalSchema: "finance",
                        principalTable: "fee_type",
                        principalColumn: "fee_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_discount_tenant_id_branch_id",
                schema: "finance",
                table: "discount",
                columns: new[] { "tenant_id", "branch_id" });

            migrationBuilder.CreateIndex(
                name: "IX_discount_tenant_id_code",
                schema: "finance",
                table: "discount",
                columns: new[] { "tenant_id", "code" },
                unique: true);

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
                name: "IX_fee_structure_fee_type_id",
                schema: "finance",
                table: "fee_structure",
                column: "fee_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_fee_structure_tenant_id_grade_level_id_fee_type_id_academic~",
                schema: "finance",
                table: "fee_structure",
                columns: new[] { "tenant_id", "grade_level_id", "fee_type_id", "academic_year_id" });

            migrationBuilder.CreateIndex(
                name: "IX_fee_type_tenant_id",
                schema: "finance",
                table: "fee_type",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_fee_type_tenant_id_code",
                schema: "finance",
                table: "fee_type",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Increment_tenant_id",
                schema: "payroll",
                table: "Increment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Increment_tenant_id_code",
                schema: "payroll",
                table: "Increment",
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

            migrationBuilder.CreateIndex(
                name: "IX_Payslip_tenant_id",
                schema: "payroll",
                table: "Payslip",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payslip_tenant_id_code",
                schema: "payroll",
                table: "Payslip",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructure_tenant_id",
                schema: "payroll",
                table: "SalaryStructure",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructure_tenant_id_code",
                schema: "payroll",
                table: "SalaryStructure",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scholarship_tenant_id_branch_id",
                schema: "finance",
                table: "scholarship",
                columns: new[] { "tenant_id", "branch_id" });

            migrationBuilder.CreateIndex(
                name: "IX_scholarship_tenant_id_code",
                schema: "finance",
                table: "scholarship",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_invoice_tenant_id",
                schema: "finance",
                table: "student_invoice",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_invoice_tenant_id_code",
                schema: "finance",
                table: "student_invoice",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_payment_tenant_id",
                schema: "finance",
                table: "student_payment",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_payment_tenant_id_code",
                schema: "finance",
                table: "student_payment",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_studentfee_tenant_id_branch_id",
                schema: "finance",
                table: "studentfee",
                columns: new[] { "tenant_id", "branch_id" });

            migrationBuilder.CreateIndex(
                name: "IX_studentfee_tenant_id_code",
                schema: "finance",
                table: "studentfee",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discount",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "employee_compensation",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "fee_structure",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "Increment",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "payroll_run",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "Payslip",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "SalaryStructure",
                schema: "payroll");

            migrationBuilder.DropTable(
                name: "scholarship",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "student_invoice",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "student_payment",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "studentfee",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "fee_type",
                schema: "finance");
        }
    }
}
