using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Activities.Persistence;

namespace SmartSchool.Modules.Activities.Persistence.Migrations.PostgreSql;

[DbContext(typeof(ActivitiesDbContext))]
[Migration("20260920160100_NormalizeStudentOfMonthMetadata")]
public sealed class NormalizeStudentOfMonthMetadata : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='activity' AND table_name='student_of_month' AND column_name='MetadataJson')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='activity' AND table_name='student_of_month' AND column_name='metadata_json') THEN
                    ALTER TABLE activity.student_of_month RENAME COLUMN "MetadataJson" TO metadata_json;
                END IF;
            END $$;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder) { }
}
