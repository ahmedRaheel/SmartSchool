using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.AICore.Agents;

/// <summary>
/// Exposes tenant-safe SmartSchool read tools to MCP agents.
/// Tools deliberately call existing module query abstractions and never access a database directly.
/// </summary>
[McpServerToolType]
public sealed class SmartSchoolAgentTools(
    ICurrentUser currentUser,
    ITenantScope tenantScope,
    SmartSchoolAgentToolsStudentQuery studentQuery,
    SmartSchoolAgentToolsStudentExamResultQuery examResultQuery,
    SmartSchoolAgentToolsStudentPerformancePredictionQuery predictionQuery)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [McpServerTool(Name = "get_student_profile")]
    [Description("Gets the authenticated tenant's student profile. Students can only request their own profile.")]
    public async Task<string> GetStudentProfileAsync(
        [Description("Student identifier. For a student login this must be the student's own id.")] Guid studentId,
        CancellationToken cancellationToken)
    {
        var tenantId = ResolveTenant();
        EnsureStudentAccess(studentId);

        var student = await studentQuery.GetByIdAsync(tenantId, studentId, cancellationToken);
        if (student is null)
        {
            return JsonSerializer.Serialize(new { found = false }, JsonOptions);
        }

        EnsureSchoolAndBranchAccess(student.SchoolId, student.BranchId);

        return JsonSerializer.Serialize(
            new
            {
                found = true,
                student.StudentId,
                student.StudentNumber,
                student.FirstName,
                student.LastName,
                student.Gender,
                student.SchoolId,
                student.BranchId,
                student.Status
            },
            JsonOptions);
    }

    [McpServerTool(Name = "get_student_exam_results")]
    [Description("Gets exam results for a student from the existing Examination query service.")]
    public async Task<string> GetStudentExamResultsAsync(
        [Description("Student identifier.")] Guid studentId,
        [Description("Maximum number of records to inspect. Maximum 100.")] int limit = 25,
        CancellationToken cancellationToken = default)
    {
        var tenantId = ResolveTenant();
        EnsureStudentAccess(studentId);
        await EnsureStudentExistsAndInScopeAsync(tenantId, studentId, cancellationToken);

        var results = (await examResultQuery.GetByStudentIdAsync(
                tenantId,
                studentId,
                limit,
                cancellationToken))
            .Select(item => new
            {
                item.StudentExamResultId,
                item.ExamSubjectId,
                item.MarksObtained,
                item.Percentage,
                item.Grade,
                item.IsAbsent,
                item.Remarks
            })
            .ToArray();

        return JsonSerializer.Serialize(results, JsonOptions);
    }

    [McpServerTool(Name = "get_student_predictions")]
    [Description("Gets AI performance predictions for a student from the existing prediction query service.")]
    public async Task<string> GetStudentPredictionsAsync(
        [Description("Student identifier.")] Guid studentId,
        [Description("Maximum number of records to inspect. Maximum 100.")] int limit = 25,
        CancellationToken cancellationToken = default)
    {
        var tenantId = ResolveTenant();
        EnsureStudentAccess(studentId);
        await EnsureStudentExistsAndInScopeAsync(tenantId, studentId, cancellationToken);

        var predictions = (await predictionQuery.GetByStudentIdAsync(
                tenantId,
                studentId,
                limit,
                cancellationToken))
            .Select(item => new
            {
                item.StudentPerformancePredictionId,
                item.SubjectId,
                item.PredictedPercentage,
                item.PredictedGrade,
                item.ConfidenceScore,
                item.PassProbability,
                item.FailProbability,
                item.Trend,
                item.RiskLevel,
                item.ExplanationSummary,
                item.GeneratedAt
            })
            .ToArray();

        return JsonSerializer.Serialize(predictions, JsonOptions);
    }

    private Guid ResolveTenant()
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("An authenticated user is required to execute AI tools.");
        }

        return tenantScope.Resolve()
            ?? throw new UnauthorizedAccessException("A tenant context is required to execute AI tools.");
    }

    private void EnsureStudentAccess(Guid studentId)
    {
        if (currentUser.StudentId.HasValue && currentUser.StudentId.Value != studentId)
        {
            throw new UnauthorizedAccessException("Students can only access their own AI context.");
        }
    }

    private async Task EnsureStudentExistsAndInScopeAsync(
        Guid tenantId,
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var student = await studentQuery.GetByIdAsync(tenantId, studentId, cancellationToken)
            ?? throw new InvalidOperationException("The requested student does not exist in the current tenant.");

        EnsureSchoolAndBranchAccess(student.SchoolId, student.BranchId);
    }

    private void EnsureSchoolAndBranchAccess(Guid schoolId, Guid branchId)
    {
        if (currentUser.IsSuperAdmin)
        {
            return;
        }

        if (currentUser.SchoolId.HasValue && currentUser.SchoolId.Value != schoolId)
        {
            throw new UnauthorizedAccessException("The requested student is outside the authenticated school scope.");
        }

        if (currentUser.BranchId.HasValue && currentUser.BranchId.Value != branchId)
        {
            throw new UnauthorizedAccessException("The requested student is outside the authenticated branch scope.");
        }
    }
}

