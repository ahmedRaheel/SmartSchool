using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Library.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "library");

            migrationBuilder.CreateTable(
                name: "book",
                schema: "library",
                columns: table => new
                {
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    isbn = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    author_text = table.Column<string>(type: "text", nullable: true),
                    publisher_text = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_book", x => x.book_id);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                schema: "library",
                columns: table => new
                {
                    reservation_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_reservation", x => x.reservation_id);
                });

            migrationBuilder.CreateTable(
                name: "book_copy",
                schema: "library",
                columns: table => new
                {
                    book_copy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_book_copy", x => x.book_copy_id);
                    table.ForeignKey(
                        name: "FK_book_copy_book_book_id",
                        column: x => x.book_id,
                        principalSchema: "library",
                        principalTable: "book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "book_loan",
                schema: "library",
                columns: table => new
                {
                    book_loan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_copy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    issued_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    returned_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_book_loan", x => x.book_loan_id);
                    table.ForeignKey(
                        name: "FK_book_loan_book_copy_book_copy_id",
                        column: x => x.book_copy_id,
                        principalSchema: "library",
                        principalTable: "book_copy",
                        principalColumn: "book_copy_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_tenant_id",
                schema: "library",
                table: "book",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_tenant_id_code",
                schema: "library",
                table: "book",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_copy_book_id",
                schema: "library",
                table: "book_copy",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_copy_tenant_id",
                schema: "library",
                table: "book_copy",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_copy_tenant_id_code",
                schema: "library",
                table: "book_copy",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_loan_book_copy_id",
                schema: "library",
                table: "book_loan",
                column: "book_copy_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_loan_tenant_id",
                schema: "library",
                table: "book_loan",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_loan_tenant_id_code",
                schema: "library",
                table: "book_loan",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservation_tenant_id",
                schema: "library",
                table: "reservation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_tenant_id_code",
                schema: "library",
                table: "reservation",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book_loan",
                schema: "library");

            migrationBuilder.DropTable(
                name: "reservation",
                schema: "library");

            migrationBuilder.DropTable(
                name: "book_copy",
                schema: "library");

            migrationBuilder.DropTable(
                name: "book",
                schema: "library");
        }
    }
}
