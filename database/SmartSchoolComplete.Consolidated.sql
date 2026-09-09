-- SmartSchool consolidated PostgreSQL schema/data script
-- Generated from the current v87 baseline plus all subsequent canonical alignment scripts.
-- Run this once for a fresh SmartSchool business database.
-- Identity/Duende tables are managed by EF Core migrations in SmartSchool.Identity.Api.
-- The Identity API then idempotently seeds roles, the bootstrap SuperAdmin and Duende clients/resources.

\set ON_ERROR_STOP on


-- ============================================================================
-- SOURCE: database/SmartSchoolComplete.v87.actor-aggregate.sql
-- ============================================================================
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
SELECT pg_catalog.set_config('search_path', 'public', false);
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
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    CONSTRAINT uq_school_tenant_code UNIQUE (tenant_id, code)
);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;

-- Existing campuses are intentionally left nullable during migration. Assign them to a school,
-- then make the column NOT NULL in environments containing legacy data.
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus (tenant_id, school_id);


-- SmartSchool actor aggregate refinement v87
BEGIN;

CREATE SCHEMA IF NOT EXISTS document;
CREATE SCHEMA IF NOT EXISTS student;
CREATE SCHEMA IF NOT EXISTS hr;
CREATE SCHEMA IF NOT EXISTS saas;
CREATE SCHEMA IF NOT EXISTS org;

-- Required-document policy. One policy table drives UI requirements and backend approval gates.
CREATE TABLE IF NOT EXISTS document.required_document (
    required_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NULL,
    actor_type varchar(40) NOT NULL,
    staff_type varchar(40) NULL,
    document_type varchar(60) NOT NULL,
    display_name varchar(120) NOT NULL,
    is_required boolean NOT NULL DEFAULT true,
    condition_code varchar(60) NULL,
    min_count smallint NOT NULL DEFAULT 1 CHECK (min_count > 0),
    allowed_mime_types varchar(500) NULL,
    max_size_bytes bigint NULL,
    sort_order smallint NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT uq_required_document_policy UNIQUE NULLS NOT DISTINCT
        (tenant_id, actor_type, staff_type, document_type, condition_code)
);

-- Contact data is separated from aggregate roots so actors can have multiple contact methods/addresses.
CREATE TABLE IF NOT EXISTS saas.tenant_contact (
    tenant_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL,
    contact_type smallint NOT NULL DEFAULT 1, contact_name varchar(150), email varchar(200), phone varchar(30),
    address_line1 varchar(250), address_line2 varchar(250), city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30),
    is_primary boolean NOT NULL DEFAULT false, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS student.student_contact (
    student_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL,
    contact_type varchar(30) NOT NULL, email varchar(200), phone varchar(30), address_line1 varchar(250), address_line2 varchar(250),
    city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30), is_primary boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS hr.employee_contact (
    employee_contact_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    contact_type varchar(30) NOT NULL, email varchar(200), phone varchar(30), address_line1 varchar(250), address_line2 varchar(250),
    city varchar(100), province varchar(100), country varchar(100), postal_code varchar(30), is_primary boolean NOT NULL DEFAULT false,
    is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now()
);

