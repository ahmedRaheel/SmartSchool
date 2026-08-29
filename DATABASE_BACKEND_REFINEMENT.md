# SmartSchool Database & Vertical Slice Refinement

## Applied in this revision

- Removed the combined `AcademicSetup` feature and its generic `AcademicSetupType`, shared request/response and switch-based persistence facade.
- Existing Academic Year, Class Section, Subject, Term, etc. remain independent vertical slices.
- Added `telephone` and `email` to `org.department` and to Department create/update/read models.
- Added `academic.department_subject_teacher` as the canonical many-to-many relationship between Department, Subject and Teacher.
  - A department can own many subjects.
  - A subject can have multiple teachers in the same department.
  - A teacher can teach multiple subjects and can be associated with multiple departments.
- Added `academic.student_teacher` as the explicit Student/Teacher/Subject relationship, anchored to `student_enrollment`, `class_section`, and `academic_year` for history.
- Added focused vertical slices for creating/listing Department-Subject-Teacher assignments and Student-Teacher assignments.
- Added PostgreSQL migration `019_academic_relationship_refinement.sql` and wired it into `000_run_all.sql`.

## Canonical ownership rules

- Master data does not duplicate display names into relationship tables; relationships store foreign keys.
- Student class history is owned by `student.student_enrollment`.
- Department teaching capability is owned by `academic.department_subject_teacher`.
- Student-to-teacher visibility is owned by `academic.student_teacher`.
- `academic.teacher_course_assignment` remains a timetable/course-offering assignment and should not be used as a generic Teacher CRUD entity.

## Next cleanup targets

The source still contains generated generic CRUD artifacts that should be refactored feature-by-feature rather than deleted in one unsafe sweep. Highest priority:

1. Replace the generic `TeacherAssignmentEntity` Code/Name CRUD with the real course-assignment contract.
2. Split `Admissions/Features/AdmissionWorkflowSlices.cs` and `AdmissionWorkflowEndpoints.cs` so every admission use case owns its request, response, validator, handler and endpoint.
3. Consolidate duplicate admission `Applicant` / `Application` concepts against the actual admission workflow before dropping tables.
4. Standardize `Campus` versus `Branch`. Do not rename the database table until all FK/query/API references are migrated atomically.
5. Audit all generic generated entities for fake `Code`, `Name`, `MetadataJson` properties that are not present in the physical table.

This sequence deliberately avoids destructive table drops before references and data migration are proven.
