using SmartSchool.Modules.AIPrediction.Persistence;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionSuite;

public static class PredictionSuiteEndpoints
{
	public static void MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		var group=endpoints.MapGroup("/api/aiprediction").WithTags("AI Prediction").RequireAuthorization();

		group.MapPost("/student/{predictionKind}", async (
			PredictionKind predictionKind, StudentPredictionRequest request,
			IPredictionSuiteService service, IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictStudentAsync(predictionKind,request,ct);
			await PersistAsync(db,request.TenantId,result,ct,request.StudentId,request.SubjectId);
			return Results.Ok(result);
		});

		group.MapPost("/early-warning", async (
			StudentPredictionRequest request, IPredictionSuiteService service, CancellationToken ct) =>
			Results.Ok(await service.GetEarlyWarningAsync(request,ct)));

		group.MapPost("/admission/{predictionKind}", async (
			PredictionKind predictionKind, AdmissionPredictionRequest request,
			IPredictionSuiteService service, IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictAdmissionAsync(predictionKind,request,ct);
			await PersistAsync(db,request.TenantId,result,ct,relatedEntityId:request.ApplicantId);
			return Results.Ok(result);
		});

		group.MapPost("/teacher/{predictionKind}", async (
			PredictionKind predictionKind, TeacherPredictionRequest request,
			IPredictionSuiteService service, IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictTeacherAsync(predictionKind,request,ct);
			await PersistAsync(db,request.TenantId,result,ct,relatedEntityId:request.TeacherEmployeeId);
			return Results.Ok(result);
		});

		group.MapPost("/payroll/anomaly", async (
			PayrollPredictionRequest request, IPredictionSuiteService service,
			IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictPayrollAsync(request,ct);
			await PersistAsync(db,request.TenantId,result,ct,relatedEntityId:request.EmployeeId);
			return Results.Ok(result);
		});

		group.MapPost("/transport/delay", async (
			TransportPredictionRequest request, IPredictionSuiteService service,
			IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictTransportAsync(request,ct);
			await PersistAsync(db,request.TenantId,result,ct,relatedEntityId:request.RouteId);
			return Results.Ok(result);
		});

		group.MapPost("/library/overdue", async (
			LibraryPredictionRequest request, IPredictionSuiteService service,
			IAIPredictionDbContext db, CancellationToken ct) =>
		{
			var result=await service.PredictLibraryAsync(request,ct);
			await PersistAsync(db,request.TenantId,result,ct,studentId:request.StudentId);
			return Results.Ok(result);
		});

		group.MapPost("/forecast/{predictionKind}", async (
			PredictionKind predictionKind, ForecastPredictionRequest request,
			IPredictionSuiteService service, CancellationToken ct) =>
			Results.Ok(await service.ForecastAsync(predictionKind,request,ct)));
	}

	private static async Task PersistAsync(
		IAIPredictionDbContext db, Guid tenantId, PredictionResult result,
		CancellationToken ct, Guid? studentId=null, Guid? subjectId=null, Guid? relatedEntityId=null)
	{
		var entity=MlPredictionResultEntity.Create(
			tenantId,result.Kind.ToString(),result.Score,result.Probability,result.RiskLevel,
			result.Outcome,result.Confidence,result.ModelVersion,result.UsedMachineLearning,
			JsonSerializer.Serialize(result.Factors),studentId,subjectId,relatedEntityId);
		await db.MlPredictionResults.AddAsync(entity,ct);
		await db.SaveChangesAsync(ct);
	}
}
