# Removed redundant legacy entities

The supplied `SmartSchoolComplete.sql` is authoritative. The following legacy aggregates had no safe one-to-one persistence table and their generated CRUD slices were removed instead of inventing a mapping.

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

## Residual references requiring domain review

- `ApplicationEntity`: src/BuildingBlocks/SmartSchool.Infrastructure/Platform.cs
