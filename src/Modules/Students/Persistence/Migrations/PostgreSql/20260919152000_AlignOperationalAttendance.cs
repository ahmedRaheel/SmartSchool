using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Students.Persistence;

namespace SmartSchool.Modules.Students.Persistence.Migrations.PostgreSql;

[DbContext(typeof(StudentsDbContext))]
[Migration("20260919152000_AlignOperationalAttendance")]
public sealed class AlignOperationalAttendance : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE student.attendance
                ADD COLUMN IF NOT EXISTS student_id uuid,
                ADD COLUMN IF NOT EXISTS class_section_id uuid,
                ADD COLUMN IF NOT EXISTS attendance_date date,
                ADD COLUMN IF NOT EXISTS attendance_status text,
                ADD COLUMN IF NOT EXISTS remarks text,
                ADD COLUMN IF NOT EXISTS marked_by uuid;

            CREATE INDEX IF NOT EXISTS "IX_attendance_tenant_id_class_section_id_attendance_date"
                ON student.attendance (tenant_id, class_section_id, attendance_date);

            CREATE INDEX IF NOT EXISTS "IX_attendance_tenant_id_student_id_attendance_date"
                ON student.attendance (tenant_id, student_id, attendance_date);

            CREATE UNIQUE INDEX IF NOT EXISTS "UX_attendance_tenant_student_section_date"
                ON student.attendance (tenant_id, student_id, class_section_id, attendance_date);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP INDEX IF EXISTS student."UX_attendance_tenant_student_section_date";
            DROP INDEX IF EXISTS student."IX_attendance_tenant_id_student_id_attendance_date";
            DROP INDEX IF EXISTS student."IX_attendance_tenant_id_class_section_id_attendance_date";
            ALTER TABLE student.attendance
                DROP COLUMN IF EXISTS marked_by,
                DROP COLUMN IF EXISTS remarks,
                DROP COLUMN IF EXISTS attendance_status,
                DROP COLUMN IF EXISTS attendance_date,
                DROP COLUMN IF EXISTS class_section_id,
                DROP COLUMN IF EXISTS student_id;
            """);
    }
}
