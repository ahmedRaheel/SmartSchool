-- SmartSchool PostgreSQL reference/master-data seed
-- Run AFTER ReferenceDbContext migrations.
-- Idempotent: updates names/sort-order and inserts only missing rows.

BEGIN;

DO $$
BEGIN
    IF to_regclass('saas.lookup_type') IS NULL THEN
        RAISE EXCEPTION 'saas.lookup_type is missing. Run the ReferenceDbContext migration first.';
    END IF;

    IF to_regclass('saas.lookup_value') IS NULL THEN
        RAISE EXCEPTION 'saas.lookup_value is missing. Run the ReferenceDbContext migration first.';
    END IF;

    IF to_regclass('reference.country') IS NULL
       OR to_regclass('reference.province') IS NULL
       OR to_regclass('reference.city') IS NULL
       OR to_regclass('reference.branch_gender_type') IS NULL
       OR to_regclass('reference.education_level') IS NULL THEN
        RAISE EXCEPTION 'Reference master tables are missing. Run the ReferenceDbContext migration first.';
    END IF;
END $$;

-- -------------------------------------------------------------------------
-- Lookup types
-- -------------------------------------------------------------------------
WITH seed(code, name, is_tenant_scoped) AS (VALUES
    ('ACADEMIC_SYSTEM_TYPE','Academic System Type',FALSE),
    ('ADMISSION_STATUS','Admission Status',FALSE),
    ('AI_EXECUTION_STATUS','AI Execution Status',FALSE),
    ('APPLICATION_STATUS','Job Application Status',FALSE),
    ('APPROVAL_STATUS','Approval Status',FALSE),
    ('ASSIGNMENT_TYPE','Academic Assignment Type',FALSE),
    ('ATTENDANCE_STATUS','Attendance Status',FALSE),
    ('AWARD_TYPE','Award Type',FALSE),
    ('BLOOD_GROUP','Blood Group',FALSE),
    ('CANDIDATE_STATUS','Candidate Status',FALSE),
    ('CONTACT_RELATIONSHIP','Emergency Contact Relationship',FALSE),
    ('CONVERSATION_TYPE','Conversation Type',FALSE),
    ('DOCUMENT_CATEGORY','Document Category',FALSE),
    ('DOCUMENT_PURPOSE','Document Purpose',FALSE),
    ('DOCUMENT_TYPE','Certificate / Letter Type',FALSE),
    ('DRIVER_STATUS','Driver Status',FALSE),
    ('EMPLOYEE_STATUS','Employee Status',FALSE),
    ('EMPLOYMENT_TYPE','Employment Type',TRUE),
    ('ENROLLMENT_TYPE','Course Enrollment Type',FALSE),
    ('EXAM_TYPE','Exam / Assessment Type',FALSE),
    ('FEE_FREQUENCY','Fee Frequency',TRUE),
    ('GENDER','Gender',FALSE),
    ('INCREMENT_REQUEST_TYPE','Increment Request Type',FALSE),
    ('INCREMENT_TYPE','Increment Type',FALSE),
    ('INQUIRY_SOURCE','Admission Inquiry Source',FALSE),
    ('INTERVIEW_TYPE','Interview Type',FALSE),
    ('INVOICE_STATUS','Invoice Status',FALSE),
    ('KNOWLEDGE_DOCUMENT_STATUS','Knowledge Document Status',FALSE),
    ('LEAVE_TYPE','Leave Type',TRUE),
    ('LIBRARY_ITEM_STATUS','Library Item Status',FALSE),
    ('LIFECYCLE_STATUS','Common Lifecycle Status',FALSE),
    ('LOAN_STATUS','Library Loan Status',FALSE),
    ('MARITAL_STATUS','Marital Status',FALSE),
    ('MESSAGE_TYPE','Message Type',FALSE),
    ('NATIONALITY','Nationality',FALSE),
    ('NOTIFICATION_CHANNEL','Notification Channel',FALSE),
    ('PAYMENT_METHOD','Payment Method',TRUE),
    ('PAYROLL_STATUS','Payroll Status',FALSE),
    ('PRIORITY','Priority',FALSE),
    ('RELATIONSHIP','Guardian Relationship',FALSE),
    ('RELIGION','Religion',FALSE),
    ('ROOM_TYPE','Room Type',FALSE),
    ('STAFF_TYPE','Staff Type',FALSE),
    ('STUDENT_STATUS','Student Status',FALSE),
    ('SUBJECT_REQUIREMENT_TYPE','Subject Requirement Type',FALSE),
    ('TENANT_STATUS','Tenant Status',FALSE),
    ('VEHICLE_STATUS','Vehicle Status',FALSE),
    ('WORK_ASSIGNMENT_STATUS','Work Assignment Status',FALSE)
)
INSERT INTO saas.lookup_type(code, name, is_tenant_scoped)
SELECT code, name, is_tenant_scoped
FROM seed
ON CONFLICT (code) DO UPDATE
SET name = EXCLUDED.name,
    is_tenant_scoped = EXCLUDED.is_tenant_scoped;

