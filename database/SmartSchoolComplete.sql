--
-- PostgreSQL database dump
--

\restrict JgF6T5FiaNDLghyzB4HTugBI1hR0RCfJwKav6daUVCYxVgCNwMp0olASTqNXkxE

-- Dumped from database version 18.6 (Debian 18.6-1.pgdg12+2)
-- Dumped by pg_dump version 18.6

-- Started on 2026-08-23 15:37:10

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;








--
-- TOC entry 5714 (class 2606 OID 16743)
-- Name: subject subject_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.subject
    ADD CONSTRAINT subject_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5768 (class 2606 OID 17335)
-- Name: teacher_course_assignment teacher_course_assignment_class_section_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT teacher_course_assignment_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5769 (class 2606 OID 17325)
-- Name: teacher_course_assignment teacher_course_assignment_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT teacher_course_assignment_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5770 (class 2606 OID 17330)
-- Name: teacher_course_assignment teacher_course_assignment_employee_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT teacher_course_assignment_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5771 (class 2606 OID 17320)
-- Name: teacher_course_assignment teacher_course_assignment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT teacher_course_assignment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5784 (class 2606 OID 17514)
-- Name: teaching_group teaching_group_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5785 (class 2606 OID 17524)
-- Name: teaching_group teaching_group_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5786 (class 2606 OID 17529)
-- Name: teaching_group teaching_group_room_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_room_id_fkey FOREIGN KEY (room_id) REFERENCES org.room(room_id);


--
-- TOC entry 5789 (class 2606 OID 17551)
-- Name: teaching_group_student teaching_group_student_student_course_enrollment_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group_student
    ADD CONSTRAINT teaching_group_student_student_course_enrollment_id_fkey FOREIGN KEY (student_course_enrollment_id) REFERENCES student.student_course_enrollment(student_course_enrollment_id);


--
-- TOC entry 5790 (class 2606 OID 17546)
-- Name: teaching_group_student teaching_group_student_teaching_group_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group_student
    ADD CONSTRAINT teaching_group_student_teaching_group_id_fkey FOREIGN KEY (teaching_group_id) REFERENCES academic.teaching_group(teaching_group_id);


--
-- TOC entry 5787 (class 2606 OID 17509)
-- Name: teaching_group teaching_group_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5788 (class 2606 OID 17519)
-- Name: teaching_group teaching_group_term_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5724 (class 2606 OID 16868)
-- Name: term term_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.term
    ADD CONSTRAINT term_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5725 (class 2606 OID 16863)
-- Name: term term_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.term
    ADD CONSTRAINT term_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5793 (class 2606 OID 17603)
-- Name: timetable timetable_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable
    ADD CONSTRAINT timetable_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5794 (class 2606 OID 17598)
-- Name: timetable timetable_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable
    ADD CONSTRAINT timetable_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5797 (class 2606 OID 17637)
-- Name: timetable_entry timetable_entry_class_section_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5798 (class 2606 OID 17647)
-- Name: timetable_entry timetable_entry_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5799 (class 2606 OID 17657)
-- Name: timetable_entry timetable_entry_room_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_room_id_fkey FOREIGN KEY (room_id) REFERENCES org.room(room_id);


--
-- TOC entry 5800 (class 2606 OID 17652)
-- Name: timetable_entry timetable_entry_teacher_course_assignment_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_teacher_course_assignment_id_fkey FOREIGN KEY (teacher_course_assignment_id) REFERENCES academic.teacher_course_assignment(teacher_course_assignment_id);


--
-- TOC entry 5801 (class 2606 OID 17642)
-- Name: timetable_entry timetable_entry_teaching_group_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_teaching_group_id_fkey FOREIGN KEY (teaching_group_id) REFERENCES academic.teaching_group(teaching_group_id);


--
-- TOC entry 5802 (class 2606 OID 17627)
-- Name: timetable_entry timetable_entry_timetable_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_timetable_id_fkey FOREIGN KEY (timetable_id) REFERENCES academic.timetable(timetable_id);


--
-- TOC entry 5803 (class 2606 OID 17632)
-- Name: timetable_entry timetable_entry_timetable_period_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_timetable_period_id_fkey FOREIGN KEY (timetable_period_id) REFERENCES academic.timetable_period(timetable_period_id);


--
-- TOC entry 5791 (class 2606 OID 17575)
-- Name: timetable_period timetable_period_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_period
    ADD CONSTRAINT timetable_period_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5792 (class 2606 OID 17570)
-- Name: timetable_period timetable_period_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_period
    ADD CONSTRAINT timetable_period_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5795 (class 2606 OID 17593)
-- Name: timetable timetable_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable
    ADD CONSTRAINT timetable_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5796 (class 2606 OID 17608)
-- Name: timetable timetable_term_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable
    ADD CONSTRAINT timetable_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5858 (class 2606 OID 18311)
-- Name: activity activity_campus_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.activity
    ADD CONSTRAINT activity_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5859 (class 2606 OID 18316)
