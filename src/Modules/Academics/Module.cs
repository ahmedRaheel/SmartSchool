using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Features;
using SmartSchool.Modules.Academics.Features.AcademicSystem;
using SmartSchool.Modules.Academics.Features.AcademicYear;
using SmartSchool.Modules.Academics.Features.ClassSection;
using SmartSchool.Modules.Academics.Features.CourseOffering;
using SmartSchool.Modules.Academics.Features.DepartmentSubjectTeacher;
using SmartSchool.Modules.Academics.Features.CourseSelection;
using SmartSchool.Modules.Academics.Features.GradeLevel;
using SmartSchool.Modules.Academics.Features.Program;
using SmartSchool.Modules.Academics.Features.Subject;
using SmartSchool.Modules.Academics.Features.StudentTeacher;
using SmartSchool.Modules.Academics.Features.TeacherAssignment;
using SmartSchool.Modules.Academics.Features.Term;
using SmartSchool.Modules.Academics.Features.Timetable;
using SmartSchool.Modules.Academics.Features.TimetableEntry;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics;

public static class Module
{
	public static IServiceCollection AddAcademicsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeaturePersistence(typeof(Module).Assembly);
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
		CreateDepartmentSubjectTeacher.MapEndpoint(endpoints);
		GetDepartmentSubjectTeachers.MapEndpoint(endpoints);
		CreateStudentTeacher.MapEndpoint(endpoints);
		GetStudentTeachers.MapEndpoint(endpoints);

		return endpoints;
	}
}
