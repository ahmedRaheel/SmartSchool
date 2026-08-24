# Dapper schema audit v69

Authoritative contract: `database/postgresql/SmartSchoolComplete.sql`.

- Qualified Dapper physical-table references scanned: **318**
- References matching SQL contract: **249**
- Invalid/unresolvable references: **36**

All safely resolvable `public.<EntityName>` references were changed in-place to the exact schema/table from the supplied SQL. Dapper was retained; no query was converted to EF Core and no query class was deleted.

## References that cannot be safely rewritten

These names do not exist anywhere in the supplied SQL contract. They were deliberately not guessed or mapped to unrelated tables:

- `src/Modules/AIPrediction/ML/MlNetPredictionSuiteService.cs` → `admission.application`
- `src/Modules/AIPrediction/ML/MlNetPredictionSuiteService.cs` → `payroll.payslip`
- `src/Modules/Activities/Persistence/StudentOfMonthQuery.cs` → `public.StudentOfMonth`
- `src/Modules/Admissions/Persistence/AdmissionDecisionQuery.cs` → `public.AdmissionDecision`
- `src/Modules/Admissions/Persistence/ApplicantQuery.cs` → `public.Applicant`
- `src/Modules/Admissions/Persistence/ApplicationQuery.cs` → `public.Application`
- `src/Modules/Admissions/Persistence/InquiryQuery.cs` → `public.Inquiry`
- `src/Modules/Documents/Persistence/CertificateQuery.cs` → `public.Certificate`
- `src/Modules/Documents/Persistence/SchoolLogoQuery.cs` → `public.SchoolLogo`
- `src/Modules/Examinations/Persistence/GradeScaleQuery.cs` → `public.GradeScale`
- `src/Modules/Finance/Persistence/DiscountQuery.cs` → `public.Discount`
- `src/Modules/Finance/Persistence/FeeStructureQuery.cs` → `public.FeeStructure`
- `src/Modules/Finance/Persistence/ScholarshipQuery.cs` → `public.Scholarship`
- `src/Modules/Finance/Persistence/StudentFeeQuery.cs` → `public.StudentFee`
- `src/Modules/HR/Persistence/EmploymentHistoryQuery.cs` → `public.EmploymentHistory`
- `src/Modules/HR/Persistence/ResumeQuery.cs` → `public.Resume`
- `src/Modules/Identity/Persistence/RoleAssignmentQuery.cs` → `public.RoleAssignment`
- `src/Modules/Identity/Persistence/UserProfileQuery.cs` → `public.UserProfile`
- `src/Modules/Inventory/Persistence/PurchaseOrderQuery.cs` → `public.PurchaseOrder`
- `src/Modules/Inventory/Persistence/StockTransactionQuery.cs` → `public.StockTransaction`
- `src/Modules/Learning/Persistence/LearningResourceQuery.cs` → `public.LearningResource`
- `src/Modules/Learning/Persistence/LessonQuery.cs` → `public.Lesson`
- `src/Modules/Library/Persistence/ReservationQuery.cs` → `public.Reservation`
- `src/Modules/Organization/Persistence/SchoolQuery.cs` → `public.School`
- `src/Modules/Payroll/Persistence/IncrementQuery.cs` → `public.Increment`
- `src/Modules/Payroll/Persistence/PayslipQuery.cs` → `public.Payslip`
- `src/Modules/Payroll/Persistence/SalaryStructureQuery.cs` → `public.SalaryStructure`
- `src/Modules/Students/Persistence/AttendanceQuery.cs` → `public.Attendance`
- `src/Modules/Teachers/Module.cs` → `hr.TeacherProfile`
- `src/Modules/Tenancy/Persistence/SubscriptionQuery.cs` → `public.Subscription`
- `src/Modules/Transport/Persistence/StopQuery.cs` → `public.Stop`
- `src/Modules/Transport/Persistence/StudentTransportQuery.cs` → `public.StudentTransport`
- `src/Modules/Workflow/Persistence/ApprovalQuery.cs` → `public.Approval`
- `src/Modules/Workflow/Persistence/WorkflowDefinitionQuery.cs` → `public.WorkflowDefinition`
- `src/Modules/Workflow/Persistence/WorkflowInstanceQuery.cs` → `public.WorkflowInstance`
- `src/Modules/Workflow/Persistence/WorkflowStepQuery.cs` → `public.WorkflowStep`