-- Name: activity activity_coordinator_employee_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.activity
    ADD CONSTRAINT activity_coordinator_employee_id_fkey FOREIGN KEY (coordinator_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5860 (class 2606 OID 18306)
-- Name: activity activity_tenant_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.activity
    ADD CONSTRAINT activity_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5861 (class 2606 OID 18328)
-- Name: student_activity student_activity_activity_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_activity
    ADD CONSTRAINT student_activity_activity_id_fkey FOREIGN KEY (activity_id) REFERENCES activity.activity(activity_id);


--
-- TOC entry 5862 (class 2606 OID 18333)
-- Name: student_activity student_activity_student_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_activity
    ADD CONSTRAINT student_activity_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5863 (class 2606 OID 18362)
-- Name: student_award student_award_generated_document_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_award
    ADD CONSTRAINT student_award_generated_document_id_fkey FOREIGN KEY (generated_document_id) REFERENCES document.generated_document(generated_document_id);


--
-- TOC entry 5864 (class 2606 OID 18357)
-- Name: student_award student_award_student_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_award
    ADD CONSTRAINT student_award_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5865 (class 2606 OID 18352)
-- Name: student_award student_award_tenant_id_fkey; Type: FK CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_award
    ADD CONSTRAINT student_award_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5962 (class 2606 OID 19509)
-- Name: class_performance_insight class_performance_insight_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5963 (class 2606 OID 19519)
-- Name: class_performance_insight class_performance_insight_class_section_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5964 (class 2606 OID 19524)
-- Name: class_performance_insight class_performance_insight_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5965 (class 2606 OID 19529)
-- Name: class_performance_insight class_performance_insight_teacher_employee_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_teacher_employee_id_fkey FOREIGN KEY (teacher_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5966 (class 2606 OID 19504)
-- Name: class_performance_insight class_performance_insight_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5967 (class 2606 OID 19514)
-- Name: class_performance_insight class_performance_insight_term_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5983 (class 2606 OID 19680)
-- Name: intervention_action intervention_action_student_intervention_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.intervention_action
    ADD CONSTRAINT intervention_action_student_intervention_id_fkey FOREIGN KEY (student_intervention_id) REFERENCES ai.student_intervention(student_intervention_id) ON DELETE CASCADE;


--
-- TOC entry 5984 (class 2606 OID 19697)
-- Name: intervention_outcome intervention_outcome_student_intervention_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.intervention_outcome
    ADD CONSTRAINT intervention_outcome_student_intervention_id_fkey FOREIGN KEY (student_intervention_id) REFERENCES ai.student_intervention(student_intervention_id);


--
-- TOC entry 5961 (class 2606 OID 19476)
-- Name: predicted_grade_probability predicted_grade_probability_student_performance_prediction_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.predicted_grade_probability
    ADD CONSTRAINT predicted_grade_probability_student_performance_prediction_fkey FOREIGN KEY (student_performance_prediction_id) REFERENCES ai.student_performance_prediction(student_performance_prediction_id) ON DELETE CASCADE;


--
-- TOC entry 5985 (class 2606 OID 19720)
-- Name: prediction_evaluation prediction_evaluation_student_exam_result_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evaluation
    ADD CONSTRAINT prediction_evaluation_student_exam_result_id_fkey FOREIGN KEY (student_exam_result_id) REFERENCES exam.student_exam_result(student_exam_result_id);


--
-- TOC entry 5986 (class 2606 OID 19715)
-- Name: prediction_evaluation prediction_evaluation_student_performance_prediction_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evaluation
    ADD CONSTRAINT prediction_evaluation_student_performance_prediction_id_fkey FOREIGN KEY (student_performance_prediction_id) REFERENCES ai.student_performance_prediction(student_performance_prediction_id);


--
-- TOC entry 5960 (class 2606 OID 19458)
-- Name: prediction_evidence prediction_evidence_student_performance_prediction_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evidence
    ADD CONSTRAINT prediction_evidence_student_performance_prediction_id_fkey FOREIGN KEY (student_performance_prediction_id) REFERENCES ai.student_performance_prediction(student_performance_prediction_id) ON DELETE CASCADE;


--
-- TOC entry 5947 (class 2606 OID 19342)
-- Name: prediction_model prediction_model_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_model
    ADD CONSTRAINT prediction_model_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5948 (class 2606 OID 19366)
-- Name: prediction prediction_prediction_model_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction
    ADD CONSTRAINT prediction_prediction_model_id_fkey FOREIGN KEY (prediction_model_id) REFERENCES ai.prediction_model(prediction_model_id);


--
-- TOC entry 5949 (class 2606 OID 19371)
-- Name: prediction prediction_student_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction
    ADD CONSTRAINT prediction_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5950 (class 2606 OID 19361)
-- Name: prediction prediction_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction
    ADD CONSTRAINT prediction_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5976 (class 2606 OID 19643)
-- Name: student_intervention student_intervention_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5977 (class 2606 OID 19653)
-- Name: student_intervention student_intervention_source_prediction_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_source_prediction_id_fkey FOREIGN KEY (source_prediction_id) REFERENCES ai.student_performance_prediction(student_performance_prediction_id);


--
-- TOC entry 5978 (class 2606 OID 19658)
-- Name: student_intervention student_intervention_source_recommendation_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_source_recommendation_id_fkey FOREIGN KEY (source_recommendation_id) REFERENCES ai.teaching_recommendation(teaching_recommendation_id);


--
-- TOC entry 5979 (class 2606 OID 19633)
-- Name: student_intervention student_intervention_student_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5980 (class 2606 OID 19638)
-- Name: student_intervention student_intervention_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5981 (class 2606 OID 19648)
-- Name: student_intervention student_intervention_teacher_employee_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_teacher_employee_id_fkey FOREIGN KEY (teacher_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5982 (class 2606 OID 19628)
-- Name: student_intervention student_intervention_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5951 (class 2606 OID 19412)
-- Name: student_performance_prediction student_performance_prediction_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5952 (class 2606 OID 19422)
-- Name: student_performance_prediction student_performance_prediction_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5953 (class 2606 OID 19442)
-- Name: student_performance_prediction student_performance_prediction_prediction_model_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_prediction_model_id_fkey FOREIGN KEY (prediction_model_id) REFERENCES ai.prediction_model(prediction_model_id);


--
-- TOC entry 5954 (class 2606 OID 19407)
-- Name: student_performance_prediction student_performance_prediction_student_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5955 (class 2606 OID 19427)
-- Name: student_performance_prediction student_performance_prediction_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5956 (class 2606 OID 19432)
-- Name: student_performance_prediction student_performance_prediction_target_exam_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_target_exam_id_fkey FOREIGN KEY (target_exam_id) REFERENCES exam.exam(exam_id);


--
-- TOC entry 5957 (class 2606 OID 19437)
-- Name: student_performance_prediction student_performance_prediction_target_exam_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_target_exam_subject_id_fkey FOREIGN KEY (target_exam_subject_id) REFERENCES exam.exam_subject(exam_subject_id);


--
-- TOC entry 5958 (class 2606 OID 19402)
-- Name: student_performance_prediction student_performance_prediction_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5959 (class 2606 OID 19417)
-- Name: student_performance_prediction student_performance_prediction_term_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5987 (class 2606 OID 19756)
-- Name: student_progress_recommendation student_progress_recommendation_prediction_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_progress_recommendation
    ADD CONSTRAINT student_progress_recommendation_prediction_id_fkey FOREIGN KEY (prediction_id) REFERENCES ai.student_performance_prediction(student_performance_prediction_id);


--
-- TOC entry 5988 (class 2606 OID 19751)
-- Name: student_progress_recommendation student_progress_recommendation_student_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_progress_recommendation
    ADD CONSTRAINT student_progress_recommendation_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5989 (class 2606 OID 19746)
-- Name: student_progress_recommendation student_progress_recommendation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_progress_recommendation
    ADD CONSTRAINT student_progress_recommendation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5970 (class 2606 OID 19587)
-- Name: teaching_recommendation teaching_recommendation_class_performance_insight_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_class_performance_insight_id_fkey FOREIGN KEY (class_performance_insight_id) REFERENCES ai.class_performance_insight(class_performance_insight_id);


--
-- TOC entry 5971 (class 2606 OID 19592)
-- Name: teaching_recommendation teaching_recommendation_class_section_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5972 (class 2606 OID 19597)
-- Name: teaching_recommendation teaching_recommendation_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5973 (class 2606 OID 19607)
-- Name: teaching_recommendation teaching_recommendation_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5974 (class 2606 OID 19602)
-- Name: teaching_recommendation teaching_recommendation_teacher_employee_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_teacher_employee_id_fkey FOREIGN KEY (teacher_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5975 (class 2606 OID 19582)
-- Name: teaching_recommendation teaching_recommendation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5968 (class 2606 OID 19550)
-- Name: topic_performance_insight topic_performance_insight_class_performance_insight_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.topic_performance_insight
    ADD CONSTRAINT topic_performance_insight_class_performance_insight_id_fkey FOREIGN KEY (class_performance_insight_id) REFERENCES ai.class_performance_insight(class_performance_insight_id) ON DELETE CASCADE;


--
-- TOC entry 5969 (class 2606 OID 19555)
-- Name: topic_performance_insight topic_performance_insight_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.topic_performance_insight
    ADD CONSTRAINT topic_performance_insight_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 6000 (class 2606 OID 20807)
-- Name: RagKnowledgeChunks FK_RagKnowledgeChunks_Document; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core."RagKnowledgeChunks"
    ADD CONSTRAINT "FK_RagKnowledgeChunks_Document" FOREIGN KEY ("DocumentId") REFERENCES ai_core."RagKnowledgeDocuments"("Id") ON DELETE CASCADE;


--
-- TOC entry 5904 (class 2606 OID 18875)
-- Name: ai_execution_log ai_execution_log_model_configuration_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.ai_execution_log
    ADD CONSTRAINT ai_execution_log_model_configuration_id_fkey FOREIGN KEY (model_configuration_id) REFERENCES ai_core.model_configuration(model_configuration_id);


--
-- TOC entry 5905 (class 2606 OID 18870)
-- Name: ai_execution_log ai_execution_log_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.ai_execution_log
    ADD CONSTRAINT ai_execution_log_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5902 (class 2606 OID 18853)
-- Name: assistant_knowledge_collection assistant_knowledge_collection_knowledge_collection_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_knowledge_collection
    ADD CONSTRAINT assistant_knowledge_collection_knowledge_collection_id_fkey FOREIGN KEY (knowledge_collection_id) REFERENCES ai_core.knowledge_collection(knowledge_collection_id);


--
-- TOC entry 5903 (class 2606 OID 18848)
-- Name: assistant_knowledge_collection assistant_knowledge_collection_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_knowledge_collection
    ADD CONSTRAINT assistant_knowledge_collection_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5900 (class 2606 OID 18826)
-- Name: assistant_tool assistant_tool_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_tool
    ADD CONSTRAINT assistant_tool_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5901 (class 2606 OID 18831)
-- Name: assistant_tool assistant_tool_tool_definition_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_tool
    ADD CONSTRAINT assistant_tool_tool_definition_id_fkey FOREIGN KEY (tool_definition_id) REFERENCES ai_core.tool_definition(tool_definition_id);


--
-- TOC entry 5899 (class 2606 OID 18788)
-- Name: knowledge_chunk knowledge_chunk_knowledge_document_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_chunk
    ADD CONSTRAINT knowledge_chunk_knowledge_document_id_fkey FOREIGN KEY (knowledge_document_id) REFERENCES ai_core.knowledge_document(knowledge_document_id);


--
-- TOC entry 5894 (class 2606 OID 18735)
-- Name: knowledge_collection knowledge_collection_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_collection
    ADD CONSTRAINT knowledge_collection_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5895 (class 2606 OID 18769)
-- Name: knowledge_document knowledge_document_academic_system_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_document
    ADD CONSTRAINT knowledge_document_academic_system_id_fkey FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id);


--
-- TOC entry 5896 (class 2606 OID 18764)
-- Name: knowledge_document knowledge_document_campus_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_document
    ADD CONSTRAINT knowledge_document_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5897 (class 2606 OID 18754)
-- Name: knowledge_document knowledge_document_knowledge_collection_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_document
    ADD CONSTRAINT knowledge_document_knowledge_collection_id_fkey FOREIGN KEY (knowledge_collection_id) REFERENCES ai_core.knowledge_collection(knowledge_collection_id);


--
-- TOC entry 5898 (class 2606 OID 18759)
-- Name: knowledge_document knowledge_document_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_document
    ADD CONSTRAINT knowledge_document_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5892 (class 2606 OID 18690)
-- Name: model_configuration model_configuration_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.model_configuration
    ADD CONSTRAINT model_configuration_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5893 (class 2606 OID 18714)
-- Name: prompt_template prompt_template_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.prompt_template
    ADD CONSTRAINT prompt_template_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5906 (class 2606 OID 18893)
-- Name: tool_execution tool_execution_ai_execution_log_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.tool_execution
    ADD CONSTRAINT tool_execution_ai_execution_log_id_fkey FOREIGN KEY (ai_execution_log_id) REFERENCES ai_core.ai_execution_log(ai_execution_log_id);


--
-- TOC entry 5907 (class 2606 OID 18898)
-- Name: tool_execution tool_execution_tool_definition_id_fkey; Type: FK CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.tool_execution
    ADD CONSTRAINT tool_execution_tool_definition_id_fkey FOREIGN KEY (tool_definition_id) REFERENCES ai_core.tool_definition(tool_definition_id);


--
-- TOC entry 5939 (class 2606 OID 19249)
-- Name: human_handoff human_handoff_inquiry_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.human_handoff
    ADD CONSTRAINT human_handoff_inquiry_conversation_id_fkey FOREIGN KEY (inquiry_conversation_id) REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id);


--
-- TOC entry 5931 (class 2606 OID 19174)
-- Name: inquiry_conversation inquiry_conversation_campus_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_conversation
    ADD CONSTRAINT inquiry_conversation_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5932 (class 2606 OID 19179)
-- Name: inquiry_conversation inquiry_conversation_interested_program_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_conversation
    ADD CONSTRAINT inquiry_conversation_interested_program_id_fkey FOREIGN KEY (interested_program_id) REFERENCES academic.program(program_id);


--
-- TOC entry 5933 (class 2606 OID 19169)
-- Name: inquiry_conversation inquiry_conversation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_conversation
    ADD CONSTRAINT inquiry_conversation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5934 (class 2606 OID 19198)
-- Name: inquiry_message inquiry_message_inquiry_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_message
    ADD CONSTRAINT inquiry_message_inquiry_conversation_id_fkey FOREIGN KEY (inquiry_conversation_id) REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id);


--
-- TOC entry 5935 (class 2606 OID 19215)
-- Name: lead_capture lead_capture_inquiry_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.lead_capture
    ADD CONSTRAINT lead_capture_inquiry_conversation_id_fkey FOREIGN KEY (inquiry_conversation_id) REFERENCES ai_inquiry.inquiry_conversation(inquiry_conversation_id);


--
-- TOC entry 5936 (class 2606 OID 19220)
-- Name: lead_capture lead_capture_interested_campus_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.lead_capture
    ADD CONSTRAINT lead_capture_interested_campus_id_fkey FOREIGN KEY (interested_campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5937 (class 2606 OID 19230)
-- Name: lead_capture lead_capture_interested_grade_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.lead_capture
    ADD CONSTRAINT lead_capture_interested_grade_id_fkey FOREIGN KEY (interested_grade_id) REFERENCES academic.grade_level(grade_level_id);


--
-- TOC entry 5938 (class 2606 OID 19225)
-- Name: lead_capture lead_capture_interested_program_id_fkey; Type: FK CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.lead_capture
    ADD CONSTRAINT lead_capture_interested_program_id_fkey FOREIGN KEY (interested_program_id) REFERENCES academic.program(program_id);


--
-- TOC entry 5940 (class 2606 OID 19272)
-- Name: parent_conversation parent_conversation_guardian_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_conversation
    ADD CONSTRAINT parent_conversation_guardian_id_fkey FOREIGN KEY (guardian_id) REFERENCES student.guardian(guardian_id);


--
-- TOC entry 5941 (class 2606 OID 19277)
-- Name: parent_conversation parent_conversation_selected_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_conversation
    ADD CONSTRAINT parent_conversation_selected_student_id_fkey FOREIGN KEY (selected_student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5942 (class 2606 OID 19267)
-- Name: parent_conversation parent_conversation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_conversation
    ADD CONSTRAINT parent_conversation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5943 (class 2606 OID 19296)
-- Name: parent_message parent_message_parent_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_message
    ADD CONSTRAINT parent_message_parent_conversation_id_fkey FOREIGN KEY (parent_conversation_id) REFERENCES ai_parent.parent_conversation(parent_conversation_id);


--
-- TOC entry 5944 (class 2606 OID 19315)
-- Name: parent_tool_execution parent_tool_execution_parent_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_tool_execution
    ADD CONSTRAINT parent_tool_execution_parent_conversation_id_fkey FOREIGN KEY (parent_conversation_id) REFERENCES ai_parent.parent_conversation(parent_conversation_id);


--
-- TOC entry 5945 (class 2606 OID 19325)
-- Name: parent_tool_execution parent_tool_execution_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_tool_execution
    ADD CONSTRAINT parent_tool_execution_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5946 (class 2606 OID 19320)
-- Name: parent_tool_execution parent_tool_execution_tool_definition_id_fkey; Type: FK CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_tool_execution
    ADD CONSTRAINT parent_tool_execution_tool_definition_id_fkey FOREIGN KEY (tool_definition_id) REFERENCES ai_core.tool_definition(tool_definition_id);


--
-- TOC entry 5928 (class 2606 OID 19126)
-- Name: generated_quiz_question generated_quiz_question_generated_quiz_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz_question
    ADD CONSTRAINT generated_quiz_question_generated_quiz_id_fkey FOREIGN KEY (generated_quiz_id) REFERENCES ai_tutor.generated_quiz(generated_quiz_id);


--
-- TOC entry 5924 (class 2606 OID 19096)
-- Name: generated_quiz generated_quiz_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz
    ADD CONSTRAINT generated_quiz_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5925 (class 2606 OID 19101)
-- Name: generated_quiz generated_quiz_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz
    ADD CONSTRAINT generated_quiz_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5926 (class 2606 OID 19091)
-- Name: generated_quiz generated_quiz_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz
    ADD CONSTRAINT generated_quiz_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5927 (class 2606 OID 19106)
-- Name: generated_quiz generated_quiz_tutor_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz
    ADD CONSTRAINT generated_quiz_tutor_conversation_id_fkey FOREIGN KEY (tutor_conversation_id) REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id);


--
-- TOC entry 5922 (class 2606 OID 19069)
-- Name: learning_recommendation learning_recommendation_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.learning_recommendation
    ADD CONSTRAINT learning_recommendation_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5923 (class 2606 OID 19074)
-- Name: learning_recommendation learning_recommendation_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.learning_recommendation
    ADD CONSTRAINT learning_recommendation_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5929 (class 2606 OID 19144)
-- Name: student_quiz_attempt student_quiz_attempt_generated_quiz_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_quiz_attempt
    ADD CONSTRAINT student_quiz_attempt_generated_quiz_id_fkey FOREIGN KEY (generated_quiz_id) REFERENCES ai_tutor.generated_quiz(generated_quiz_id);


--
-- TOC entry 5930 (class 2606 OID 19149)
-- Name: student_quiz_attempt student_quiz_attempt_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_quiz_attempt
    ADD CONSTRAINT student_quiz_attempt_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5919 (class 2606 OID 19041)
-- Name: student_topic_mastery student_topic_mastery_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_topic_mastery
    ADD CONSTRAINT student_topic_mastery_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5920 (class 2606 OID 19046)
-- Name: student_topic_mastery student_topic_mastery_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_topic_mastery
    ADD CONSTRAINT student_topic_mastery_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5921 (class 2606 OID 19036)
-- Name: student_topic_mastery student_topic_mastery_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_topic_mastery
    ADD CONSTRAINT student_topic_mastery_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5908 (class 2606 OID 18926)
-- Name: tutor_conversation tutor_conversation_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5909 (class 2606 OID 18931)
-- Name: tutor_conversation tutor_conversation_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5910 (class 2606 OID 18921)
-- Name: tutor_conversation tutor_conversation_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5911 (class 2606 OID 18936)
-- Name: tutor_conversation tutor_conversation_subject_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5912 (class 2606 OID 18916)
-- Name: tutor_conversation tutor_conversation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5917 (class 2606 OID 19014)
-- Name: tutor_feedback tutor_feedback_student_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_feedback
    ADD CONSTRAINT tutor_feedback_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5918 (class 2606 OID 19009)
-- Name: tutor_feedback tutor_feedback_tutor_message_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_feedback
    ADD CONSTRAINT tutor_feedback_tutor_message_id_fkey FOREIGN KEY (tutor_message_id) REFERENCES ai_tutor.tutor_message(tutor_message_id);


--
-- TOC entry 5914 (class 2606 OID 18973)
-- Name: tutor_message_reference tutor_message_reference_knowledge_chunk_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_message_reference
    ADD CONSTRAINT tutor_message_reference_knowledge_chunk_id_fkey FOREIGN KEY (knowledge_chunk_id) REFERENCES ai_core.knowledge_chunk(knowledge_chunk_id);


--
-- TOC entry 5915 (class 2606 OID 18968)
-- Name: tutor_message_reference tutor_message_reference_tutor_message_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_message_reference
    ADD CONSTRAINT tutor_message_reference_tutor_message_id_fkey FOREIGN KEY (tutor_message_id) REFERENCES ai_tutor.tutor_message(tutor_message_id);


--
-- TOC entry 5913 (class 2606 OID 18955)
-- Name: tutor_message tutor_message_tutor_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_message
    ADD CONSTRAINT tutor_message_tutor_conversation_id_fkey FOREIGN KEY (tutor_conversation_id) REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id);


--
-- TOC entry 5916 (class 2606 OID 18990)
-- Name: tutor_session tutor_session_tutor_conversation_id_fkey; Type: FK CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_session
    ADD CONSTRAINT tutor_session_tutor_conversation_id_fkey FOREIGN KEY (tutor_conversation_id) REFERENCES ai_tutor.tutor_conversation(tutor_conversation_id);


--
-- TOC entry 5999 (class 2606 OID 20409)
-- Name: ChatAttachments FK_ChatAttachments_Message; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatAttachments"
    ADD CONSTRAINT "FK_ChatAttachments_Message" FOREIGN KEY ("MessageId") REFERENCES communication."ChatMessages"("Id") ON DELETE CASCADE;


--
-- TOC entry 5998 (class 2606 OID 20390)
-- Name: ChatMessages FK_ChatMessages_Conversation; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatMessages"
    ADD CONSTRAINT "FK_ChatMessages_Conversation" FOREIGN KEY ("ConversationId") REFERENCES communication."ChatConversations"("Id") ON DELETE CASCADE;


--
-- TOC entry 5997 (class 2606 OID 20369)
-- Name: ChatParticipants FK_ChatParticipants_Conversation; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatParticipants"
    ADD CONSTRAINT "FK_ChatParticipants_Conversation" FOREIGN KEY ("ConversationId") REFERENCES communication."ChatConversations"("Id") ON DELETE CASCADE;


--
-- TOC entry 5866 (class 2606 OID 18383)
-- Name: conversation conversation_campus_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5867 (class 2606 OID 18393)
-- Name: conversation conversation_class_section_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5871 (class 2606 OID 18412)
-- Name: conversation_participant conversation_participant_conversation_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation_participant
    ADD CONSTRAINT conversation_participant_conversation_id_fkey FOREIGN KEY (conversation_id) REFERENCES communication.conversation(conversation_id);


--
-- TOC entry 5868 (class 2606 OID 18388)
-- Name: conversation conversation_student_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5869 (class 2606 OID 18398)
-- Name: conversation conversation_subject_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5870 (class 2606 OID 18378)
-- Name: conversation conversation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5872 (class 2606 OID 18432)
-- Name: message message_conversation_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.message
    ADD CONSTRAINT message_conversation_id_fkey FOREIGN KEY (conversation_id) REFERENCES communication.conversation(conversation_id);


--
-- TOC entry 5874 (class 2606 OID 18449)
-- Name: message_receipt message_receipt_message_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.message_receipt
    ADD CONSTRAINT message_receipt_message_id_fkey FOREIGN KEY (message_id) REFERENCES communication.message(message_id);


--
-- TOC entry 5873 (class 2606 OID 18437)
-- Name: message message_reply_to_message_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.message
    ADD CONSTRAINT message_reply_to_message_id_fkey FOREIGN KEY (reply_to_message_id) REFERENCES communication.message(message_id);


--
-- TOC entry 5875 (class 2606 OID 18471)
-- Name: notification notification_tenant_id_fkey; Type: FK CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.notification
    ADD CONSTRAINT notification_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5851 (class 2606 OID 18250)
-- Name: document_template document_template_academic_system_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.document_template
    ADD CONSTRAINT document_template_academic_system_id_fkey FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id);


