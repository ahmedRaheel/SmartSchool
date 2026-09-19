using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.Workflow.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workflow");

            migrationBuilder.CreateTable(
                name: "workflowdefinition",
                schema: "workflow",
                columns: table => new
                {
                    workflow_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    trigger_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflowdefinition", x => x.workflow_definition_id);
                });

            migrationBuilder.CreateTable(
                name: "workflowinstance",
                schema: "workflow",
                columns: table => new
                {
                    workflow_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workflow_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    current_step_order = table.Column<int>(type: "integer", nullable: false),
                    started_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    context_json = table.Column<string>(type: "jsonb", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflowinstance", x => x.workflow_instance_id);
                    table.ForeignKey(
                        name: "FK_workflowinstance_workflowdefinition_workflow_definition_id",
                        column: x => x.workflow_definition_id,
                        principalSchema: "workflow",
                        principalTable: "workflowdefinition",
                        principalColumn: "workflow_definition_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workflowstep",
                schema: "workflow",
                columns: table => new
                {
                    workflow_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workflow_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: false),
                    step_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    approver_role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    action_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_required = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflowstep", x => x.workflow_step_id);
                    table.ForeignKey(
                        name: "FK_workflowstep_workflowdefinition_workflow_definition_id",
                        column: x => x.workflow_definition_id,
                        principalSchema: "workflow",
                        principalTable: "workflowdefinition",
                        principalColumn: "workflow_definition_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "approval",
                schema: "workflow",
                columns: table => new
                {
                    approval_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workflow_instance_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workflow_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    assigned_role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    decision_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    decided_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    comments = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval", x => x.approval_id);
                    table.ForeignKey(
                        name: "FK_approval_workflowinstance_workflow_instance_id",
                        column: x => x.workflow_instance_id,
                        principalSchema: "workflow",
                        principalTable: "workflowinstance",
                        principalColumn: "workflow_instance_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_approval_workflowstep_workflow_step_id",
                        column: x => x.workflow_step_id,
                        principalSchema: "workflow",
                        principalTable: "workflowstep",
                        principalColumn: "workflow_step_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_approval_tenant_id_status_assigned_role",
                schema: "workflow",
                table: "approval",
                columns: new[] { "tenant_id", "status", "assigned_role" });

            migrationBuilder.CreateIndex(
                name: "IX_approval_tenant_id_workflow_instance_id_workflow_step_id",
                schema: "workflow",
                table: "approval",
                columns: new[] { "tenant_id", "workflow_instance_id", "workflow_step_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_approval_workflow_instance_id",
                schema: "workflow",
                table: "approval",
                column: "workflow_instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_approval_workflow_step_id",
                schema: "workflow",
                table: "approval",
                column: "workflow_step_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflowdefinition_tenant_id_code",
                schema: "workflow",
                table: "workflowdefinition",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workflowdefinition_tenant_id_entity_type_status",
                schema: "workflow",
                table: "workflowdefinition",
                columns: new[] { "tenant_id", "entity_type", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_workflowinstance_tenant_id_code",
                schema: "workflow",
                table: "workflowinstance",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workflowinstance_tenant_id_status_started_at",
                schema: "workflow",
                table: "workflowinstance",
                columns: new[] { "tenant_id", "status", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_workflowinstance_workflow_definition_id",
                schema: "workflow",
                table: "workflowinstance",
                column: "workflow_definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflowstep_tenant_id_workflow_definition_id_step_order",
                schema: "workflow",
                table: "workflowstep",
                columns: new[] { "tenant_id", "workflow_definition_id", "step_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workflowstep_workflow_definition_id",
                schema: "workflow",
                table: "workflowstep",
                column: "workflow_definition_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflowinstance",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflowstep",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflowdefinition",
                schema: "workflow");
        }
    }
}
