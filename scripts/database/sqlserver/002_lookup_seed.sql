-- SmartSchool SQL Server - Lookup Seed Data
SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'BUSINESS')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000001', 'BUSINESS', 'Business Owner', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'PRIVATE_EMPLOYEE')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000002', 'PRIVATE_EMPLOYEE', 'Private Sector Employee', 2);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'GOVERNMENT_EMPLOYEE')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000003', 'GOVERNMENT_EMPLOYEE', 'Government Employee', 3);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'TEACHER')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000004', 'TEACHER', 'Teacher', 4);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'DOCTOR')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000005', 'DOCTOR', 'Doctor', 5);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'ENGINEER')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000006', 'ENGINEER', 'Engineer', 6);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'LAWYER')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000007', 'LAWYER', 'Lawyer', 7);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'SELF_EMPLOYED')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000008', 'SELF_EMPLOYED', 'Self Employed', 8);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'HOMEMAKER')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000009', 'HOMEMAKER', 'Homemaker', 9);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'RETIRED')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000010', 'RETIRED', 'Retired', 10);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'UNEMPLOYED')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000011', 'UNEMPLOYED', 'Unemployed', 11);

IF NOT EXISTS (SELECT 1 FROM dbo.OccupationType WHERE Code = 'OTHER')
    INSERT INTO dbo.OccupationType (Id, Code, Name, DisplayOrder) VALUES ('10000000-0000-0000-0000-000000000012', 'OTHER', 'Other', 99);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'FATHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000001', 'FATHER', 'Father', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'MOTHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000002', 'MOTHER', 'Mother', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'GUARDIAN')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000003', 'GUARDIAN', 'Guardian', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'BROTHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000004', 'BROTHER', 'Brother', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'SISTER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000005', 'SISTER', 'Sister', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'GRANDFATHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000006', 'GRANDFATHER', 'Grandfather', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'GRANDMOTHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000007', 'GRANDMOTHER', 'Grandmother', 7, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'UNCLE')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000008', 'UNCLE', 'Uncle', 8, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'AUNT')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000009', 'AUNT', 'Aunt', 9, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.RelationshipType WHERE Code = 'OTHER')
    INSERT INTO dbo.RelationshipType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('11000000-0000-0000-0000-000000000010', 'OTHER', 'Other', 99, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.GenderType WHERE Code = 'MALE')
    INSERT INTO dbo.GenderType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('12000000-0000-0000-0000-000000000001', 'MALE', 'Male', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.GenderType WHERE Code = 'FEMALE')
    INSERT INTO dbo.GenderType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('12000000-0000-0000-0000-000000000002', 'FEMALE', 'Female', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.GenderType WHERE Code = 'OTHER')
    INSERT INTO dbo.GenderType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('12000000-0000-0000-0000-000000000003', 'OTHER', 'Other', 99, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'A+')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000001', 'A+', 'A+', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'A-')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000002', 'A-', 'A-', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'B+')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000003', 'B+', 'B+', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'B-')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000004', 'B-', 'B-', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'AB+')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000005', 'AB+', 'AB+', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'AB-')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000006', 'AB-', 'AB-', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'O+')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000007', 'O+', 'O+', 7, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.BloodGroupType WHERE Code = 'O-')
    INSERT INTO dbo.BloodGroupType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('13000000-0000-0000-0000-000000000008', 'O-', 'O-', 8, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'ACTIVE')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000001', 'ACTIVE', 'Active', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'PROBATION')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000002', 'PROBATION', 'Probation', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'ON_LEAVE')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000003', 'ON_LEAVE', 'On Leave', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'SUSPENDED')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000004', 'SUSPENDED', 'Suspended', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'RESIGNED')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000005', 'RESIGNED', 'Resigned', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'TERMINATED')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000006', 'TERMINATED', 'Terminated', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentStatusType WHERE Code = 'RETIRED')
    INSERT INTO dbo.EmploymentStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('14000000-0000-0000-0000-000000000007', 'RETIRED', 'Retired', 7, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentType WHERE Code = 'PERMANENT')
    INSERT INTO dbo.EmploymentType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('15000000-0000-0000-0000-000000000001', 'PERMANENT', 'Permanent', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentType WHERE Code = 'CONTRACT')
    INSERT INTO dbo.EmploymentType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('15000000-0000-0000-0000-000000000002', 'CONTRACT', 'Contract', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentType WHERE Code = 'PART_TIME')
    INSERT INTO dbo.EmploymentType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('15000000-0000-0000-0000-000000000003', 'PART_TIME', 'Part Time', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentType WHERE Code = 'TEMPORARY')
    INSERT INTO dbo.EmploymentType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('15000000-0000-0000-0000-000000000004', 'TEMPORARY', 'Temporary', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.EmploymentType WHERE Code = 'INTERN')
    INSERT INTO dbo.EmploymentType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('15000000-0000-0000-0000-000000000005', 'INTERN', 'Intern', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.MaritalStatusType WHERE Code = 'SINGLE')
    INSERT INTO dbo.MaritalStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('16000000-0000-0000-0000-000000000001', 'SINGLE', 'Single', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.MaritalStatusType WHERE Code = 'MARRIED')
    INSERT INTO dbo.MaritalStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('16000000-0000-0000-0000-000000000002', 'MARRIED', 'Married', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.MaritalStatusType WHERE Code = 'DIVORCED')
    INSERT INTO dbo.MaritalStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('16000000-0000-0000-0000-000000000003', 'DIVORCED', 'Divorced', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.MaritalStatusType WHERE Code = 'WIDOWED')
    INSERT INTO dbo.MaritalStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('16000000-0000-0000-0000-000000000004', 'WIDOWED', 'Widowed', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'CASH')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000001', 'CASH', 'Cash', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'BANK_TRANSFER')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000002', 'BANK_TRANSFER', 'Bank Transfer', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'CARD')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000003', 'CARD', 'Debit/Credit Card', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'CHEQUE')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000004', 'CHEQUE', 'Cheque', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'ONLINE')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000005', 'ONLINE', 'Online Payment', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PaymentMethodType WHERE Code = 'MOBILE_WALLET')
    INSERT INTO dbo.PaymentMethodType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('17000000-0000-0000-0000-000000000006', 'MOBILE_WALLET', 'Mobile Wallet', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'PENDING')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000001', 'PENDING', 'Pending', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'PARTIALLY_PAID')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000002', 'PARTIALLY_PAID', 'Partially Paid', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'PAID')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000003', 'PAID', 'Paid', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'OVERDUE')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000004', 'OVERDUE', 'Overdue', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'WAIVED')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000005', 'WAIVED', 'Waived', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.FeeStatusType WHERE Code = 'CANCELLED')
    INSERT INTO dbo.FeeStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('18000000-0000-0000-0000-000000000006', 'CANCELLED', 'Cancelled', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.AttendanceStatusType WHERE Code = 'PRESENT')
    INSERT INTO dbo.AttendanceStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('19000000-0000-0000-0000-000000000001', 'PRESENT', 'Present', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.AttendanceStatusType WHERE Code = 'ABSENT')
    INSERT INTO dbo.AttendanceStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('19000000-0000-0000-0000-000000000002', 'ABSENT', 'Absent', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.AttendanceStatusType WHERE Code = 'LATE')
    INSERT INTO dbo.AttendanceStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('19000000-0000-0000-0000-000000000003', 'LATE', 'Late', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.AttendanceStatusType WHERE Code = 'EXCUSED')
    INSERT INTO dbo.AttendanceStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('19000000-0000-0000-0000-000000000004', 'EXCUSED', 'Excused', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.AttendanceStatusType WHERE Code = 'LEAVE')
    INSERT INTO dbo.AttendanceStatusType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('19000000-0000-0000-0000-000000000005', 'LEAVE', 'Leave', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'CLASS_TEST')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000001', 'CLASS_TEST', 'Class Test', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'MONTHLY_TEST')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000002', 'MONTHLY_TEST', 'Monthly Test', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'MIDTERM')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000003', 'MIDTERM', 'Midterm', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'ANNUAL')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000004', 'ANNUAL', 'Annual Examination', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'PRE_BOARD')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000005', 'PRE_BOARD', 'Pre-Board', 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ExamType WHERE Code = 'SUPPLEMENTARY')
    INSERT INTO dbo.ExamType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('20000000-0000-0000-0000-000000000006', 'SUPPLEMENTARY', 'Supplementary', 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'PROFILE_PICTURE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000001', 'PROFILE_PICTURE', 'Profile Picture', 'ANY', 0, 0, 0, 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'BIRTH_CERTIFICATE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000002', 'BIRTH_CERTIFICATE', 'Birth Certificate', 'STUDENT', 1, 0, 1, 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'B_FORM')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000003', 'B_FORM', 'B-Form', 'STUDENT', 1, 0, 1, 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'CNIC_FRONT')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000004', 'CNIC_FRONT', 'CNIC Front', 'ADULT', 1, 1, 1, 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'CNIC_BACK')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000005', 'CNIC_BACK', 'CNIC Back', 'ADULT', 1, 1, 1, 5, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'PASSPORT')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000006', 'PASSPORT', 'Passport', 'ANY', 1, 1, 1, 6, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'ACADEMIC_CERTIFICATE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000007', 'ACADEMIC_CERTIFICATE', 'Academic Certificate', 'ANY', 0, 0, 1, 7, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'DEGREE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000008', 'DEGREE', 'Degree', 'EMPLOYEE', 0, 0, 1, 8, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'EXPERIENCE_CERTIFICATE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000009', 'EXPERIENCE_CERTIFICATE', 'Experience Certificate', 'EMPLOYEE', 0, 0, 1, 9, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'DRIVING_LICENSE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000010', 'DRIVING_LICENSE', 'Driving License', 'DRIVER', 1, 1, 1, 10, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'POLICE_VERIFICATION')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000011', 'POLICE_VERIFICATION', 'Police Verification', 'EMPLOYEE', 1, 1, 1, 11, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'MEDICAL_CERTIFICATE')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000012', 'MEDICAL_CERTIFICATE', 'Medical Certificate', 'ANY', 1, 1, 1, 12, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'RESUME')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000013', 'RESUME', 'Resume', 'EMPLOYEE', 0, 0, 0, 13, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'EMPLOYMENT_CONTRACT')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000014', 'EMPLOYMENT_CONTRACT', 'Employment Contract', 'EMPLOYEE', 1, 1, 1, 14, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.DocumentTypeLookup WHERE Code = 'OTHER')
    INSERT INTO dbo.DocumentTypeLookup (Id, Code, Name, OwnerCategory, IsIdentityDocument, RequiresExpiryDate, RequiresVerification, DisplayOrder, IsActive) VALUES ('21000000-0000-0000-0000-000000000015', 'OTHER', 'Other', 'ANY', 0, 0, 0, 99, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.VehicleType WHERE Code = 'BUS')
    INSERT INTO dbo.VehicleType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('22000000-0000-0000-0000-000000000001', 'BUS', 'Bus', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.VehicleType WHERE Code = 'VAN')
    INSERT INTO dbo.VehicleType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('22000000-0000-0000-0000-000000000002', 'VAN', 'Van', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.VehicleType WHERE Code = 'COASTER')
    INSERT INTO dbo.VehicleType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('22000000-0000-0000-0000-000000000003', 'COASTER', 'Coaster', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.VehicleType WHERE Code = 'CAR')
    INSERT INTO dbo.VehicleType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('22000000-0000-0000-0000-000000000004', 'CAR', 'Car', 4, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.VehicleType WHERE Code = 'OTHER')
    INSERT INTO dbo.VehicleType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('22000000-0000-0000-0000-000000000005', 'OTHER', 'Other', 99, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.LicenseCategoryType WHERE Code = 'LTV')
    INSERT INTO dbo.LicenseCategoryType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('23000000-0000-0000-0000-000000000001', 'LTV', 'Light Transport Vehicle', 1, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.LicenseCategoryType WHERE Code = 'HTV')
    INSERT INTO dbo.LicenseCategoryType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('23000000-0000-0000-0000-000000000002', 'HTV', 'Heavy Transport Vehicle', 2, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.LicenseCategoryType WHERE Code = 'PSV')
    INSERT INTO dbo.LicenseCategoryType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('23000000-0000-0000-0000-000000000003', 'PSV', 'Public Service Vehicle', 3, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.LicenseCategoryType WHERE Code = 'OTHER')
    INSERT INTO dbo.LicenseCategoryType (Id, Code, Name, DisplayOrder, IsActive) VALUES ('23000000-0000-0000-0000-000000000004', 'OTHER', 'Other', 99, 1);
