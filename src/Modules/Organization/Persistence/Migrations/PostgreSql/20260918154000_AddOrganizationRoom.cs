using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Organization.Persistence;

#nullable disable

namespace SmartSchool.Modules.Organization.Persistence.Migrations.PostgreSql;

[DbContext(typeof(OrganizationDbContext))]
[Migration("20260918154000_AddOrganizationRoom")]
public sealed class AddOrganizationRoom : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "org");

        migrationBuilder.CreateTable(
            name: "room",
            schema: "org",
            columns: table => new
            {
                room_id = table.Column<Guid>(type: "uuid", nullable: false),
                campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                capacity = table.Column<int>(type: "integer", nullable: true),
                room_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_room", x => x.room_id);
                table.ForeignKey(
                    name: "FK_room_campus_campus_id",
                    column: x => x.campus_id,
                    principalSchema: "org",
                    principalTable: "campus",
                    principalColumn: "campus_id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_room_campus_id_code",
            schema: "org",
            table: "room",
            columns: new[] { "campus_id", "code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_room_tenant_id",
            schema: "org",
            table: "room",
            column: "tenant_id");

        migrationBuilder.CreateIndex(
            name: "IX_class_section_room_id",
            schema: "academic",
            table: "class_section",
            column: "room_id");

        migrationBuilder.CreateIndex(
            name: "IX_timetable_entry_room_id",
            schema: "academic",
            table: "timetable_entry",
            column: "room_id");

        // NOT VALID avoids failing a deployment because of legacy orphan room ids.
        // PostgreSQL still enforces the constraint for new/updated rows.
        migrationBuilder.Sql("""
            ALTER TABLE academic.class_section
            ADD CONSTRAINT "FK_class_section_room_room_id"
            FOREIGN KEY (room_id)
            REFERENCES org.room(room_id)
            ON DELETE RESTRICT
            NOT VALID;
            """);

        migrationBuilder.Sql("""
            ALTER TABLE academic.timetable_entry
            ADD CONSTRAINT "FK_timetable_entry_room_room_id"
            FOREIGN KEY (room_id)
            REFERENCES org.room(room_id)
            ON DELETE RESTRICT
            NOT VALID;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE IF EXISTS academic.timetable_entry
            DROP CONSTRAINT IF EXISTS "FK_timetable_entry_room_room_id";
            """);

        migrationBuilder.Sql("""
            ALTER TABLE IF EXISTS academic.class_section
            DROP CONSTRAINT IF EXISTS "FK_class_section_room_room_id";
            """);

        migrationBuilder.DropIndex(
            name: "IX_timetable_entry_room_id",
            schema: "academic",
            table: "timetable_entry");

        migrationBuilder.DropIndex(
            name: "IX_class_section_room_id",
            schema: "academic",
            table: "class_section");

        migrationBuilder.DropTable(
            name: "room",
            schema: "org");
    }
}
