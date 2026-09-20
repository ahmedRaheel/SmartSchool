using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Finance.Persistence.Migrations.PostgreSql;

/// <inheritdoc />
public partial class Finance : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "MetadataJson",
            schema: "finance",
            table: "studentfee",
            newName: "metadata_json");

        migrationBuilder.RenameColumn(
            name: "MetadataJson",
            schema: "finance",
            table: "scholarship",
            newName: "metadata_json");

        migrationBuilder.RenameColumn(
            name: "MetadataJson",
            schema: "finance",
            table: "discount",
            newName: "metadata_json");

        // EF Core doesn't support USING in AlterColumn — use raw SQL instead
        migrationBuilder.Sql(
            @"ALTER TABLE finance.studentfee
                  ALTER COLUMN metadata_json TYPE jsonb
                  USING metadata_json::jsonb;");

        migrationBuilder.Sql(
            @"ALTER TABLE finance.scholarship
                  ALTER COLUMN metadata_json TYPE jsonb
                  USING metadata_json::jsonb;");

        migrationBuilder.Sql(
            @"ALTER TABLE finance.discount
                  ALTER COLUMN metadata_json TYPE jsonb
                  USING metadata_json::jsonb;");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "metadata_json",
            schema: "finance",
            table: "studentfee",
            newName: "MetadataJson");

        migrationBuilder.RenameColumn(
            name: "metadata_json",
            schema: "finance",
            table: "scholarship",
            newName: "MetadataJson");

        migrationBuilder.RenameColumn(
            name: "metadata_json",
            schema: "finance",
            table: "discount",
            newName: "MetadataJson");

        // Cast back to text — always safe, no USING needed
        migrationBuilder.Sql(
            @"ALTER TABLE finance.studentfee
                  ALTER COLUMN ""MetadataJson"" TYPE text
                  USING ""MetadataJson""::text;");

        migrationBuilder.Sql(
            @"ALTER TABLE finance.scholarship
                  ALTER COLUMN ""MetadataJson"" TYPE text
                  USING ""MetadataJson""::text;");

        migrationBuilder.Sql(
            @"ALTER TABLE finance.discount
                  ALTER COLUMN ""MetadataJson"" TYPE text
                  USING ""MetadataJson""::text;");
    }
}
