using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.HR.Persistence;

namespace SmartSchool.Modules.HR.Persistence.Migrations.PostgreSql;

[DbContext(typeof(HRDbContext))]
[Migration("20260920160030_AddJobFamilyRuntimeTable")]
public sealed class AddJobFamilyRuntimeTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS hr.job_family
            (
                job_family_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                code varchar(100) NOT NULL,
                name varchar(250) NOT NULL,
                metadata_json jsonb NULL,
                is_active boolean NOT NULL DEFAULT TRUE,
                created_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at timestamptz NULL,
                row_version bytea NOT NULL DEFAULT decode(md5(random()::text || clock_timestamp()::text), 'hex'),
                CONSTRAINT pk_job_family PRIMARY KEY (job_family_id)
            );

            CREATE INDEX IF NOT EXISTS ix_job_family_tenant_id
                ON hr.job_family (tenant_id);
            CREATE UNIQUE INDEX IF NOT EXISTS ux_job_family_tenant_code
                ON hr.job_family (tenant_id, code);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TABLE IF EXISTS hr.job_family;");
    }
}
