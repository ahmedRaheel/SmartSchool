IF SCHEMA_ID('ai') IS NULL EXEC('CREATE SCHEMA ai');
IF OBJECT_ID('ai.ml_prediction_result','U') IS NULL
CREATE TABLE ai.ml_prediction_result(
	ml_prediction_result_id uniqueidentifier NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
	tenant_id uniqueidentifier NOT NULL,
	prediction_type varchar(80) NOT NULL,
	student_id uniqueidentifier NULL,
	subject_id uniqueidentifier NULL,
	related_entity_id uniqueidentifier NULL,
	score decimal(8,4) NOT NULL,
	probability decimal(8,6) NOT NULL,
	risk_level varchar(30) NOT NULL,
	outcome varchar(80) NOT NULL,
	confidence_score decimal(8,6) NOT NULL,
	model_version varchar(80) NOT NULL,
	used_machine_learning bit NOT NULL DEFAULT 0,
	factors_json nvarchar(max) NULL,
	generated_at datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	is_active bit NOT NULL DEFAULT 1,
	created_at datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
	updated_at datetimeoffset NULL,
	row_version rowversion NOT NULL
);