/// <summary>
/// Feature-owned data access for SmartSchoolAgentTools. Do not share across slices.
/// </summary>
public sealed class SmartSchoolAgentToolsStudentQuery(IDbConnectionFactory connectionFactory)
{
    public sealed record Row(Guid StudentId, string? StudentNumber, string FirstName, string? LastName, string? Gender, Guid SchoolId, Guid BranchId, string Status);

    public async Task<Row?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT student_id AS "StudentId", student_number AS "StudentNumber", first_name AS "FirstName",
                   last_name AS "LastName", gender AS "Gender", school_id AS "SchoolId", branch_id AS "BranchId", status AS "Status"
            FROM student.student
            WHERE tenant_id=@TenantId AND student_id=@Id AND is_active=TRUE;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Row>(new CommandDefinition(sql, new { TenantId=tenantId, Id=id }, cancellationToken:cancellationToken));
    }
}

public sealed class SmartSchoolAgentToolsStudentPerformancePredictionQuery(IDbConnectionFactory connectionFactory)
{
    public sealed record Row(Guid StudentPerformancePredictionId, Guid? SubjectId, decimal? PredictedPercentage, string? PredictedGrade, decimal? ConfidenceScore, decimal? PassProbability, decimal? FailProbability, string? Trend, string? RiskLevel, string? ExplanationSummary, DateTimeOffset GeneratedAt);

    public async Task<IReadOnlyCollection<Row>> GetByStudentIdAsync(Guid tenantId, Guid studentId, int limit, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT student_performance_prediction_id AS "StudentPerformancePredictionId", subject_id AS "SubjectId",
                   predicted_percentage AS "PredictedPercentage", predicted_grade AS "PredictedGrade", confidence_score AS "ConfidenceScore",
                   pass_probability AS "PassProbability", fail_probability AS "FailProbability", trend AS "Trend", risk_level AS "RiskLevel",
                   explanation_summary AS "ExplanationSummary", generated_at AS "GeneratedAt"
            FROM ai.student_performance_prediction
            WHERE tenant_id=@TenantId AND student_id=@StudentId AND is_active=TRUE
            ORDER BY generated_at DESC LIMIT @Limit;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<Row>(new CommandDefinition(sql, new { TenantId=tenantId, StudentId=studentId, Limit=Math.Clamp(limit,1,100) }, cancellationToken:cancellationToken))).AsList();
    }
}

public sealed class SmartSchoolAgentToolsStudentExamResultQuery(IDbConnectionFactory connectionFactory)
{
    public sealed record Row(Guid StudentExamResultId, Guid ExamSubjectId, decimal? MarksObtained, decimal? Percentage, string? Grade, bool IsAbsent, string? Remarks);

    public async Task<IReadOnlyCollection<Row>> GetByStudentIdAsync(Guid tenantId, Guid studentId, int limit, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT student_exam_result_id AS "StudentExamResultId", exam_subject_id AS "ExamSubjectId", marks_obtained AS "MarksObtained",
                   percentage AS "Percentage", grade AS "Grade", is_absent AS "IsAbsent", remarks AS "Remarks"
            FROM exam.student_exam_result
            WHERE tenant_id=@TenantId AND student_id=@StudentId AND is_active=TRUE
            ORDER BY created_at DESC LIMIT @Limit;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<Row>(new CommandDefinition(sql, new { TenantId=tenantId, StudentId=studentId, Limit=Math.Clamp(limit,1,100) }, cancellationToken:cancellationToken))).AsList();
    }
}
