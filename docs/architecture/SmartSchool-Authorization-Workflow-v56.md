# SmartSchool Authorization & Workflow Architecture v56

## Impersonation
Impersonation is restricted to:
- SuperAdmin: may impersonate any enabled user in any tenant.
- SchoolAdmin/Admin: may impersonate any enabled user inside their own tenant only.

Teacher, Student, Parent, Driver, Examiner and other staff can never impersonate another account.

Impersonation must be server-issued and audited. Store original subject, effective subject, tenant, school/branch, reason, issued/expiry time, IP/user-agent and trace/correlation ID. React must never manufacture or replace role/tenant claims.

## User administration
SuperAdmin:
- create/update/disable/enable/delete/reset-password for any tenant user.
- create tenant administrators.
- tenant lifecycle management.

SchoolAdmin/Admin:
- create/update/disable/enable/delete/reset-password for users in own tenant.
- create Student, Parent, Teacher, Driver, Examiner and Staff accounts.
- cannot administer SuperAdmin accounts or users belonging to another tenant.

Deletion should normally be soft-delete/deactivation where academic, finance, attendance, examination or audit history exists.

## Workflow engine principle
Business operations that require approval, validation, capacity checks, scheduling or cross-module coordination must run through Workflow rather than direct state mutation.

Every workflow has:
Requested -> Validating -> PendingApproval (optional) -> Approved -> Applying -> Completed
and may transition to Rejected, Cancelled or Failed.

All transitions are audited and publish integration/domain events through the existing Kafka infrastructure where appropriate.

## Required workflows

### Student Admission
Application -> document verification -> eligibility -> assessment/interview (configurable) -> fee/admission approval -> student account -> enrollment -> class/section placement -> parent linkage -> notification.

### Class / Section Assignment
Request -> validate academic year/program/class -> capacity check -> timetable/conflict check -> approve -> enrollment/section assignment -> notify student/parent/teachers.

### Student Section Change
Request -> reason -> current/new section validation -> capacity -> timetable/subject compatibility -> approval -> effective-date transfer -> update assignments -> notify.

### Test / Class Test
Teacher draft -> validate teacher/class/subject assignment -> schedule conflict check -> publish -> students notified -> submission/attendance -> marking -> moderation when configured -> result publish -> prediction refresh event.

### Student Leave
Student/Parent request -> date/reason/document -> attendance/timetable validation -> teacher/admin approval according to policy -> attendance update -> transport notification when relevant -> parent/student notification.

### Teacher Leave
Teacher request -> workload/classes impact -> substitute teacher recommendation -> approval -> timetable substitution -> affected class notifications.

### Teacher Subject Assignment / Workload
Need identified -> eligible teacher search -> qualification/subject match -> calculate existing periods/classes/workload -> conflict check -> rank candidates -> admin/principal approval -> assignment -> timetable update -> teacher notification.
Never auto-assign solely from AI recommendation; final assignment is policy/approval controlled.

### Exam Lifecycle
Examiner/Admin draft -> subjects/classes -> schedule conflict validation -> room/invigilator assignment -> approval/publish -> marks entry -> moderation -> result approval -> publish -> prediction/evaluation event.

### Fee Concession / Waiver
Request -> eligibility/rule validation -> finance approval -> optional higher approval threshold -> ledger/invoice adjustment -> audit -> notification.

### Staff Hiring
Candidate -> screening -> interview -> decision -> offer -> employee/user creation -> role/branch assignment -> onboarding.

### Student Withdrawal / Transfer
Request -> dues/library/asset clearance -> academic approval -> certificates/documents -> enrollment close -> account state update -> notification.

### Document / Notice Publication
Draft -> audience selection -> optional approval -> publish -> Kafka notification fan-out -> read receipts.

### Timetable Change
Request -> teacher/class/room conflict checks -> workload validation -> approval -> effective change -> notify impacted actors.

### Transport Assignment / Change
Request -> route/vehicle capacity -> driver assignment -> guardian/admin confirmation as configured -> effective assignment -> notifications.

### Role / Permission Change
Admin request -> boundary validation -> elevated-role approval where required -> identity update -> revoke/refresh sessions -> audit.

## Actor/data scope
RBAC is only the first gate. APIs must additionally enforce:
- tenant boundary for every tenant-owned record;
- branch/school boundary where applicable;
- Teacher -> assigned class/section/subject;
- Student -> own record;
- Parent -> linked children;
- Driver -> assigned vehicle/route/students;
- Examiner -> assigned examination scope.

Feature flags never replace authorization.
