# Solution audit v63

## Actor coverage
- SuperAdmin: tenancy, user administration, impersonation, platform/dashboard APIs.
- Tenant Admin: school/branch/user administration, dashboards, communication and workflows.
- Teacher: NEW dedicated Teachers module + teacher actor/leave tables; self profile, classes, students, timetable, workload, assignments, grading, leave and dashboard.
- Student: Students/Learning/Examinations + student dashboard and AI tutor.
- Parent: parent dashboard, AIParent, communication.
- Driver: Transport + driver dashboard.
- Examiner: Examinations + NEW examiner dashboard.

## Cleanup performed
- Removed duplicate CommunicationConfigurations.cs which mapped the same SignalR chat/notification entities to conflicting table names/casing.
- Added dedicated ChatAttachmentEntityConfiguration and NotificationPreferenceEntityConfiguration with communication schema.
- Removed temporary /api/debug/auth endpoint from the production API pipeline.
- Kept legacy non-Entity model classes because reference scan found they are still referenced; deleting them would break compilation.
- Teacher is no longer treated only as a generic HR employee: dedicated actor module and persistence extension added.

## Modules audited
- AICore
- AIInquiry
- AIParent
- AIPrediction
- AITutor
- Academics
- Activities
- Admissions
- Audit
- Communication
- Documents
- Examinations
- Finance
- HR
- Identity
- Inventory
- Learning
- Library
- Organization
- Payroll
- Reference
- Students
- Teachers
- Tenancy
- Transport
- Workflow