-- SmartSchool v118 setup/payroll normalization
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS head_of_department_employee_id uuid NULL;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS department_id uuid NULL;

CREATE TABLE IF NOT EXISTS academic.department_class (
  department_id uuid NOT NULL, class_id uuid NOT NULL, tenant_id uuid NOT NULL,
  is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(),
  PRIMARY KEY (department_id, class_id)
);

CREATE TABLE IF NOT EXISTS payroll.employee_payroll (
  employee_payroll_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, school_id uuid NOT NULL, branch_id uuid NOT NULL,
  employee_id uuid NOT NULL, payroll_year int NOT NULL, payroll_month int NOT NULL,
  basic_salary numeric(18,2) NOT NULL DEFAULT 0, house_allowance numeric(18,2) NOT NULL DEFAULT 0,
  medical_allowance numeric(18,2) NOT NULL DEFAULT 0, transport_allowance numeric(18,2) NOT NULL DEFAULT 0,
  other_allowance numeric(18,2) NOT NULL DEFAULT 0, bonus numeric(18,2) NOT NULL DEFAULT 0, overtime numeric(18,2) NOT NULL DEFAULT 0,
  gross_pay numeric(18,2) NOT NULL DEFAULT 0, tax_deduction numeric(18,2) NOT NULL DEFAULT 0,
  provident_fund numeric(18,2) NOT NULL DEFAULT 0, loan_deduction numeric(18,2) NOT NULL DEFAULT 0,
  absence_deduction numeric(18,2) NOT NULL DEFAULT 0, other_deduction numeric(18,2) NOT NULL DEFAULT 0,
  total_deductions numeric(18,2) NOT NULL DEFAULT 0, net_pay numeric(18,2) NOT NULL DEFAULT 0,
  status varchar(30) NOT NULL DEFAULT 'DRAFT', is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(),
  updated_at timestamptz NULL, row_version bigint NOT NULL DEFAULT 1, UNIQUE(tenant_id, employee_id, payroll_year, payroll_month)
);

CREATE INDEX IF NOT EXISTS ix_department_branch ON org.department(tenant_id, campus_id);
CREATE INDEX IF NOT EXISTS ix_employee_department ON hr.employee(tenant_id, branch_id, department_id);
