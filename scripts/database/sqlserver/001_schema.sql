-- SmartSchool SQL Server - Core Lookup DDL

IF OBJECT_ID('dbo.--', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.-- SmartSchool PostgreSQL - Core Lookup DDL
END;
GO

IF OBJECT_ID('dbo.OccupationType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OccupationType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(150) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.RelationshipType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RelationshipType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.GenderType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.GenderType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(30) NOT NULL UNIQUE,
    Name varchar(50) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.BloodGroupType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BloodGroupType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(10) NOT NULL UNIQUE,
    Name varchar(20) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.EmploymentStatusType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EmploymentStatusType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.EmploymentType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EmploymentType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.MaritalStatusType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MaritalStatusType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(30) NOT NULL UNIQUE,
    Name varchar(50) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.PaymentMethodType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentMethodType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.FeeStatusType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.FeeStatusType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.AttendanceStatusType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AttendanceStatusType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(30) NOT NULL UNIQUE,
    Name varchar(50) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.ExamType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ExamType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.DocumentTypeLookup', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DocumentTypeLookup (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(80) NOT NULL UNIQUE,
    Name varchar(150) NOT NULL,
    OwnerCategory varchar(50) NOT NULL,
    IsIdentityDocument bit NOT NULL DEFAULT 0,
    RequiresExpiryDate bit NOT NULL DEFAULT 0,
    RequiresVerification bit NOT NULL DEFAULT 0,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.VehicleType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.VehicleType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(50) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO

IF OBJECT_ID('dbo.LicenseCategoryType', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.LicenseCategoryType (
    Id uniqueidentifier PRIMARY KEY,
    Code varchar(30) NOT NULL UNIQUE,
    Name varchar(100) NOT NULL,
    DisplayOrder int NOT NULL DEFAULT 0,
    IsActive bit NOT NULL DEFAULT 1
);
END;
GO
