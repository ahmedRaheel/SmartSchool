using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Organization.Persistence;

namespace SmartSchool.Modules.Organization.Persistence.Migrations.PostgreSql;

[DbContext(typeof(OrganizationDbContext))]
[Migration("20260920160020_AddAcademicRuntimeReferenceTables")]
public sealed class AddAcademicRuntimeReferenceTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS academic.teaching_group
            (
                teaching_group_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                code varchar(100) NOT NULL,
                name varchar(250) NOT NULL,
                metadata_json jsonb NULL,
                is_active boolean NOT NULL DEFAULT TRUE,
                created_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at timestamptz NULL,
                row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text), 'hex'),
                CONSTRAINT pk_teaching_group PRIMARY KEY (teaching_group_id)
            );

            CREATE INDEX IF NOT EXISTS ix_teaching_group_tenant_id
                ON academic.teaching_group (tenant_id);

            CREATE UNIQUE INDEX IF NOT EXISTS ux_teaching_group_tenant_code
                ON academic.teaching_group (tenant_id, code);

            CREATE TABLE IF NOT EXISTS academic.program_subject
            (
                program_subject_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                program_id uuid NULL,
                subject_id uuid NOT NULL,
                code varchar(100) NOT NULL,
                name varchar(250) NOT NULL,
                metadata_json jsonb NULL,
                is_active boolean NOT NULL DEFAULT TRUE,
                created_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at timestamptz NULL,
                row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text), 'hex'),
                CONSTRAINT pk_program_subject PRIMARY KEY (program_subject_id)
            );

            CREATE INDEX IF NOT EXISTS ix_program_subject_tenant_id
                ON academic.program_subject (tenant_id);
            CREATE INDEX IF NOT EXISTS ix_program_subject_subject_id
                ON academic.program_subject (subject_id);
            CREATE INDEX IF NOT EXISTS ix_program_subject_program_id
                ON academic.program_subject (program_id);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE IF EXISTS academic.program_subject;
            DROP TABLE IF EXISTS academic.teaching_group;
            """);
    }
}
