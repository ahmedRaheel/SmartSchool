# Database / Entity Alignment

`database/postgresql/SmartSchoolComplete.sql` is the canonical persistence contract. Every resolved EF configuration now uses explicit `ToTable(table, schema)` and explicit column mappings. `Entity.Id` is initialized with `Guid.NewGuid()`.

## Unresolved legacy entities

These legacy entities have no safe one-to-one table in the supplied complete database and therefore must not be silently mapped to an unrelated table:

- `Organization.SchoolEntity`
- `Learning.LessonEntity`
- `Learning.LearningResourceEntity`
- `Inventory.PurchaseOrderEntity`
- `Inventory.StockTransactionEntity`
- `Activities.StudentOfMonthEntity`
- `Library.ReservationEntity`
- `Identity.RoleAssignmentEntity`
- `Identity.UserProfileEntity`
- `HR.ResumeEntity`
- `HR.TeacherProfileEntity`
- `HR.PayrollProfileEntity`
- `HR.EmploymentHistoryEntity`
- `Finance.FeeStructureEntity`
- `Finance.StudentFeeEntity`
- `Finance.DiscountEntity`
- `Finance.ScholarshipEntity`
- `Students.ParentProfileEntity`
- `Students.StudentProfileEntity`
- `Students.AttendanceEntity`
- `Documents.SchoolLogoEntity`
- `Documents.CertificateEntity`
- `Admissions.AdmissionDecisionEntity`
- `Admissions.ApplicantEntity`
- `Admissions.ApplicationEntity`
- `Admissions.InquiryEntity`
- `Tenancy.SubscriptionEntity`
- `Payroll.SalaryStructureEntity`
- `Payroll.PayslipEntity`
- `Payroll.IncrementEntity`
- `Workflow.WorkflowDefinitionEntity`
- `Workflow.WorkflowInstanceEntity`
- `Workflow.ApprovalEntity`
- `Workflow.WorkflowStepEntity`
- `Examinations.GradeScaleEntity`
- `Transport.StudentTransportEntity`
- `Transport.StopEntity`

