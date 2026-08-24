namespace SmartSchool.SharedKernel.Constants;

public static class ApiRoutes
{
	public const string ApiPrefix = "/api";
	public const string Health = "/health";
	public const string OperationsJobs = "/ops/jobs";
	public const string CorrelationHeader = "X-Correlation-ID";
	public const string TraceHeader = "X-Trace-Id";

	public static string EntityCollection(
		string module,
		string entity) =>
		$"{ApiPrefix}/{module}/{entity}";

	public static string EntityById(
		string module,
		string entity) =>
		$"{EntityCollection(module, entity)}/{{id:guid}}";
}
