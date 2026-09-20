using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Library.Persistence;

namespace SmartSchool.Modules.Library.Persistence.Migrations.PostgreSql;

[DbContext(typeof(LibraryDbContext))]
[Migration("20260920160010_NormalizeReservationTable")]
public sealed class NormalizeReservationTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF to_regclass('library."Reservation"') IS NOT NULL
                   AND to_regclass('library.reservation') IS NULL THEN
                    ALTER TABLE library."Reservation" RENAME TO reservation;
                END IF;
            END $$;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF to_regclass('library.reservation') IS NOT NULL
                   AND to_regclass('library."Reservation"') IS NULL THEN
                    ALTER TABLE library.reservation RENAME TO "Reservation";
                END IF;
            END $$;
            """);
    }
}