--
-- TOC entry 5852 (class 2606 OID 18245)
-- Name: document_template document_template_campus_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.document_template
    ADD CONSTRAINT document_template_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5853 (class 2606 OID 18240)
-- Name: document_template document_template_tenant_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.document_template
    ADD CONSTRAINT document_template_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5994 (class 2606 OID 19980)
-- Name: candidatedocument fk_candidatedocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.candidatedocument
    ADD CONSTRAINT fk_candidatedocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5995 (class 2606 OID 20014)
-- Name: driverdocument fk_driverdocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.driverdocument
    ADD CONSTRAINT fk_driverdocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5993 (class 2606 OID 19946)
-- Name: employeedocument fk_employeedocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.employeedocument
    ADD CONSTRAINT fk_employeedocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5991 (class 2606 OID 19878)
-- Name: parentdocument fk_parentdocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.parentdocument
    ADD CONSTRAINT fk_parentdocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5990 (class 2606 OID 19844)
-- Name: studentdocument fk_studentdocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.studentdocument
    ADD CONSTRAINT fk_studentdocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5992 (class 2606 OID 19912)
-- Name: teacherdocument fk_teacherdocument_documenttype; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.teacherdocument
    ADD CONSTRAINT fk_teacherdocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5854 (class 2606 OID 18280)
