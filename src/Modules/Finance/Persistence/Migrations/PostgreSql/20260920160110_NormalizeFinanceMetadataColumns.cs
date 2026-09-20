using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Finance.Persistence;

namespace SmartSchool.Modules.Finance.Persistence.Migrations.PostgreSql;

[DbContext(typeof(FinanceDbContext))]
[Migration("20260920160110_NormalizeFinanceMetadataColumns")]
public sealed class NormalizeFinanceMetadataColumns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            DECLARE tbl text;
            BEGIN
                FOREACH tbl IN ARRAY ARRAY['student_fee','discount','scholarship'] LOOP
                    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='finance' AND table_name=tbl AND column_name='MetadataJson')
                       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='finance' AND table_name=tbl AND column_name='metadata_json') THEN
                        EXECUTE format('ALTER TABLE finance.%I RENAME COLUMN %I TO metadata_json', tbl, 'MetadataJson');
                    END IF;
                END LOOP;
            END $$;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder) { }
}
