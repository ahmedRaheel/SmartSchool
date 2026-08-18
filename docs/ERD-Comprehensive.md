# SmartSchool Comprehensive ERD — Documents and Read Models

```mermaid
erDiagram
	TENANT ||--o{ STUDENT : owns
	TENANT ||--o{ PARENT : owns
	TENANT ||--o{ EMPLOYEE : owns
	TENANT ||--o{ DRIVER : owns
	TENANT ||--o{ DOCUMENT_TYPE : defines

	STUDENT ||--o{ STUDENT_DOCUMENT : has
	PARENT ||--o{ PARENT_DOCUMENT : has
	TEACHER ||--o{ TEACHER_DOCUMENT : has
	EMPLOYEE ||--o{ EMPLOYEE_DOCUMENT : has
	CANDIDATE ||--o{ CANDIDATE_DOCUMENT : has
	DRIVER ||--o{ DRIVER_DOCUMENT : has
	SCHOOL ||--o{ SCHOOL_DOCUMENT : has

	DOCUMENT_TYPE ||--o{ STUDENT_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ PARENT_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ TEACHER_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ EMPLOYEE_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ CANDIDATE_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ DRIVER_DOCUMENT : classifies
	DOCUMENT_TYPE ||--o{ SCHOOL_DOCUMENT : classifies

	STUDENT ||--|| STUDENT_DIRECTORY_READ : projects
	TEACHER ||--|| TEACHER_DIRECTORY_READ : projects
	DRIVER ||--|| DRIVER_DIRECTORY_READ : projects

	STUDENT {
		uuid Id PK
		uuid TenantId
		string AdmissionNumber
		string FirstName
		string LastName
		date DateOfBirth
		uuid CurrentProgramId
		uuid CurrentClassId
		uuid CurrentSectionId
	}

	PARENT {
		uuid Id PK
		uuid TenantId
		string Cnic
		string FirstName
		string LastName
		string RelationshipCode
		string MobileNumber
	}

	DRIVER {
		uuid Id PK
		uuid TenantId
		string Cnic
		string DrivingLicenseNumber
		date LicenseExpiryDate
		uuid AssignedVehicleId
	}

	DOCUMENT_TYPE {
		uuid Id PK
		uuid TenantId
		string Code
		string Name
		string OwnerCategory
		bool IsIdentityDocument
		bool RequiresExpiryDate
		bool RequiresVerification
	}

	STUDENT_DOCUMENT {
		uuid Id PK
		uuid TenantId
		uuid StudentId FK
		uuid DocumentTypeId FK
		string OriginalFileName
		string ContentType
		long FileSizeBytes
		string StorageProvider
		string StorageKey
		string Sha256Hash
		string DocumentNumber
		date IssuedOn
		date ExpiresOn
		bool IsVerified
		uuid VerifiedByUserId
		datetime VerifiedAt
		string Notes
	}

	PARENT_DOCUMENT {
		uuid Id PK
		uuid TenantId
		uuid ParentId FK
		uuid DocumentTypeId FK
		string StorageKey
		string Sha256Hash
		bool IsVerified
	}

	TEACHER_DOCUMENT {
		uuid Id PK
		uuid TenantId
		uuid TeacherId FK
		uuid DocumentTypeId FK
		string StorageKey
		string Sha256Hash
		bool IsVerified
	}

	DRIVER_DOCUMENT {
		uuid Id PK
		uuid TenantId
		uuid DriverId FK
		uuid DocumentTypeId FK
		string StorageKey
		string Sha256Hash
		date ExpiresOn
		bool IsVerified
	}

	STUDENT_DIRECTORY_READ {
		uuid StudentId UK
		uuid TenantId
		string AdmissionNumber
		string StudentName
		string ProgramName
		string ClassName
		string SectionName
		string PrimaryGuardianName
		decimal AttendancePercentage
		decimal LatestExamPercentage
		decimal OutstandingBalance
		int DocumentCount
		int VerifiedDocumentCount
	}
```

## Design decisions

1. **Separate owner document tables** are intentional. Student, parent, teacher, driver, candidate and school documents have different authorization, retention, verification and required-document policies.
2. **DocumentType is normalized**. Birth certificate, B-Form, CNIC front/back, passport, degree, experience certificate, driving license and similar categories are data rather than repeated strings.
3. **File bytes are not stored in PostgreSQL/SQL Server by default.** Relational tables hold metadata; the storage abstraction holds the file in Local/S3/Azure Blob or another provider.
4. **StorageKey is private**, not a permanent public URL. Download APIs should authorize the caller and return/stream the file or issue a short-lived signed URL.
5. **SHA-256** supports integrity checks and duplicate detection.
6. **CNIC/document numbers are sensitive PII.** They should be encrypted/tokenized where required, excluded from ordinary list DTOs and never logged.
7. **Read tables are application-managed materialized projections.** This keeps the architecture portable between PostgreSQL and SQL Server while providing fast dashboards and directories.
8. Transactional normalized tables remain the system of record. Read tables may be rebuilt if necessary.
