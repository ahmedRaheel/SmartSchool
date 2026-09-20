using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.AITutor.Persistence;

namespace SmartSchool.Modules.AITutor.Persistence.Migrations.PostgreSql;

[DbContext(typeof(AITutorDbContext))]
[Migration("20260920160040_AlignAITutorOperationalFields")]
public sealed class AlignAITutorOperationalFields : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("ALTER TABLE IF EXISTS ai_tutor.generated_quiz ALTER COLUMN subject_id DROP NOT NULL;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("UPDATE ai_tutor.generated_quiz SET subject_id = '00000000-0000-0000-0000-000000000000'::uuid WHERE subject_id IS NULL;");
        migrationBuilder.Sql("ALTER TABLE IF EXISTS ai_tutor.generated_quiz ALTER COLUMN subject_id SET NOT NULL;");
    }
}
