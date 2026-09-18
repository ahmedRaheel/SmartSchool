CREATE SCHEMA IF NOT EXISTS ai;

CREATE TABLE IF NOT EXISTS ai.ml_prediction_result (
	ml_prediction_result_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	tenant_id uuid NOT NULL,
	prediction_type varchar(80) NOT NULL,
	student_id uuid NULL,
	subject_id uuid NULL,
	related_entity_id uuid NULL,
	score numeric(8,4) NOT NULL,
	probability numeric(8,6) NOT NULL,
	risk_level varchar(30) NOT NULL,
	outcome varchar(80) NOT NULL,
	confidence_score numeric(8,6) NOT NULL,
	model_version varchar(80) NOT NULL,
	used_machine_learning boolean NOT NULL DEFAULT false,
	factors_json jsonb NULL,
	generated_at timestamptz NOT NULL DEFAULT now(),
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NULL,
	row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);
CREATE INDEX IF NOT EXISTS ix_ml_prediction_student_type
	ON ai.ml_prediction_result(tenant_id,student_id,prediction_type,generated_at DESC);

-- Normalized attendance events used by AttendanceRisk once populated by the attendance module.
CREATE TABLE IF NOT EXISTS student.attendance_event (
	attendance_event_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	tenant_id uuid NOT NULL,
	student_id uuid NOT NULL,
	attendance_date date NOT NULL,
	status_code varchar(40) NOT NULL,
	class_section_id uuid NULL,
	remarks varchar(500) NULL,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NULL,
	row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
	UNIQUE(tenant_id,student_id,attendance_date)
);
CREATE INDEX IF NOT EXISTS ix_attendance_event_student_date
	ON student.attendance_event(tenant_id,student_id,attendance_date DESC);

-- Transport telemetry is separated from route master data so delay models can be trained.
CREATE TABLE IF NOT EXISTS transport.trip_history (
	trip_history_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	tenant_id uuid NOT NULL,
	route_id uuid NOT NULL,
	trip_date date NOT NULL,
	scheduled_arrival timestamptz NULL,
	actual_arrival timestamptz NULL,
	delay_minutes int NULL,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NULL,
	row_version bytea NOT NULL DEFAULT gen_random_bytes(8)
);
CREATE INDEX IF NOT EXISTS ix_trip_history_route_date
	ON transport.trip_history(tenant_id,route_id,trip_date DESC);