-- Name: generated_document generated_document_document_template_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_document_template_id_fkey FOREIGN KEY (document_template_id) REFERENCES document.document_template(document_template_id);


--
-- TOC entry 5855 (class 2606 OID 18290)
-- Name: generated_document generated_document_employee_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5856 (class 2606 OID 18285)
-- Name: generated_document generated_document_student_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5857 (class 2606 OID 18275)
-- Name: generated_document generated_document_tenant_id_fkey; Type: FK CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5811 (class 2606 OID 17771)
-- Name: exam exam_academic_system_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_academic_system_id_fkey FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id);


--
-- TOC entry 5812 (class 2606 OID 17761)
-- Name: exam exam_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5813 (class 2606 OID 17756)
-- Name: exam exam_campus_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5816 (class 2606 OID 17791)
-- Name: exam_subject exam_subject_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam_subject
    ADD CONSTRAINT exam_subject_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5817 (class 2606 OID 17786)
-- Name: exam_subject exam_subject_exam_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam_subject
    ADD CONSTRAINT exam_subject_exam_id_fkey FOREIGN KEY (exam_id) REFERENCES exam.exam(exam_id);


--
-- TOC entry 5818 (class 2606 OID 17796)
-- Name: exam_subject exam_subject_room_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam_subject
    ADD CONSTRAINT exam_subject_room_id_fkey FOREIGN KEY (room_id) REFERENCES org.room(room_id);


