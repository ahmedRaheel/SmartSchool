/*
 SmartSchool SQL Server document/read-model extension.
 Owner-specific tables are normalized and store metadata only.
 Binary files should be stored in configured object/file storage.
*/
CREATE TABLE dbo.DocumentType (
	Id uniqueidentifier NOT NULL PRIMARY KEY,
	TenantId uniqueidentifier NOT NULL,
	Code varchar(80) NOT NULL,
	Name nvarchar(150) NOT NULL,
	OwnerCategory varchar(50) NOT NULL,
	IsIdentityDocument bit NOT NULL DEFAULT 0,
	RequiresExpiryDate bit NOT NULL DEFAULT 0,
	RequiresVerification bit NOT NULL DEFAULT 0,
	IsActive bit NOT NULL DEFAULT 1,
	CreatedAt datetimeoffset NOT NULL,
	UpdatedAt datetimeoffset NULL,
	RowVersion rowversion NOT NULL,
	CONSTRAINT UQ_DocumentType_Tenant_Code UNIQUE (TenantId, Code)
);
GO

-- Apply the same normalized metadata shape to:
-- StudentDocument(StudentId), ParentDocument(ParentId),
-- TeacherDocument(TeacherId), EmployeeDocument(EmployeeId),
-- CandidateDocument(CandidateId), DriverDocument(DriverId),
-- SchoolDocument(SchoolId).
-- EF Core IEntityTypeConfiguration classes in the solution are the
-- authoritative SQL Server mapping and migration source.