-- -------------------------------------------------------------------------
-- Lookup values. Universal types are stored with tenant_id = NULL.
-- Tenant-scoped types are copied to every existing tenant.
-- -------------------------------------------------------------------------
CREATE TEMP TABLE tmp_smartschool_lookup_seed (
    type_code varchar(80) NOT NULL,
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    sort_order integer NOT NULL
) ON COMMIT DROP;

INSERT INTO tmp_smartschool_lookup_seed(type_code, code, name, sort_order) VALUES
    ('ACADEMIC_SYSTEM_TYPE','CAMBRIDGE','Cambridge',1),
    ('ACADEMIC_SYSTEM_TYPE','MATRIC','Matric / SSC',2),
    ('ACADEMIC_SYSTEM_TYPE','INTERMEDIATE','Intermediate / HSSC',3),
    ('ACADEMIC_SYSTEM_TYPE','IB','International Baccalaureate',4),
    ('ACADEMIC_SYSTEM_TYPE','AMERICAN','American',5),
    ('ACADEMIC_SYSTEM_TYPE','CUSTOM','Custom',99),
    ('ADMISSION_STATUS','NEW','New',1),
    ('ADMISSION_STATUS','UNDER_REVIEW','Under Review',2),
    ('ADMISSION_STATUS','TEST_SCHEDULED','Test Scheduled',3),
    ('ADMISSION_STATUS','APPROVED','Approved',4),
    ('ADMISSION_STATUS','REJECTED','Rejected',5),
    ('ADMISSION_STATUS','ENROLLED','Enrolled',6),
    ('ADMISSION_STATUS','WITHDRAWN','Withdrawn',7),
    ('APPLICATION_STATUS','APPLIED','Applied',1),
    ('APPLICATION_STATUS','SCREENING','Screening',2),
    ('APPLICATION_STATUS','SHORTLISTED','Shortlisted',3),
    ('APPLICATION_STATUS','INTERVIEW','Interview',4),
    ('APPLICATION_STATUS','OFFERED','Offered',5),
    ('APPLICATION_STATUS','HIRED','Hired',6),
    ('APPLICATION_STATUS','REJECTED','Rejected',7),
    ('APPLICATION_STATUS','WITHDRAWN','Withdrawn',8),
    ('APPROVAL_STATUS','DRAFT','Draft',1),
    ('APPROVAL_STATUS','PENDING','Pending',2),
    ('APPROVAL_STATUS','APPROVED','Approved',3),
    ('APPROVAL_STATUS','REJECTED','Rejected',4),
    ('APPROVAL_STATUS','CANCELLED','Cancelled',5),
    ('ASSIGNMENT_TYPE','HOMEWORK','Homework',1),
    ('ASSIGNMENT_TYPE','CLASSWORK','Classwork',2),
    ('ASSIGNMENT_TYPE','PROJECT','Project',3),
    ('ASSIGNMENT_TYPE','RESEARCH','Research',4),
    ('ASSIGNMENT_TYPE','PRESENTATION','Presentation',5),
    ('ASSIGNMENT_TYPE','PRACTICAL','Practical',6),
    ('ASSIGNMENT_TYPE','LAB_WORK','Lab Work',7),
    ('ASSIGNMENT_TYPE','ESSAY','Essay',8),
    ('ASSIGNMENT_TYPE','READING','Reading',9),
    ('ASSIGNMENT_TYPE','GROUP_WORK','Group Work',10),
    ('ASSIGNMENT_TYPE','HOLIDAY_HOMEWORK','Holiday Homework',11),
    ('ASSIGNMENT_TYPE','CUSTOM','Custom',99),
    ('ATTENDANCE_STATUS','PRESENT','Present',1),
    ('ATTENDANCE_STATUS','ABSENT','Absent',2),
    ('ATTENDANCE_STATUS','LATE','Late',3),
    ('ATTENDANCE_STATUS','EXCUSED','Excused',4),
    ('ATTENDANCE_STATUS','LEAVE','Leave',5),
    ('ATTENDANCE_STATUS','HALF_DAY','Half Day',6),
    ('AWARD_TYPE','STUDENT_OF_MONTH','Student of the Month',1),
    ('AWARD_TYPE','ACADEMIC_EXCELLENCE','Academic Excellence',2),
    ('AWARD_TYPE','BEST_ATTENDANCE','Best Attendance',3),
    ('AWARD_TYPE','MOST_IMPROVED','Most Improved',4),
    ('AWARD_TYPE','LEADERSHIP','Leadership',5),
    ('AWARD_TYPE','SPORTS_EXCELLENCE','Sports Excellence',6),
    ('AWARD_TYPE','COMMUNITY_SERVICE','Community Service',7),
    ('AWARD_TYPE','APPRECIATION','Appreciation',8),
    ('BLOOD_GROUP','A_POSITIVE','A+',1),
    ('BLOOD_GROUP','A_NEGATIVE','A-',2),
    ('BLOOD_GROUP','B_POSITIVE','B+',3),
    ('BLOOD_GROUP','B_NEGATIVE','B-',4),
    ('BLOOD_GROUP','AB_POSITIVE','AB+',5),
    ('BLOOD_GROUP','AB_NEGATIVE','AB-',6),
    ('BLOOD_GROUP','O_POSITIVE','O+',7),
    ('BLOOD_GROUP','O_NEGATIVE','O-',8),
    ('CANDIDATE_STATUS','NEW','New',1),
    ('CANDIDATE_STATUS','SCREENING','Screening',2),
    ('CANDIDATE_STATUS','SHORTLISTED','Shortlisted',3),
    ('CANDIDATE_STATUS','INTERVIEW','Interview',4),
    ('CANDIDATE_STATUS','ASSESSMENT','Assessment',5),
    ('CANDIDATE_STATUS','SELECTED','Selected',6),
    ('CANDIDATE_STATUS','OFFER','Offer',7),
    ('CANDIDATE_STATUS','HIRED','Hired',8),
    ('CANDIDATE_STATUS','REJECTED','Rejected',9),
    ('CANDIDATE_STATUS','WITHDRAWN','Withdrawn',10),
    ('CANDIDATE_STATUS','ON_HOLD','On Hold',11),
    ('CONVERSATION_TYPE','PARENT_TEACHER','Parent / Teacher',1),
    ('CONVERSATION_TYPE','CLASS','Class Channel',2),
    ('CONVERSATION_TYPE','SUBJECT','Subject Channel',3),
    ('CONVERSATION_TYPE','ADMIN','Administration',4),
    ('CONVERSATION_TYPE','STAFF','Staff',5),
    ('DOCUMENT_TYPE','SCHOOL_LEAVING','School Leaving Certificate',1),
    ('DOCUMENT_TYPE','TRANSFER','Transfer Certificate',2),
    ('DOCUMENT_TYPE','MIGRATION','Migration Certificate',3),
    ('DOCUMENT_TYPE','CHARACTER','Character / Conduct Certificate',4),
    ('DOCUMENT_TYPE','BONAFIDE','Bonafide / Enrollment Certificate',5),
    ('DOCUMENT_TYPE','APPRECIATION','Appreciation Certificate',6),
    ('DOCUMENT_TYPE','STUDENT_OF_MONTH','Student of the Month Certificate',7),
    ('DOCUMENT_TYPE','ACHIEVEMENT','Achievement Certificate',8),
    ('DOCUMENT_TYPE','SPORTS','Sports Certificate',9),
    ('DOCUMENT_TYPE','ACTIVITY','Co-curricular Activity Certificate',10),
    ('DOCUMENT_TYPE','ADMISSION_OFFER','Admission Offer Letter',11),
    ('DOCUMENT_TYPE','WARNING','Warning Letter',12),
    ('DOCUMENT_TYPE','EMPLOYMENT','Employment Letter',13),
    ('DOCUMENT_TYPE','EXPERIENCE','Experience Letter',14),
    ('DOCUMENT_TYPE','CUSTOM','Custom Document',99),
    ('EMPLOYMENT_TYPE','PERMANENT','Permanent',1),
    ('EMPLOYMENT_TYPE','CONTRACT','Contract',2),
    ('EMPLOYMENT_TYPE','PART_TIME','Part Time',3),
    ('EMPLOYMENT_TYPE','TEMPORARY','Temporary',4),
    ('EMPLOYMENT_TYPE','VISITING','Visiting',5),
    ('EMPLOYMENT_TYPE','INTERN','Intern',6),
    ('ENROLLMENT_TYPE','MANDATORY','Mandatory',1),
    ('ENROLLMENT_TYPE','ELECTIVE','Elective',2),
    ('ENROLLMENT_TYPE','OPTIONAL','Optional',3),
    ('ENROLLMENT_TYPE','TRANSFERRED','Transferred',4),
    ('EXAM_TYPE','QUIZ','Quiz',1),
    ('EXAM_TYPE','CLASS_TEST','Class Test',2),
    ('EXAM_TYPE','WEEKLY_TEST','Weekly Test',3),
    ('EXAM_TYPE','MONTHLY_TEST','Monthly Test',4),
    ('EXAM_TYPE','UNIT_TEST','Unit / Chapter Test',5),
    ('EXAM_TYPE','MIDTERM','Midterm',6),
    ('EXAM_TYPE','TERM','Term Examination',7),
    ('EXAM_TYPE','PREBOARD','Pre-Board',8),
    ('EXAM_TYPE','MOCK','Mock Examination',9),
    ('EXAM_TYPE','ANNUAL','Annual Examination',10),
    ('EXAM_TYPE','FINAL','Final Examination',11),
    ('EXAM_TYPE','PRACTICAL','Practical',12),
    ('EXAM_TYPE','VIVA','Oral / Viva',13),
    ('EXAM_TYPE','PROJECT','Project / Coursework',14),
    ('EXAM_TYPE','SUPPLEMENTARY','Supplementary',15),
    ('EXAM_TYPE','RESIT','Re-sit',16),
    ('FEE_FREQUENCY','MONTHLY','Monthly',1),
    ('FEE_FREQUENCY','TERM','Term',2),
    ('FEE_FREQUENCY','ANNUAL','Annual',3),
    ('FEE_FREQUENCY','ONE_TIME','One Time',4),
    ('GENDER','MALE','Male',1),
    ('GENDER','FEMALE','Female',2),
    ('GENDER','OTHER','Other',3),
    ('GENDER','PREFER_NOT_TO_SAY','Prefer not to say',4),
    ('INCREMENT_REQUEST_TYPE','AUTO','Automatic Proposal',1),
    ('INCREMENT_REQUEST_TYPE','MANUAL','Manual Proposal',2),
    ('INCREMENT_TYPE','PERCENTAGE','Percentage',1),
    ('INCREMENT_TYPE','FIXED','Fixed Amount',2),
    ('INCREMENT_TYPE','NEW_SALARY','New Salary',3),
    ('INCREMENT_TYPE','GRADE_STEP','Grade / Step',4),
    ('INQUIRY_SOURCE','WALK_IN','Walk-In',1),
    ('INQUIRY_SOURCE','WEBSITE','Website',2),
    ('INQUIRY_SOURCE','REFERRAL','Referral',3),
    ('INQUIRY_SOURCE','AI_CHATBOT','AI Chatbot',4),
    ('INQUIRY_SOURCE','SOCIAL_MEDIA','Social Media',5),
    ('INQUIRY_SOURCE','PHONE','Phone',6),
    ('INTERVIEW_TYPE','HR_SCREENING','HR Screening',1),
    ('INTERVIEW_TYPE','SUBJECT','Subject / Technical Interview',2),
    ('INTERVIEW_TYPE','TEACHING_DEMO','Teaching Demo',3),
    ('INTERVIEW_TYPE','PANEL','Panel Interview',4),
    ('INTERVIEW_TYPE','PRINCIPAL','Principal Interview',5),
    ('INTERVIEW_TYPE','FINAL','Final Interview',6),
    ('INVOICE_STATUS','PENDING','Pending',1),
    ('INVOICE_STATUS','PARTIAL','Partially Paid',2),
    ('INVOICE_STATUS','PAID','Paid',3),
    ('INVOICE_STATUS','OVERDUE','Overdue',4),
    ('INVOICE_STATUS','CANCELLED','Cancelled',5),
    ('AI_EXECUTION_STATUS','QUEUED','Queued',1),
    ('AI_EXECUTION_STATUS','RUNNING','Running',2),
    ('AI_EXECUTION_STATUS','SUCCEEDED','Succeeded',3),
    ('AI_EXECUTION_STATUS','FAILED','Failed',4),
    ('AI_EXECUTION_STATUS','CANCELLED','Cancelled',5),
    ('CONTACT_RELATIONSHIP','PARENT','Parent',1),
    ('CONTACT_RELATIONSHIP','SPOUSE','Spouse',2),
    ('CONTACT_RELATIONSHIP','SIBLING','Sibling',3),
    ('CONTACT_RELATIONSHIP','CHILD','Child',4),
    ('CONTACT_RELATIONSHIP','RELATIVE','Relative',5),
    ('CONTACT_RELATIONSHIP','FRIEND','Friend',6),
    ('CONTACT_RELATIONSHIP','OTHER','Other',99),
    ('DOCUMENT_CATEGORY','STUDENT','Student',1),
    ('DOCUMENT_CATEGORY','STAFF','Staff',2),
    ('DOCUMENT_CATEGORY','ACADEMIC','Academic',3),
    ('DOCUMENT_CATEGORY','FINANCE','Finance',4),
    ('DOCUMENT_CATEGORY','HR','Human Resources',5),
    ('DOCUMENT_CATEGORY','LEGAL','Legal',6),
    ('DOCUMENT_CATEGORY','GENERAL','General',99),
    ('DOCUMENT_PURPOSE','IDENTITY','Identity',1),
    ('DOCUMENT_PURPOSE','ADMISSION','Admission',2),
    ('DOCUMENT_PURPOSE','ACADEMIC','Academic',3),
    ('DOCUMENT_PURPOSE','EMPLOYMENT','Employment',4),
    ('DOCUMENT_PURPOSE','FINANCE','Finance',5),
    ('DOCUMENT_PURPOSE','CERTIFICATE','Certificate / Letter',6),
    ('DOCUMENT_PURPOSE','COMMUNICATION','Communication',7),
    ('DOCUMENT_PURPOSE','OTHER','Other',99),
    ('DRIVER_STATUS','ACTIVE','Active',1),
    ('DRIVER_STATUS','ON_LEAVE','On Leave',2),
    ('DRIVER_STATUS','SUSPENDED','Suspended',3),
    ('DRIVER_STATUS','INACTIVE','Inactive',4),
    ('DRIVER_STATUS','TERMINATED','Terminated',5),
    ('EMPLOYEE_STATUS','ACTIVE','Active',1),
    ('EMPLOYEE_STATUS','PROBATION','Probation',2),
    ('EMPLOYEE_STATUS','ON_LEAVE','On Leave',3),
    ('EMPLOYEE_STATUS','SUSPENDED','Suspended',4),
    ('EMPLOYEE_STATUS','RESIGNED','Resigned',5),
    ('EMPLOYEE_STATUS','TERMINATED','Terminated',6),
    ('EMPLOYEE_STATUS','RETIRED','Retired',7),
    ('KNOWLEDGE_DOCUMENT_STATUS','UPLOADED','Uploaded',1),
    ('KNOWLEDGE_DOCUMENT_STATUS','PROCESSING','Processing',2),
    ('KNOWLEDGE_DOCUMENT_STATUS','INDEXED','Indexed',3),
    ('KNOWLEDGE_DOCUMENT_STATUS','FAILED','Failed',4),
    ('KNOWLEDGE_DOCUMENT_STATUS','ARCHIVED','Archived',5),
    ('LIBRARY_ITEM_STATUS','AVAILABLE','Available',1),
    ('LIBRARY_ITEM_STATUS','ISSUED','Issued',2),
    ('LIBRARY_ITEM_STATUS','RESERVED','Reserved',3),
    ('LIBRARY_ITEM_STATUS','LOST','Lost',4),
    ('LIBRARY_ITEM_STATUS','DAMAGED','Damaged',5),
    ('LIBRARY_ITEM_STATUS','INACTIVE','Inactive',6),
    ('LOAN_STATUS','ISSUED','Issued',1),
    ('LOAN_STATUS','RETURNED','Returned',2),
    ('LOAN_STATUS','OVERDUE','Overdue',3),
    ('LOAN_STATUS','LOST','Lost',4),
    ('LOAN_STATUS','DAMAGED','Damaged',5),
    ('STUDENT_STATUS','ACTIVE','Active',1),
    ('STUDENT_STATUS','INACTIVE','Inactive',2),
    ('STUDENT_STATUS','GRADUATED','Graduated',3),
    ('STUDENT_STATUS','TRANSFERRED','Transferred',4),
    ('STUDENT_STATUS','STRUCK_OFF','Struck Off',5),
    ('STUDENT_STATUS','SUSPENDED','Suspended',6),
    ('STUDENT_STATUS','WITHDRAWN','Withdrawn',7),
    ('VEHICLE_STATUS','ACTIVE','Active',1),
    ('VEHICLE_STATUS','IN_SERVICE','In Service',2),
    ('VEHICLE_STATUS','MAINTENANCE','Maintenance',3),
    ('VEHICLE_STATUS','OUT_OF_SERVICE','Out of Service',4),
    ('VEHICLE_STATUS','RETIRED','Retired',5),
    ('LEAVE_TYPE','ANNUAL','Annual Leave',1),
    ('LEAVE_TYPE','SICK','Sick Leave',2),
    ('LEAVE_TYPE','CASUAL','Casual Leave',3),
    ('LEAVE_TYPE','MATERNITY','Maternity Leave',4),
    ('LEAVE_TYPE','PATERNITY','Paternity Leave',5),
    ('LEAVE_TYPE','UNPAID','Unpaid Leave',6),
    ('LIFECYCLE_STATUS','DRAFT','Draft',1),
    ('LIFECYCLE_STATUS','SUBMITTED','Submitted',2),
    ('LIFECYCLE_STATUS','PENDING','Pending',3),
    ('LIFECYCLE_STATUS','ACTIVE','Active',4),
    ('LIFECYCLE_STATUS','INACTIVE','Inactive',5),
    ('LIFECYCLE_STATUS','APPROVED','Approved',6),
    ('LIFECYCLE_STATUS','REJECTED','Rejected',7),
    ('LIFECYCLE_STATUS','COMPLETED','Completed',8),
    ('LIFECYCLE_STATUS','CANCELLED','Cancelled',9),
    ('MARITAL_STATUS','SINGLE','Single',1),
    ('MARITAL_STATUS','MARRIED','Married',2),
    ('MARITAL_STATUS','DIVORCED','Divorced',3),
    ('MARITAL_STATUS','WIDOWED','Widowed',4),
    ('MESSAGE_TYPE','TEXT','Text',1),
    ('MESSAGE_TYPE','IMAGE','Image',2),
    ('MESSAGE_TYPE','FILE','File',3),
    ('MESSAGE_TYPE','VOICE','Voice Note',4),
    ('MESSAGE_TYPE','SYSTEM','System',5),
    ('NATIONALITY','PAKISTANI','Pakistani',1),
    ('NATIONALITY','SAUDI','Saudi',2),
    ('NATIONALITY','EMIRATI','Emirati',3),
    ('NATIONALITY','BRITISH','British',4),
    ('NATIONALITY','OTHER','Other',99),
    ('NOTIFICATION_CHANNEL','IN_APP','In-App',1),
    ('NOTIFICATION_CHANNEL','PUSH','Push',2),
    ('NOTIFICATION_CHANNEL','EMAIL','Email',3),
    ('NOTIFICATION_CHANNEL','SMS','SMS',4),
    ('NOTIFICATION_CHANNEL','WHATSAPP','WhatsApp',5),
    ('PAYMENT_METHOD','CASH','Cash',1),
    ('PAYMENT_METHOD','BANK_TRANSFER','Bank Transfer',2),
    ('PAYMENT_METHOD','ONLINE_PORTAL','Online Portal',3),
    ('PAYMENT_METHOD','CHEQUE','Cheque',4),
    ('PAYMENT_METHOD','WALLET','Wallet',5),
    ('PAYROLL_STATUS','DRAFT','Draft',1),
    ('PAYROLL_STATUS','CALCULATED','Calculated',2),
    ('PAYROLL_STATUS','HR_REVIEW','HR Review',3),
    ('PAYROLL_STATUS','FINANCE_REVIEW','Finance Review',4),
    ('PAYROLL_STATUS','APPROVED','Approved',5),
    ('PAYROLL_STATUS','LOCKED','Locked',6),
    ('PAYROLL_STATUS','PAID','Paid',7),
    ('PRIORITY','LOW','Low',1),
    ('PRIORITY','NORMAL','Normal',2),
    ('PRIORITY','HIGH','High',3),
    ('PRIORITY','URGENT','Urgent',4),
    ('RELATIONSHIP','FATHER','Father',1),
    ('RELATIONSHIP','MOTHER','Mother',2),
    ('RELATIONSHIP','GUARDIAN','Guardian',3),
    ('RELATIONSHIP','BROTHER','Brother',4),
    ('RELATIONSHIP','SISTER','Sister',5),
    ('RELATIONSHIP','GRANDFATHER','Grandfather',6),
    ('RELATIONSHIP','GRANDMOTHER','Grandmother',7),
    ('RELATIONSHIP','OTHER','Other',99),
    ('RELIGION','ISLAM','Islam',1),
    ('RELIGION','CHRISTIANITY','Christianity',2),
    ('RELIGION','HINDUISM','Hinduism',3),
    ('RELIGION','SIKHISM','Sikhism',4),
    ('RELIGION','OTHER','Other',99),
    ('ROOM_TYPE','CLASSROOM','Classroom',1),
    ('ROOM_TYPE','LABORATORY','Laboratory',2),
    ('ROOM_TYPE','HALL','Hall',3),
    ('ROOM_TYPE','LIBRARY','Library',4),
    ('ROOM_TYPE','STAFF_ROOM','Staff Room',5),
    ('STAFF_TYPE','TEACHER','Teacher',1),
    ('STAFF_TYPE','ADMIN_OFFICER','Admin Officer',2),
    ('STAFF_TYPE','ACCOUNTANT','Accountant',3),
    ('STAFF_TYPE','LIBRARIAN','Librarian',4),
    ('STAFF_TYPE','DRIVER','Driver',5),
    ('STAFF_TYPE','SUPPORT_STAFF','Support Staff',6),
    ('STAFF_TYPE','HEAD_OF_DEPARTMENT','Head of Department',7),
    ('SUBJECT_REQUIREMENT_TYPE','MANDATORY','Mandatory',1),
    ('SUBJECT_REQUIREMENT_TYPE','OPTIONAL','Optional',2),
    ('SUBJECT_REQUIREMENT_TYPE','ELECTIVE','Elective',3),
    ('TENANT_STATUS','TRIAL','Trial',1),
    ('TENANT_STATUS','ACTIVE','Active',2),
    ('TENANT_STATUS','SUSPENDED','Suspended',3),
    ('TENANT_STATUS','CANCELLED','Cancelled',4),
    ('WORK_ASSIGNMENT_STATUS','DRAFT','Draft',1),
    ('WORK_ASSIGNMENT_STATUS','ASSIGNED','Assigned',2),
    ('WORK_ASSIGNMENT_STATUS','ACCEPTED','Accepted',3),
    ('WORK_ASSIGNMENT_STATUS','IN_PROGRESS','In Progress',4),
    ('WORK_ASSIGNMENT_STATUS','BLOCKED','Blocked',5),
    ('WORK_ASSIGNMENT_STATUS','COMPLETED','Completed',6),
    ('WORK_ASSIGNMENT_STATUS','REJECTED','Rejected',7),
    ('WORK_ASSIGNMENT_STATUS','CANCELLED','Cancelled',8),
    ('WORK_ASSIGNMENT_STATUS','OVERDUE','Overdue',9);

