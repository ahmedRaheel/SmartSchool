using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchool.Modules.AIPrediction.Persistence.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class InitialAIPrediction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ai");

            migrationBuilder.CreateTable(
                name: "class_performance_insight",
                schema: "ai",
                columns: table => new
                {
                    class_performance_insight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    students_count = table.Column<int>(type: "integer", nullable: false),
                    on_track_count = table.Column<int>(type: "integer", nullable: false),
                    needs_attention_count = table.Column<int>(type: "integer", nullable: false),
                    high_risk_count = table.Column<int>(type: "integer", nullable: false),
                    predicted_class_average = table.Column<decimal>(type: "numeric", nullable: true),
                    current_class_average = table.Column<decimal>(type: "numeric", nullable: true),
                    trend = table.Column<string>(type: "text", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_class_performance_insight", x => x.class_performance_insight_id);
                });

            migrationBuilder.CreateTable(
                name: "ml_prediction_result",
                schema: "ai",
                columns: table => new
                {
                    ml_prediction_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prediction_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    score = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    probability = table.Column<decimal>(type: "numeric(8,6)", precision: 8, scale: 6, nullable: false),
                    risk_level = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    outcome = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    confidence_score = table.Column<decimal>(type: "numeric(8,6)", precision: 8, scale: 6, nullable: false),
                    model_version = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    used_machine_learning = table.Column<bool>(type: "boolean", nullable: false),
                    factors_json = table.Column<string>(type: "jsonb", nullable: true),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ml_prediction_result", x => x.ml_prediction_result_id);
                });

            migrationBuilder.CreateTable(
                name: "prediction_model",
                schema: "ai",
                columns: table => new
                {
                    prediction_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prediction_type = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_prediction_model", x => x.prediction_model_id);
                });

            migrationBuilder.CreateTable(
                name: "teaching_recommendation",
                schema: "ai",
                columns: table => new
                {
                    teaching_recommendation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_performance_insight_id = table.Column<Guid>(type: "uuid", nullable: true),
                    class_section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    topic = table.Column<string>(type: "text", nullable: true),
                    recommendation_type = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    recommendation_text = table.Column<string>(type: "text", nullable: false),
                    rationale = table.Column<string>(type: "text", nullable: true),
                    priority = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    reviewed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    teacher_comments = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_teaching_recommendation", x => x.teaching_recommendation_id);
                    table.ForeignKey(
                        name: "FK_teaching_recommendation_class_performance_insight_class_per~",
                        column: x => x.class_performance_insight_id,
                        principalSchema: "ai",
                        principalTable: "class_performance_insight",
                        principalColumn: "class_performance_insight_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "topic_performance_insight",
                schema: "ai",
                columns: table => new
                {
                    topic_performance_insight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_performance_insight_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic = table.Column<string>(type: "text", nullable: false),
                    average_mastery_score = table.Column<decimal>(type: "numeric", nullable: true),
                    students_struggling_count = table.Column<int>(type: "integer", nullable: false),
                    students_mastered_count = table.Column<int>(type: "integer", nullable: false),
                    risk_level = table.Column<string>(type: "text", nullable: true),
                    recommended_focus = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_topic_performance_insight", x => x.topic_performance_insight_id);
                    table.ForeignKey(
                        name: "FK_topic_performance_insight_class_performance_insight_class_p~",
                        column: x => x.class_performance_insight_id,
                        principalSchema: "ai",
                        principalTable: "class_performance_insight",
                        principalColumn: "class_performance_insight_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prediction",
                schema: "ai",
                columns: table => new
                {
                    prediction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_exam_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_exam_subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_exam_type_code = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    predicted_marks = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    predicted_percentage = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    predicted_grade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    lower_bound_percentage = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    upper_bound_percentage = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    confidence_score = table.Column<decimal>(type: "numeric(7,4)", precision: 7, scale: 4, nullable: false),
                    pass_probability = table.Column<decimal>(type: "numeric(7,4)", precision: 7, scale: 4, nullable: false),
                    trend = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    risk_level = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    model_version = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    historical_result_count = table.Column<int>(type: "integer", nullable: false),
                    used_machine_learning = table.Column<bool>(type: "boolean", nullable: false),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    prediction_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    prediction_type = table.Column<string>(type: "text", nullable: false),
                    score = table.Column<decimal>(type: "numeric", nullable: true),
                    explanation = table.Column<string>(type: "text", nullable: true),
                    predicted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    row_version = table.Column<byte[]>(type: "bytea", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prediction", x => x.prediction_id);
                    table.ForeignKey(
                        name: "FK_prediction_prediction_model_prediction_model_id",
                        column: x => x.prediction_model_id,
                        principalSchema: "ai",
                        principalTable: "prediction_model",
                        principalColumn: "prediction_model_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_performance_prediction",
                schema: "ai",
                columns: table => new
                {
                    student_performance_prediction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    academic_year_id = table.Column<Guid>(type: "uuid", nullable: false),
                    term_id = table.Column<Guid>(type: "uuid", nullable: true),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_exam_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_exam_subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_exam_type_code = table.Column<string>(type: "text", nullable: true),
                    target_date = table.Column<DateOnly>(type: "date", nullable: true),
                    predicted_marks = table.Column<decimal>(type: "numeric", nullable: true),
                    predicted_percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    predicted_grade = table.Column<string>(type: "text", nullable: true),
                    lower_bound_percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    upper_bound_percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    confidence_score = table.Column<decimal>(type: "numeric", nullable: true),
                    pass_probability = table.Column<decimal>(type: "numeric", nullable: true),
                    fail_probability = table.Column<decimal>(type: "numeric", nullable: true),
                    target_grade = table.Column<string>(type: "text", nullable: true),
                    target_grade_probability = table.Column<decimal>(type: "numeric", nullable: true),
                    trend = table.Column<string>(type: "text", nullable: true),
                    risk_level = table.Column<string>(type: "text", nullable: true),
                    explanation_summary = table.Column<string>(type: "text", nullable: true),
                    explanation = table.Column<string>(type: "text", nullable: true),
                    prediction_model_id = table.Column<Guid>(type: "uuid", nullable: true),
                    model_version = table.Column<string>(type: "text", nullable: true),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_student_performance_prediction", x => x.student_performance_prediction_id);
                    table.ForeignKey(
                        name: "FK_student_performance_prediction_prediction_model_prediction_~",
                        column: x => x.prediction_model_id,
                        principalSchema: "ai",
                        principalTable: "prediction_model",
                        principalColumn: "prediction_model_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prediction_evaluation",
                schema: "ai",
                columns: table => new
                {
                    prediction_evaluation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_performance_prediction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_exam_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    predicted_percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    actual_percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    absolute_error = table.Column<decimal>(type: "numeric", nullable: true),
                    predicted_grade = table.Column<string>(type: "text", nullable: true),
                    actual_grade = table.Column<string>(type: "text", nullable: true),
                    grade_correct = table.Column<bool>(type: "boolean", nullable: true),
                    evaluated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_prediction_evaluation", x => x.prediction_evaluation_id);
                    table.ForeignKey(
                        name: "FK_prediction_evaluation_student_performance_prediction_studen~",
                        column: x => x.student_performance_prediction_id,
                        principalSchema: "ai",
                        principalTable: "student_performance_prediction",
                        principalColumn: "student_performance_prediction_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prediction_evidence",
                schema: "ai",
                columns: table => new
                {
                    prediction_evidence_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_performance_prediction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    evidence_type = table.Column<string>(type: "text", nullable: false),
                    source_entity_type = table.Column<string>(type: "text", nullable: true),
                    source_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    numeric_value = table.Column<decimal>(type: "numeric", nullable: true),
                    text_value = table.Column<string>(type: "text", nullable: true),
                    normalized_value = table.Column<decimal>(type: "numeric", nullable: true),
                    weight = table.Column<decimal>(type: "numeric", nullable: true),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    explanation = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_prediction_evidence", x => x.prediction_evidence_id);
                    table.ForeignKey(
                        name: "FK_prediction_evidence_student_performance_prediction_student_~",
                        column: x => x.student_performance_prediction_id,
                        principalSchema: "ai",
                        principalTable: "student_performance_prediction",
                        principalColumn: "student_performance_prediction_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_intervention",
                schema: "ai",
                columns: table => new
                {
                    student_intervention_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teacher_employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_prediction_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_recommendation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    target_outcome = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    target_date = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_student_intervention", x => x.student_intervention_id);
                    table.ForeignKey(
                        name: "FK_student_intervention_student_performance_prediction_source_~",
                        column: x => x.source_prediction_id,
                        principalSchema: "ai",
                        principalTable: "student_performance_prediction",
                        principalColumn: "student_performance_prediction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_intervention_teaching_recommendation_source_recomme~",
                        column: x => x.source_recommendation_id,
                        principalSchema: "ai",
                        principalTable: "teaching_recommendation",
                        principalColumn: "teaching_recommendation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_performance_insight_tenant_id",
                schema: "ai",
                table: "class_performance_insight",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_performance_insight_tenant_id_code",
                schema: "ai",
                table: "class_performance_insight",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ml_prediction_result_tenant_id_prediction_type_student_id",
                schema: "ai",
                table: "ml_prediction_result",
                columns: new[] { "tenant_id", "prediction_type", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_prediction_prediction_model_id",
                schema: "ai",
                table: "prediction",
                column: "prediction_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_tenant_id_student_id_subject_id_generated_at",
                schema: "ai",
                table: "prediction",
                columns: new[] { "tenant_id", "student_id", "subject_id", "generated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evaluation_student_performance_prediction_id",
                schema: "ai",
                table: "prediction_evaluation",
                column: "student_performance_prediction_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evaluation_tenant_id",
                schema: "ai",
                table: "prediction_evaluation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evaluation_tenant_id_code",
                schema: "ai",
                table: "prediction_evaluation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evidence_student_performance_prediction_id",
                schema: "ai",
                table: "prediction_evidence",
                column: "student_performance_prediction_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evidence_tenant_id",
                schema: "ai",
                table: "prediction_evidence",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_evidence_tenant_id_code",
                schema: "ai",
                table: "prediction_evidence",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prediction_model_tenant_id",
                schema: "ai",
                table: "prediction_model",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_prediction_model_tenant_id_code",
                schema: "ai",
                table: "prediction_model",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_intervention_source_prediction_id",
                schema: "ai",
                table: "student_intervention",
                column: "source_prediction_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_intervention_source_recommendation_id",
                schema: "ai",
                table: "student_intervention",
                column: "source_recommendation_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_intervention_tenant_id",
                schema: "ai",
                table: "student_intervention",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_intervention_tenant_id_code",
                schema: "ai",
                table: "student_intervention",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_performance_prediction_prediction_model_id",
                schema: "ai",
                table: "student_performance_prediction",
                column: "prediction_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_performance_prediction_tenant_id",
                schema: "ai",
                table: "student_performance_prediction",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_performance_prediction_tenant_id_code",
                schema: "ai",
                table: "student_performance_prediction",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teaching_recommendation_class_performance_insight_id",
                schema: "ai",
                table: "teaching_recommendation",
                column: "class_performance_insight_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_recommendation_tenant_id",
                schema: "ai",
                table: "teaching_recommendation",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_recommendation_tenant_id_code",
                schema: "ai",
                table: "teaching_recommendation",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topic_performance_insight_class_performance_insight_id",
                schema: "ai",
                table: "topic_performance_insight",
                column: "class_performance_insight_id");

            migrationBuilder.CreateIndex(
                name: "IX_topic_performance_insight_tenant_id",
                schema: "ai",
                table: "topic_performance_insight",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_topic_performance_insight_tenant_id_code",
                schema: "ai",
                table: "topic_performance_insight",
                columns: new[] { "tenant_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ml_prediction_result",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "prediction",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "prediction_evaluation",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "prediction_evidence",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "student_intervention",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "topic_performance_insight",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "student_performance_prediction",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "teaching_recommendation",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "prediction_model",
                schema: "ai");

            migrationBuilder.DropTable(
                name: "class_performance_insight",
                schema: "ai");
        }
    }
}
