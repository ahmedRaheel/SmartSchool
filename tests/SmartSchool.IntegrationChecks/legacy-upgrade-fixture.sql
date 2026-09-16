INSERT INTO saas.tenant(tenant_id,code,name) VALUES('aa000000-0000-0000-0000-000000000001','LEGACY-CHECK','Legacy school');
INSERT INTO org.school(school_id,tenant_id,code,name) VALUES('aa000000-0000-0000-0000-000000000002','aa000000-0000-0000-0000-000000000001','LS','Legacy school');
INSERT INTO org.campus(campus_id,tenant_id,school_id,code,name,branch_gender_type_id,branch_type)
VALUES('aa000000-0000-0000-0000-000000000003','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000002','LC','Legacy campus',(SELECT branch_gender_type_id FROM reference.branch_gender_type WHERE code='CO_EDUCATION'),3);
INSERT INTO academic.academic_year(academic_year_id,tenant_id,campus_id,name,start_date,end_date)
VALUES('aa000000-0000-0000-0000-000000000004','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000003','2026','2026-08-01','2027-07-31');
INSERT INTO academic.class(class_id,tenant_id,school_id,branch_id,code,name,education_level_id)
VALUES('aa000000-0000-0000-0000-000000000005','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000002','aa000000-0000-0000-0000-000000000003','LC5','Class 5','20000000-0000-0000-0000-000000000002');
INSERT INTO academic.class_section(class_section_id,tenant_id,campus_id,academic_year_id,class_id,code,name)
VALUES('aa000000-0000-0000-0000-000000000006','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000003','aa000000-0000-0000-0000-000000000004','aa000000-0000-0000-0000-000000000005','LCA','A');
INSERT INTO academic.subject(subject_id,tenant_id,code,name)
VALUES('aa000000-0000-0000-0000-000000000007','aa000000-0000-0000-0000-000000000001','LM','Mathematics');
INSERT INTO hr.employee(employee_id,tenant_id,branch_id,first_name,hire_date,employment_type_code,staff_type)
VALUES('aa000000-0000-0000-0000-000000000008','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000003','Teacher','2020-01-01','FULL_TIME','TEACHER');
INSERT INTO hr.teacher_teaching_assignment(teacher_teaching_assignment_id,tenant_id,school_id,campus_id,employee_id,class_section_id,subject_id,code,name,is_class_teacher)
VALUES('aa000000-0000-0000-0000-000000000009','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000002','aa000000-0000-0000-0000-000000000003','aa000000-0000-0000-0000-000000000008','aa000000-0000-0000-0000-000000000006','aa000000-0000-0000-0000-000000000007','LT1','Math teacher',true);
INSERT INTO admission.student_application(application_id,tenant_id,school_id,branch_id,academic_year_id,class_id,class_section_id,first_name,guardian_name)
VALUES('aa000000-0000-0000-0000-000000000010','aa000000-0000-0000-0000-000000000001','aa000000-0000-0000-0000-000000000002','aa000000-0000-0000-0000-000000000003','aa000000-0000-0000-0000-000000000004','aa000000-0000-0000-0000-000000000005','aa000000-0000-0000-0000-000000000006','Legacy student','Legacy guardian');