--
-- TOC entry 5814 (class 2606 OID 17751)
-- Name: exam exam_tenant_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5815 (class 2606 OID 17766)
-- Name: exam exam_term_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5819 (class 2606 OID 17816)
-- Name: student_exam_result student_exam_result_exam_subject_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.student_exam_result
    ADD CONSTRAINT student_exam_result_exam_subject_id_fkey FOREIGN KEY (exam_subject_id) REFERENCES exam.exam_subject(exam_subject_id);


--
-- TOC entry 5820 (class 2606 OID 17821)
-- Name: student_exam_result student_exam_result_student_id_fkey; Type: FK CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.student_exam_result
    ADD CONSTRAINT student_exam_result_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5821 (class 2606 OID 17838)
-- Name: fee_type fee_type_tenant_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.fee_type
    ADD CONSTRAINT fee_type_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5829 (class 2606 OID 17938)
-- Name: payment_allocation payment_allocation_student_invoice_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.payment_allocation
    ADD CONSTRAINT payment_allocation_student_invoice_id_fkey FOREIGN KEY (student_invoice_id) REFERENCES finance.student_invoice(student_invoice_id);


--
-- TOC entry 5830 (class 2606 OID 17933)
-- Name: payment_allocation payment_allocation_student_payment_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.payment_allocation
    ADD CONSTRAINT payment_allocation_student_payment_id_fkey FOREIGN KEY (student_payment_id) REFERENCES finance.student_payment(student_payment_id);


--
-- TOC entry 5822 (class 2606 OID 17872)
-- Name: student_invoice student_invoice_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice
    ADD CONSTRAINT student_invoice_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5825 (class 2606 OID 17892)
-- Name: student_invoice_line student_invoice_line_fee_type_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice_line
    ADD CONSTRAINT student_invoice_line_fee_type_id_fkey FOREIGN KEY (fee_type_id) REFERENCES finance.fee_type(fee_type_id);


