CREATE SCHEMA IF NOT EXISTS teacher;
CREATE TABLE IF NOT EXISTS teacher.teacher_actor (
 teacher_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
 user_id uuid,
 primary_campus_id uuid REFERENCES org.campus(campus_id),
 qualification varchar(250),
 specialization varchar(250),
 teaching_experience_years int,
 max_periods_per_week int NOT NULL DEFAULT 30,
 status varchar(30) NOT NULL DEFAULT 'ACTIVE',
 is_active boolean NOT NULL DEFAULT true,
 created_at timestamptz NOT NULL DEFAULT now(),
 updated_at timestamptz,
 UNIQUE(tenant_id,employee_id),
 UNIQUE(tenant_id,user_id)
);
CREATE INDEX IF NOT EXISTS ix_teacher_actor_tenant_campus ON teacher.teacher_actor(tenant_id,primary_campus_id,is_active);
CREATE INDEX IF NOT EXISTS ix_teacher_actor_user ON teacher.teacher_actor(user_id) WHERE user_id IS NOT NULL;
CREATE TABLE IF NOT EXISTS teacher.leave_request (
 leave_request_id uuid PRIMARY KEY,
 tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
 employee_id uuid NOT NULL REFERENCES hr.employee(employee_id),
 leave_type varchar(50) NOT NULL,
 from_date date NOT NULL,
 to_date date NOT NULL,
 reason text NOT NULL,
 status varchar(30) NOT NULL DEFAULT 'PENDING',
 approved_by uuid,
 decision_at timestamptz,
 decision_note text,
 created_at timestamptz NOT NULL DEFAULT now(),
 CHECK(to_date>=from_date)
);
CREATE INDEX IF NOT EXISTS ix_teacher_leave_employee_status ON teacher.leave_request(tenant_id,employee_id,status);
