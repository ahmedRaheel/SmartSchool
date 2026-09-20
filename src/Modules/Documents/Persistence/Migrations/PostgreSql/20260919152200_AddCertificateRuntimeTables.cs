using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Documents.Persistence;

namespace SmartSchool.Modules.Documents.Persistence.Migrations.PostgreSql;

[DbContext(typeof(DocumentsDbContext))]
[Migration("20260919152200_AddCertificateRuntimeTables")]
public sealed class AddCertificateRuntimeTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS document.document_template
            (
                document_template_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                campus_id uuid,
                academic_system_id uuid,
                document_type_code text NOT NULL,
                subject_template text,
                header_html text,
                body_html text NOT NULL,
                footer_html text,
                language_code text NOT NULL,
                version integer NOT NULL,
                requires_approval boolean NOT NULL,
                code varchar(100) NOT NULL,
                name varchar(250) NOT NULL,
                metadata_json jsonb,
                is_active boolean NOT NULL,
                created_at timestamp with time zone NOT NULL,
                updated_at timestamp with time zone,
                row_version bytea NOT NULL,
                CONSTRAINT "PK_document_template" PRIMARY KEY (document_template_id)
            );

            CREATE INDEX IF NOT EXISTS "IX_document_template_tenant_id"
                ON document.document_template (tenant_id);
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_document_template_tenant_id_code"
                ON document.document_template (tenant_id, code);

            CREATE TABLE IF NOT EXISTS document.generated_document
            (
                generated_document_id uuid NOT NULL,
                tenant_id uuid NOT NULL,
                document_template_id uuid NOT NULL,
                template_version integer NOT NULL,
                student_id uuid,
                employee_id uuid,
                document_number text NOT NULL,
                rendered_content_snapshot text NOT NULL,
                file_url text,
                verification_code text,
                issued_by uuid,
                approved_by uuid,
                issued_at timestamp with time zone,
                status text NOT NULL,
                code varchar(100) NOT NULL,
                name varchar(250) NOT NULL,
                metadata_json jsonb,
                is_active boolean NOT NULL,
                created_at timestamp with time zone NOT NULL,
                updated_at timestamp with time zone,
                row_version bytea NOT NULL,
                CONSTRAINT "PK_generated_document" PRIMARY KEY (generated_document_id),
                CONSTRAINT "FK_generated_document_document_template"
                    FOREIGN KEY (document_template_id)
                    REFERENCES document.document_template (document_template_id)
                    ON DELETE RESTRICT
            );

            CREATE INDEX IF NOT EXISTS "IX_generated_document_tenant_id"
                ON document.generated_document (tenant_id);
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_generated_document_tenant_id_code"
                ON document.generated_document (tenant_id, code);
            CREATE INDEX IF NOT EXISTS "IX_generated_document_template_id"
                ON document.generated_document (document_template_id);
            CREATE INDEX IF NOT EXISTS "IX_generated_document_verification_code"
                ON document.generated_document (verification_code);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE IF EXISTS document.generated_document;
            DROP TABLE IF EXISTS document.document_template;
            """);
    }
}
