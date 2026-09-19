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
                name: "reference");

            migrationBuilder.EnsureSchema(
                name: "saas");

            migrationBuilder.CreateTable(
                name: "branch_gender_type",
                schema: "reference",
                columns: table => new
                {
                    branch_gender_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_gender_type", x => x.branch_gender_type_id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                schema: "reference",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "education_level",
                schema: "reference",
                columns: table => new
                {
                    education_level_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_education_level", x => x.education_level_id);
                });

            migrationBuilder.CreateTable(
                name: "lookup_type",
                schema: "saas",
                columns: table => new
                {
                    lookup_type_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    is_tenant_scoped = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookup_type", x => x.lookup_type_id);
                });

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

            migrationBuilder.CreateTable(
                name: "province",
                schema: "reference",
                columns: table => new
                {
                    province_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_province", x => x.province_id);
                    table.ForeignKey(
                        name: "FK_province_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "reference",
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "city",
                schema: "reference",
                columns: table => new
                {
                    city_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    province_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_city_province_province_id",
                        column: x => x.province_id,
                        principalSchema: "reference",
                        principalTable: "province",
                        principalColumn: "province_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_branch_gender_type_code",
                schema: "reference",
                table: "branch_gender_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_city_province_id_code",
                schema: "reference",
                table: "city",
                columns: new[] { "province_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_country_code",
                schema: "reference",
                table: "country",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_education_level_code",
                schema: "reference",
                table: "education_level",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lookup_type_code",
                schema: "saas",
                table: "lookup_type",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lookup_value_lookup_type_id_tenant_id_code",
                schema: "saas",
                table: "lookup_value",
                columns: new[] { "lookup_type_id", "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_province_country_id_code",
                schema: "reference",
                table: "province",
                columns: new[] { "country_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "branch_gender_type",
                schema: "reference");

            migrationBuilder.DropTable(
                name: "city",
                schema: "reference");

            migrationBuilder.DropTable(
                name: "education_level",
                schema: "reference");

            migrationBuilder.DropTable(
                name: "lookup_type",
                schema: "saas");

            migrationBuilder.DropTable(
                name: "lookup_value",
                schema: "saas");

            migrationBuilder.DropTable(
                name: "province",
                schema: "reference");

            migrationBuilder.DropTable(
                name: "country",
                schema: "reference");
        }
    }
}
