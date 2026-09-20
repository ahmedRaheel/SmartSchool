using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Organization.Persistence;

namespace SmartSchool.Modules.Organization.Persistence.Migrations.PostgreSql;

[DbContext(typeof(OrganizationDbContext))]
[Migration("20260919152100_AddTimetablePeriodRuntimeTable")]
public sealed class AddTimetablePeriodRuntimeTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS academic.timetable_period
            (
                timetable_period_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                campus_id uuid NOT NULL,
                period_number integer,
                name varchar(80) NOT NULL,
                start_time time without time zone NOT NULL,
                end_time time without time zone NOT NULL,
                period_type varchar(30) NOT NULL,
                is_active boolean NOT NULL DEFAULT TRUE,
                created_at timestamp with time zone NOT NULL DEFAULT now(),
                updated_at timestamp with time zone,
                row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text), 'hex'),
                CONSTRAINT "PK_timetable_period" PRIMARY KEY (timetable_period_id),
                CONSTRAINT "CK_timetable_period_time_range" CHECK (end_time > start_time)
            );

            CREATE INDEX IF NOT EXISTS "IX_timetable_period_tenant_id"
                ON academic.timetable_period (tenant_id);

            CREATE INDEX IF NOT EXISTS "IX_timetable_period_tenant_campus"
                ON academic.timetable_period (tenant_id, campus_id);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TABLE IF EXISTS academic.timetable_period;");
    }
}
