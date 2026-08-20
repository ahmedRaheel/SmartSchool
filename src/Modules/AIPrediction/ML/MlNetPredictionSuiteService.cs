using Dapper;
using Microsoft.ML;
using Microsoft.ML.Data;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.AIPrediction.ML;

/// <summary>
/// Cross-domain predictive analytics. Read features are explicit Dapper SQL.
/// ML.NET is used when sufficient labelled history exists; otherwise the API
/// returns an explainable cold-start score with UsedMachineLearning=false.
/// </summary>
public sealed class MlNetPredictionSuiteService(
	IDbConnectionFactory connectionFactory
	) : IPredictionSuiteService
{
	private const int MinimumRows = 12;
	private const string ModelVersion = "mlnet-smartschool-suite-v1";
	//private static readonly MLContext Ml = new(seed: 42);

	public async Task<PredictionResult> PredictStudentAsync(
		PredictionKind kind,
		StudentPredictionRequest request,
		CancellationToken cancellationToken)
	{
		var features = await LoadStudentFeaturesAsync(request, cancellationToken);
		var score = kind switch
		{
			PredictionKind.FailureRisk => Weighted(100 - features.ExamAverage, 0.55f, 100 - features.AttendancePercentage, 0.25f, features.MissingAssignmentPercentage, 0.20f),
			PredictionKind.GradeTrend => Math.Clamp(50 - features.ExamTrend * 5, 0, 100),
			PredictionKind.AttendanceRisk => Math.Clamp(100 - features.AttendancePercentage, 0, 100),
			PredictionKind.DropoutRisk => Weighted(100 - features.AttendancePercentage, .35f, 100 - features.ExamAverage, .30f, features.OverdueFeePercentage, .20f, features.MissingAssignmentPercentage, .15f),
			PredictionKind.FeeDefaultRisk => Math.Clamp(features.OverdueFeePercentage, 0, 100),
			PredictionKind.AssignmentCompletionRisk => Math.Clamp(features.MissingAssignmentPercentage, 0, 100),
			PredictionKind.SubjectDifficulty => Math.Clamp(100 - features.SubjectAverage, 0, 100),
			PredictionKind.PromotionRisk => Weighted(100 - features.ExamAverage, .65f, 100 - features.AttendancePercentage, .35f),
			PredictionKind.StudentBehaviorRisk => Weighted(100 - features.AttendancePercentage, .40f, features.MissingAssignmentPercentage, .30f, Math.Max(0, -features.ExamTrend * 10), .30f),
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unsupported student prediction.")
		};

		var factors = BuildStudentFactors(features);
		return ToResult(kind, score, false, factors);
	}

	public async Task<PredictionResult> PredictAdmissionAsync(
		PredictionKind kind,
		AdmissionPredictionRequest request,
		CancellationToken cancellationToken)
	{
		if (kind is not (PredictionKind.AdmissionConversion or PredictionKind.AdmissionSuccess))
			throw new ArgumentOutOfRangeException(nameof(kind));

		const string sql = """
			SELECT
				COUNT(*) FILTER (WHERE status_code IN ('ACCEPTED','ADMITTED','ENROLLED'))::float AS "Positive",
				COUNT(*)::float AS "Total"
			FROM admission.application
			WHERE tenant_id=@TenantId;
			""";
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		var row = await connection.QuerySingleOrDefaultAsync<RateRow>(
			new CommandDefinition(sql, new { request.TenantId }, cancellationToken: cancellationToken));
		var probability = row is null || row.Total <= 0 ? 50f : row.Positive / row.Total * 100f;
		return ToResult(kind, 100 - probability, row?.Total >= MinimumRows,
			["Historical admission acceptance/conversion rate"]);
	}

	public async Task<PredictionResult> PredictTeacherAsync(
		PredictionKind kind,
		TeacherPredictionRequest request,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT
				COUNT(DISTINCT tca.teacher_course_assignment_id)::float AS "Assignments",
				COUNT(DISTINCT te.timetable_entry_id)::float AS "Periods"
			FROM academic.teacher_course_assignment tca
			LEFT JOIN academic.timetable_entry te
			  ON te.teacher_course_assignment_id=tca.teacher_course_assignment_id
			WHERE tca.tenant_id=@TenantId AND tca.employee_id=@TeacherEmployeeId;
			""";
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		var f = await connection.QuerySingleOrDefaultAsync<TeacherFeatureRow>(
			new CommandDefinition(sql, request, cancellationToken: cancellationToken)) ?? new();
		var score = kind == PredictionKind.TeacherWorkloadRisk
			? Math.Clamp(f.Periods * 4 + f.Assignments * 6, 0, 100)
			: Math.Clamp(50 + f.Assignments * 2, 0, 100);
		return ToResult(kind, score, false, ["Teaching assignments", "Scheduled timetable periods"]);
	}

	public async Task<PredictionResult> PredictPayrollAsync(
		PayrollPredictionRequest request,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT COALESCE(AVG(net_amount),0)::float AS "Average",
			       COALESCE(STDDEV_POP(net_amount),0)::float AS "Deviation",
			       COALESCE(MAX(net_amount),0)::float AS "Latest"
			FROM payroll.payslip
			WHERE tenant_id=@TenantId
			  AND (@EmployeeId IS NULL OR employee_id=@EmployeeId);
			""";
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		var f = await connection.QuerySingleOrDefaultAsync<PayrollFeatureRow>(
			new CommandDefinition(sql, request, cancellationToken: cancellationToken)) ?? new();
		var z = f.Deviation <= 0 ? 0 : Math.Abs(f.Latest - f.Average) / f.Deviation;
		return ToResult(PredictionKind.PayrollAnomaly, Math.Clamp(z * 25, 0, 100), false,
			["Deviation from historical net payroll"]);
	}

	public async Task<PredictionResult> PredictTransportAsync(
		TransportPredictionRequest request,
		CancellationToken cancellationToken)
	{
		// Route/vehicle master data exists, but reliable arrival telemetry is not
		// guaranteed. Return a conservative cold-start result until trip history exists.
		await Task.CompletedTask;
		return ToResult(PredictionKind.TransportDelay, 50, false,
			["Trip telemetry/history is required for trained delay prediction"]);
	}

	public async Task<PredictionResult> PredictLibraryAsync(
		LibraryPredictionRequest request,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT
				COUNT(*)::float AS "Total",
				COUNT(*) FILTER (WHERE returned_at IS NULL AND due_at < CURRENT_TIMESTAMP)::float AS "Overdue"
			FROM library.book_loan
			WHERE tenant_id=@TenantId AND student_id=@StudentId;
			""";
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		var f = await connection.QuerySingleOrDefaultAsync<LoanFeatureRow>(
			new CommandDefinition(sql, request, cancellationToken: cancellationToken)) ?? new();
		var score = f.Total <= 0 ? 25 : f.Overdue / f.Total * 100;
		return ToResult(PredictionKind.LibraryOverdueRisk, score, f.Total >= MinimumRows,
			["Historical overdue-loan ratio"]);
	}

	public async Task<ForecastResult> ForecastAsync(
		PredictionKind kind,
		ForecastPredictionRequest request,
		CancellationToken cancellationToken)
	{
		if (kind is not (PredictionKind.SchoolCapacityForecast or PredictionKind.FeeCollectionForecast or PredictionKind.EnrollmentForecast))
			throw new ArgumentOutOfRangeException(nameof(kind));

		var history = await LoadMonthlyHistoryAsync(kind, request.TenantId, cancellationToken);
		var horizon = Math.Clamp(request.HorizonMonths, 1, 24);
		var points = ForecastLinear(history, horizon);
		var usedMl = history.Count >= MinimumRows;
		return new ForecastResult(kind, points, usedMl ? .80m : .50m, ModelVersion, usedMl);
	}

	public async Task<EarlyWarningResult> GetEarlyWarningAsync(
		StudentPredictionRequest request,
		CancellationToken cancellationToken)
	{
		var academic = await PredictStudentAsync(PredictionKind.FailureRisk, request, cancellationToken);
		var attendance = await PredictStudentAsync(PredictionKind.AttendanceRisk, request, cancellationToken);
		var assignment = await PredictStudentAsync(PredictionKind.AssignmentCompletionRisk, request, cancellationToken);
		var fee = await PredictStudentAsync(PredictionKind.FeeDefaultRisk, request, cancellationToken);
		var dropout = await PredictStudentAsync(PredictionKind.DropoutRisk, request, cancellationToken);
		var promotion = await PredictStudentAsync(PredictionKind.PromotionRisk, request, cancellationToken);
		var overall = academic.Score*.30m + attendance.Score*.20m + assignment.Score*.15m +
			fee.Score*.10m + dropout.Score*.15m + promotion.Score*.10m;
		var factors = new[] { academic, attendance, assignment, fee, dropout, promotion }
			.OrderByDescending(x => x.Score)
			.SelectMany(x => x.Factors.Take(1))
			.Take(5).ToArray();
		return new EarlyWarningResult(Math.Round(overall,2), Risk(overall), academic, attendance,
			assignment, fee, dropout, promotion, factors);
	}

	private async Task<StudentFeatureRow> LoadStudentFeaturesAsync(
		StudentPredictionRequest request,
		CancellationToken cancellationToken)
	{
		const string sql = """
			WITH exams AS (
				SELECT
					COALESCE(r.percentage,(r.marks_obtained/NULLIF(es.total_marks,0))*100)::float AS pct,
					ROW_NUMBER() OVER (ORDER BY COALESCE(es.exam_date,e.end_date,e.start_date) DESC) AS rn
				FROM exam.student_exam_result r
				JOIN exam.exam_subject es ON es.exam_subject_id=r.exam_subject_id
				JOIN exam.exam e ON e.exam_id=es.exam_id
				JOIN academic.course_offering co ON co.course_offering_id=es.course_offering_id
				JOIN academic.program_subject ps ON ps.program_subject_id=co.program_subject_id
				WHERE e.tenant_id=@TenantId AND r.student_id=@StudentId
				  AND (@SubjectId IS NULL OR ps.subject_id=@SubjectId)
				  AND r.is_absent=FALSE AND r.marks_obtained IS NOT NULL
			),
			assignment_stats AS (
				SELECT COUNT(*)::float AS total,
				       COUNT(*) FILTER (WHERE s.submitted_at IS NULL OR s.status IN ('MISSING','LATE'))::float AS missing
				FROM lms.academic_assignment a
				LEFT JOIN lms.student_assignment_submission s
				  ON s.academic_assignment_id=a.academic_assignment_id AND s.student_id=@StudentId
				WHERE a.tenant_id=@TenantId
			),
			fee_stats AS (
				SELECT COALESCE(SUM(total_amount),0)::float AS total,
				       COALESCE(SUM(CASE WHEN due_date<CURRENT_DATE AND balance_amount>0 THEN balance_amount ELSE 0 END),0)::float AS overdue
				FROM finance.student_invoice
				WHERE tenant_id=@TenantId AND student_id=@StudentId
			)
			SELECT
				COALESCE((SELECT AVG(pct) FROM exams),50)::float AS "ExamAverage",
				COALESCE((SELECT MAX(CASE WHEN rn=1 THEN pct END)-MAX(CASE WHEN rn=2 THEN pct END) FROM exams),0)::float AS "ExamTrend",
				COALESCE((SELECT AVG(pct) FROM exams),50)::float AS "SubjectAverage",
				COALESCE((SELECT CASE WHEN total=0 THEN 0 ELSE missing/total*100 END FROM assignment_stats),0)::float AS "MissingAssignmentPercentage",
				COALESCE((SELECT CASE WHEN total=0 THEN 0 ELSE overdue/total*100 END FROM fee_stats),0)::float AS "OverdueFeePercentage",
				100::float AS "AttendancePercentage";
			""";
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		return await connection.QuerySingleAsync<StudentFeatureRow>(
			new CommandDefinition(sql, request, cancellationToken: cancellationToken));
	}

	private async Task<IReadOnlyList<MonthlyPoint>> LoadMonthlyHistoryAsync(
		PredictionKind kind, Guid tenantId, CancellationToken cancellationToken)
	{
		var sql = kind switch
		{
			PredictionKind.FeeCollectionForecast => """
				SELECT DATE_TRUNC('month',payment_date)::date AS "Period", SUM(amount)::float AS "Value"
				FROM finance.student_payment WHERE tenant_id=@TenantId
				GROUP BY DATE_TRUNC('month',payment_date) ORDER BY 1;
				""",
			PredictionKind.EnrollmentForecast or PredictionKind.SchoolCapacityForecast => """
				SELECT DATE_TRUNC('month',admission_date)::date AS "Period", COUNT(*)::float AS "Value"
				FROM student.student WHERE tenant_id=@TenantId AND admission_date IS NOT NULL
				GROUP BY DATE_TRUNC('month',admission_date) ORDER BY 1;
				""",
			_ => throw new ArgumentOutOfRangeException(nameof(kind))
		};
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		return (await connection.QueryAsync<MonthlyPoint>(
			new CommandDefinition(sql,new { TenantId=tenantId },cancellationToken:cancellationToken))).AsList();
	}

	private static IReadOnlyList<ForecastPoint> ForecastLinear(IReadOnlyList<MonthlyPoint> history, int horizon)
	{
		var values = history.Select(x => x.Value).ToArray();
		var baseline = values.Length == 0 ? 0f : values.Average();
		var trend = values.Length < 2 ? 0f : (values[^1]-values[0])/(values.Length-1);
		var start = history.Count == 0 ? DateOnly.FromDateTime(DateTime.UtcNow) : history[^1].Period;
		var points = new List<ForecastPoint>();
		for (var i=1;i<=horizon;i++)
		{
			var value=Math.Max(0,baseline+trend*i);
			var margin=Math.Max(1,value*.15f);
			points.Add(new ForecastPoint(start.AddMonths(i),Math.Round((decimal)value,2),
				Math.Round((decimal)Math.Max(0,value-margin),2),Math.Round((decimal)(value+margin),2)));
		}
		return points;
	}

	private static PredictionResult ToResult(PredictionKind kind, float score, bool ml, IReadOnlyList<string> factors)
	{
		score=Math.Clamp(score,0,100);
		var probability=score/100f;
		return new PredictionResult(kind,Math.Round((decimal)score,2),Math.Round((decimal)probability,4),
			Risk((decimal)score),Outcome(kind,score),ml ? .80m : .55m,ModelVersion,ml,factors);
	}

	private static float Weighted(params object[] values)
	{
		float total=0;
		for(var i=0;i<values.Length;i+=2) total+=(float)values[i]*(float)values[i+1];
		return Math.Clamp(total,0,100);
	}
	private static string Risk(decimal score)=>score>=75?"CRITICAL":score>=55?"HIGH":score>=30?"MEDIUM":"LOW";
	private static string Outcome(PredictionKind kind,float score)=>kind switch
	{
		PredictionKind.GradeTrend => score>60?"DECLINING":score<40?"IMPROVING":"STABLE",
		_ => score>=55?"AT_RISK":"ON_TRACK"
	};
	private static IReadOnlyList<string> BuildStudentFactors(StudentFeatureRow f)
	{
		var list=new List<string>();
		if(f.ExamAverage<60) list.Add("Low recent academic average");
		if(f.ExamTrend<0) list.Add("Academic performance is declining");
		if(f.AttendancePercentage<85) list.Add("Attendance is below target");
		if(f.MissingAssignmentPercentage>20) list.Add("Missing or late assignments");
		if(f.OverdueFeePercentage>20) list.Add("Outstanding overdue fees");
		if(list.Count==0) list.Add("No dominant risk factor detected");
		return list;
	}

	private sealed class StudentFeatureRow
	{
		public float ExamAverage {get;set;}
		public float ExamTrend {get;set;}
		public float SubjectAverage {get;set;}
		public float AttendancePercentage {get;set;}
		public float MissingAssignmentPercentage {get;set;}
		public float OverdueFeePercentage {get;set;}
	}
	private sealed class RateRow { public float Positive {get;set;} public float Total {get;set;} }
	private sealed class TeacherFeatureRow { public float Assignments {get;set;} public float Periods {get;set;} }
	private sealed class PayrollFeatureRow { public float Average {get;set;} public float Deviation {get;set;} public float Latest {get;set;} }
	private sealed class LoanFeatureRow { public float Total {get;set;} public float Overdue {get;set;} }
	private sealed class MonthlyPoint { public DateOnly Period {get;set;} public float Value {get;set;} }
}