-- Typed document links. document.document remains the aggregate root/file metadata + binary store.
CREATE TABLE IF NOT EXISTS document.tenant_document (
    tenant_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(tenant_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.student_document (
    student_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(student_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.teacher_document (
    teacher_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, teacher_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(teacher_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.admin_officer_document (
    admin_officer_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(employee_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.staff_document (
    staff_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(employee_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.driver_document (
    driver_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, driver_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(driver_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.guardian_document (
    guardian_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, guardian_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(guardian_id, document_id)
);
CREATE TABLE IF NOT EXISTS document.campus_document (
    campus_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, campus_id uuid NOT NULL, document_id uuid NOT NULL,
    required_document_id uuid NULL, created_at timestamptz NOT NULL DEFAULT now(), UNIQUE(campus_id, document_id)
);

-- Teacher/staff evidence. Certificates are documents; these tables hold the structured facts.
CREATE TABLE IF NOT EXISTS hr.employee_education (
    employee_education_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    qualification varchar(150) NOT NULL, institute varchar(200), field_of_study varchar(150), start_date date, end_date date,
    grade varchar(50), is_highest boolean NOT NULL DEFAULT false, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS hr.employee_experience (
    employee_experience_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, employee_id uuid NOT NULL,
    employer varchar(200) NOT NULL, job_title varchar(150) NOT NULL, start_date date NOT NULL, end_date date,
    responsibilities text, created_at timestamptz NOT NULL DEFAULT now()
);

-- Normalize the student/guardian bridge; older dumps lacked an aggregate link id and tenant audit columns.
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS student_guardian_id uuid DEFAULT gen_random_uuid();
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS tenant_id uuid;
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS created_at timestamptz NOT NULL DEFAULT now();
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS row_version bigint NOT NULL DEFAULT 0;
UPDATE student.student_guardian sg SET tenant_id=s.tenant_id FROM student.student s WHERE sg.student_id=s.student_id AND sg.tenant_id IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_student_guardian_pair ON student.student_guardian(student_id,guardian_id);

-- Pending admission placement. Enrollment is created only when admission is approved.
CREATE TABLE IF NOT EXISTS student.admission_placement (
    admission_placement_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, student_id uuid NOT NULL,
    academic_year_id uuid NOT NULL, class_section_id uuid NOT NULL, requested_at timestamptz NOT NULL DEFAULT now(),
    status varchar(20) NOT NULL DEFAULT 'PENDING', approved_at timestamptz NULL,
    CONSTRAINT uq_student_pending_placement UNIQUE(student_id, academic_year_id)
);

-- Ensure enrollment has a business enrollment number and direct class/section traceability.
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS enrollment_number varchar(30);
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS class_id uuid;
CREATE UNIQUE INDEX IF NOT EXISTS ux_student_enrollment_number ON student.student_enrollment(tenant_id, enrollment_number) WHERE enrollment_number IS NOT NULL;

-- Default global policies. Tenant-specific rows can override/extend these.
INSERT INTO document.required_document(tenant_id,actor_type,staff_type,document_type,display_name,is_required,condition_code,sort_order)
VALUES
(NULL,'STUDENT',NULL,'PHOTO','Student photograph',true,NULL,10),
(NULL,'STUDENT',NULL,'BIRTH_CERTIFICATE','Birth certificate',true,NULL,20),
(NULL,'STUDENT',NULL,'CNIC_BFORM','CNIC / B-Form',true,NULL,30),
(NULL,'GUARDIAN',NULL,'CNIC','Guardian CNIC',true,NULL,10),
(NULL,'EMPLOYEE',NULL,'PHOTO','Photograph',true,NULL,10),
(NULL,'EMPLOYEE',NULL,'CNIC','CNIC / national ID',true,NULL,20),
(NULL,'EMPLOYEE','TEACHER','EDUCATION_CERTIFICATE','Education certificate',true,NULL,30),
(NULL,'EMPLOYEE','TEACHER','EXPERIENCE_CERTIFICATE','Experience certificate',true,'EXPERIENCE_PRESENT',40),
(NULL,'EMPLOYEE','DRIVER','DRIVING_LICENSE','Driving licence',true,NULL,30),
(NULL,'TENANT',NULL,'REGISTRATION_CERTIFICATE','Registration certificate',true,NULL,10),
(NULL,'CAMPUS',NULL,'LOGO','Campus / school logo',true,NULL,10)
ON CONFLICT DO NOTHING;

COMMIT;


-- ============================================================================
-- SOURCE: database/postgresql/SmartSchool-v66-CAG-Impersonation.sql
-- ============================================================================
-- SmartSchool v66 CAG + impersonation
-- CAG requires no new table: it uses HybridCache/Redis and ai_core.rag_knowledge_chunk.
-- Impersonation grant is normally synchronized by DuendeConfigurationSeeder.
-- Existing installations should restart SmartSchool.Identity.Api after deploying v66.

CREATE EXTENSION IF NOT EXISTS vector;
CREATE INDEX IF NOT EXISTS ix_rag_knowledge_chunk_tenant_collection
    ON ai_core.rag_knowledge_chunk (tenant_id, collection, is_active);


-- ============================================================================
-- SOURCE: database/postgresql/V92__organization_school_branch.sql
-- ============================================================================
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
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    CONSTRAINT uq_school_tenant_code UNIQUE (tenant_id, code)
);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;

-- Existing campuses are intentionally left nullable during migration. Assign them to a school,
-- then make the column NOT NULL in environments containing legacy data.
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus (tenant_id, school_id);


-- ============================================================================
-- SOURCE: database/postgresql/V95__organization_contact_and_branch_type.sql
-- ============================================================================
ALTER TABLE org.school
    ADD COLUMN IF NOT EXISTS fax varchar(50),
    ADD COLUMN IF NOT EXISTS province varchar(120);

ALTER TABLE org.campus
    ADD COLUMN IF NOT EXISTS branch_type varchar(40),
    ADD COLUMN IF NOT EXISTS city varchar(120),
    ADD COLUMN IF NOT EXISTS province varchar(120),
    ADD COLUMN IF NOT EXISTS fax varchar(50),
    ADD COLUMN IF NOT EXISTS mobile varchar(50),
    ADD COLUMN IF NOT EXISTS logo_url varchar(500);

UPDATE org.campus
SET branch_type = 'REGIONAL_BRANCH'
WHERE branch_type IS NULL OR btrim(branch_type) = '';

ALTER TABLE org.campus
    ALTER COLUMN branch_type SET DEFAULT 'REGIONAL_BRANCH',
    ALTER COLUMN branch_type SET NOT NULL;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'ck_campus_branch_type'
    ) THEN
        ALTER TABLE org.campus
            ADD CONSTRAINT ck_campus_branch_type
            CHECK (branch_type IN ('HEAD_OFFICE', 'REGIONAL_HEAD_OFFICE', 'REGIONAL_BRANCH'));
    END IF;
END $$;


-- ============================================================================
-- SOURCE: database/postgresql/V96__business_number_sequences.sql
-- ============================================================================
CREATE SCHEMA IF NOT EXISTS platform;

CREATE TABLE IF NOT EXISTS platform.business_number_sequence
(
    tenant_id uuid NOT NULL,
    sequence_name varchar(80) NOT NULL,
    last_value bigint NOT NULL,
    CONSTRAINT pk_business_number_sequence PRIMARY KEY (tenant_id, sequence_name)
);

ALTER TABLE student.student ALTER COLUMN student_number DROP NOT NULL;
ALTER TABLE hr.employee ALTER COLUMN employee_number DROP NOT NULL;


-- ============================================================================
-- SOURCE: database/postgresql/V97__security_numbering_document_management.sql
-- ============================================================================
-- V97: token-scoped organization ownership, business numbering and central document management
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS enrollment_number varchar(80);

CREATE SCHEMA IF NOT EXISTS document;
CREATE TABLE IF NOT EXISTS document.document (
 document_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, school_id uuid NULL, branch_id uuid NULL,
 document_number varchar(50) NOT NULL, original_file_name varchar(255) NOT NULL, stored_file_name varchar(255) NOT NULL,
 extension varchar(20), mime_type varchar(150) NOT NULL, size_bytes bigint NOT NULL, sha256 varchar(64) NOT NULL,
 storage_provider varchar(30) NOT NULL DEFAULT 'DATABASE', storage_key varchar(1000), blob_data bytea,
 category varchar(60) NOT NULL, document_type varchar(80) NOT NULL, title varchar(250), description varchar(1000),
 version_no int NOT NULL DEFAULT 1, status varchar(30) NOT NULL DEFAULT 'ACTIVE', is_confidential boolean NOT NULL DEFAULT false,
 expires_on date NULL, uploaded_by uuid NOT NULL, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz NULL,
 row_version bigint NOT NULL DEFAULT 1, UNIQUE(tenant_id, document_number)
);
CREATE TABLE IF NOT EXISTS document.document_link (
 document_link_id uuid PRIMARY KEY, tenant_id uuid NOT NULL, document_id uuid NOT NULL REFERENCES document.document(document_id),
 entity_type varchar(80) NOT NULL, entity_id uuid NOT NULL, purpose varchar(80) NOT NULL, is_primary boolean NOT NULL DEFAULT false,
 display_order int NOT NULL DEFAULT 0, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS ix_document_link_entity ON document.document_link(tenant_id, entity_type, entity_id);
CREATE INDEX IF NOT EXISTS ix_document_scope ON document.document(tenant_id, school_id, branch_id, category);

ALTER TABLE identity."AspNetUsers" ADD COLUMN IF NOT EXISTS "BranchId" uuid;

ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS branch_id uuid;


-- ============================================================================
-- SOURCE: database/postgresql/V99__admission_criteria_geo_staff_workflow.sql
-- ============================================================================
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


-- ============================================================================
-- SOURCE: database/postgresql/V100__academic_setup_admission_vertical_slice.sql
-- ============================================================================
BEGIN;
CREATE TABLE IF NOT EXISTS academic.class (class_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id), school_id uuid NOT NULL REFERENCES org.school(school_id), branch_id uuid NOT NULL REFERENCES org.campus(campus_id), code varchar(30) NOT NULL, name varchar(100) NOT NULL, sort_order integer NOT NULL DEFAULT 0, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz, row_version bytea NOT NULL DEFAULT gen_random_bytes(8), UNIQUE(branch_id,code));
ALTER TABLE academic.section ADD COLUMN IF NOT EXISTS branch_id uuid REFERENCES org.campus(campus_id);
ALTER TABLE academic.section ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS school_id uuid REFERENCES org.school(school_id);
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS branch_id uuid REFERENCES org.campus(campus_id);
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code varchar(30);
UPDATE academic.academic_year SET branch_id=campus_id WHERE branch_id IS NULL;
UPDATE academic.academic_year SET school_id=c.school_id FROM org.campus c WHERE academic.academic_year.campus_id=c.campus_id AND academic.academic_year.school_id IS NULL;
UPDATE academic.academic_year SET code=replace(name,'/','-') WHERE code IS NULL;
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS class_section_id uuid REFERENCES academic.class_section(class_section_id);
ALTER TABLE admission.admission_criteria ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
CREATE INDEX IF NOT EXISTS ix_academic_year_branch ON academic.academic_year(tenant_id,branch_id);
CREATE INDEX IF NOT EXISTS ix_class_branch ON academic.class(tenant_id,branch_id);
CREATE INDEX IF NOT EXISTS ix_section_branch_class ON academic.section(tenant_id,branch_id,class_id);
COMMIT;


-- ============================================================================
-- SOURCE: database/postgresql/V103__branch_gender_education_levels.sql
-- ============================================================================
BEGIN;

CREATE SCHEMA IF NOT EXISTS reference;

CREATE TABLE IF NOT EXISTS reference.branch_gender_type (
    branch_gender_type_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS reference.education_level (
    education_level_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);

INSERT INTO reference.branch_gender_type (branch_gender_type_id, code, name, sort_order) VALUES
('10000000-0000-0000-0000-000000000001','BOYS_ONLY','Boys Only',1),
('10000000-0000-0000-0000-000000000002','GIRLS_ONLY','Girls Only',2),
('10000000-0000-0000-0000-000000000003','CO_EDUCATION','Co-Education',3)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name, sort_order=EXCLUDED.sort_order, is_active=TRUE;

INSERT INTO reference.education_level (education_level_id, code, name, sort_order) VALUES
('20000000-0000-0000-0000-000000000001','PRE_PRIMARY','Pre-Primary',1),
('20000000-0000-0000-0000-000000000002','PRIMARY','Primary',2),
('20000000-0000-0000-0000-000000000003','MIDDLE','Middle',3),
('20000000-0000-0000-0000-000000000004','SECONDARY','Secondary',4),
('20000000-0000-0000-0000-000000000005','HIGHER_SECONDARY','Higher Secondary',5)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name, sort_order=EXCLUDED.sort_order, is_active=TRUE;

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_gender_type_id uuid;
UPDATE org.campus SET branch_gender_type_id='10000000-0000-0000-0000-000000000003' WHERE branch_gender_type_id IS NULL;
ALTER TABLE org.campus ALTER COLUMN branch_gender_type_id SET NOT NULL;
ALTER TABLE org.campus DROP CONSTRAINT IF EXISTS fk_campus_branch_gender_type;
ALTER TABLE org.campus ADD CONSTRAINT fk_campus_branch_gender_type FOREIGN KEY (branch_gender_type_id) REFERENCES reference.branch_gender_type(branch_gender_type_id);

CREATE TABLE IF NOT EXISTS org.branch_education_level (
    branch_id uuid NOT NULL REFERENCES org.campus(campus_id) ON DELETE CASCADE,
    education_level_id uuid NOT NULL REFERENCES reference.education_level(education_level_id),
    created_at timestamptz NOT NULL DEFAULT now(),
    PRIMARY KEY (branch_id, education_level_id)
);

ALTER TABLE academic.class ADD COLUMN IF NOT EXISTS education_level_id uuid;
ALTER TABLE academic.class DROP CONSTRAINT IF EXISTS fk_class_education_level;
ALTER TABLE academic.class ADD CONSTRAINT fk_class_education_level FOREIGN KEY (education_level_id) REFERENCES reference.education_level(education_level_id);
CREATE INDEX IF NOT EXISTS ix_class_branch_education_level ON academic.class(branch_id, education_level_id);

ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS gender varchar(20);
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS gender varchar(20);

COMMIT;


-- ============================================================================
-- SOURCE: database/postgresql/V114__workflow_setup_schema_alignment.sql
-- ============================================================================
BEGIN;

-- Admission workflow columns used by the current Dapper query/command slices.
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS class_id uuid REFERENCES academic.class(class_id);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS class_section_id uuid REFERENCES academic.class_section(class_section_id);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS gender varchar(30);
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS decision_notes text;
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS student_id uuid;
ALTER TABLE admission.student_application ADD COLUMN IF NOT EXISTS is_active boolean NOT NULL DEFAULT true;
CREATE INDEX IF NOT EXISTS ix_admission_application_tenant_active_submitted
    ON admission.student_application(tenant_id, is_active, submitted_at DESC);

-- Department and fee type are first-class setup masters. Existing installations are aligned idempotently.
CREATE TABLE IF NOT EXISTS org.department (
    department_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);

CREATE TABLE IF NOT EXISTS finance.fee_type (
    fee_type_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(50) NOT NULL,
    name varchar(200) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT true,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);

COMMIT;


-- ============================================================================
-- SOURCE: database/migrations/20260828_v118_department_employee_payroll.sql
-- ============================================================================
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


-- ============================================================================
-- SOURCE: database/postgresql/V119__academic_department_runtime_fix.sql
-- ============================================================================
-- SmartSchool v119 runtime schema alignment.
-- Safe to run repeatedly.
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code varchar(30);
UPDATE academic.academic_year SET branch_id = campus_id WHERE branch_id IS NULL;
UPDATE academic.academic_year ay SET school_id = c.school_id FROM org.campus c WHERE ay.campus_id = c.campus_id AND ay.school_id IS NULL;
UPDATE academic.academic_year SET code = replace(name, '/', '-') WHERE code IS NULL;

ALTER TABLE org.department ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS head_of_department_employee_id uuid;
CREATE INDEX IF NOT EXISTS ix_department_tenant_campus ON org.department(tenant_id, campus_id);


-- ============================================================================
-- SOURCE: database/postgresql/V120__remove_public_legacy_read_models.sql
-- ============================================================================
BEGIN;

-- Legacy EF read-model tables were created in public by older generated code.
-- They are not referenced by the current application and duplicate canonical module data.
DROP TABLE IF EXISTS public.driverdirectoryread;
DROP TABLE IF EXISTS public.studentdirectoryread;
DROP TABLE IF EXISTS public.teacherdirectoryread;
DROP TABLE IF EXISTS public.schooldocument;

COMMIT;


-- ============================================================================
-- SOURCE: database/postgresql/V121__consolidate_ai_schemas.sql
-- ============================================================================
BEGIN;

CREATE SCHEMA IF NOT EXISTS ai_core;
CREATE SCHEMA IF NOT EXISTS ai;
CREATE SCHEMA IF NOT EXISTS ai_tutor;

DO $$
DECLARE
    item record;
BEGIN
    FOR item IN
        SELECT schemaname, tablename
        FROM pg_tables
        WHERE schemaname IN ('ai_parent', 'ai_inquiry')
    LOOP
        EXECUTE format('ALTER TABLE %I.%I SET SCHEMA ai_core', item.schemaname, item.tablename);
    END LOOP;

    FOR item IN
        SELECT schemaname, tablename
        FROM pg_tables
        WHERE schemaname = 'ai_prediction'
    LOOP
        EXECUTE format('ALTER TABLE %I.%I SET SCHEMA ai', item.schemaname, item.tablename);
    END LOOP;
END $$;

DROP SCHEMA IF EXISTS ai_parent;
DROP SCHEMA IF EXISTS ai_inquiry;
DROP SCHEMA IF EXISTS ai_prediction;

COMMIT;


-- ============================================================================
-- SOURCE: database/migrations/20260830_ui_contract_alignment.sql
-- ============================================================================
BEGIN;

-- UI contract alignment: Organization / Campus.
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_type varchar(40);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_gender_type_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS city varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS province varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS country varchar(120);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS fax varchar(50);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS mobile varchar(50);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS logo_url varchar(500);
CREATE INDEX IF NOT EXISTS ix_campus_tenant_school ON org.campus(tenant_id, school_id);
CREATE INDEX IF NOT EXISTS ix_campus_academic_system ON org.campus(tenant_id, academic_system_id);

-- UI contract alignment: HR / Employee. Existing normalized education and experience
-- tables remain canonical; qualification/experience are not duplicated on employee.
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS branch_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS department_id uuid;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS staff_type varchar(30) DEFAULT 'OTHER';
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS date_of_birth date;
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS gender varchar(30);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS job_title varchar(150);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS alternate_phone varchar(50);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS address varchar(500);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_name varchar(200);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS emergency_contact_phone varchar(50);
CREATE INDEX IF NOT EXISTS ix_employee_tenant_branch ON hr.employee(tenant_id, branch_id);
CREATE INDEX IF NOT EXISTS ix_employee_tenant_department ON hr.employee(tenant_id, department_id);

-- UI contract alignment: Student. These columns already exist in the domain model in newer
-- source revisions but are missing from older consolidated database dumps.
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS branch_id uuid;
CREATE INDEX IF NOT EXISTS ix_student_tenant_branch ON student.student(tenant_id, branch_id);

COMMIT;


-- ============================================================================
-- SOURCE: database/migrations/20260830_ui_all_screens_alignment.sql
-- ============================================================================
BEGIN;
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS frequency varchar(30) NOT NULL DEFAULT 'Monthly';
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS description varchar(500);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;
CREATE TABLE IF NOT EXISTS finance.fee_structure (fee_structure_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, grade_level_id uuid NOT NULL, fee_type_id uuid NOT NULL, academic_year_id uuid, amount numeric(18,2) NOT NULL DEFAULT 0, frequency varchar(30) NOT NULL DEFAULT 'Monthly', effective_from date, effective_to date, code varchar(100) NOT NULL, name varchar(250) NOT NULL, metadata_json jsonb, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz, row_version bytea NOT NULL DEFAULT gen_random_bytes(8));
CREATE INDEX IF NOT EXISTS ix_fee_structure_scope ON finance.fee_structure(tenant_id,grade_level_id,academic_year_id);
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS fee_type_id uuid;
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS paid_amount numeric(18,2) NOT NULL DEFAULT 0;
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS issue_date date;
UPDATE finance.student_invoice SET issue_date=invoice_date WHERE issue_date IS NULL;
COMMIT;


-- ============================================================================
-- SOURCE: database/migrations/20260830_universal_lookup_alignment.sql
-- ============================================================================
-- SmartSchool universal lookup alignment.
-- Categorical UI/domain values are represented by saas.lookup_value IDs.

WITH wanted(code,name) AS (VALUES
 ('GENDER','Gender'),('BLOOD_GROUP','Blood Group'),('RELIGION','Religion'),('NATIONALITY','Nationality'),('RELATIONSHIP','Guardian Relationship'),
 ('LEAVE_TYPE','Leave Type'),('STAFF_TYPE','Staff Type'),('EMPLOYEE_STATUS','Employee Status'),('STUDENT_STATUS','Student Status'),
 ('ADMISSION_STATUS','Admission Status'),('INQUIRY_SOURCE','Admission Inquiry Source'),('FEE_FREQUENCY','Fee Frequency'),('PAYMENT_METHOD','Payment Method'),
 ('INVOICE_STATUS','Invoice Status'),('ROOM_TYPE','Room Type'),('DOCUMENT_CATEGORY','Document Category'),('DOCUMENT_PURPOSE','Document Purpose'),
 ('PRIORITY','Priority'),('VEHICLE_STATUS','Vehicle Status'),('DRIVER_STATUS','Driver Status'),('LIBRARY_ITEM_STATUS','Library Item Status'),
 ('LOAN_STATUS','Library Loan Status'),('AI_EXECUTION_STATUS','AI Execution Status'),('KNOWLEDGE_DOCUMENT_STATUS','Knowledge Document Status'),
 ('LIFECYCLE_STATUS','Common Lifecycle Status'),('MARITAL_STATUS','Marital Status'),('CONTACT_RELATIONSHIP','Emergency Contact Relationship')
)
INSERT INTO saas.lookup_type(code,name)
SELECT code,name FROM wanted
ON CONFLICT(code) DO UPDATE SET name=EXCLUDED.name;

WITH seed(type_code,code,name,sort_order) AS (VALUES
 ('GENDER','MALE','Male',1),('GENDER','FEMALE','Female',2),('GENDER','OTHER','Other',3),('GENDER','PREFER_NOT_TO_SAY','Prefer not to say',4),
 ('BLOOD_GROUP','A_POSITIVE','A+',1),('BLOOD_GROUP','A_NEGATIVE','A-',2),('BLOOD_GROUP','B_POSITIVE','B+',3),('BLOOD_GROUP','B_NEGATIVE','B-',4),('BLOOD_GROUP','AB_POSITIVE','AB+',5),('BLOOD_GROUP','AB_NEGATIVE','AB-',6),('BLOOD_GROUP','O_POSITIVE','O+',7),('BLOOD_GROUP','O_NEGATIVE','O-',8),
 ('RELIGION','ISLAM','Islam',1),('RELIGION','CHRISTIANITY','Christianity',2),('RELIGION','HINDUISM','Hinduism',3),('RELIGION','SIKHISM','Sikhism',4),('RELIGION','OTHER','Other',99),
 ('NATIONALITY','PAKISTANI','Pakistani',1),('NATIONALITY','SAUDI','Saudi',2),('NATIONALITY','EMIRATI','Emirati',3),('NATIONALITY','BRITISH','British',4),('NATIONALITY','OTHER','Other',99),
 ('RELATIONSHIP','FATHER','Father',1),('RELATIONSHIP','MOTHER','Mother',2),('RELATIONSHIP','GUARDIAN','Guardian',3),('RELATIONSHIP','BROTHER','Brother',4),('RELATIONSHIP','SISTER','Sister',5),('RELATIONSHIP','GRANDFATHER','Grandfather',6),('RELATIONSHIP','GRANDMOTHER','Grandmother',7),('RELATIONSHIP','OTHER','Other',99),
 ('LEAVE_TYPE','ANNUAL','Annual Leave',1),('LEAVE_TYPE','SICK','Sick Leave',2),('LEAVE_TYPE','CASUAL','Casual Leave',3),('LEAVE_TYPE','MATERNITY','Maternity Leave',4),('LEAVE_TYPE','PATERNITY','Paternity Leave',5),('LEAVE_TYPE','UNPAID','Unpaid Leave',6),
 ('STAFF_TYPE','TEACHER','Teacher',1),('STAFF_TYPE','ADMIN_OFFICER','Admin Officer',2),('STAFF_TYPE','ACCOUNTANT','Accountant',3),('STAFF_TYPE','LIBRARIAN','Librarian',4),('STAFF_TYPE','DRIVER','Driver',5),('STAFF_TYPE','SUPPORT_STAFF','Support Staff',6),('STAFF_TYPE','HEAD_OF_DEPARTMENT','Head of Department',7),
 ('FEE_FREQUENCY','MONTHLY','Monthly',1),('FEE_FREQUENCY','TERM','Term',2),('FEE_FREQUENCY','ANNUAL','Annual',3),('FEE_FREQUENCY','ONE_TIME','One Time',4),
 ('PAYMENT_METHOD','CASH','Cash',1),('PAYMENT_METHOD','BANK_TRANSFER','Bank Transfer',2),('PAYMENT_METHOD','ONLINE_PORTAL','Online Portal',3),('PAYMENT_METHOD','CHEQUE','Cheque',4),('PAYMENT_METHOD','WALLET','Wallet',5),
 ('INVOICE_STATUS','PENDING','Pending',1),('INVOICE_STATUS','PARTIAL','Partially Paid',2),('INVOICE_STATUS','PAID','Paid',3),('INVOICE_STATUS','OVERDUE','Overdue',4),('INVOICE_STATUS','CANCELLED','Cancelled',5),
 ('ADMISSION_STATUS','NEW','New',1),('ADMISSION_STATUS','UNDER_REVIEW','Under Review',2),('ADMISSION_STATUS','TEST_SCHEDULED','Test Scheduled',3),('ADMISSION_STATUS','APPROVED','Approved',4),('ADMISSION_STATUS','REJECTED','Rejected',5),('ADMISSION_STATUS','ENROLLED','Enrolled',6),('ADMISSION_STATUS','WITHDRAWN','Withdrawn',7),
 ('INQUIRY_SOURCE','WALK_IN','Walk-In',1),('INQUIRY_SOURCE','WEBSITE','Website',2),('INQUIRY_SOURCE','REFERRAL','Referral',3),('INQUIRY_SOURCE','AI_CHATBOT','AI Chatbot',4),('INQUIRY_SOURCE','SOCIAL_MEDIA','Social Media',5),('INQUIRY_SOURCE','PHONE','Phone',6),
 ('ROOM_TYPE','CLASSROOM','Classroom',1),('ROOM_TYPE','LABORATORY','Laboratory',2),('ROOM_TYPE','HALL','Hall',3),('ROOM_TYPE','LIBRARY','Library',4),('ROOM_TYPE','STAFF_ROOM','Staff Room',5),
 ('PRIORITY','LOW','Low',1),('PRIORITY','NORMAL','Normal',2),('PRIORITY','HIGH','High',3),('PRIORITY','URGENT','Urgent',4),
 ('MARITAL_STATUS','SINGLE','Single',1),('MARITAL_STATUS','MARRIED','Married',2),('MARITAL_STATUS','DIVORCED','Divorced',3),('MARITAL_STATUS','WIDOWED','Widowed',4),
 ('LIFECYCLE_STATUS','DRAFT','Draft',1),('LIFECYCLE_STATUS','SUBMITTED','Submitted',2),('LIFECYCLE_STATUS','PENDING','Pending',3),('LIFECYCLE_STATUS','ACTIVE','Active',4),('LIFECYCLE_STATUS','INACTIVE','Inactive',5),('LIFECYCLE_STATUS','APPROVED','Approved',6),('LIFECYCLE_STATUS','REJECTED','Rejected',7),('LIFECYCLE_STATUS','COMPLETED','Completed',8),('LIFECYCLE_STATUS','CANCELLED','Cancelled',9)
)
INSERT INTO saas.lookup_value(lookup_type_id,code,name,sort_order,is_active,metadata)
SELECT t.lookup_type_id,s.code,s.name,s.sort_order,TRUE,NULL
FROM seed s JOIN saas.lookup_type t ON t.code=s.type_code
ON CONFLICT(lookup_type_id,code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;

-- Canonical lookup FK columns for UI-backed categorical fields. Existing text is backfilled then removed by a later destructive migration after deployment verification.
ALTER TABLE student.student ADD COLUMN IF NOT EXISTS gender_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS gender_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS staff_type_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE hr.employee ADD COLUMN IF NOT EXISTS employment_type_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS frequency_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS payment_method_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS relationship_lookup_id bigint NULL REFERENCES saas.lookup_value(lookup_value_id);

UPDATE student.student s SET gender_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='GENDER' WHERE s.gender_lookup_id IS NULL AND upper(replace(coalesce(s.gender,''),' ','_'))=v.code;
UPDATE hr.employee e SET gender_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='GENDER' WHERE e.gender_lookup_id IS NULL AND upper(replace(coalesce(e.gender,''),' ','_'))=v.code;
UPDATE finance.fee_type f SET frequency_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='FEE_FREQUENCY' WHERE f.frequency_lookup_id IS NULL AND upper(replace(coalesce(f.frequency,''),' ','_'))=v.code;
UPDATE finance.student_payment p SET payment_method_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='PAYMENT_METHOD' WHERE p.payment_method_lookup_id IS NULL AND upper(replace(coalesce(p.payment_method,''),' ','_'))=v.code;
UPDATE student.student_guardian g SET relationship_lookup_id=v.lookup_value_id FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id AND t.code='RELATIONSHIP' WHERE g.relationship_lookup_id IS NULL AND upper(replace(coalesce(g.relationship,''),' ','_'))=v.code;



-- UI read-model alignment (2026-09-02)
BEGIN;
ALTER TABLE academic.academic_system ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.program ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.term ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.prediction_model ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.knowledge_collection ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS conversation_participant_id uuid;
ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS message_receipt_id uuid;
ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS action_url character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS is_read boolean;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS message character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS occurred_at timestamp with time zone;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS priority character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS read_at timestamp with time zone;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS recipient_user_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_id uuid;
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_type character varying(500);
ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS type character varying(500);
ALTER TABLE document.document_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS description character varying(500);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS frequency character varying(500);
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE hr.job_grade ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE inventory.item ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_gender_type_id uuid;
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_type character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS city character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS country character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS fax character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS logo_url character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS mobile character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS province character varying(500);
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS school_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS email character varying(500);
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS head_of_department_employee_id uuid;
ALTER TABLE org.department ADD COLUMN IF NOT EXISTS telephone character varying(500);
ALTER TABLE org.school ADD COLUMN IF NOT EXISTS fax character varying(500);
ALTER TABLE org.school ADD COLUMN IF NOT EXISTS province character varying(500);
ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS name character varying(500);
ALTER TABLE transport.route ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS code character varying(500);
ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS name character varying(500);

-- ============================================================================
-- SOURCE: database/20260902_runtime_log_errors_fix.sql
-- ============================================================================
-- SmartSchool runtime fixes derived from 2026-09-02 API error log.
-- Idempotent and safe to re-run.
BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE SCHEMA IF NOT EXISTS reference;
CREATE SCHEMA IF NOT EXISTS workflow;

-- Organization branch-policy lookups used by BranchPolicyQuery.
CREATE TABLE IF NOT EXISTS reference.branch_gender_type (
    branch_gender_type_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);
CREATE TABLE IF NOT EXISTS reference.education_level (
    education_level_id uuid PRIMARY KEY,
    code varchar(40) NOT NULL UNIQUE,
    name varchar(100) NOT NULL,
    sort_order integer NOT NULL DEFAULT 0,
    is_active boolean NOT NULL DEFAULT TRUE
);
INSERT INTO reference.branch_gender_type(branch_gender_type_id,code,name,sort_order,is_active) VALUES
('10000000-0000-0000-0000-000000000001','BOYS_ONLY','Boys Only',1,TRUE),
('10000000-0000-0000-0000-000000000002','GIRLS_ONLY','Girls Only',2,TRUE),
('10000000-0000-0000-0000-000000000003','CO_EDUCATION','Co-Education',3,TRUE)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;
INSERT INTO reference.education_level(education_level_id,code,name,sort_order,is_active) VALUES
('20000000-0000-0000-0000-000000000001','PRE_PRIMARY','Pre-Primary',1,TRUE),
('20000000-0000-0000-0000-000000000002','PRIMARY','Primary',2,TRUE),
('20000000-0000-0000-0000-000000000003','MIDDLE','Middle',3,TRUE),
('20000000-0000-0000-0000-000000000004','SECONDARY','Secondary',4,TRUE),
('20000000-0000-0000-0000-000000000005','HIGHER_SECONDARY','Higher Secondary',5,TRUE)
ON CONFLICT (code) DO UPDATE SET name=EXCLUDED.name,sort_order=EXCLUDED.sort_order,is_active=TRUE;

-- Workflow Vertical Slice queries use these canonical lowercase PostgreSQL names.
CREATE TABLE IF NOT EXISTS workflow.workflowdefinition (
    workflow_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.workflowinstance (
    workflow_instance_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.workflowstep (
    workflow_step_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE TABLE IF NOT EXISTS workflow.approval (
    approval_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    tenant_id uuid NOT NULL REFERENCES saas.tenant(tenant_id),
    code varchar(100) NOT NULL,
    name varchar(250) NOT NULL,
    metadata_json text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamptz NOT NULL DEFAULT now(),
    updated_at timestamptz,
    row_version bytea NOT NULL DEFAULT gen_random_bytes(8),
    UNIQUE(tenant_id, code)
);
CREATE INDEX IF NOT EXISTS ix_workflowdefinition_tenant_active ON workflow.workflowdefinition(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_workflowinstance_tenant_active ON workflow.workflowinstance(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_workflowstep_tenant_active ON workflow.workflowstep(tenant_id,is_active);
CREATE INDEX IF NOT EXISTS ix_approval_tenant_active ON workflow.approval(tenant_id,is_active);

COMMIT;



-- 2026-09-06: runtime schema contract alignment. Idempotent and safe on existing databases.
-- Keeps PostgreSQL schema synchronized with EF/Dapper contracts to prevent 42703 failures.
CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE IF EXISTS academic.academic_system ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.academic_year ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.course_offering ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.program ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.term ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.timetable_entry ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.student_activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS activity.student_award ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.class_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_evaluation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_evidence ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.prediction_model ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.student_intervention ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.student_performance_prediction ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.teaching_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai.topic_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.human_handoff ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.inquiry_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.inquiry_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_collection ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.lead_capture ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.parent_tool_execution ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.prompt_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_core.tool_definition ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS audit.audit_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.conversation_participant ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.message ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS communication.message_receipt ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS document.document_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS document.generated_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.exam ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.exam_subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.fee_structure ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.candidate ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.interview ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.job ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.job_grade ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS hr.position ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS inventory.item ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS lms.academic_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS lms.student_assignment_submission ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS org.department ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS saas.school_branding ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS saas.tenant ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS student.student_course_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS transport.route ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS transport.vehicle ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.approval ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowdefinition ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowinstance ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS workflow.workflowstep ADD COLUMN IF NOT EXISTS metadata_json jsonb;

-- Finance contract used by FeeTypeEntity.
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS frequency varchar(30) NOT NULL DEFAULT 'Monthly';
ALTER TABLE IF EXISTS finance.fee_type ADD COLUMN IF NOT EXISTS description varchar(500);

-- Campus-owned grade hierarchy.
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE IF EXISTS academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
CREATE INDEX IF NOT EXISTS ix_grade_level_tenant_campus ON academic.grade_level(tenant_id, campus_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_grade_level_tenant_campus_code ON academic.grade_level(tenant_id, campus_id, code) WHERE campus_id IS NOT NULL;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS grade_level_id uuid;
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE IF EXISTS academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE IF EXISTS academic.class_section ALTER COLUMN program_grade_id DROP NOT NULL;
CREATE INDEX IF NOT EXISTS ix_class_section_tenant_grade ON academic.class_section(tenant_id, grade_level_id);

DO $$ BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_grade_level_campus') THEN
    ALTER TABLE academic.grade_level ADD CONSTRAINT fk_grade_level_campus FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id) ON DELETE CASCADE;
  END IF;
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_grade_level_academic_system') THEN
    ALTER TABLE academic.grade_level ADD CONSTRAINT fk_grade_level_academic_system FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id) ON DELETE RESTRICT;
  END IF;
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_class_section_grade_level') THEN
    ALTER TABLE academic.class_section ADD CONSTRAINT fk_class_section_grade_level FOREIGN KEY (grade_level_id) REFERENCES academic.grade_level(grade_level_id) ON DELETE RESTRICT;
  END IF;
END $$;

-- RAG table indexes used by chatbot retrieval.
CREATE INDEX IF NOT EXISTS ix_rag_knowledge_chunk_tenant_collection ON ai_core.rag_knowledge_chunk(tenant_id, collection) WHERE is_active = true;


-- 20260908 subject ownership and HR teaching assignments
BEGIN;
ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS department_id uuid;
UPDATE academic.subject s SET department_id = d.department_id FROM org.department d WHERE s.department_id IS NULL AND d.tenant_id=s.tenant_id AND d.campus_id=s.branch_id;
ALTER TABLE academic.subject DROP COLUMN IF EXISTS branch_id;
DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='subject_department_id_fkey') THEN ALTER TABLE academic.subject ADD CONSTRAINT subject_department_id_fkey FOREIGN KEY (department_id) REFERENCES org.department(department_id); END IF; END $$;
CREATE INDEX IF NOT EXISTS ix_subject_tenant_department ON academic.subject(tenant_id, department_id);

CREATE TABLE IF NOT EXISTS hr.teacher_teaching_assignment (
 teacher_teaching_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(), tenant_id uuid NOT NULL, school_id uuid NOT NULL, campus_id uuid NOT NULL, employee_id uuid NOT NULL, class_section_id uuid NOT NULL, subject_id uuid NOT NULL, code varchar(50) NOT NULL, name varchar(250) NOT NULL, periods_per_week integer, is_class_teacher boolean NOT NULL DEFAULT false, effective_from date, effective_to date, is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz, row_version bytea NOT NULL DEFAULT public.gen_random_bytes(8), CONSTRAINT uq_teacher_teaching_assignment_code UNIQUE(tenant_id,code));
CREATE INDEX IF NOT EXISTS ix_teacher_teaching_assignment_employee ON hr.teacher_teaching_assignment(tenant_id, employee_id);
CREATE INDEX IF NOT EXISTS ix_teacher_teaching_assignment_campus ON hr.teacher_teaching_assignment(tenant_id, campus_id);
COMMIT;

-- ============================================================================
-- SOURCE: database/postgresql/20260908_domain_schema_sync_department_grading.sql
-- Applicant/Discount/StudentOfMonth + Department-owned fees + Campus grading.
-- ============================================================================
\i database/postgresql/20260908_domain_schema_sync_department_grading.sql