-- Update existing universal values.
UPDATE saas.lookup_value existing
SET name = seed.name,
    sort_order = seed.sort_order,
    is_active = TRUE
FROM tmp_smartschool_lookup_seed seed
JOIN saas.lookup_type type ON type.code = seed.type_code
WHERE existing.lookup_type_id = type.lookup_type_id
  AND type.is_tenant_scoped = FALSE
  AND existing.tenant_id IS NULL
  AND existing.code = seed.code;

-- Insert missing universal values.
INSERT INTO saas.lookup_value(
    lookup_type_id, tenant_id, code, name, sort_order, is_active, metadata)
SELECT
    type.lookup_type_id, NULL, seed.code, seed.name, seed.sort_order, TRUE, NULL
FROM tmp_smartschool_lookup_seed seed
JOIN saas.lookup_type type ON type.code = seed.type_code
WHERE type.is_tenant_scoped = FALSE
  AND NOT EXISTS (
      SELECT 1
      FROM saas.lookup_value existing
      WHERE existing.lookup_type_id = type.lookup_type_id
        AND existing.tenant_id IS NULL
        AND existing.code = seed.code
  );

-- Update existing tenant-scoped values for every current tenant.
UPDATE saas.lookup_value existing
SET name = seed.name,
    sort_order = seed.sort_order,
    is_active = TRUE
