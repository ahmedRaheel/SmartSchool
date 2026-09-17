-- SmartSchool universal lookup alignment.
-- Categorical UI/domain values are represented by saas.lookup_value IDs.

WITH wanted(code,name) AS (VALUES
 ('GENDER','Gender'),('BLOOD_GROUP','Blood Group'),('RELIGION','Religion'),('NATIONALITY','Nationality'),('RELATIONSHIP','Guardian Relationship'),
 ('LEAVE_TYPE','Leave Type'),('STAFF_TYPE','Staff Type'),('EMPLOYEE_STATUS','Employee Status'),('STUDENT_STATUS','Student Status'),
 ('ADMISSION_STATUS','Admission Status'),('INQUIRY_SOURCE','Admission Inquiry Source'),('FEE_FREQUENCY','Fee Frequency'),('PAYMENT_METHOD','Payment Method'),
 ('INVOICE_STATUS','Invoice Status'),('ROOM_TYPE','Room Type'),('DOCUMENT_CATEGORY','Document Category'),('DOCUMENT_PURPOSE','Document Purpose'),
 ('PRIORITY','Priority'),('VEHICLE_STATUS','Vehicle Status'),('DRIVER_STATUS','Driver Status'),('LIBRARY_ITEM_STATUS','Library Item Status'),
 ('LOAN_STATUS','Library Loan Status'),('AI_EXECUTION_STATUS','AI Execution Status'),('KNOWLEDGE_DOCUMENT_STATUS','Knowledge Document Status'),
 ('LIFECYCLE_STATUS','Common Lifecycle Status'),('MARITAL_STATUS','Marital Status'),('CONTACT_RELATIONSHIP','Emergency Contact Relationship')
)
INSERT INTO saas.lookup_type(code,name)
SELECT code,name FROM wanted
ON CONFLICT(code) DO UPDATE SET name=EXCLUDED.name;

