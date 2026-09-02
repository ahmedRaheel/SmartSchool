using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;
using SmartSchool.Modules.Students.Features.Student;

namespace SmartSchool.Modules.AICore.Agents;

/// <summary>
/// Exposes tenant-safe SmartSchool read tools to MCP agents.
/// Tools deliberately call existing module query abstractions and never access a database directly.
/// </summary>
[McpServerToolType]
public sealed class SmartSchoolAgentTools(
    ICurrentUser currentUser,
    ITenantScope tenantScope,
    IStudentQuery studentQuery,
    IStudentExamResultQuery examResultQuery,
    IStudentPerformancePredictionQuery predictionQuery)
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