FROM tmp_smartschool_lookup_seed seed
JOIN saas.lookup_type type ON type.code = seed.type_code
WHERE type.is_tenant_scoped = TRUE
  AND existing.lookup_type_id = type.lookup_type_id
  AND existing.code = seed.code
  AND existing.tenant_id IS NOT NULL;

-- Insert missing tenant-scoped values for every current tenant.
INSERT INTO saas.lookup_value(
    lookup_type_id, tenant_id, code, name, sort_order, is_active, metadata)
SELECT
    type.lookup_type_id, tenant.tenant_id, seed.code, seed.name, seed.sort_order, TRUE, NULL
FROM tmp_smartschool_lookup_seed seed
JOIN saas.lookup_type type ON type.code = seed.type_code
CROSS JOIN saas.tenant tenant
WHERE type.is_tenant_scoped = TRUE
  AND NOT EXISTS (
      SELECT 1
      FROM saas.lookup_value existing
      WHERE existing.lookup_type_id = type.lookup_type_id
        AND existing.tenant_id = tenant.tenant_id
        AND existing.code = seed.code
  );

-- -------------------------------------------------------------------------
-- Branch policy reference data
-- -------------------------------------------------------------------------
INSERT INTO reference.branch_gender_type(
    branch_gender_type_id, code, name, sort_order, is_active)
