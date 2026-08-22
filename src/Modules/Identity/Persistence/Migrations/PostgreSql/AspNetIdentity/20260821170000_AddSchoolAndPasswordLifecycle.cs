using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace SmartSchool.Modules.Identity.Persistence.Migrations.PostgreSql.AspNetIdentity;

public partial class AddSchoolAndPasswordLifecycle : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "SchoolId", schema: "identity", table: "AspNetUsers",
            type: "uuid", nullable: true);
        migrationBuilder.AddColumn<bool>(
            name: "MustChangePassword", schema: "identity", table: "AspNetUsers",
            type: "boolean", nullable: false, defaultValue: false);
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "PasswordChangedAt", schema: "identity", table: "AspNetUsers",
            type: "timestamp with time zone", nullable: true);
        migrationBuilder.CreateIndex(
            name: "IX_AspNetUsers_TenantId_SchoolId", schema: "identity",
            table: "AspNetUsers", columns: new[] { "TenantId", "SchoolId" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_AspNetUsers_TenantId_SchoolId", schema: "identity", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "SchoolId", schema: "identity", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "MustChangePassword", schema: "identity", table: "AspNetUsers");
        migrationBuilder.DropColumn(name: "PasswordChangedAt", schema: "identity", table: "AspNetUsers");
    }
}
