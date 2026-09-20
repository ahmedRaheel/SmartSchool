using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Activities.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class MissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetadataJson",
                schema: "activity",
                table: "student_of_month",
                newName: "metadata_json");


            migrationBuilder.Sql(
                @"ALTER TABLE activity.student_of_month
                  ALTER COLUMN metadata_json TYPE jsonb
                  USING metadata_json::jsonb;");            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "metadata_json",
                schema: "activity",
                table: "student_of_month",
                newName: "MetadataJson");

            migrationBuilder.Sql(
                  @"ALTER TABLE activity.student_of_month
                  ALTER COLUMN ""MetadataJson"" TYPE text
                  USING ""MetadataJson""::text;");
        }
    }
}