--
-- TOC entry 5826 (class 2606 OID 17887)
-- Name: student_invoice_line student_invoice_line_student_invoice_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice_line
    ADD CONSTRAINT student_invoice_line_student_invoice_id_fkey FOREIGN KEY (student_invoice_id) REFERENCES finance.student_invoice(student_invoice_id);


--
-- TOC entry 5823 (class 2606 OID 17867)
-- Name: student_invoice student_invoice_student_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice
    ADD CONSTRAINT student_invoice_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5824 (class 2606 OID 17862)
-- Name: student_invoice student_invoice_tenant_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice
    ADD CONSTRAINT student_invoice_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5827 (class 2606 OID 17918)
-- Name: student_payment student_payment_student_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_payment
    ADD CONSTRAINT student_payment_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5828 (class 2606 OID 17913)
-- Name: student_payment student_payment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_payment
    ADD CONSTRAINT student_payment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 6002 (class 2606 OID 23024)
-- Name: jobparameter jobparameter_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 6001 (class 2606 OID 22999)
-- Name: state state_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 5773 (class 2606 OID 17375)
-- Name: candidate_document candidate_document_candidate_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.candidate_document
    ADD CONSTRAINT candidate_document_candidate_id_fkey FOREIGN KEY (candidate_id) REFERENCES hr.candidate(candidate_id);


--
-- TOC entry 5772 (class 2606 OID 17355)
-- Name: candidate candidate_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.candidate
    ADD CONSTRAINT candidate_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5832 (class 2606 OID 17987)
-- Name: employee_compensation employee_compensation_employee_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_compensation
    ADD CONSTRAINT employee_compensation_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5833 (class 2606 OID 17992)
-- Name: employee_compensation employee_compensation_job_grade_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_compensation
    ADD CONSTRAINT employee_compensation_job_grade_id_fkey FOREIGN KEY (job_grade_id) REFERENCES hr.job_grade(job_grade_id);


--
-- TOC entry 5834 (class 2606 OID 17982)
-- Name: employee_compensation employee_compensation_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_compensation
    ADD CONSTRAINT employee_compensation_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5764 (class 2606 OID 17295)
-- Name: employee_position employee_position_employee_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_position
    ADD CONSTRAINT employee_position_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5765 (class 2606 OID 17300)
-- Name: employee_position employee_position_position_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_position
    ADD CONSTRAINT employee_position_position_id_fkey FOREIGN KEY (position_id) REFERENCES hr."position"(position_id);


--
-- TOC entry 5766 (class 2606 OID 17290)
-- Name: employee_position employee_position_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_position
    ADD CONSTRAINT employee_position_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5835 (class 2606 OID 18006)
-- Name: employee_salary_component employee_salary_component_employee_compensation_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_salary_component
    ADD CONSTRAINT employee_salary_component_employee_compensation_id_fkey FOREIGN KEY (employee_compensation_id) REFERENCES hr.employee_compensation(employee_compensation_id);


--
-- TOC entry 5836 (class 2606 OID 18011)
-- Name: employee_salary_component employee_salary_component_salary_component_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_salary_component
    ADD CONSTRAINT employee_salary_component_salary_component_id_fkey FOREIGN KEY (salary_component_id) REFERENCES hr.salary_component(salary_component_id);


--
-- TOC entry 5763 (class 2606 OID 17265)
-- Name: employee employee_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee
    ADD CONSTRAINT employee_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5841 (class 2606 OID 18092)
-- Name: increment_approval increment_approval_increment_request_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.increment_approval
    ADD CONSTRAINT increment_approval_increment_request_id_fkey FOREIGN KEY (increment_request_id) REFERENCES hr.salary_increment_request(increment_request_id);


--
-- TOC entry 5837 (class 2606 OID 18040)
-- Name: increment_policy increment_policy_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.increment_policy
    ADD CONSTRAINT increment_policy_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5782 (class 2606 OID 17486)
-- Name: interview_evaluation interview_evaluation_interview_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_evaluation
    ADD CONSTRAINT interview_evaluation_interview_id_fkey FOREIGN KEY (interview_id) REFERENCES hr.interview(interview_id);


