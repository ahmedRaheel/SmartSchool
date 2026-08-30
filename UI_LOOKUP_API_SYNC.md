# UI / API / Lookup synchronization

## Universal rule
Every categorical value shown as a select/radio/status choice is backed by `saas.lookup_type` + `saas.lookup_value`. Writes persist the numeric `lookup_value_id`; reads return both the ID and resolved code/name when the UI needs display text. Free text (name, address, remarks, notes, description) is not a lookup.

## Canonical lookup catalogue
Existing catalogue retained: TENANT_STATUS, ACADEMIC_SYSTEM_TYPE, SUBJECT_REQUIREMENT_TYPE, ENROLLMENT_TYPE, EXAM_TYPE, ATTENDANCE_STATUS, ASSIGNMENT_TYPE, WORK_ASSIGNMENT_STATUS, EMPLOYMENT_TYPE, CANDIDATE_STATUS, APPLICATION_STATUS, INTERVIEW_TYPE, DOCUMENT_TYPE, INCREMENT_REQUEST_TYPE, INCREMENT_TYPE, APPROVAL_STATUS, PAYROLL_STATUS, MESSAGE_TYPE, CONVERSATION_TYPE, AWARD_TYPE, NOTIFICATION_CHANNEL.

Added from UI/domain: GENDER, BLOOD_GROUP, RELIGION, NATIONALITY, RELATIONSHIP, LEAVE_TYPE, STAFF_TYPE, EMPLOYEE_STATUS, STUDENT_STATUS, ADMISSION_STATUS, INQUIRY_SOURCE, FEE_FREQUENCY, PAYMENT_METHOD, INVOICE_STATUS, ROOM_TYPE, DOCUMENT_CATEGORY, DOCUMENT_PURPOSE, PRIORITY, VEHICLE_STATUS, DRIVER_STATUS, LIBRARY_ITEM_STATUS, LOAN_STATUS, AI_EXECUTION_STATUS, KNOWLEDGE_DOCUMENT_STATUS, LIFECYCLE_STATUS, MARITAL_STATUS, CONTACT_RELATIONSHIP.

## API completion
The UI already called POST/PUT/DELETE `/api/lookups`, but Reference exposed GET only. Feature-owned lookup-value create/update/delete endpoints and EF persistence are now present. Lookup Settings no longer uses its local MOCK catalogue; it loads lookup types and values from the backend and writes through those endpoints.

## Database transition
`database/migrations/20260830_universal_lookup_alignment.sql` is idempotent. It seeds the complete catalogue and adds canonical lookup FK columns for the highest-use UI fields (student/employee gender, staff/employment type, fee frequency, payment method, guardian relationship), with backfill from legacy text. The old text columns are intentionally not dropped in the same migration: deploy/backfill/verify first, then remove them in the destructive cleanup migration after all callers have moved to IDs.

## UI rule
Do not introduce new hard-coded categorical arrays. Use `core/api/lookupTypes.ts` + `lookupApi.getValues(typeCode)`. Entity selectors such as School, Campus/Branch, Academic Year, Grade Level/Class, Section, Department, Subject, Student, Teacher and Fee Type are domain master-data APIs, not generic lookup values.
