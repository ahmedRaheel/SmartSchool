# SmartSchool bounded-context ERD rules

## Ownership

- **HR** owns employee lifecycle, employment, hiring, leave and teacher-as-employee data.
- **Organization** owns school, campus, academic structure, class/section, subject, course offering and timetable structure.
- **Learning** owns lessons, learning resources, assignments, assignment targeting and submissions.
- **Students** owns student identity/profile, guardian, enrollment and attendance.
- **AICore** owns AI orchestration and uses feature-local Dapper projections for data it needs; it has no project reference to Students, Examinations or AIPrediction.

## Dependency rule

Module projects must not reference another module project. Cross-context identifiers (StudentId, TeacherEmployeeId, ClassSectionId, CourseOfferingId) are scalar business identifiers only. A module must not use another module's Entity, DbContext, Reader, Command or Query type. Cross-context reads use feature-local Dapper projections or integration events/contracts from BuildingBlocks.

## Assignment relationship

An assignment is a Learning aggregate. A teacher creates an assignment for a course/class/section and can target zero or more students. Student targeting is many-to-many:

```text
Learning.Assignment 1 --- * Learning.AssignmentStudent * --- 1 StudentId
Learning.Assignment 1 --- * Learning.AssignmentSubmission
```

`AssignmentStudent` is owned by Learning and stores `StudentId` as an external identifier; it does not reference `StudentEntity`. A student's assignment inbox is queried from `lms.assignment_student` + `lms.academic_assignment` by StudentId.

The same rule applies to natural cross-context links: keep the relationship in the module that owns the business operation and store external IDs rather than creating assembly/entity references.
