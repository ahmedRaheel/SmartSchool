CREATE SCHEMA IF NOT EXISTS admission;
CREATE SCHEMA IF NOT EXISTS reference;

ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS staff_type varchar(30) NOT NULL DEFAULT 'OTHER';

CREATE TABLE IF NOT EXISTS admission.admission_criteria (
 admission_criteria_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, school_id uuid NOT NULL, branch_id uuid NOT NULL, academic_year_id uuid NOT NULL, class_section_id uuid NOT NULL, minimum_marks numeric(5,2) NOT NULL DEFAULT 0, entrance_test_minimum numeric(5,2), minimum_age integer, maximum_age integer, interview_required boolean NOT NULL DEFAULT false, required_documents text, status varchar(20) NOT NULL DEFAULT 'ACTIVE', created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_admission_criteria_scope ON admission.admission_criteria(tenant_id,school_id,branch_id,academic_year_id,class_section_id);

CREATE TABLE IF NOT EXISTS admission.student_application (
 application_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, school_id uuid NOT NULL, branch_id uuid NOT NULL, academic_year_id uuid, class_section_id uuid, first_name varchar(100) NOT NULL, last_name varchar(100), date_of_birth date, gender varchar(30), email varchar(250), phone varchar(50), address text, guardian_name varchar(200) NOT NULL, guardian_cnic varchar(30), guardian_email varchar(250), guardian_phone varchar(50), relationship varchar(50), previous_school varchar(250), previous_marks numeric(5,2), status varchar(30) NOT NULL DEFAULT 'SUBMITTED_APPLICATION', submitted_at timestamptz NOT NULL DEFAULT now(), decided_at timestamptz, decision_notes text, student_id uuid, is_active boolean NOT NULL DEFAULT true
);
CREATE INDEX IF NOT EXISTS ix_admission_application_tenant_status ON admission.student_application(tenant_id,status);

CREATE TABLE IF NOT EXISTS reference.country (country_id serial PRIMARY KEY, code varchar(3) UNIQUE NOT NULL, name varchar(100) NOT NULL);
CREATE TABLE IF NOT EXISTS reference.province (province_id serial PRIMARY KEY, country_id int NOT NULL REFERENCES reference.country(country_id), code varchar(20) NOT NULL, name varchar(100) NOT NULL, UNIQUE(country_id,code));
CREATE TABLE IF NOT EXISTS reference.city (city_id serial PRIMARY KEY, province_id int NOT NULL REFERENCES reference.province(province_id), code varchar(30) NOT NULL, name varchar(120) NOT NULL, UNIQUE(province_id,code));
INSERT INTO reference.country(code,name) VALUES ('PK','Pakistan') ON CONFLICT DO NOTHING;
INSERT INTO reference.province(country_id,code,name) SELECT country_id,'SD','Sindh' FROM reference.country WHERE code='PK' ON CONFLICT DO NOTHING;
INSERT INTO reference.province(country_id,code,name) SELECT country_id,'PB','Punjab' FROM reference.country WHERE code='PK' ON CONFLICT DO NOTHING;
INSERT INTO reference.province(country_id,code,name) SELECT country_id,'KP','Khyber Pakhtunkhwa' FROM reference.country WHERE code='PK' ON CONFLICT DO NOTHING;
INSERT INTO reference.province(country_id,code,name) SELECT country_id,'BA','Balochistan' FROM reference.country WHERE code='PK' ON CONFLICT DO NOTHING;
INSERT INTO reference.city(province_id,code,name) SELECT province_id,'KHI','Karachi' FROM reference.province WHERE code='SD' ON CONFLICT DO NOTHING;
INSERT INTO reference.city(province_id,code,name) SELECT province_id,'LHE','Lahore' FROM reference.province WHERE code='PB' ON CONFLICT DO NOTHING;

ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS alternate_phone varchar(50);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS address varchar(500);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_name varchar(200);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_phone varchar(50);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS country varchar(120);