VALUES
    ('10000000-0000-0000-0000-000000000001','BOYS_ONLY','Boys Only',1,TRUE),
    ('10000000-0000-0000-0000-000000000002','GIRLS_ONLY','Girls Only',2,TRUE),
    ('10000000-0000-0000-0000-000000000003','CO_EDUCATION','Co-Education',3,TRUE)
ON CONFLICT (code) DO UPDATE
SET name = EXCLUDED.name,
    sort_order = EXCLUDED.sort_order,
    is_active = TRUE;

INSERT INTO reference.education_level(
    education_level_id, code, name, sort_order, is_active)
VALUES
    ('20000000-0000-0000-0000-000000000001','PRE_PRIMARY','Pre-Primary',1,TRUE),
    ('20000000-0000-0000-0000-000000000002','PRIMARY','Primary',2,TRUE),
    ('20000000-0000-0000-0000-000000000003','MIDDLE','Middle',3,TRUE),
    ('20000000-0000-0000-0000-000000000004','SECONDARY','Secondary',4,TRUE),
    ('20000000-0000-0000-0000-000000000005','HIGHER_SECONDARY','Higher Secondary',5,TRUE)
ON CONFLICT (code) DO UPDATE
SET name = EXCLUDED.name,
    sort_order = EXCLUDED.sort_order,
    is_active = TRUE;

