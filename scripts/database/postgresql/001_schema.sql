-- SmartSchool PostgreSQL - Core Lookup DDL
CREATE TABLE IF NOT EXISTS "OccupationType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(150) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "RelationshipType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "GenderType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(30) NOT NULL UNIQUE,
    "Name" varchar(50) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "BloodGroupType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(10) NOT NULL UNIQUE,
    "Name" varchar(20) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "EmploymentStatusType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "EmploymentType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "MaritalStatusType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(30) NOT NULL UNIQUE,
    "Name" varchar(50) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "PaymentMethodType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "FeeStatusType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "AttendanceStatusType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(30) NOT NULL UNIQUE,
    "Name" varchar(50) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "ExamType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "DocumentTypeLookup" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(80) NOT NULL UNIQUE,
    "Name" varchar(150) NOT NULL,
    "OwnerCategory" varchar(50) NOT NULL,
    "IsIdentityDocument" boolean NOT NULL DEFAULT false,
    "RequiresExpiryDate" boolean NOT NULL DEFAULT false,
    "RequiresVerification" boolean NOT NULL DEFAULT false,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "VehicleType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(50) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS "LicenseCategoryType" (
    "Id" uuid PRIMARY KEY,
    "Code" varchar(30) NOT NULL UNIQUE,
    "Name" varchar(100) NOT NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "IsActive" boolean NOT NULL DEFAULT true
);
