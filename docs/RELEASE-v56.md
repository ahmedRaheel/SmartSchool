# v56 Workflow + Administration refinement
- Impersonation policy is exclusively SuperAdmin + SchoolAdmin/Admin.
- User administration policy permits SuperAdmin globally and SchoolAdmin within tenant boundary.
- Added formal workflow architecture and workflow catalog.
- Added Student Admission, Class/Section Assignment, Section Change, Class Test, Student Leave, Teacher Leave, workload-based Teacher Subject Assignment, Exam Lifecycle, Fee Concession, Hiring, Withdrawal/Transfer, Notice Publication, Timetable Change, Transport Assignment and Role Change workflows.
- Added Workflow Center portal page backed by API catalog.
- Existing Workflow persistence remains the execution engine; catalog defines governed business flows.
- Relationship/data-scope checks remain mandatory when implementing each transition handler.