WITH seed(type_code,code,name,sort_order) AS (VALUES
 ('GENDER','MALE','Male',1),('GENDER','FEMALE','Female',2),('GENDER','OTHER','Other',3),('GENDER','PREFER_NOT_TO_SAY','Prefer not to say',4),
 ('BLOOD_GROUP','A_POSITIVE','A+',1),('BLOOD_GROUP','A_NEGATIVE','A-',2),('BLOOD_GROUP','B_POSITIVE','B+',3),('BLOOD_GROUP','B_NEGATIVE','B-',4),('BLOOD_GROUP','AB_POSITIVE','AB+',5),('BLOOD_GROUP','AB_NEGATIVE','AB-',6),('BLOOD_GROUP','O_POSITIVE','O+',7),('BLOOD_GROUP','O_NEGATIVE','O-',8),
 ('RELIGION','ISLAM','Islam',1),('RELIGION','CHRISTIANITY','Christianity',2),('RELIGION','HINDUISM','Hinduism',3),('RELIGION','SIKHISM','Sikhism',4),('RELIGION','OTHER','Other',99),
 ('NATIONALITY','PAKISTANI','Pakistani',1),('NATIONALITY','SAUDI','Saudi',2),('NATIONALITY','EMIRATI','Emirati',3),('NATIONALITY','BRITISH','British',4),('NATIONALITY','OTHER','Other',99),
 ('RELATIONSHIP','FATHER','Father',1),('RELATIONSHIP','MOTHER','Mother',2),('RELATIONSHIP','GUARDIAN','Guardian',3),('RELATIONSHIP','BROTHER','Brother',4),('RELATIONSHIP','SISTER','Sister',5),('RELATIONSHIP','GRANDFATHER','Grandfather',6),('RELATIONSHIP','GRANDMOTHER','Grandmother',7),('RELATIONSHIP','OTHER','Other',99),
 ('LEAVE_TYPE','ANNUAL','Annual Leave',1),('LEAVE_TYPE','SICK','Sick Leave',2),('LEAVE_TYPE','CASUAL','Casual Leave',3),('LEAVE_TYPE','MATERNITY','Maternity Leave',4),('LEAVE_TYPE','PATERNITY','Paternity Leave',5),('LEAVE_TYPE','UNPAID','Unpaid Leave',6),
 ('STAFF_TYPE','TEACHER','Teacher',1),('STAFF_TYPE','ADMIN_OFFICER','Admin Officer',2),('STAFF_TYPE','ACCOUNTANT','Accountant',3),('STAFF_TYPE','LIBRARIAN','Librarian',4),('STAFF_TYPE','DRIVER','Driver',5),('STAFF_TYPE','SUPPORT_STAFF','Support Staff',6),('STAFF_TYPE','HEAD_OF_DEPARTMENT','Head of Department',7),
 ('FEE_FREQUENCY','MONTHLY','Monthly',1),('FEE_FREQUENCY','TERM','Term',2),('FEE_FREQUENCY','ANNUAL','Annual',3),('FEE_FREQUENCY','ONE_TIME','One Time',4),
 ('PAYMENT_METHOD','CASH','Cash',1),('PAYMENT_METHOD','BANK_TRANSFER','Bank Transfer',2),('PAYMENT_METHOD','ONLINE_PORTAL','Online Portal',3),('PAYMENT_METHOD','CHEQUE','Cheque',4),('PAYMENT_METHOD','WALLET','Wallet',5),
 ('INVOICE_STATUS','PENDING','Pending',1),('INVOICE_STATUS','PARTIAL','Partially Paid',2),('INVOICE_STATUS','PAID','Paid',3),('INVOICE_STATUS','OVERDUE','Overdue',4),('INVOICE_STATUS','CANCELLED','Cancelled',5),
 ('ADMISSION_STATUS','NEW','New',1),('ADMISSION_STATUS','UNDER_REVIEW','Under Review',2),('ADMISSION_STATUS','TEST_SCHEDULED','Test Scheduled',3),('ADMISSION_STATUS','APPROVED','Approved',4),('ADMISSION_STATUS','REJECTED','Rejected',5),('ADMISSION_STATUS','ENROLLED','Enrolled',6),('ADMISSION_STATUS','WITHDRAWN','Withdrawn',7),
 ('INQUIRY_SOURCE','WALK_IN','Walk-In',1),('INQUIRY_SOURCE','WEBSITE','Website',2),('INQUIRY_SOURCE','REFERRAL','Referral',3),('INQUIRY_SOURCE','AI_CHATBOT','AI Chatbot',4),('INQUIRY_SOURCE','SOCIAL_MEDIA','Social Media',5),('INQUIRY_SOURCE','PHONE','Phone',6),
 ('ROOM_TYPE','CLASSROOM','Classroom',1),('ROOM_TYPE','LABORATORY','Laboratory',2),('ROOM_TYPE','HALL','Hall',3),('ROOM_TYPE','LIBRARY','Library',4),('ROOM_TYPE','STAFF_ROOM','Staff Room',5),
 ('PRIORITY','LOW','Low',1),('PRIORITY','NORMAL','Normal',2),('PRIORITY','HIGH','High',3),('PRIORITY','URGENT','Urgent',4),
 ('MARITAL_STATUS','SINGLE','Single',1),('MARITAL_STATUS','MARRIED','Married',2),('MARITAL_STATUS','DIVORCED','Divorced',3),('MARITAL_STATUS','WIDOWED','Widowed',4),
 ('LIFECYCLE_STATUS','DRAFT','Draft',1),('LIFECYCLE_STATUS','SUBMITTED','Submitted',2),('LIFECYCLE_STATUS','PENDING','Pending',3),('LIFECYCLE_STATUS','ACTIVE','Active',4),('LIFECYCLE_STATUS','INACTIVE','Inactive',5),('LIFECYCLE_STATUS','APPROVED','Approved',6),('LIFECYCLE_STATUS','REJECTED','Rejected',7),('LIFECYCLE_STATUS','COMPLETED','Completed',8),('LIFECYCLE_STATUS','CANCELLED','Cancelled',9)
)
INSERT INTO saas.lookup_value(lookup_type_id,code,name,sort_order,is_active,metadata)
SELECT t.lookup_type_id,s.code,s.name,s.sort_order,TRUE,NULL
FROM seed s JOIN saas.lookup_type t ON t.code=s.type_code
ON CONFLICT(lookup_type_id,code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;

-- Canonical lookup FK columns for UI-backed categorical fields. Existing text is backfilled then removed by a later destructive migration after deployment verification.
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS gender_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS gender_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS staff_type_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS employment_type_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS frequency_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS payment_method_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS relationship_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);

UPDATE student.student s SET gender_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='GENDER' WHERE s.gender_lookup_id IS NULL AND upper(replace(coalesce(s.gender,''),' ','_'))=v.code;
UPDATE hr.employee e SET gender_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='GENDER' WHERE e.gender_lookup_id IS NULL AND upper(replace(coalesce(e.gender,''),' ','_'))=v.code;
UPDATE finance.fee_type f SET frequency_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='FEE_FREQUENCY' WHERE f.frequency_lookup_id IS NULL AND upper(replace(coalesce(f.frequency,''),' ','_'))=v.code;
UPDATE finance.student_payment p SET payment_method_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='PAYMENT_METHOD' WHERE p.payment_method_lookup_id IS NULL AND upper(replace(coalesce(p.payment_method,''),' ','_'))=v.code;
UPDATE student.student_guardian g SET relationship_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='RELATIONSHIP' WHERE g.relationship_lookup_id IS NULL AND upper(replace(coalesce(g.relationship,''),' ','_'))=v.code;
