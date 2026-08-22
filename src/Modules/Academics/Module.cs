using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Features.AcademicSystem;
using SmartSchool.Modules.Academics.Features.AcademicYear;
using SmartSchool.Modules.Academics.Features.ClassSection;
using SmartSchool.Modules.Academics.Features.CourseOffering;
using SmartSchool.Modules.Academics.Features.CourseSelection;
using SmartSchool.Modules.Academics.Features.GradeLevel;
using SmartSchool.Modules.Academics.Features.Program;
using SmartSchool.Modules.Academics.Features.Subject;
using SmartSchool.Modules.Academics.Features.TeacherAssignment;
using SmartSchool.Modules.Academics.Features.Term;
using SmartSchool.Modules.Academics.Features.Timetable;
using SmartSchool.Modules.Academics.Features.TimetableEntry;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics;

public static class Module
{
	public static IServiceCollection AddAcademicsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IAcademicSystemQuery, AcademicSystemQuery>();
		services.AddScoped<IAcademicSystemCommand, AcademicSystemCommand>();
		services.AddScoped<IAcademicYearQuery, AcademicYearQuery>();
		services.AddScoped<IAcademicYearCommand, AcademicYearCommand>();
		services.AddScoped<IClassSectionQuery, ClassSectionQuery>();
		services.AddScoped<IClassSectionCommand, ClassSectionCommand>();
		services.AddScoped<ICourseOfferingQuery, CourseOfferingQuery>();
		services.AddScoped<ICourseOfferingCommand, CourseOfferingCommand>();
		services.AddScoped<ICourseSelectionQuery, CourseSelectionQuery>();
		services.AddScoped<ICourseSelectionCommand, CourseSelectionCommand>();
		services.AddScoped<IGradeLevelQuery, GradeLevelQuery>();
		services.AddScoped<IGradeLevelCommand, GradeLevelCommand>();
		services.AddScoped<IProgramQuery, ProgramQuery>();
		services.AddScoped<IProgramCommand, ProgramCommand>();
		services.AddScoped<ISubjectQuery, SubjectQuery>();
		services.AddScoped<ISubjectCommand, SubjectCommand>();
		services.AddScoped<ITeacherAssignmentQuery, TeacherAssignmentQuery>();
		services.AddScoped<ITeacherAssignmentCommand, TeacherAssignmentCommand>();
		services.AddScoped<ITermQuery, TermQuery>();
		services.AddScoped<ITermCommand, TermCommand>();
		services.AddScoped<ITimetableQuery, TimetableQuery>();
		services.AddScoped<ITimetableCommand, TimetableCommand>();
		services.AddScoped<ITimetableEntryQuery, TimetableEntryQuery>();
		services.AddScoped<ITimetableEntryCommand, TimetableEntryCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapAcademicsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateAcademicSystem.MapEndpoint(endpoints);
		GetAcademicSystemById.MapEndpoint(endpoints);
		GetAcademicSystemPage.MapEndpoint(endpoints);
		UpdateAcademicSystem.MapEndpoint(endpoints);
		DeleteAcademicSystem.MapEndpoint(endpoints);
		CreateAcademicYear.MapEndpoint(endpoints);
		GetAcademicYearById.MapEndpoint(endpoints);
		GetAcademicYearPage.MapEndpoint(endpoints);
		UpdateAcademicYear.MapEndpoint(endpoints);
		DeleteAcademicYear.MapEndpoint(endpoints);
		CreateClassSection.MapEndpoint(endpoints);
		GetClassSectionById.MapEndpoint(endpoints);
		GetClassSectionPage.MapEndpoint(endpoints);
		UpdateClassSection.MapEndpoint(endpoints);
		DeleteClassSection.MapEndpoint(endpoints);
		CreateCourseOffering.MapEndpoint(endpoints);
		GetCourseOfferingById.MapEndpoint(endpoints);
		GetCourseOfferingPage.MapEndpoint(endpoints);
		UpdateCourseOffering.MapEndpoint(endpoints);
		DeleteCourseOffering.MapEndpoint(endpoints);
		CreateCourseSelection.MapEndpoint(endpoints);
		GetCourseSelectionById.MapEndpoint(endpoints);
		GetCourseSelectionPage.MapEndpoint(endpoints);
		UpdateCourseSelection.MapEndpoint(endpoints);
		DeleteCourseSelection.MapEndpoint(endpoints);
		CreateGradeLevel.MapEndpoint(endpoints);
		GetGradeLevelById.MapEndpoint(endpoints);
		GetGradeLevelPage.MapEndpoint(endpoints);
		UpdateGradeLevel.MapEndpoint(endpoints);
		DeleteGradeLevel.MapEndpoint(endpoints);
		CreateProgram.MapEndpoint(endpoints);
		GetProgramById.MapEndpoint(endpoints);
		GetProgramPage.MapEndpoint(endpoints);
		UpdateProgram.MapEndpoint(endpoints);
		DeleteProgram.MapEndpoint(endpoints);
		CreateSubject.MapEndpoint(endpoints);
		GetSubjectById.MapEndpoint(endpoints);
		GetSubjectPage.MapEndpoint(endpoints);
		UpdateSubject.MapEndpoint(endpoints);
		DeleteSubject.MapEndpoint(endpoints);
		CreateTeacherAssignment.MapEndpoint(endpoints);
		GetTeacherAssignmentById.MapEndpoint(endpoints);
		GetTeacherAssignmentPage.MapEndpoint(endpoints);
		UpdateTeacherAssignment.MapEndpoint(endpoints);
		DeleteTeacherAssignment.MapEndpoint(endpoints);
		CreateTerm.MapEndpoint(endpoints);
		GetTermById.MapEndpoint(endpoints);
		GetTermPage.MapEndpoint(endpoints);
		UpdateTerm.MapEndpoint(endpoints);
		DeleteTerm.MapEndpoint(endpoints);
		CreateTimetable.MapEndpoint(endpoints);
		GetTimetableById.MapEndpoint(endpoints);
		GetTimetablePage.MapEndpoint(endpoints);
		UpdateTimetable.MapEndpoint(endpoints);
		DeleteTimetable.MapEndpoint(endpoints);
		CreateTimetableEntry.MapEndpoint(endpoints);
		GetTimetableEntryById.MapEndpoint(endpoints);
		GetTimetableEntryPage.MapEndpoint(endpoints);
		UpdateTimetableEntry.MapEndpoint(endpoints);
		DeleteTimetableEntry.MapEndpoint(endpoints);

		return endpoints;
	}
}
