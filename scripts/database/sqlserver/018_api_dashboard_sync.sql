-- v46 lifecycle sync for core actors (SQL Server). Safe to rerun.
IF COL_LENGTH('student.student','is_active') IS NULL ALTER TABLE student.student ADD is_active bit NOT NULL CONSTRAINT DF_student_is_active DEFAULT 1;
IF COL_LENGTH('student.student','created_at') IS NULL ALTER TABLE student.student ADD created_at datetimeoffset NOT NULL CONSTRAINT DF_student_created_at DEFAULT SYSDATETIMEOFFSET();
IF COL_LENGTH('student.student','updated_at') IS NULL ALTER TABLE student.student ADD updated_at datetimeoffset NULL;
IF COL_LENGTH('student.student','row_version') IS NULL ALTER TABLE student.student ADD row_version rowversion;
IF COL_LENGTH('student.guardian','is_active') IS NULL ALTER TABLE student.guardian ADD is_active bit NOT NULL CONSTRAINT DF_guardian_is_active DEFAULT 1;
IF COL_LENGTH('student.guardian','created_at') IS NULL ALTER TABLE student.guardian ADD created_at datetimeoffset NOT NULL CONSTRAINT DF_guardian_created_at DEFAULT SYSDATETIMEOFFSET();
IF COL_LENGTH('student.guardian','updated_at') IS NULL ALTER TABLE student.guardian ADD updated_at datetimeoffset NULL;
IF COL_LENGTH('student.guardian','row_version') IS NULL ALTER TABLE student.guardian ADD row_version rowversion;
IF COL_LENGTH('hr.employee','is_active') IS NULL ALTER TABLE hr.employee ADD is_active bit NOT NULL CONSTRAINT DF_employee_is_active DEFAULT 1;
IF COL_LENGTH('hr.employee','created_at') IS NULL ALTER TABLE hr.employee ADD created_at datetimeoffset NOT NULL CONSTRAINT DF_employee_created_at DEFAULT SYSDATETIMEOFFSET();
IF COL_LENGTH('hr.employee','updated_at') IS NULL ALTER TABLE hr.employee ADD updated_at datetimeoffset NULL;
IF COL_LENGTH('hr.employee','row_version') IS NULL ALTER TABLE hr.employee ADD row_version rowversion;
