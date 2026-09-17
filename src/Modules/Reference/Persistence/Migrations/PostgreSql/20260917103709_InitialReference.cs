using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartSchool.Modules.Reference.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "saas");

            migrationBuilder.CreateTable(
                name: "lookup_value",
                schema: "saas",
                columns: table => new
                {
                    lookup_value_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lookup_type_id = table.Column<long>(type: "bigint", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    metadata = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookup_value", x => x.lookup_value_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lookup_value_lookup_type_id_tenant_id_code",
                schema: "saas",
                table: "lookup_value",
                columns: new[] { "lookup_type_id", "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lookup_value",
                schema: "saas");
        }
    }
}