-- -------------------------------------------------------------------------
-- Geography baseline
-- -------------------------------------------------------------------------
INSERT INTO reference.country(code, name)
VALUES ('PK', 'Pakistan')
ON CONFLICT (code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.province(country_id, code, name)
SELECT country_id, 'SD', 'Sindh' FROM reference.country WHERE code = 'PK'
ON CONFLICT (country_id, code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.province(country_id, code, name)
SELECT country_id, 'PB', 'Punjab' FROM reference.country WHERE code = 'PK'
ON CONFLICT (country_id, code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.province(country_id, code, name)
SELECT country_id, 'KP', 'Khyber Pakhtunkhwa' FROM reference.country WHERE code = 'PK'
ON CONFLICT (country_id, code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.province(country_id, code, name)
SELECT country_id, 'BA', 'Balochistan' FROM reference.country WHERE code = 'PK'
ON CONFLICT (country_id, code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.city(province_id, code, name)
SELECT province_id, 'KHI', 'Karachi' FROM reference.province WHERE code = 'SD'
ON CONFLICT (province_id, code) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO reference.city(province_id, code, name)
SELECT province_id, 'LHE', 'Lahore' FROM reference.province WHERE code = 'PB'
ON CONFLICT (province_id, code) DO UPDATE SET name = EXCLUDED.name;

COMMIT;

-- Verification summary
SELECT 'lookup_type' AS object_name, COUNT(*) AS row_count FROM saas.lookup_type
UNION ALL
SELECT 'lookup_value', COUNT(*) FROM saas.lookup_value
UNION ALL
SELECT 'branch_gender_type', COUNT(*) FROM reference.branch_gender_type
UNION ALL
SELECT 'education_level', COUNT(*) FROM reference.education_level
UNION ALL
SELECT 'country', COUNT(*) FROM reference.country
UNION ALL
SELECT 'province', COUNT(*) FROM reference.province
UNION ALL
SELECT 'city', COUNT(*) FROM reference.city
ORDER BY object_name;
