# Production UI / Backend contract

## Ownership decisions
- `/api/academics/*` is implemented by the Organization module because academic structures are campus-owned.
- Teacher/Principal/Examiner/Accountant/HR Manager/Admin Officer/Driver are employee designations in HR. Teacher-facing endpoints operate on HR EmployeeId; there is no duplicate Teacher employee master.
- Employee designation is a `short` enum persisted as PostgreSQL `smallint`.
- Document MIME/content type, URLs, provider/model names and extensible entity-type strings remain strings; finite business state/category/type fields are enum candidates.

## Academics API restored under Organization
CRUD endpoints are registered for academic-system, academic-year, class-section, course-offering, grade-level, program, subject, term and timetable using the existing feature-owned Vertical Slice implementation and `/api/academics/*` public routes.

## Database alignment
`database/20260902_ui_backend_alignment.sql` adds missing HR employee fields used by the entity model and the `designation smallint` column, backfills designation from legacy `staff_type`, and creates the tenant/designation index.
