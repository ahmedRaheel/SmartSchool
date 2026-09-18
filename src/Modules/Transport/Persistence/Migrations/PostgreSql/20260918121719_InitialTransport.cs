using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Transport.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialTransport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transport");

            migrationBuilder.CreateTable(
                name: "driver",
                schema: "transport",
                columns: table => new
                {
                    driver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_number = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    cnic = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    mobile_number = table.Column<string>(type: "text", nullable: false),
                    driving_license_number = table.Column<string>(type: "text", nullable: false),
                    driving_license_category = table.Column<string>(type: "text", nullable: false),
                    license_expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    joining_date = table.Column<DateOnly>(type: "date", nullable: false),
                    employment_status_code = table.Column<string>(type: "text", nullable: false),
                    emergency_contact_name = table.Column<string>(type: "text", nullable: true),
                    emergency_contact_phone = table.Column<string>(type: "text", nullable: true),
                    assigned_vehicle_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    driver_number = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    cnic_number = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    alternate_phone = table.Column<string>(type: "text", nullable: true),
                    driving_license_issued_on = table.Column<DateOnly>(type: "date", nullable: true),
                    driving_license_expires_on = table.Column<DateOnly>(type: "date", nullable: true),
                    picture = table.Column<byte[]>(type: "bytea", nullable: true),
                    picture_content_type = table.Column<string>(type: "text", nullable: true),
                    picture_file_name = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver", x => x.driver_id);
                });

            migrationBuilder.CreateTable(
                name: "route",
                schema: "transport",
                columns: table => new
                {
                    route_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: true),
                    driver_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    arrival_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    dismissal_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route", x => x.route_id);
                });

            migrationBuilder.CreateTable(
                name: "route_notice",
                schema: "transport",
                columns: table => new
                {
                    route_notice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_date = table.Column<DateOnly>(type: "date", nullable: false),
                    delay_minutes = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_notice", x => x.route_notice_id);
                });

            migrationBuilder.CreateTable(
                name: "stop",
                schema: "transport",
                columns: table => new
                {
                    stop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    route_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sequence = table.Column<int>(type: "integer", nullable: false),
                    pickup_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    dropoff_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stop", x => x.stop_id);
                });

            migrationBuilder.CreateTable(
                name: "studenttransport",
                schema: "transport",
                columns: table => new
                {
                    student_transport_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    route_id = table.Column<Guid>(type: "uuid", nullable: true),
                    stop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studenttransport", x => x.student_transport_id);
                });

            migrationBuilder.CreateTable(
                name: "trip_record",
                schema: "transport",
                columns: table => new
                {
                    trip_record_id = table.Column<Guid>(type: "uuid", nullable: false),
                    route_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_date = table.Column<DateOnly>(type: "date", nullable: false),
                    direction = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    recorded_by = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_record", x => x.trip_record_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                schema: "transport",
                columns: table => new
                {
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_no = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.vehicle_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_driver_tenant_id",
                schema: "transport",
                table: "driver",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_route_tenant_id",
                schema: "transport",
                table: "route",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_route_tenant_id_code",
                schema: "transport",
                table: "route",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stop_tenant_id",
                schema: "transport",
                table: "stop",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_stop_tenant_id_code",
                schema: "transport",
                table: "stop",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_studenttransport_tenant_id",
                schema: "transport",
                table: "studenttransport",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_studenttransport_tenant_id_code",
                schema: "transport",
                table: "studenttransport",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trip_record_tenant_id_route_id_student_id_service_date_dire~",
                schema: "transport",
                table: "trip_record",
                columns: new[] { "tenant_id", "route_id", "student_id", "service_date", "direction" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_tenant_id",
                schema: "transport",
                table: "vehicle",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_tenant_id_code",
                schema: "transport",
                table: "vehicle",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "route",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "route_notice",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "stop",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "studenttransport",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "trip_record",
                schema: "transport");

            migrationBuilder.DropTable(
                name: "vehicle",
                schema: "transport");
        }
    }
}
