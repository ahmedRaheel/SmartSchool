using SmartSchool.SharedKernel;

namespace SmartSchool.Api.Samples;

public sealed record CreateUserSampleRequest(
	Guid TenantId,
	string UserName,
	string Email,
	string FirstName,
	string LastName,
	string[] Roles);

public sealed record CreateStudentSampleRequest(
	Guid TenantId,
	string AdmissionNo,
	string FirstName,
	string LastName,
	DateOnly DateOfBirth,
	string Gender,
	Guid ProgramId,
	Guid ClassSectionId,
	Guid AcademicYearId,
	Guid? UserId);

public sealed record CreateTeacherSampleRequest(
	Guid TenantId,
	string EmployeeNo,
	string FirstName,
	string LastName,
	string Email,
	string Phone,
	Guid JobId,
	Guid JobGradeId,
	DateOnly JoiningDate,
	Guid? UserId);

public sealed record CreateParentSampleRequest(
	Guid TenantId,
	string FirstName,
	string LastName,
	string Email,
	string Phone,
	string Relationship,
	Guid StudentId,
	Guid? UserId);

public static class SampleEndpoints
{
	public static IEndpointRouteBuilder MapSampleEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints
			.MapGroup("/api/samples")
			.WithTags("Samples")
			.RequireAuthorization();

		group.MapPost(
			"/users",
			(CreateUserSampleRequest request) =>
			{
				var response = new
				{
					request.TenantId,
					request.UserName,
					request.Email,
					request.FirstName,
					request.LastName,
					request.Roles,
					Guidance =
						"Provision the identity first, then persist the SmartSchool user profile."
				};

				return Results.Ok(
					Result<object>.Success(response));
			});

		group.MapPost(
			"/students",
			(CreateStudentSampleRequest request) =>
			{
				var response = new
				{
					request.TenantId,
					request.AdmissionNo,
					request.FirstName,
					request.LastName,
					request.ProgramId,
					request.ClassSectionId,
					request.AcademicYearId,
					Guidance =
						"Create the student and enrollment in one application transaction."
				};

				return Results.Ok(
					Result<object>.Success(response));
			});

		group.MapPost(
			"/teachers",
			(CreateTeacherSampleRequest request) =>
			{
				var response = new
				{
					request.TenantId,
					request.EmployeeNo,
					request.FirstName,
					request.LastName,
					request.JobId,
					request.JobGradeId,
					Guidance =
						"Create the employee, job assignment and teacher role through application handlers."
				};

				return Results.Ok(
					Result<object>.Success(response));
			});

		group.MapPost(
			"/parents",
			(CreateParentSampleRequest request) =>
			{
				var response = new
				{
					request.TenantId,
					request.FirstName,
					request.LastName,
					request.Relationship,
					request.StudentId,
					Guidance =
						"Create the guardian and student relationship after duplicate and authorization checks."
				};

				return Results.Ok(
					Result<object>.Success(response));
			});

		return endpoints;
	}
}
