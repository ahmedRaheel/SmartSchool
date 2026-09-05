using System.Collections.Concurrent;
using Dapper;
using Microsoft.ML;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.AIPrediction.ML;

/// <summary>
/// Predicts exam performance with ML.NET regression. Historical data is read
/// with Dapper using explicit columns; no EF metadata or reflection is used.
/// </summary>
public sealed class MlNetExamPredictionService(
    IDbConnectionFactory connectionFactory,
    ILogger<MlNetExamPredictionService> logger) : IExamPredictionService
{
    private const int MinimumTrainingRows = 8;
    private const string ModelVersion = "mlnet-sdca-v1";
    private static readonly MLContext MlContext = new(seed: 42);
    private static readonly ConcurrentDictionary<string, CachedModel> Models = new();
    private static readonly TimeSpan ModelLifetime = TimeSpan.FromMinutes(30);

    public async Task<ExamPerformancePrediction> PredictAsync(
        PredictExamPerformanceRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.TargetExamTypeCode);

        var history = await LoadHistoryAsync(request, cancellationToken);
        var studentHistory = history
            .Where(x => x.StudentId == request.StudentId)
            .OrderBy(x => x.ExamDate)
            .ToArray();

        if (studentHistory.Length == 0)
        {
            throw new InvalidOperationException(
                "At least one historical exam result is required before a prediction can be generated.");
        }

        var targetTotalMarks = await ResolveTargetTotalMarksAsync(request, studentHistory, cancellationToken);
        var input = BuildPredictionInput(studentHistory, request.TargetExamTypeCode);
        var trainingRows = BuildTrainingRows(history);

        float predictedPercentage;
        var usedMachineLearning = trainingRows.Count >= MinimumTrainingRows;

        if (usedMachineLearning)
        {
            var model = GetOrTrainModel(request.TenantId, request.SubjectId, trainingRows);
            var engine = MlContext.Model.CreatePredictionEngine<ExamTrainingRow, ExamScorePrediction>(model);
            predictedPercentage = engine.Predict(input).Score;
        }
        else
        {
            // Safe cold-start fallback until enough labelled results exist.
            predictedPercentage =
                (input.RecentThreeAverage * 0.50f) +
                (input.AveragePercentage * 0.30f) +
                (input.PreviousPercentage * 0.20f);
        }

        predictedPercentage = Math.Clamp(predictedPercentage, 0f, 100f);
        var standardDeviation = CalculateStandardDeviation(studentHistory.Select(x => (float)x.Percentage));
        var confidence = CalculateConfidence(studentHistory.Length, standardDeviation, usedMachineLearning);
        var margin = Math.Clamp(standardDeviation * 0.75f, 3f, 15f);
        var lower = Math.Clamp(predictedPercentage - margin, 0f, 100f);
        var upper = Math.Clamp(predictedPercentage + margin, 0f, 100f);
        var predictedMarks = targetTotalMarks * (decimal)predictedPercentage / 100m;
        var passProbability = CalculatePassProbability(predictedPercentage, standardDeviation);

        return new ExamPerformancePrediction(
            Math.Round(predictedMarks, 2),
            Math.Round((decimal)predictedPercentage, 2),
            ToGrade(predictedPercentage),
            Math.Round((decimal)lower, 2),
            Math.Round((decimal)upper, 2),
            Math.Round((decimal)confidence, 4),
            Math.Round((decimal)passProbability, 4),
            ToTrend(input.Trend),
            ToRiskLevel(predictedPercentage, input.Trend),
            request.TargetExamTypeCode.Trim().ToUpperInvariant(),
            ModelVersion,
            studentHistory.Length,
            usedMachineLearning);
    }

    private async Task<IReadOnlyList<HistoricalExamResult>> LoadHistoryAsync(
        PredictExamPerformanceRequest request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                r.student_id AS "StudentId",
                COALESCE(es.exam_date, e.end_date, e.start_date, CURRENT_DATE) AS "ExamDate",
                e.exam_type_code AS "ExamTypeCode",
                COALESCE(r.percentage, (r.marks_obtained / NULLIF(es.total_marks, 0)) * 100) AS "Percentage",
                es.total_marks AS "TotalMarks"
            FROM exam.student_exam_result r
            JOIN exam.exam_subject es ON es.exam_subject_id = r.exam_subject_id
            JOIN exam.exam e ON e.exam_id = es.exam_id
            JOIN academic.course_offering co ON co.course_offering_id = es.course_offering_id
            JOIN academic.program_subject ps ON ps.program_subject_id = co.program_subject_id
            WHERE e.tenant_id = @TenantId
              AND ps.subject_id = @SubjectId
              AND r.is_absent = FALSE
              AND r.marks_obtained IS NOT NULL
              AND COALESCE(r.percentage, (r.marks_obtained / NULLIF(es.total_marks, 0)) * 100) IS NOT NULL
            ORDER BY r.student_id, COALESCE(es.exam_date, e.end_date, e.start_date);
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<HistoricalExamResult>(
            new CommandDefinition(
                sql,
                new { request.TenantId, request.SubjectId },
                cancellationToken: cancellationToken))).AsList();
    }

    private async Task<decimal> ResolveTargetTotalMarksAsync(
        PredictExamPerformanceRequest request,
        IReadOnlyList<HistoricalExamResult> studentHistory,
        CancellationToken cancellationToken)
    {
        if (request.TargetExamSubjectId is null)
        {
            return studentHistory.Last().TotalMarks;
        }

        const string sql = """
            SELECT total_marks
            FROM exam.exam_subject
            WHERE exam_subject_id = @TargetExamSubjectId;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<decimal>(
            new CommandDefinition(
                sql,
                new { request.TargetExamSubjectId },
                cancellationToken: cancellationToken));
    }

    private ITransformer GetOrTrainModel(Guid tenantId, Guid subjectId, IReadOnlyList<ExamTrainingRow> rows)
    {
        var key = $"{tenantId:N}:{subjectId:N}";
        if (Models.TryGetValue(key, out var cached) && DateTimeOffset.UtcNow - cached.TrainedAt < ModelLifetime)
        {
            return cached.Model;
        }

        var data = MlContext.Data.LoadFromEnumerable(rows);
        var pipeline = MlContext.Transforms.Categorical.OneHotEncoding(
            outputColumnName: "ExamTypeEncoded",
            inputColumnName: nameof(ExamTrainingRow.ExamTypeCode))
            .Append(MlContext.Transforms.Concatenate(
                "Features",
                nameof(ExamTrainingRow.PreviousPercentage),
                nameof(ExamTrainingRow.AveragePercentage),
                nameof(ExamTrainingRow.RecentThreeAverage),
                nameof(ExamTrainingRow.Trend),
                nameof(ExamTrainingRow.ResultCount),
                "ExamTypeEncoded"))
            .Append(MlContext.Regression.Trainers.Sdca(
                labelColumnName: nameof(ExamTrainingRow.Label),
                featureColumnName: "Features"));

        var model = pipeline.Fit(data);
        Models[key] = new CachedModel(model, DateTimeOffset.UtcNow);
        logger.LogInformation(
            "Trained ML.NET exam prediction model for tenant {TenantId}, subject {SubjectId} using {RowCount} rows",
            tenantId,
            subjectId,
            rows.Count);
        return model;
    }

    private static List<ExamTrainingRow> BuildTrainingRows(IReadOnlyList<HistoricalExamResult> history)
    {
        var rows = new List<ExamTrainingRow>();
        foreach (var studentGroup in history.GroupBy(x => x.StudentId))
        {
            var ordered = studentGroup.OrderBy(x => x.ExamDate).ToArray();
            for (var index = 1; index < ordered.Length; index++)
            {
                var previous = ordered.Take(index).Select(x => (float)x.Percentage).ToArray();
                rows.Add(BuildRow(previous, ordered[index].ExamTypeCode, (float)ordered[index].Percentage));
            }
        }
        return rows;
    }

    private static ExamTrainingRow BuildPredictionInput(
        IReadOnlyList<HistoricalExamResult> history,
        string targetExamTypeCode)
    {
        return BuildRow(history.Select(x => (float)x.Percentage).ToArray(), targetExamTypeCode, 0f);
    }

    private static ExamTrainingRow BuildRow(float[] previous, string examTypeCode, float label)
    {
        var recent = previous.TakeLast(Math.Min(3, previous.Length)).ToArray();
        var trend = previous.Length < 2 ? 0f : previous[^1] - previous[^2];
        return new ExamTrainingRow
        {
            PreviousPercentage = previous[^1],
            AveragePercentage = previous.Average(),
            RecentThreeAverage = recent.Average(),
            Trend = trend,
            ResultCount = previous.Length,
            ExamTypeCode = examTypeCode.Trim().ToUpperInvariant(),
            Label = label
        };
    }

    private static float CalculateStandardDeviation(IEnumerable<float> values)
    {
        var data = values.ToArray();
        if (data.Length < 2) return 8f;
        var average = data.Average();
        return MathF.Sqrt(data.Sum(x => MathF.Pow(x - average, 2)) / data.Length);
    }

    private static float CalculateConfidence(int count, float standardDeviation, bool usedMachineLearning)
    {
        var volume = Math.Clamp(count / 8f, 0.25f, 1f);
        var stability = Math.Clamp(1f - (standardDeviation / 35f), 0.25f, 1f);
        var modelFactor = usedMachineLearning ? 1f : 0.75f;
        return Math.Clamp(volume * stability * modelFactor, 0.15f, 0.98f);
    }

    private static float CalculatePassProbability(float percentage, float standardDeviation)
    {
        const float passMark = 40f;
        var scale = Math.Max(standardDeviation, 5f);
        return 1f / (1f + MathF.Exp(-(percentage - passMark) / scale));
    }

    private static string ToGrade(float percentage) => percentage switch
    {
        >= 90f => "A+",
        >= 80f => "A",
        >= 70f => "B",
        >= 60f => "C",
        >= 50f => "D",
        >= 40f => "E",
        _ => "F"
    };

    private static string ToTrend(float trend) => trend switch
    {
        > 3f => "IMPROVING",
        < -3f => "DECLINING",
        _ => "STABLE"
    };

    private static string ToRiskLevel(float percentage, float trend) =>
        percentage < 40f ? "CRITICAL" :
        percentage < 50f || trend < -8f ? "HIGH" :
        percentage < 65f || trend < -3f ? "MEDIUM" : "LOW";

    private sealed record CachedModel(ITransformer Model, DateTimeOffset TrainedAt);
}