--
-- TOC entry 5783 (class 2606 OID 17491)
-- Name: interview_evaluation interview_evaluation_interviewer_employee_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_evaluation
    ADD CONSTRAINT interview_evaluation_interviewer_employee_id_fkey FOREIGN KEY (interviewer_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5779 (class 2606 OID 17453)
-- Name: interview interview_job_application_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview
    ADD CONSTRAINT interview_job_application_id_fkey FOREIGN KEY (job_application_id) REFERENCES hr.job_application(job_application_id);


--
-- TOC entry 5780 (class 2606 OID 17470)
-- Name: interview_panel interview_panel_employee_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_panel
    ADD CONSTRAINT interview_panel_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5781 (class 2606 OID 17465)
-- Name: interview_panel interview_panel_interview_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_panel
    ADD CONSTRAINT interview_panel_interview_id_fkey FOREIGN KEY (interview_id) REFERENCES hr.interview(interview_id);


--
-- TOC entry 5776 (class 2606 OID 17428)
-- Name: job_application job_application_candidate_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_application
    ADD CONSTRAINT job_application_candidate_id_fkey FOREIGN KEY (candidate_id) REFERENCES hr.candidate(candidate_id);


--
-- TOC entry 5777 (class 2606 OID 17433)
-- Name: job_application job_application_job_vacancy_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_application
    ADD CONSTRAINT job_application_job_vacancy_id_fkey FOREIGN KEY (job_vacancy_id) REFERENCES hr.job_vacancy(job_vacancy_id);


--
-- TOC entry 5778 (class 2606 OID 17423)
-- Name: job_application job_application_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_application
    ADD CONSTRAINT job_application_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5752 (class 2606 OID 17169)
-- Name: job job_department_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job
    ADD CONSTRAINT job_department_id_fkey FOREIGN KEY (department_id) REFERENCES org.department(department_id);


--
-- TOC entry 5750 (class 2606 OID 17120)
-- Name: job_family job_family_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_family
    ADD CONSTRAINT job_family_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5755 (class 2606 OID 17193)
-- Name: job_grade_mapping job_grade_mapping_job_grade_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade_mapping
    ADD CONSTRAINT job_grade_mapping_job_grade_id_fkey FOREIGN KEY (job_grade_id) REFERENCES hr.job_grade(job_grade_id);


--
-- TOC entry 5756 (class 2606 OID 17188)
-- Name: job_grade_mapping job_grade_mapping_job_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade_mapping
    ADD CONSTRAINT job_grade_mapping_job_id_fkey FOREIGN KEY (job_id) REFERENCES hr.job(job_id);


--
-- TOC entry 5751 (class 2606 OID 17141)
-- Name: job_grade job_grade_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade
    ADD CONSTRAINT job_grade_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5753 (class 2606 OID 17174)
-- Name: job job_job_family_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job
    ADD CONSTRAINT job_job_family_id_fkey FOREIGN KEY (job_family_id) REFERENCES hr.job_family(job_family_id);


--
-- TOC entry 5754 (class 2606 OID 17164)
-- Name: job job_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job
    ADD CONSTRAINT job_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5774 (class 2606 OID 17398)
-- Name: job_vacancy job_vacancy_position_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_vacancy
    ADD CONSTRAINT job_vacancy_position_id_fkey FOREIGN KEY (position_id) REFERENCES hr."position"(position_id);


--
-- TOC entry 5775 (class 2606 OID 17393)
-- Name: job_vacancy job_vacancy_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_vacancy
    ADD CONSTRAINT job_vacancy_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5757 (class 2606 OID 17220)
-- Name: position position_campus_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5758 (class 2606 OID 17225)
-- Name: position position_department_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_department_id_fkey FOREIGN KEY (department_id) REFERENCES org.department(department_id);


--
-- TOC entry 5759 (class 2606 OID 17235)
-- Name: position position_job_grade_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_job_grade_id_fkey FOREIGN KEY (job_grade_id) REFERENCES hr.job_grade(job_grade_id);


--
-- TOC entry 5760 (class 2606 OID 17230)
-- Name: position position_job_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_job_id_fkey FOREIGN KEY (job_id) REFERENCES hr.job(job_id);


--
-- TOC entry 5761 (class 2606 OID 17240)
-- Name: position position_reports_to_position_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_reports_to_position_id_fkey FOREIGN KEY (reports_to_position_id) REFERENCES hr."position"(position_id);


--
-- TOC entry 5762 (class 2606 OID 17215)
-- Name: position position_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5831 (class 2606 OID 17962)
-- Name: salary_component salary_component_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_component
    ADD CONSTRAINT salary_component_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5838 (class 2606 OID 18070)
-- Name: salary_increment_request salary_increment_request_employee_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_increment_request
    ADD CONSTRAINT salary_increment_request_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5839 (class 2606 OID 18075)
-- Name: salary_increment_request salary_increment_request_increment_policy_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_increment_request
    ADD CONSTRAINT salary_increment_request_increment_policy_id_fkey FOREIGN KEY (increment_policy_id) REFERENCES hr.increment_policy(increment_policy_id);


--
-- TOC entry 5840 (class 2606 OID 18065)
-- Name: salary_increment_request salary_increment_request_tenant_id_fkey; Type: FK CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_increment_request
    ADD CONSTRAINT salary_increment_request_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5882 (class 2606 OID 18555)
-- Name: item item_tenant_id_fkey; Type: FK CONSTRAINT; Schema: inventory; Owner: postgres
--

ALTER TABLE ONLY inventory.item
    ADD CONSTRAINT item_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5877 (class 2606 OID 18506)
-- Name: book_copy book_copy_book_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_copy
    ADD CONSTRAINT book_copy_book_id_fkey FOREIGN KEY (book_id) REFERENCES library.book(book_id);


--
-- TOC entry 5878 (class 2606 OID 18511)
-- Name: book_copy book_copy_campus_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_copy
    ADD CONSTRAINT book_copy_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5879 (class 2606 OID 18528)
-- Name: book_loan book_loan_book_copy_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_loan
    ADD CONSTRAINT book_loan_book_copy_id_fkey FOREIGN KEY (book_copy_id) REFERENCES library.book_copy(book_copy_id);


--
-- TOC entry 5880 (class 2606 OID 18538)
-- Name: book_loan book_loan_employee_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_loan
    ADD CONSTRAINT book_loan_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5881 (class 2606 OID 18533)
-- Name: book_loan book_loan_student_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_loan
    ADD CONSTRAINT book_loan_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5876 (class 2606 OID 18487)
-- Name: book book_tenant_id_fkey; Type: FK CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book
    ADD CONSTRAINT book_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5804 (class 2606 OID 17694)
-- Name: academic_assignment academic_assignment_class_section_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5805 (class 2606 OID 17689)
-- Name: academic_assignment academic_assignment_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5806 (class 2606 OID 17704)
-- Name: academic_assignment academic_assignment_teacher_employee_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_teacher_employee_id_fkey FOREIGN KEY (teacher_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5807 (class 2606 OID 17699)
-- Name: academic_assignment academic_assignment_teaching_group_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_teaching_group_id_fkey FOREIGN KEY (teaching_group_id) REFERENCES academic.teaching_group(teaching_group_id);


--
-- TOC entry 5808 (class 2606 OID 17684)
-- Name: academic_assignment academic_assignment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5809 (class 2606 OID 17726)
-- Name: student_assignment_submission student_assignment_submission_academic_assignment_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.student_assignment_submission
    ADD CONSTRAINT student_assignment_submission_academic_assignment_id_fkey FOREIGN KEY (academic_assignment_id) REFERENCES lms.academic_assignment(academic_assignment_id);


--
-- TOC entry 5810 (class 2606 OID 17731)
-- Name: student_assignment_submission student_assignment_submission_student_id_fkey; Type: FK CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.student_assignment_submission
    ADD CONSTRAINT student_assignment_submission_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5698 (class 2606 OID 16536)
-- Name: campus campus_tenant_id_fkey; Type: FK CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.campus
    ADD CONSTRAINT campus_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5699 (class 2606 OID 16558)
-- Name: department department_campus_id_fkey; Type: FK CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.department
    ADD CONSTRAINT department_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5700 (class 2606 OID 16553)
-- Name: department department_tenant_id_fkey; Type: FK CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.department
    ADD CONSTRAINT department_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5701 (class 2606 OID 16581)
-- Name: room room_campus_id_fkey; Type: FK CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.room
    ADD CONSTRAINT room_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5702 (class 2606 OID 16576)
-- Name: room room_tenant_id_fkey; Type: FK CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.room
    ADD CONSTRAINT room_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5845 (class 2606 OID 18162)
-- Name: employee_payroll employee_payroll_employee_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.employee_payroll
    ADD CONSTRAINT employee_payroll_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5846 (class 2606 OID 18157)
-- Name: employee_payroll employee_payroll_payroll_run_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.employee_payroll
    ADD CONSTRAINT employee_payroll_payroll_run_id_fkey FOREIGN KEY (payroll_run_id) REFERENCES payroll.payroll_run(payroll_run_id);


--
-- TOC entry 5847 (class 2606 OID 18176)
-- Name: payroll_line_item payroll_line_item_employee_payroll_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_line_item
    ADD CONSTRAINT payroll_line_item_employee_payroll_id_fkey FOREIGN KEY (employee_payroll_id) REFERENCES payroll.employee_payroll(employee_payroll_id);


--
-- TOC entry 5848 (class 2606 OID 18181)
-- Name: payroll_line_item payroll_line_item_salary_component_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_line_item
    ADD CONSTRAINT payroll_line_item_salary_component_id_fkey FOREIGN KEY (salary_component_id) REFERENCES hr.salary_component(salary_component_id);


--
-- TOC entry 5842 (class 2606 OID 18112)
-- Name: payroll_period payroll_period_tenant_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_period
    ADD CONSTRAINT payroll_period_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5843 (class 2606 OID 18135)
-- Name: payroll_run payroll_run_payroll_period_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_run
    ADD CONSTRAINT payroll_run_payroll_period_id_fkey FOREIGN KEY (payroll_period_id) REFERENCES payroll.payroll_period(payroll_period_id);


--
-- TOC entry 5844 (class 2606 OID 18130)
-- Name: payroll_run payroll_run_tenant_id_fkey; Type: FK CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_run
    ADD CONSTRAINT payroll_run_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5996 (class 2606 OID 20048)
-- Name: schooldocument fk_schooldocument_documenttype; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.schooldocument
    ADD CONSTRAINT fk_schooldocument_documenttype FOREIGN KEY (documenttypeid) REFERENCES document.documenttype(id);


--
-- TOC entry 5696 (class 2606 OID 16481)
-- Name: lookup_value lookup_value_lookup_type_id_fkey; Type: FK CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.lookup_value
    ADD CONSTRAINT lookup_value_lookup_type_id_fkey FOREIGN KEY (lookup_type_id) REFERENCES saas.lookup_type(lookup_type_id);


--
-- TOC entry 5697 (class 2606 OID 16515)
-- Name: school_branding school_branding_tenant_id_fkey; Type: FK CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.school_branding
    ADD CONSTRAINT school_branding_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5740 (class 2606 OID 17008)
-- Name: guardian guardian_tenant_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.guardian
    ADD CONSTRAINT guardian_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5747 (class 2606 OID 17103)
-- Name: student_course_enrollment student_course_enrollment_course_offering_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_course_enrollment
    ADD CONSTRAINT student_course_enrollment_course_offering_id_fkey FOREIGN KEY (course_offering_id) REFERENCES academic.course_offering(course_offering_id);


--
-- TOC entry 5748 (class 2606 OID 17098)
-- Name: student_course_enrollment student_course_enrollment_student_enrollment_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_course_enrollment
    ADD CONSTRAINT student_course_enrollment_student_enrollment_id_fkey FOREIGN KEY (student_enrollment_id) REFERENCES student.student_enrollment(student_enrollment_id);


--
-- TOC entry 5749 (class 2606 OID 17093)
-- Name: student_course_enrollment student_course_enrollment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_course_enrollment
    ADD CONSTRAINT student_course_enrollment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5743 (class 2606 OID 17066)
-- Name: student_enrollment student_enrollment_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5744 (class 2606 OID 17071)
-- Name: student_enrollment student_enrollment_class_section_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_class_section_id_fkey FOREIGN KEY (class_section_id) REFERENCES academic.class_section(class_section_id);


--
-- TOC entry 5745 (class 2606 OID 17061)
-- Name: student_enrollment student_enrollment_student_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5746 (class 2606 OID 17056)
-- Name: student_enrollment student_enrollment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5741 (class 2606 OID 17034)
-- Name: student_guardian student_guardian_guardian_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_guardian
    ADD CONSTRAINT student_guardian_guardian_id_fkey FOREIGN KEY (guardian_id) REFERENCES student.guardian(guardian_id);


--
-- TOC entry 5742 (class 2606 OID 17029)
-- Name: student_guardian student_guardian_student_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_guardian
    ADD CONSTRAINT student_guardian_student_id_fkey FOREIGN KEY (student_id) REFERENCES student.student(student_id);


--
-- TOC entry 5739 (class 2606 OID 16990)
-- Name: student student_tenant_id_fkey; Type: FK CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student
    ADD CONSTRAINT student_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 6006 (class 2606 OID 23390)
-- Name: leave_request leave_request_employee_id_fkey; Type: FK CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.leave_request
    ADD CONSTRAINT leave_request_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 6007 (class 2606 OID 23385)
-- Name: leave_request leave_request_tenant_id_fkey; Type: FK CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.leave_request
    ADD CONSTRAINT leave_request_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 6003 (class 2606 OID 23354)
-- Name: teacher_actor teacher_actor_employee_id_fkey; Type: FK CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 6004 (class 2606 OID 23359)
-- Name: teacher_actor teacher_actor_primary_campus_id_fkey; Type: FK CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_primary_campus_id_fkey FOREIGN KEY (primary_campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 6005 (class 2606 OID 23349)
-- Name: teacher_actor teacher_actor_tenant_id_fkey; Type: FK CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5883 (class 2606 OID 18591)
-- Name: driver driver_employee_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5884 (class 2606 OID 18586)
-- Name: driver driver_tenant_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5890 (class 2606 OID 18669)
-- Name: route route_campus_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.route
    ADD CONSTRAINT route_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5891 (class 2606 OID 18664)
-- Name: route route_tenant_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.route
    ADD CONSTRAINT route_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5885 (class 2606 OID 18616)
-- Name: vehicle vehicle_campus_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle
    ADD CONSTRAINT vehicle_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5887 (class 2606 OID 18645)
-- Name: vehicle_driver_assignment vehicle_driver_assignment_driver_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle_driver_assignment
    ADD CONSTRAINT vehicle_driver_assignment_driver_id_fkey FOREIGN KEY (driver_id) REFERENCES transport.driver(driver_id);


--
-- TOC entry 5888 (class 2606 OID 18635)
-- Name: vehicle_driver_assignment vehicle_driver_assignment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle_driver_assignment
    ADD CONSTRAINT vehicle_driver_assignment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5889 (class 2606 OID 18640)
-- Name: vehicle_driver_assignment vehicle_driver_assignment_vehicle_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle_driver_assignment
    ADD CONSTRAINT vehicle_driver_assignment_vehicle_id_fkey FOREIGN KEY (vehicle_id) REFERENCES transport.vehicle(vehicle_id);


--
-- TOC entry 5886 (class 2606 OID 18611)
-- Name: vehicle vehicle_tenant_id_fkey; Type: FK CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle
    ADD CONSTRAINT vehicle_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5849 (class 2606 OID 18209)
-- Name: work_assignment work_assignment_campus_id_fkey; Type: FK CONSTRAINT; Schema: workflow; Owner: postgres
--

ALTER TABLE ONLY workflow.work_assignment
    ADD CONSTRAINT work_assignment_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5850 (class 2606 OID 18204)
-- Name: work_assignment work_assignment_tenant_id_fkey; Type: FK CONSTRAINT; Schema: workflow; Owner: postgres
--

ALTER TABLE ONLY workflow.work_assignment
    ADD CONSTRAINT work_assignment_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


-- Completed on 2026-08-23 15:37:10

--
-- PostgreSQL database dump complete
--

\unrestrict JgF6T5FiaNDLghyzB4HTugBI1hR0RCfJwKav6daUVCYxVgCNwMp0olASTqNXkxE

CREATE TABLE IF NOT EXISTS org.school (
    school_id uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    tenant_id uuid NOT NULL,
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    registration_number varchar(100),
    email varchar(200),
    phone varchar(50),
    website varchar(300),
    address text,
    city varchar(120),
    country varchar(120),
    logo_url varchar(500),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamptz DEFAULT now() NOT NULL,
    updated_at timestamptz,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT uq_school_tenant_code UNIQUE (tenant_id, code)
);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;

-- Existing campuses are intentionally left nullable during migration. Assign them to a school,
-- then make the column NOT NULL in environments containing legacy data.
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus (tenant_id, school_id);
