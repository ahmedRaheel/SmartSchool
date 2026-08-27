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
-- TOC entry 12 (class 2615 OID 16431)
-- Name: academic; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA academic;


ALTER SCHEMA academic OWNER TO postgres;

--
-- TOC entry 23 (class 2615 OID 16442)
-- Name: activity; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA activity;


ALTER SCHEMA activity OWNER TO postgres;

--
-- TOC entry 14 (class 2615 OID 16433)
-- Name: admission; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA admission;


ALTER SCHEMA admission OWNER TO postgres;

--
-- TOC entry 27 (class 2615 OID 16446)
-- Name: ai; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA ai;


ALTER SCHEMA ai OWNER TO postgres;

--
-- TOC entry 28 (class 2615 OID 16447)
-- Name: ai_core; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA ai_core;


ALTER SCHEMA ai_core OWNER TO postgres;

--
-- TOC entry 30 (class 2615 OID 16449)
-- Name: ai_inquiry; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA ai_inquiry;


ALTER SCHEMA ai_inquiry OWNER TO postgres;

--
-- TOC entry 31 (class 2615 OID 16450)
-- Name: ai_parent; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA ai_parent;


ALTER SCHEMA ai_parent OWNER TO postgres;

--
-- TOC entry 29 (class 2615 OID 16448)
-- Name: ai_tutor; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA ai_tutor;


ALTER SCHEMA ai_tutor OWNER TO postgres;

--
-- TOC entry 32 (class 2615 OID 16451)
-- Name: audit; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA audit;


ALTER SCHEMA audit OWNER TO postgres;

--
-- TOC entry 21 (class 2615 OID 16440)
-- Name: communication; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA communication;


ALTER SCHEMA communication OWNER TO postgres;

--
-- TOC entry 20 (class 2615 OID 16439)
-- Name: document; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA document;


ALTER SCHEMA document OWNER TO postgres;

--
-- TOC entry 16 (class 2615 OID 16435)
-- Name: exam; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA exam;


ALTER SCHEMA exam OWNER TO postgres;

--
-- TOC entry 17 (class 2615 OID 16436)
-- Name: finance; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA finance;


ALTER SCHEMA finance OWNER TO postgres;

--
-- TOC entry 34 (class 2615 OID 22760)
-- Name: hangfire; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA hangfire;


ALTER SCHEMA hangfire OWNER TO postgres;

--
-- TOC entry 18 (class 2615 OID 16437)
-- Name: hr; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA hr;


ALTER SCHEMA hr OWNER TO postgres;

--
-- TOC entry 10 (class 2615 OID 16429)
-- Name: infrastructure; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA infrastructure;


ALTER SCHEMA infrastructure OWNER TO postgres;

--
-- TOC entry 26 (class 2615 OID 16445)
-- Name: inventory; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA inventory;


ALTER SCHEMA inventory OWNER TO postgres;

--
-- TOC entry 25 (class 2615 OID 16444)
-- Name: library; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA library;


ALTER SCHEMA library OWNER TO postgres;

--
-- TOC entry 15 (class 2615 OID 16434)
-- Name: lms; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA lms;


ALTER SCHEMA lms OWNER TO postgres;

--
-- TOC entry 35 (class 2615 OID 23200)
-- Name: observability; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA observability;


ALTER SCHEMA observability OWNER TO postgres;

--
-- TOC entry 11 (class 2615 OID 16430)
-- Name: org; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA org;


ALTER SCHEMA org OWNER TO postgres;

--
-- TOC entry 19 (class 2615 OID 16438)
-- Name: payroll; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA payroll;


ALTER SCHEMA payroll OWNER TO postgres;

--
-- TOC entry 6 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 6433 (class 0 OID 0)
-- Dependencies: 6
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 9 (class 2615 OID 16428)
-- Name: reference; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA reference;


ALTER SCHEMA reference OWNER TO postgres;

--
-- TOC entry 8 (class 2615 OID 16427)
-- Name: saas; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA saas;


ALTER SCHEMA saas OWNER TO postgres;

--
-- TOC entry 13 (class 2615 OID 16432)
-- Name: student; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA student;


ALTER SCHEMA student OWNER TO postgres;

--
-- TOC entry 36 (class 2615 OID 23325)
-- Name: teacher; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA teacher;


ALTER SCHEMA teacher OWNER TO postgres;

--
-- TOC entry 24 (class 2615 OID 16443)
-- Name: transport; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA transport;


ALTER SCHEMA transport OWNER TO postgres;

--
-- TOC entry 22 (class 2615 OID 16441)
-- Name: workflow; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA workflow;


ALTER SCHEMA workflow OWNER TO postgres;

--
-- TOC entry 679 (class 1255 OID 21892)
-- Name: smartschool_set_entity_update_fields(); Type: FUNCTION; Schema: infrastructure; Owner: postgres
--

CREATE FUNCTION infrastructure.smartschool_set_entity_update_fields() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.updated_at = now();
    NEW.row_version = gen_random_bytes(8);
    RETURN NEW;
END;
$$;


ALTER FUNCTION infrastructure.smartschool_set_entity_update_fields() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 259 (class 1259 OID 16586)
-- Name: academic_system; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.academic_system (
    academic_system_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(150) NOT NULL,
    system_type_code character varying(40) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.academic_system OWNER TO postgres;

--
-- TOC entry 269 (class 1259 OID 16822)
-- Name: academic_year; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.academic_year (
    academic_year_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    name character varying(80) NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    is_current boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.academic_year OWNER TO postgres;

--
-- TOC entry 262 (class 1259 OID 16650)
-- Name: campus_program; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.campus_program (
    campus_program_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    program_id uuid NOT NULL,
    effective_from date,
    effective_to date,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.campus_program OWNER TO postgres;

--
-- TOC entry 272 (class 1259 OID 16890)
-- Name: class_section; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.class_section (
    class_section_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    program_grade_id uuid NOT NULL,
    section_id uuid NOT NULL,
    class_teacher_employee_id uuid,
    room_id uuid,
    capacity integer,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.class_section OWNER TO postgres;

--
-- TOC entry 273 (class 1259 OID 16936)
-- Name: course_offering; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.course_offering (
    course_offering_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    program_subject_id uuid NOT NULL,
    display_name character varying(150),
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.course_offering OWNER TO postgres;

--
-- TOC entry 267 (class 1259 OID 16780)
-- Name: course_selection_group; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.course_selection_group (
    selection_group_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    program_grade_id uuid NOT NULL,
    name character varying(150) NOT NULL,
    min_selections integer DEFAULT 0 NOT NULL,
    max_selections integer NOT NULL,
    requires_approval boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.course_selection_group OWNER TO postgres;

--
-- TOC entry 268 (class 1259 OID 16805)
-- Name: course_selection_group_course; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.course_selection_group_course (
    selection_group_id uuid NOT NULL,
    program_subject_id uuid NOT NULL
);


ALTER TABLE academic.course_selection_group_course OWNER TO postgres;

--
-- TOC entry 260 (class 1259 OID 16606)
-- Name: education_board; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.education_board (
    education_board_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(200) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.education_board OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 16679)
-- Name: grade_level; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.grade_level (
    grade_level_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    sort_order integer DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.grade_level OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 16623)
-- Name: program; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.program (
    program_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    academic_system_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(150) NOT NULL,
    description text,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.program OWNER TO postgres;

--
-- TOC entry 264 (class 1259 OID 16698)
-- Name: program_grade; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.program_grade (
    program_grade_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    program_id uuid NOT NULL,
    grade_level_id uuid NOT NULL,
    sort_order integer DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.program_grade OWNER TO postgres;

--
-- TOC entry 266 (class 1259 OID 16748)
-- Name: program_subject; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.program_subject (
    program_subject_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    program_grade_id uuid NOT NULL,
    subject_id uuid NOT NULL,
    requirement_type_code character varying(30) NOT NULL,
    periods_per_week integer,
    minimum_pass_marks numeric(7,2),
    display_order integer DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.program_subject OWNER TO postgres;

--
-- TOC entry 271 (class 1259 OID 16873)
-- Name: section; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.section (
    section_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(30) NOT NULL,
    name character varying(80) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.section OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 16727)
-- Name: subject; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.subject (
    subject_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(150) NOT NULL,
    short_name character varying(50),
    is_practical boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.subject OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 17306)
-- Name: teacher_course_assignment; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.teacher_course_assignment (
    teacher_course_assignment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    class_section_id uuid,
    teaching_group_id uuid,
    assignment_role character varying(40) DEFAULT 'PRIMARY'::character varying NOT NULL,
    periods_per_week integer,
    effective_from date,
    effective_to date,
    is_primary boolean DEFAULT true NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.teacher_course_assignment OWNER TO postgres;

--
-- TOC entry 294 (class 1259 OID 17496)
-- Name: teaching_group; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.teaching_group (
    teaching_group_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    course_offering_id uuid NOT NULL,
    name character varying(150) NOT NULL,
    capacity integer,
    room_id uuid,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.teaching_group OWNER TO postgres;

--
-- TOC entry 295 (class 1259 OID 17539)
-- Name: teaching_group_student; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.teaching_group_student (
    teaching_group_id uuid NOT NULL,
    student_course_enrollment_id uuid NOT NULL
);


ALTER TABLE academic.teaching_group_student OWNER TO postgres;

--
-- TOC entry 270 (class 1259 OID 16848)
-- Name: term; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.term (
    term_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    code character varying(40) NOT NULL,
    name character varying(100) NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.term OWNER TO postgres;

--
-- TOC entry 297 (class 1259 OID 17580)
-- Name: timetable; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.timetable (
    timetable_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    name character varying(150) NOT NULL,
    effective_from date,
    effective_to date,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.timetable OWNER TO postgres;

--
-- TOC entry 298 (class 1259 OID 17613)
-- Name: timetable_entry; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.timetable_entry (
    timetable_entry_id uuid DEFAULT gen_random_uuid() NOT NULL,
    timetable_id uuid NOT NULL,
    day_of_week smallint NOT NULL,
    timetable_period_id uuid NOT NULL,
    class_section_id uuid,
    teaching_group_id uuid,
    course_offering_id uuid,
    teacher_course_assignment_id uuid,
    room_id uuid,
    entry_type character varying(30) DEFAULT 'SUBJECT'::character varying NOT NULL,
    CONSTRAINT timetable_entry_check CHECK (((class_section_id IS NOT NULL) OR (teaching_group_id IS NOT NULL))),
    CONSTRAINT timetable_entry_day_of_week_check CHECK (((day_of_week >= 1) AND (day_of_week <= 7)))
);


ALTER TABLE academic.timetable_entry OWNER TO postgres;

--
-- TOC entry 296 (class 1259 OID 17556)
-- Name: timetable_period; Type: TABLE; Schema: academic; Owner: postgres
--

CREATE TABLE academic.timetable_period (
    timetable_period_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    period_number integer,
    name character varying(80) NOT NULL,
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    period_type character varying(30) DEFAULT 'SUBJECT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE academic.timetable_period OWNER TO postgres;

--
-- TOC entry 322 (class 1259 OID 18295)
-- Name: activity; Type: TABLE; Schema: activity; Owner: postgres
--

CREATE TABLE activity.activity (
    activity_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    name character varying(180) NOT NULL,
    category character varying(100),
    coordinator_employee_id uuid,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE activity.activity OWNER TO postgres;

--
-- TOC entry 323 (class 1259 OID 18321)
-- Name: student_activity; Type: TABLE; Schema: activity; Owner: postgres
--

CREATE TABLE activity.student_activity (
    activity_id uuid NOT NULL,
    student_id uuid NOT NULL,
    role_name character varying(100),
    joined_at date,
    left_at date
);


ALTER TABLE activity.student_activity OWNER TO postgres;

--
-- TOC entry 324 (class 1259 OID 18338)
-- Name: student_award; Type: TABLE; Schema: activity; Owner: postgres
--

CREATE TABLE activity.student_award (
    student_award_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    award_type_code character varying(50) NOT NULL,
    title character varying(180) NOT NULL,
    description text,
    award_date date NOT NULL,
    approved_by uuid,
    generated_document_id uuid,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE activity.student_award OWNER TO postgres;

--
-- TOC entry 370 (class 1259 OID 19481)
-- Name: class_performance_insight; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.class_performance_insight (
    class_performance_insight_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    class_section_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    teacher_employee_id uuid,
    students_count integer DEFAULT 0 NOT NULL,
    on_track_count integer DEFAULT 0 NOT NULL,
    needs_attention_count integer DEFAULT 0 NOT NULL,
    high_risk_count integer DEFAULT 0 NOT NULL,
    predicted_class_average numeric(7,3),
    current_class_average numeric(7,3),
    trend character varying(30),
    summary text,
    generated_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai.class_performance_insight OWNER TO postgres;

--
-- TOC entry 374 (class 1259 OID 19663)
-- Name: intervention_action; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.intervention_action (
    intervention_action_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_intervention_id uuid NOT NULL,
    sequence_no integer NOT NULL,
    action_type character varying(60) NOT NULL,
    description text NOT NULL,
    related_entity_type character varying(100),
    related_entity_id uuid,
    due_at timestamp with time zone,
    completed_at timestamp with time zone,
    status character varying(30) DEFAULT 'PENDING'::character varying NOT NULL
);


ALTER TABLE ai.intervention_action OWNER TO postgres;

--
-- TOC entry 375 (class 1259 OID 19685)
-- Name: intervention_outcome; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.intervention_outcome (
    intervention_outcome_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_intervention_id uuid NOT NULL,
    measured_at timestamp with time zone DEFAULT now() NOT NULL,
    before_score numeric(7,3),
    after_score numeric(7,3),
    improvement numeric(7,3),
    outcome_status character varying(30),
    teacher_notes text
);


ALTER TABLE ai.intervention_outcome OWNER TO postgres;

--
-- TOC entry 369 (class 1259 OID 19463)
-- Name: predicted_grade_probability; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.predicted_grade_probability (
    predicted_grade_probability_id uuid DEFAULT gen_random_uuid() CONSTRAINT predicted_grade_probability_predicted_grade_probabilit_not_null NOT NULL,
    student_performance_prediction_id uuid CONSTRAINT predicted_grade_probability_student_performance_predic_not_null NOT NULL,
    grade character varying(20) NOT NULL,
    probability numeric(7,4) NOT NULL,
    CONSTRAINT predicted_grade_probability_probability_check CHECK (((probability >= (0)::numeric) AND (probability <= (1)::numeric)))
);


ALTER TABLE ai.predicted_grade_probability OWNER TO postgres;

--
-- TOC entry 366 (class 1259 OID 19347)
-- Name: prediction; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.prediction (
    prediction_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    prediction_model_id uuid NOT NULL,
    student_id uuid,
    prediction_type character varying(80) NOT NULL,
    score numeric(10,6),
    risk_level character varying(30),
    explanation jsonb,
    predicted_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai.prediction OWNER TO postgres;

--
-- TOC entry 376 (class 1259 OID 19702)
-- Name: prediction_evaluation; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.prediction_evaluation (
    prediction_evaluation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_performance_prediction_id uuid CONSTRAINT prediction_evaluation_student_performance_prediction_i_not_null NOT NULL,
    student_exam_result_id uuid NOT NULL,
    predicted_percentage numeric(7,3),
    actual_percentage numeric(7,3),
    absolute_error numeric(7,3),
    predicted_grade character varying(20),
    actual_grade character varying(20),
    grade_correct boolean,
    evaluated_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE ai.prediction_evaluation OWNER TO postgres;

--
-- TOC entry 368 (class 1259 OID 19447)
-- Name: prediction_evidence; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.prediction_evidence (
    prediction_evidence_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_performance_prediction_id uuid NOT NULL,
    evidence_type character varying(60) NOT NULL,
    source_entity_type character varying(100),
    source_entity_id uuid,
    numeric_value numeric(18,6),
    text_value text,
    normalized_value numeric(10,6),
    weight numeric(10,6),
    occurred_at timestamp with time zone,
    explanation text
);


ALTER TABLE ai.prediction_evidence OWNER TO postgres;

--
-- TOC entry 365 (class 1259 OID 19330)
-- Name: prediction_model; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.prediction_model (
    prediction_model_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid,
    code character varying(80) NOT NULL,
    name character varying(180) NOT NULL,
    prediction_type character varying(80) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai.prediction_model OWNER TO postgres;

--
-- TOC entry 373 (class 1259 OID 19612)
-- Name: student_intervention; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.student_intervention (
    student_intervention_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    subject_id uuid,
    course_offering_id uuid,
    teacher_employee_id uuid,
    source_prediction_id uuid,
    source_recommendation_id uuid,
    title character varying(250) NOT NULL,
    reason text,
    target_outcome text,
    start_date date,
    target_date date,
    status character varying(30) DEFAULT 'PLANNED'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai.student_intervention OWNER TO postgres;

--
-- TOC entry 367 (class 1259 OID 19382)
-- Name: student_performance_prediction; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.student_performance_prediction (
    student_performance_prediction_id uuid DEFAULT gen_random_uuid() CONSTRAINT student_performance_predict_student_performance_predic_not_null NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    course_offering_id uuid NOT NULL,
    subject_id uuid NOT NULL,
    target_exam_id uuid,
    target_exam_subject_id uuid,
    target_exam_type_code character varying(40),
    target_date date,
    predicted_marks numeric(8,2),
    predicted_percentage numeric(7,3),
    predicted_grade character varying(20),
    lower_bound_percentage numeric(7,3),
    upper_bound_percentage numeric(7,3),
    confidence_score numeric(7,4),
    pass_probability numeric(7,4),
    fail_probability numeric(7,4),
    target_grade character varying(20),
    target_grade_probability numeric(7,4),
    trend character varying(30),
    risk_level character varying(30),
    explanation_summary text,
    explanation jsonb,
    prediction_model_id uuid,
    model_version character varying(80),
    generated_at timestamp with time zone DEFAULT now() NOT NULL,
    expires_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT student_performance_prediction_confidence_score_check CHECK (((confidence_score IS NULL) OR ((confidence_score >= (0)::numeric) AND (confidence_score <= (1)::numeric)))),
    CONSTRAINT student_performance_prediction_fail_probability_check CHECK (((fail_probability IS NULL) OR ((fail_probability >= (0)::numeric) AND (fail_probability <= (1)::numeric)))),
    CONSTRAINT student_performance_prediction_pass_probability_check CHECK (((pass_probability IS NULL) OR ((pass_probability >= (0)::numeric) AND (pass_probability <= (1)::numeric)))),
    CONSTRAINT student_performance_prediction_target_grade_probability_check CHECK (((target_grade_probability IS NULL) OR ((target_grade_probability >= (0)::numeric) AND (target_grade_probability <= (1)::numeric))))
);


ALTER TABLE ai.student_performance_prediction OWNER TO postgres;

--
-- TOC entry 377 (class 1259 OID 19725)
-- Name: student_progress_recommendation; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.student_progress_recommendation (
    student_progress_recommendation_id uuid DEFAULT gen_random_uuid() CONSTRAINT student_progress_recommenda_student_progress_recommend_not_null NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    prediction_id uuid,
    audience character varying(20) NOT NULL,
    title character varying(250) NOT NULL,
    recommendation_text text NOT NULL,
    priority character varying(30) DEFAULT 'NORMAL'::character varying NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    generated_at timestamp with time zone DEFAULT now() NOT NULL,
    expires_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT student_progress_recommendation_audience_check CHECK (((audience)::text = ANY ((ARRAY['STUDENT'::character varying, 'PARENT'::character varying, 'TEACHER'::character varying])::text[])))
);


ALTER TABLE ai.student_progress_recommendation OWNER TO postgres;

--
-- TOC entry 372 (class 1259 OID 19560)
-- Name: teaching_recommendation; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.teaching_recommendation (
    teaching_recommendation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    class_performance_insight_id uuid,
    class_section_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    teacher_employee_id uuid NOT NULL,
    subject_id uuid,
    topic character varying(250),
    recommendation_type character varying(60) NOT NULL,
    title character varying(250) NOT NULL,
    recommendation_text text NOT NULL,
    rationale text,
    priority character varying(30) DEFAULT 'NORMAL'::character varying NOT NULL,
    status character varying(30) DEFAULT 'PROPOSED'::character varying NOT NULL,
    generated_at timestamp with time zone DEFAULT now() NOT NULL,
    reviewed_at timestamp with time zone,
    reviewed_by uuid,
    teacher_comments text,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai.teaching_recommendation OWNER TO postgres;

--
-- TOC entry 371 (class 1259 OID 19534)
-- Name: topic_performance_insight; Type: TABLE; Schema: ai; Owner: postgres
--

CREATE TABLE ai.topic_performance_insight (
    topic_performance_insight_id uuid DEFAULT gen_random_uuid() NOT NULL,
    class_performance_insight_id uuid NOT NULL,
    subject_id uuid NOT NULL,
    topic character varying(250) NOT NULL,
    average_mastery_score numeric(7,4),
    students_struggling_count integer DEFAULT 0 NOT NULL,
    students_mastered_count integer DEFAULT 0 NOT NULL,
    risk_level character varying(30),
    recommended_focus text
);


ALTER TABLE ai.topic_performance_insight OWNER TO postgres;

--
-- TOC entry 413 (class 1259 OID 20792)
-- Name: RagKnowledgeChunks; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core."RagKnowledgeChunks" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "DocumentId" uuid NOT NULL,
    "ChunkIndex" integer NOT NULL,
    "Content" text NOT NULL,
    "CitationLabel" character varying(500) NOT NULL,
    "Embedding" public.vector(768)
);


ALTER TABLE ai_core."RagKnowledgeChunks" OWNER TO postgres;

--
-- TOC entry 412 (class 1259 OID 20778)
-- Name: RagKnowledgeDocuments; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core."RagKnowledgeDocuments" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "CollectionId" uuid,
    "Title" character varying(300) NOT NULL,
    "SourceName" character varying(500) NOT NULL,
    "Audience" character varying(500),
    "ContentHash" character varying(128) NOT NULL,
    "IsApproved" boolean DEFAULT false NOT NULL,
    "IndexedAt" timestamp with time zone
);


ALTER TABLE ai_core."RagKnowledgeDocuments" OWNER TO postgres;

--
-- TOC entry 346 (class 1259 OID 18858)
-- Name: ai_execution_log; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.ai_execution_log (
    ai_execution_log_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    assistant_type character varying(50) NOT NULL,
    conversation_reference_id uuid,
    user_id uuid,
    model_configuration_id uuid,
    prompt_tokens integer,
    completion_tokens integer,
    total_tokens integer,
    estimated_cost numeric(18,8),
    latency_ms integer,
    status character varying(30) NOT NULL,
    correlation_id character varying(100),
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.ai_execution_log OWNER TO postgres;

--
-- TOC entry 345 (class 1259 OID 18836)
-- Name: assistant_knowledge_collection; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.assistant_knowledge_collection (
    assistant_knowledge_collection_id uuid DEFAULT gen_random_uuid() CONSTRAINT assistant_knowledge_collect_assistant_knowledge_collec_not_null NOT NULL,
    tenant_id uuid NOT NULL,
    assistant_type character varying(50) NOT NULL,
    knowledge_collection_id uuid NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.assistant_knowledge_collection OWNER TO postgres;

--
-- TOC entry 344 (class 1259 OID 18813)
-- Name: assistant_tool; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.assistant_tool (
    assistant_tool_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid,
    assistant_type character varying(50) NOT NULL,
    tool_definition_id uuid NOT NULL,
    is_enabled boolean DEFAULT true NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.assistant_tool OWNER TO postgres;

--
-- TOC entry 342 (class 1259 OID 18774)
-- Name: knowledge_chunk; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.knowledge_chunk (
    knowledge_chunk_id uuid DEFAULT gen_random_uuid() NOT NULL,
    knowledge_document_id uuid NOT NULL,
    chunk_index integer NOT NULL,
    content text NOT NULL,
    metadata jsonb,
    embedding_reference character varying(250),
    embedding public.vector(768)
);


ALTER TABLE ai_core.knowledge_chunk OWNER TO postgres;

--
-- TOC entry 340 (class 1259 OID 18719)
-- Name: knowledge_collection; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.knowledge_collection (
    knowledge_collection_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(80) NOT NULL,
    name character varying(150) NOT NULL,
    description text,
    access_scope character varying(50) DEFAULT 'TENANT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.knowledge_collection OWNER TO postgres;

--
-- TOC entry 341 (class 1259 OID 18740)
-- Name: knowledge_document; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.knowledge_document (
    knowledge_document_id uuid DEFAULT gen_random_uuid() NOT NULL,
    knowledge_collection_id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    academic_system_id uuid,
    title character varying(250) NOT NULL,
    document_type character varying(80),
    source_url text,
    metadata jsonb,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.knowledge_document OWNER TO postgres;

--
-- TOC entry 338 (class 1259 OID 18674)
-- Name: model_configuration; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.model_configuration (
    model_configuration_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid,
    code character varying(80) NOT NULL,
    provider character varying(80) NOT NULL,
    model_name character varying(150) NOT NULL,
    configuration jsonb,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.model_configuration OWNER TO postgres;

--
-- TOC entry 339 (class 1259 OID 18695)
-- Name: prompt_template; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.prompt_template (
    prompt_template_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid,
    assistant_type character varying(50) NOT NULL,
    prompt_type character varying(30) NOT NULL,
    code character varying(100) NOT NULL,
    prompt_text text NOT NULL,
    version integer DEFAULT 1 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_core.prompt_template OWNER TO postgres;

--
-- TOC entry 527 (class 1259 OID 23305)
-- Name: rag_knowledge_chunk; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.rag_knowledge_chunk (
    id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    collection character varying(80) NOT NULL,
    document_name character varying(250) NOT NULL,
    content text NOT NULL,
    embedding public.vector(768) NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL
);


ALTER TABLE ai_core.rag_knowledge_chunk OWNER TO postgres;

--
-- TOC entry 343 (class 1259 OID 18793)
-- Name: tool_definition; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.tool_definition (
    tool_definition_id uuid DEFAULT gen_random_uuid() NOT NULL,
    code character varying(100) NOT NULL,
    name character varying(150) NOT NULL,
    description text,
    handler_key character varying(200) NOT NULL,
    requires_user_authorization boolean DEFAULT true NOT NULL,
    requires_human_approval boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL
);


ALTER TABLE ai_core.tool_definition OWNER TO postgres;

--
-- TOC entry 347 (class 1259 OID 18880)
-- Name: tool_execution; Type: TABLE; Schema: ai_core; Owner: postgres
--

CREATE TABLE ai_core.tool_execution (
    tool_execution_id uuid DEFAULT gen_random_uuid() NOT NULL,
    ai_execution_log_id uuid,
    tool_definition_id uuid NOT NULL,
    input_payload jsonb,
    output_payload jsonb,
    status character varying(30) NOT NULL,
    error_message text,
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    completed_at timestamp with time zone
);


ALTER TABLE ai_core.tool_execution OWNER TO postgres;

--
-- TOC entry 361 (class 1259 OID 19235)
-- Name: human_handoff; Type: TABLE; Schema: ai_inquiry; Owner: postgres
--

CREATE TABLE ai_inquiry.human_handoff (
    human_handoff_id uuid DEFAULT gen_random_uuid() NOT NULL,
    inquiry_conversation_id uuid NOT NULL,
    requested_at timestamp with time zone DEFAULT now() NOT NULL,
    reason text,
    assigned_to_user_id uuid,
    accepted_at timestamp with time zone,
    resolved_at timestamp with time zone,
    status character varying(30) DEFAULT 'REQUESTED'::character varying NOT NULL
);


ALTER TABLE ai_inquiry.human_handoff OWNER TO postgres;

--
-- TOC entry 358 (class 1259 OID 19154)
-- Name: inquiry_conversation; Type: TABLE; Schema: ai_inquiry; Owner: postgres
--

CREATE TABLE ai_inquiry.inquiry_conversation (
    inquiry_conversation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    visitor_session_id character varying(150) NOT NULL,
    user_id uuid,
    visitor_name character varying(200),
    phone character varying(50),
    email character varying(250),
    interested_program_id uuid,
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    ended_at timestamp with time zone,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_inquiry.inquiry_conversation OWNER TO postgres;

--
-- TOC entry 359 (class 1259 OID 19184)
-- Name: inquiry_message; Type: TABLE; Schema: ai_inquiry; Owner: postgres
--

CREATE TABLE ai_inquiry.inquiry_message (
    inquiry_message_id uuid DEFAULT gen_random_uuid() NOT NULL,
    inquiry_conversation_id uuid NOT NULL,
    role character varying(20) NOT NULL,
    content text,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT inquiry_message_role_check CHECK (((role)::text = ANY ((ARRAY['system'::character varying, 'user'::character varying, 'assistant'::character varying, 'tool'::character varying])::text[])))
);


ALTER TABLE ai_inquiry.inquiry_message OWNER TO postgres;

--
-- TOC entry 360 (class 1259 OID 19203)
-- Name: lead_capture; Type: TABLE; Schema: ai_inquiry; Owner: postgres
--

CREATE TABLE ai_inquiry.lead_capture (
    lead_capture_id uuid DEFAULT gen_random_uuid() NOT NULL,
    inquiry_conversation_id uuid NOT NULL,
    name character varying(200),
    phone character varying(50),
    email character varying(250),
    interested_campus_id uuid,
    interested_program_id uuid,
    interested_grade_id uuid,
    notes text,
    captured_at timestamp with time zone DEFAULT now() NOT NULL,
    converted_inquiry_id uuid
);


ALTER TABLE ai_inquiry.lead_capture OWNER TO postgres;

--
-- TOC entry 362 (class 1259 OID 19254)
-- Name: parent_conversation; Type: TABLE; Schema: ai_parent; Owner: postgres
--

CREATE TABLE ai_parent.parent_conversation (
    parent_conversation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    guardian_id uuid NOT NULL,
    selected_student_id uuid,
    title character varying(250),
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    ended_at timestamp with time zone,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_parent.parent_conversation OWNER TO postgres;

--
-- TOC entry 363 (class 1259 OID 19282)
-- Name: parent_message; Type: TABLE; Schema: ai_parent; Owner: postgres
--

CREATE TABLE ai_parent.parent_message (
    parent_message_id uuid DEFAULT gen_random_uuid() NOT NULL,
    parent_conversation_id uuid NOT NULL,
    role character varying(20) NOT NULL,
    content text,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT parent_message_role_check CHECK (((role)::text = ANY ((ARRAY['system'::character varying, 'user'::character varying, 'assistant'::character varying, 'tool'::character varying])::text[])))
);


ALTER TABLE ai_parent.parent_message OWNER TO postgres;

--
-- TOC entry 364 (class 1259 OID 19301)
-- Name: parent_tool_execution; Type: TABLE; Schema: ai_parent; Owner: postgres
--

CREATE TABLE ai_parent.parent_tool_execution (
    parent_tool_execution_id uuid DEFAULT gen_random_uuid() NOT NULL,
    parent_conversation_id uuid NOT NULL,
    tool_definition_id uuid NOT NULL,
    student_id uuid,
    input_payload jsonb,
    output_payload jsonb,
    status character varying(30) NOT NULL,
    executed_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE ai_parent.parent_tool_execution OWNER TO postgres;

--
-- TOC entry 355 (class 1259 OID 19079)
-- Name: generated_quiz; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.generated_quiz (
    generated_quiz_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    subject_id uuid NOT NULL,
    tutor_conversation_id uuid,
    topic character varying(250),
    difficulty character varying(30),
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_tutor.generated_quiz OWNER TO postgres;

--
-- TOC entry 356 (class 1259 OID 19111)
-- Name: generated_quiz_question; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.generated_quiz_question (
    generated_quiz_question_id uuid DEFAULT gen_random_uuid() NOT NULL,
    generated_quiz_id uuid NOT NULL,
    sequence_no integer NOT NULL,
    question_text text NOT NULL,
    question_type character varying(30) NOT NULL,
    options jsonb,
    expected_answer text,
    explanation text
);


ALTER TABLE ai_tutor.generated_quiz_question OWNER TO postgres;

--
-- TOC entry 354 (class 1259 OID 19051)
-- Name: learning_recommendation; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.learning_recommendation (
    learning_recommendation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_id uuid NOT NULL,
    subject_id uuid,
    topic character varying(250),
    recommendation_type character varying(50) NOT NULL,
    recommendation_text text NOT NULL,
    priority integer DEFAULT 0 NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE ai_tutor.learning_recommendation OWNER TO postgres;

--
-- TOC entry 357 (class 1259 OID 19131)
-- Name: student_quiz_attempt; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.student_quiz_attempt (
    student_quiz_attempt_id uuid DEFAULT gen_random_uuid() NOT NULL,
    generated_quiz_id uuid NOT NULL,
    student_id uuid NOT NULL,
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    completed_at timestamp with time zone,
    score numeric(7,3),
    answers jsonb
);


ALTER TABLE ai_tutor.student_quiz_attempt OWNER TO postgres;

--
-- TOC entry 353 (class 1259 OID 19019)
-- Name: student_topic_mastery; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.student_topic_mastery (
    student_topic_mastery_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    subject_id uuid NOT NULL,
    topic character varying(250) NOT NULL,
    mastery_score numeric(7,4),
    confidence_score numeric(7,4),
    evidence_count integer DEFAULT 0 NOT NULL,
    last_assessed_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT student_topic_mastery_confidence_score_check CHECK (((confidence_score >= (0)::numeric) AND (confidence_score <= (1)::numeric))),
    CONSTRAINT student_topic_mastery_mastery_score_check CHECK (((mastery_score >= (0)::numeric) AND (mastery_score <= (1)::numeric)))
);


ALTER TABLE ai_tutor.student_topic_mastery OWNER TO postgres;

--
-- TOC entry 348 (class 1259 OID 18903)
-- Name: tutor_conversation; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.tutor_conversation (
    tutor_conversation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    academic_year_id uuid,
    course_offering_id uuid,
    subject_id uuid,
    title character varying(250),
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    ended_at timestamp with time zone,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE ai_tutor.tutor_conversation OWNER TO postgres;

--
-- TOC entry 352 (class 1259 OID 18995)
-- Name: tutor_feedback; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.tutor_feedback (
    tutor_feedback_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tutor_message_id uuid NOT NULL,
    student_id uuid NOT NULL,
    rating smallint,
    was_helpful boolean,
    comments text,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT tutor_feedback_rating_check CHECK (((rating >= 1) AND (rating <= 5)))
);


ALTER TABLE ai_tutor.tutor_feedback OWNER TO postgres;

--
-- TOC entry 349 (class 1259 OID 18941)
-- Name: tutor_message; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.tutor_message (
    tutor_message_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tutor_conversation_id uuid NOT NULL,
    role character varying(20) NOT NULL,
    content text,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT tutor_message_role_check CHECK (((role)::text = ANY ((ARRAY['system'::character varying, 'user'::character varying, 'assistant'::character varying, 'tool'::character varying])::text[])))
);


ALTER TABLE ai_tutor.tutor_message OWNER TO postgres;

--
-- TOC entry 350 (class 1259 OID 18960)
-- Name: tutor_message_reference; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.tutor_message_reference (
    tutor_message_reference_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tutor_message_id uuid NOT NULL,
    knowledge_chunk_id uuid,
    citation_label character varying(150),
    relevance_score numeric(10,6)
);


ALTER TABLE ai_tutor.tutor_message_reference OWNER TO postgres;

--
-- TOC entry 351 (class 1259 OID 18978)
-- Name: tutor_session; Type: TABLE; Schema: ai_tutor; Owner: postgres
--

CREATE TABLE ai_tutor.tutor_session (
    tutor_session_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tutor_conversation_id uuid NOT NULL,
    topic character varying(250),
    learning_objective text,
    started_at timestamp with time zone DEFAULT now() NOT NULL,
    ended_at timestamp with time zone,
    session_summary text
);


ALTER TABLE ai_tutor.tutor_session OWNER TO postgres;

--
-- TOC entry 379 (class 1259 OID 19770)
-- Name: audit_log; Type: TABLE; Schema: audit; Owner: postgres
--

CREATE TABLE audit.audit_log (
    audit_log_id bigint NOT NULL,
    tenant_id uuid,
    user_id uuid,
    action character varying(100) NOT NULL,
    entity_type character varying(150) NOT NULL,
    entity_id character varying(100),
    old_values jsonb,
    new_values jsonb,
    ip_address inet,
    correlation_id character varying(100),
    occurred_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE audit.audit_log OWNER TO postgres;

--
-- TOC entry 378 (class 1259 OID 19769)
-- Name: audit_log_audit_log_id_seq; Type: SEQUENCE; Schema: audit; Owner: postgres
--

ALTER TABLE audit.audit_log ALTER COLUMN audit_log_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME audit.audit_log_audit_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 409 (class 1259 OID 20395)
-- Name: ChatAttachments; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."ChatAttachments" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "MessageId" uuid NOT NULL,
    "FileName" character varying(255) NOT NULL,
    "ContentType" character varying(150) NOT NULL,
    "FileSizeBytes" bigint NOT NULL,
    "StorageKey" character varying(500) NOT NULL
);


ALTER TABLE communication."ChatAttachments" OWNER TO postgres;

--
-- TOC entry 406 (class 1259 OID 20342)
-- Name: ChatConversations; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."ChatConversations" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "Title" character varying(200) NOT NULL,
    "ConversationType" character varying(50) NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "RelatedEntityId" uuid,
    "RelatedEntityType" character varying(100),
    "IsClosed" boolean DEFAULT false NOT NULL,
    "ClosedAt" timestamp with time zone
);


ALTER TABLE communication."ChatConversations" OWNER TO postgres;

--
-- TOC entry 408 (class 1259 OID 20374)
-- Name: ChatMessages; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."ChatMessages" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "ConversationId" uuid NOT NULL,
    "SenderUserId" uuid NOT NULL,
    "MessageType" character varying(30) NOT NULL,
    "Message" character varying(4000) NOT NULL,
    "ReplyToMessageId" uuid,
    "SentAt" timestamp with time zone NOT NULL,
    "EditedAt" timestamp with time zone,
    "IsDeleted" boolean DEFAULT false NOT NULL
);


ALTER TABLE communication."ChatMessages" OWNER TO postgres;

--
-- TOC entry 407 (class 1259 OID 20354)
-- Name: ChatParticipants; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."ChatParticipants" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "ConversationId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Role" character varying(50) NOT NULL,
    "JoinedAt" timestamp with time zone NOT NULL,
    "LastReadAt" timestamp with time zone,
    "IsMuted" boolean DEFAULT false NOT NULL
);


ALTER TABLE communication."ChatParticipants" OWNER TO postgres;

--
-- TOC entry 411 (class 1259 OID 20431)
-- Name: NotificationPreferences; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."NotificationPreferences" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "NotificationType" character varying(80) NOT NULL,
    "InAppEnabled" boolean DEFAULT true NOT NULL,
    "PushEnabled" boolean DEFAULT true NOT NULL,
    "EmailEnabled" boolean DEFAULT false NOT NULL,
    "SmsEnabled" boolean DEFAULT false NOT NULL
);


ALTER TABLE communication."NotificationPreferences" OWNER TO postgres;

--
-- TOC entry 405 (class 1259 OID 20329)
-- Name: NotificationTypeLookup; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."NotificationTypeLookup" (
    "Id" uuid NOT NULL,
    "Code" character varying(100) NOT NULL,
    "Name" character varying(150) NOT NULL,
    "DisplayOrder" integer NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE communication."NotificationTypeLookup" OWNER TO postgres;

--
-- TOC entry 410 (class 1259 OID 20414)
-- Name: Notifications; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication."Notifications" (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "RecipientUserId" uuid NOT NULL,
    "Type" character varying(80) NOT NULL,
    "Title" character varying(200) NOT NULL,
    "Message" character varying(2000) NOT NULL,
    "RelatedEntityId" uuid,
    "RelatedEntityType" character varying(100),
    "ActionUrl" character varying(500),
    "Priority" character varying(20) NOT NULL,
    "IsRead" boolean DEFAULT false NOT NULL,
    "ReadAt" timestamp with time zone,
    "OccurredAt" timestamp with time zone NOT NULL
);


ALTER TABLE communication."Notifications" OWNER TO postgres;

--
-- TOC entry 524 (class 1259 OID 23238)
-- Name: chat_conversation; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.chat_conversation (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "Title" character varying(250) NOT NULL,
    "ConversationType" character varying(50) NOT NULL,
    "CreatedByUserId" uuid NOT NULL,
    "RelatedEntityId" uuid,
    "RelatedEntityType" character varying(100),
    "IsClosed" boolean DEFAULT false NOT NULL,
    "ClosedAt" timestamp with time zone,
    "IsActive" boolean DEFAULT true NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "RowVersion" bytea DEFAULT '\x5c78'::bytea NOT NULL
);


ALTER TABLE communication.chat_conversation OWNER TO postgres;

--
-- TOC entry 526 (class 1259 OID 23281)
-- Name: chat_message; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.chat_message (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "ConversationId" uuid NOT NULL,
    "SenderUserId" uuid NOT NULL,
    "MessageType" character varying(30) DEFAULT 'Text'::character varying NOT NULL,
    "Message" character varying(5000) NOT NULL,
    "ReplyToMessageId" uuid,
    "SentAt" timestamp with time zone NOT NULL,
    "EditedAt" timestamp with time zone,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "RowVersion" bytea DEFAULT '\x5c78'::bytea NOT NULL
);


ALTER TABLE communication.chat_message OWNER TO postgres;

--
-- TOC entry 525 (class 1259 OID 23258)
-- Name: chat_participant; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.chat_participant (
    "Id" uuid NOT NULL,
    "TenantId" uuid NOT NULL,
    "ConversationId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Role" character varying(50) NOT NULL,
    "JoinedAt" timestamp with time zone NOT NULL,
    "LastReadAt" timestamp with time zone,
    "IsMuted" boolean DEFAULT false NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "RowVersion" bytea DEFAULT '\x5c78'::bytea NOT NULL
);


ALTER TABLE communication.chat_participant OWNER TO postgres;

--
-- TOC entry 325 (class 1259 OID 18367)
-- Name: conversation; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.conversation (
    conversation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    conversation_type_code character varying(40) NOT NULL,
    student_id uuid,
    class_section_id uuid,
    subject_id uuid,
    title character varying(200),
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE communication.conversation OWNER TO postgres;

--
-- TOC entry 326 (class 1259 OID 18403)
-- Name: conversation_participant; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.conversation_participant (
    conversation_id uuid NOT NULL,
    user_id uuid NOT NULL,
    joined_at timestamp with time zone DEFAULT now() NOT NULL,
    left_at timestamp with time zone
);


ALTER TABLE communication.conversation_participant OWNER TO postgres;

--
-- TOC entry 327 (class 1259 OID 18417)
-- Name: message; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.message (
    message_id uuid DEFAULT gen_random_uuid() NOT NULL,
    conversation_id uuid NOT NULL,
    sender_user_id uuid NOT NULL,
    reply_to_message_id uuid,
    message_type_code character varying(30) DEFAULT 'TEXT'::character varying NOT NULL,
    body text,
    sent_at timestamp with time zone DEFAULT now() NOT NULL,
    edited_at timestamp with time zone,
    deleted_at timestamp with time zone
);


ALTER TABLE communication.message OWNER TO postgres;

--
-- TOC entry 328 (class 1259 OID 18442)
-- Name: message_receipt; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.message_receipt (
    message_id uuid NOT NULL,
    user_id uuid NOT NULL,
    delivered_at timestamp with time zone,
    read_at timestamp with time zone
);


ALTER TABLE communication.message_receipt OWNER TO postgres;

--
-- TOC entry 329 (class 1259 OID 18454)
-- Name: notification; Type: TABLE; Schema: communication; Owner: postgres
--

CREATE TABLE communication.notification (
    notification_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    user_id uuid NOT NULL,
    title character varying(250) NOT NULL,
    body text,
    channel_code character varying(30) NOT NULL,
    status character varying(30) DEFAULT 'QUEUED'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    sent_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE communication.notification OWNER TO postgres;

--
-- TOC entry 385 (class 1259 OID 19953)
-- Name: candidatedocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.candidatedocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    candidateid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT candidatedocument_filesizebytes_check CHECK ((filesizebytes > 0)),
    CONSTRAINT ck_candidatedocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon)))
);


ALTER TABLE document.candidatedocument OWNER TO postgres;

--
-- TOC entry 320 (class 1259 OID 18214)
-- Name: document_template; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.document_template (
    document_template_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    academic_system_id uuid,
    document_type_code character varying(50) NOT NULL,
    code character varying(80) NOT NULL,
    name character varying(180) NOT NULL,
    subject_template text,
    header_html text,
    body_html text NOT NULL,
    footer_html text,
    language_code character varying(10) DEFAULT 'en'::character varying NOT NULL,
    version integer DEFAULT 1 NOT NULL,
    requires_approval boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE document.document_template OWNER TO postgres;

--
-- TOC entry 380 (class 1259 OID 19793)
-- Name: documenttype; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.documenttype (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    code character varying(80) NOT NULL,
    name character varying(150) NOT NULL,
    ownercategory character varying(50) NOT NULL,
    isidentitydocument boolean DEFAULT false NOT NULL,
    requiresexpirydate boolean DEFAULT false NOT NULL,
    requiresverification boolean DEFAULT false NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL
);


ALTER TABLE document.documenttype OWNER TO postgres;

--
-- TOC entry 386 (class 1259 OID 19987)
-- Name: driverdocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.driverdocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    driverid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_driverdocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT driverdocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE document.driverdocument OWNER TO postgres;

--
-- TOC entry 384 (class 1259 OID 19919)
-- Name: employeedocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.employeedocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    employeeid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_employeedocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT employeedocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE document.employeedocument OWNER TO postgres;

--
-- TOC entry 321 (class 1259 OID 18255)
-- Name: generated_document; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.generated_document (
    generated_document_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    document_template_id uuid NOT NULL,
    template_version integer NOT NULL,
    student_id uuid,
    employee_id uuid,
    document_number character varying(100) NOT NULL,
    rendered_content_snapshot text NOT NULL,
    file_url text,
    verification_code character varying(100),
    issued_by uuid,
    approved_by uuid,
    issued_at timestamp with time zone,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE document.generated_document OWNER TO postgres;

--
-- TOC entry 382 (class 1259 OID 19851)
-- Name: parentdocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.parentdocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    parentid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_parentdocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT parentdocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE document.parentdocument OWNER TO postgres;

--
-- TOC entry 381 (class 1259 OID 19817)
-- Name: studentdocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.studentdocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    studentid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_studentdocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT studentdocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE document.studentdocument OWNER TO postgres;

--
-- TOC entry 383 (class 1259 OID 19885)
-- Name: teacherdocument; Type: TABLE; Schema: document; Owner: postgres
--

CREATE TABLE document.teacherdocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    teacherid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_teacherdocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT teacherdocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE document.teacherdocument OWNER TO postgres;

--
-- TOC entry 301 (class 1259 OID 17736)
-- Name: exam; Type: TABLE; Schema: exam; Owner: postgres
--

CREATE TABLE exam.exam (
    exam_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    term_id uuid,
    academic_system_id uuid NOT NULL,
    exam_type_code character varying(40) NOT NULL,
    name character varying(180) NOT NULL,
    start_date date,
    end_date date,
    result_publish_date date,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE exam.exam OWNER TO postgres;

--
-- TOC entry 302 (class 1259 OID 17776)
-- Name: exam_subject; Type: TABLE; Schema: exam; Owner: postgres
--

CREATE TABLE exam.exam_subject (
    exam_subject_id uuid DEFAULT gen_random_uuid() NOT NULL,
    exam_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    exam_date date,
    start_time time without time zone,
    duration_minutes integer,
    total_marks numeric(8,2) NOT NULL,
    passing_marks numeric(8,2),
    room_id uuid
);


ALTER TABLE exam.exam_subject OWNER TO postgres;

--
-- TOC entry 303 (class 1259 OID 17801)
-- Name: student_exam_result; Type: TABLE; Schema: exam; Owner: postgres
--

CREATE TABLE exam.student_exam_result (
    student_exam_result_id uuid DEFAULT gen_random_uuid() NOT NULL,
    exam_subject_id uuid NOT NULL,
    student_id uuid NOT NULL,
    marks_obtained numeric(8,2),
    percentage numeric(7,3),
    grade character varying(20),
    is_absent boolean DEFAULT false NOT NULL,
    remarks text,
    entered_by uuid,
    verified_by uuid
);


ALTER TABLE exam.student_exam_result OWNER TO postgres;

--
-- TOC entry 304 (class 1259 OID 17826)
-- Name: fee_type; Type: TABLE; Schema: finance; Owner: postgres
--

CREATE TABLE finance.fee_type (
    fee_type_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE finance.fee_type OWNER TO postgres;

--
-- TOC entry 308 (class 1259 OID 17923)
-- Name: payment_allocation; Type: TABLE; Schema: finance; Owner: postgres
--

CREATE TABLE finance.payment_allocation (
    payment_allocation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_payment_id uuid NOT NULL,
    student_invoice_id uuid NOT NULL,
    amount numeric(18,2) NOT NULL
);


ALTER TABLE finance.payment_allocation OWNER TO postgres;

--
-- TOC entry 305 (class 1259 OID 17843)
-- Name: student_invoice; Type: TABLE; Schema: finance; Owner: postgres
--

CREATE TABLE finance.student_invoice (
    student_invoice_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    academic_year_id uuid,
    invoice_number character varying(80) NOT NULL,
    invoice_date date NOT NULL,
    due_date date,
    status character varying(30) DEFAULT 'OPEN'::character varying NOT NULL,
    total_amount numeric(18,2) DEFAULT 0 NOT NULL,
    balance_amount numeric(18,2) DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE finance.student_invoice OWNER TO postgres;

--
-- TOC entry 306 (class 1259 OID 17877)
-- Name: student_invoice_line; Type: TABLE; Schema: finance; Owner: postgres
--

CREATE TABLE finance.student_invoice_line (
    student_invoice_line_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_invoice_id uuid NOT NULL,
    fee_type_id uuid NOT NULL,
    description character varying(250),
    amount numeric(18,2) NOT NULL
);


ALTER TABLE finance.student_invoice_line OWNER TO postgres;

--
-- TOC entry 307 (class 1259 OID 17897)
-- Name: student_payment; Type: TABLE; Schema: finance; Owner: postgres
--

CREATE TABLE finance.student_payment (
    student_payment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    payment_number character varying(80) NOT NULL,
    payment_date timestamp with time zone DEFAULT now() NOT NULL,
    amount numeric(18,2) NOT NULL,
    payment_method character varying(40) NOT NULL,
    reference_no character varying(150),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE finance.student_payment OWNER TO postgres;

--
-- TOC entry 521 (class 1259 OID 23115)
-- Name: aggregatedcounter; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.aggregatedcounter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


ALTER TABLE hangfire.aggregatedcounter OWNER TO postgres;

--
-- TOC entry 520 (class 1259 OID 23114)
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.aggregatedcounter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.aggregatedcounter_id_seq OWNER TO postgres;

--
-- TOC entry 6434 (class 0 OID 0)
-- Dependencies: 520
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.aggregatedcounter_id_seq OWNED BY hangfire.aggregatedcounter.id;


--
-- TOC entry 503 (class 1259 OID 22768)
-- Name: counter; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.counter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


ALTER TABLE hangfire.counter OWNER TO postgres;

--
-- TOC entry 502 (class 1259 OID 22767)
-- Name: counter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.counter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.counter_id_seq OWNER TO postgres;

--
-- TOC entry 6435 (class 0 OID 0)
-- Dependencies: 502
-- Name: counter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.counter_id_seq OWNED BY hangfire.counter.id;


--
-- TOC entry 505 (class 1259 OID 22779)
-- Name: hash; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.hash (
    id bigint NOT NULL,
    key text NOT NULL,
    field text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.hash OWNER TO postgres;

--
-- TOC entry 504 (class 1259 OID 22778)
-- Name: hash_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.hash_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.hash_id_seq OWNER TO postgres;

--
-- TOC entry 6436 (class 0 OID 0)
-- Dependencies: 504
-- Name: hash_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.hash_id_seq OWNED BY hangfire.hash.id;


--
-- TOC entry 507 (class 1259 OID 22793)
-- Name: job; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.job (
    id bigint NOT NULL,
    stateid bigint,
    statename text,
    invocationdata jsonb NOT NULL,
    arguments jsonb NOT NULL,
    createdat timestamp with time zone NOT NULL,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.job OWNER TO postgres;

--
-- TOC entry 506 (class 1259 OID 22792)
-- Name: job_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.job_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.job_id_seq OWNER TO postgres;

--
-- TOC entry 6437 (class 0 OID 0)
-- Dependencies: 506
-- Name: job_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.job_id_seq OWNED BY hangfire.job.id;


--
-- TOC entry 518 (class 1259 OID 22872)
-- Name: jobparameter; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.jobparameter (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    name text NOT NULL,
    value text,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.jobparameter OWNER TO postgres;

--
-- TOC entry 517 (class 1259 OID 22871)
-- Name: jobparameter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.jobparameter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.jobparameter_id_seq OWNER TO postgres;

--
-- TOC entry 6438 (class 0 OID 0)
-- Dependencies: 517
-- Name: jobparameter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.jobparameter_id_seq OWNED BY hangfire.jobparameter.id;


--
-- TOC entry 511 (class 1259 OID 22826)
-- Name: jobqueue; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.jobqueue (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    queue text NOT NULL,
    fetchedat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.jobqueue OWNER TO postgres;

--
-- TOC entry 510 (class 1259 OID 22825)
-- Name: jobqueue_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.jobqueue_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.jobqueue_id_seq OWNER TO postgres;

--
-- TOC entry 6439 (class 0 OID 0)
-- Dependencies: 510
-- Name: jobqueue_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.jobqueue_id_seq OWNED BY hangfire.jobqueue.id;


--
-- TOC entry 513 (class 1259 OID 22837)
-- Name: list; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.list (
    id bigint NOT NULL,
    key text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.list OWNER TO postgres;

--
-- TOC entry 512 (class 1259 OID 22836)
-- Name: list_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.list_id_seq OWNER TO postgres;

--
-- TOC entry 6440 (class 0 OID 0)
-- Dependencies: 512
-- Name: list_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.list_id_seq OWNED BY hangfire.list.id;


--
-- TOC entry 519 (class 1259 OID 22889)
-- Name: lock; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.lock (
    resource text NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL,
    acquired timestamp with time zone
);


ALTER TABLE hangfire.lock OWNER TO postgres;

--
-- TOC entry 501 (class 1259 OID 22761)
-- Name: schema; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.schema (
    version integer NOT NULL
);


ALTER TABLE hangfire.schema OWNER TO postgres;

--
-- TOC entry 514 (class 1259 OID 22847)
-- Name: server; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.server (
    id text NOT NULL,
    data jsonb,
    lastheartbeat timestamp with time zone NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.server OWNER TO postgres;

--
-- TOC entry 516 (class 1259 OID 22857)
-- Name: set; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.set (
    id bigint NOT NULL,
    key text NOT NULL,
    score double precision NOT NULL,
    value text NOT NULL,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.set OWNER TO postgres;

--
-- TOC entry 515 (class 1259 OID 22856)
-- Name: set_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.set_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.set_id_seq OWNER TO postgres;

--
-- TOC entry 6441 (class 0 OID 0)
-- Dependencies: 515
-- Name: set_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.set_id_seq OWNED BY hangfire.set.id;


--
-- TOC entry 509 (class 1259 OID 22807)
-- Name: state; Type: TABLE; Schema: hangfire; Owner: postgres
--

CREATE TABLE hangfire.state (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    name text NOT NULL,
    reason text,
    createdat timestamp with time zone NOT NULL,
    data jsonb,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.state OWNER TO postgres;

--
-- TOC entry 508 (class 1259 OID 22806)
-- Name: state_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: postgres
--

CREATE SEQUENCE hangfire.state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.state_id_seq OWNER TO postgres;

--
-- TOC entry 6442 (class 0 OID 0)
-- Dependencies: 508
-- Name: state_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: postgres
--

ALTER SEQUENCE hangfire.state_id_seq OWNED BY hangfire.state.id;


--
-- TOC entry 287 (class 1259 OID 17340)
-- Name: candidate; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.candidate (
    candidate_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100),
    email character varying(250),
    phone character varying(50),
    current_job_title character varying(150),
    current_employer character varying(200),
    total_experience_years numeric(5,2),
    highest_qualification character varying(250),
    expected_salary numeric(18,2),
    notice_period_days integer,
    status_code character varying(30) DEFAULT 'NEW'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.candidate OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 17360)
-- Name: candidate_document; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.candidate_document (
    candidate_document_id uuid DEFAULT gen_random_uuid() NOT NULL,
    candidate_id uuid NOT NULL,
    document_type character varying(50) NOT NULL,
    file_name character varying(255) NOT NULL,
    file_url text NOT NULL,
    mime_type character varying(120),
    size_bytes bigint,
    uploaded_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE hr.candidate_document OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 17245)
-- Name: employee; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.employee (
    employee_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    user_id uuid,
    employee_number character varying(60) NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100),
    cnic_number character varying(20),
    photo bytea,
    photo_content_type character varying(150),
    photo_file_name character varying(255),
    email character varying(250),
    phone character varying(50),
    hire_date date NOT NULL,
    employment_type_code character varying(30) NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    source_candidate_id uuid,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.employee OWNER TO postgres;

--
-- TOC entry 310 (class 1259 OID 17967)
-- Name: employee_compensation; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.employee_compensation (
    employee_compensation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    job_grade_id uuid,
    effective_from date NOT NULL,
    effective_to date,
    basic_salary numeric(18,2) NOT NULL,
    gross_salary numeric(18,2),
    currency_code character(3) DEFAULT 'PKR'::bpchar NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.employee_compensation OWNER TO postgres;

--
-- TOC entry 285 (class 1259 OID 17275)
-- Name: employee_position; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.employee_position (
    employee_position_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    position_id uuid NOT NULL,
    effective_from date NOT NULL,
    effective_to date,
    is_primary boolean DEFAULT true NOT NULL,
    change_reason character varying(150),
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.employee_position OWNER TO postgres;

--
-- TOC entry 311 (class 1259 OID 17997)
-- Name: employee_salary_component; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.employee_salary_component (
    employee_compensation_id uuid NOT NULL,
    salary_component_id uuid NOT NULL,
    amount numeric(18,2),
    percentage numeric(9,4),
    formula text
);


ALTER TABLE hr.employee_salary_component OWNER TO postgres;

--
-- TOC entry 314 (class 1259 OID 18080)
-- Name: increment_approval; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.increment_approval (
    increment_approval_id uuid DEFAULT gen_random_uuid() NOT NULL,
    increment_request_id uuid NOT NULL,
    approval_level integer NOT NULL,
    approver_user_id uuid NOT NULL,
    decision character varying(30),
    comments text,
    decision_at timestamp with time zone
);


ALTER TABLE hr.increment_approval OWNER TO postgres;

--
-- TOC entry 312 (class 1259 OID 18016)
-- Name: increment_policy; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.increment_policy (
    increment_policy_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    name character varying(150) NOT NULL,
    frequency character varying(30) DEFAULT 'ANNUAL'::character varying NOT NULL,
    increment_type_code character varying(30) NOT NULL,
    increment_value numeric(18,4),
    minimum_service_months integer DEFAULT 12 NOT NULL,
    minimum_performance_score numeric(6,2),
    requires_hr_approval boolean DEFAULT true NOT NULL,
    requires_finance_approval boolean DEFAULT false NOT NULL,
    requires_principal_approval boolean DEFAULT true NOT NULL,
    is_automatic boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.increment_policy OWNER TO postgres;

--
-- TOC entry 291 (class 1259 OID 17438)
-- Name: interview; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.interview (
    interview_id uuid DEFAULT gen_random_uuid() NOT NULL,
    job_application_id uuid NOT NULL,
    interview_type_code character varying(40) NOT NULL,
    round_number integer DEFAULT 1 NOT NULL,
    scheduled_at timestamp with time zone,
    duration_minutes integer,
    location character varying(250),
    meeting_url text,
    status character varying(30) DEFAULT 'SCHEDULED'::character varying NOT NULL,
    overall_score numeric(6,2),
    recommendation character varying(100),
    notes text
);


ALTER TABLE hr.interview OWNER TO postgres;

--
-- TOC entry 293 (class 1259 OID 17475)
-- Name: interview_evaluation; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.interview_evaluation (
    interview_evaluation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    interview_id uuid NOT NULL,
    interviewer_employee_id uuid NOT NULL,
    score numeric(6,2),
    strengths text,
    weaknesses text,
    comments text,
    recommendation character varying(100),
    submitted_at timestamp with time zone
);


ALTER TABLE hr.interview_evaluation OWNER TO postgres;

--
-- TOC entry 292 (class 1259 OID 17458)
-- Name: interview_panel; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.interview_panel (
    interview_id uuid NOT NULL,
    employee_id uuid NOT NULL
);


ALTER TABLE hr.interview_panel OWNER TO postgres;

--
-- TOC entry 281 (class 1259 OID 17146)
-- Name: job; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job (
    job_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    department_id uuid,
    job_family_id uuid,
    code character varying(50) NOT NULL,
    title character varying(150) NOT NULL,
    description text,
    responsibilities text,
    minimum_qualification text,
    minimum_experience_years numeric(5,2),
    is_teaching_position boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.job OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 17403)
-- Name: job_application; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job_application (
    job_application_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    candidate_id uuid NOT NULL,
    job_vacancy_id uuid NOT NULL,
    application_date date DEFAULT CURRENT_DATE NOT NULL,
    status_code character varying(30) DEFAULT 'APPLIED'::character varying NOT NULL,
    screening_score numeric(6,2),
    final_score numeric(6,2),
    rejection_reason text,
    eligible_for_future_opening boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.job_application OWNER TO postgres;

--
-- TOC entry 279 (class 1259 OID 17108)
-- Name: job_family; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job_family (
    job_family_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.job_family OWNER TO postgres;

--
-- TOC entry 280 (class 1259 OID 17125)
-- Name: job_grade; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job_grade (
    job_grade_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    grade_level integer,
    minimum_salary numeric(18,2),
    midpoint_salary numeric(18,2),
    maximum_salary numeric(18,2),
    currency_code character(3) DEFAULT 'PKR'::bpchar NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.job_grade OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 17179)
-- Name: job_grade_mapping; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job_grade_mapping (
    job_id uuid NOT NULL,
    job_grade_id uuid NOT NULL,
    is_default boolean DEFAULT false NOT NULL,
    effective_from date,
    effective_to date
);


ALTER TABLE hr.job_grade_mapping OWNER TO postgres;

--
-- TOC entry 289 (class 1259 OID 17380)
-- Name: job_vacancy; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.job_vacancy (
    job_vacancy_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    position_id uuid NOT NULL,
    number_of_positions integer DEFAULT 1 NOT NULL,
    opening_date date,
    closing_date date,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.job_vacancy OWNER TO postgres;

--
-- TOC entry 283 (class 1259 OID 17198)
-- Name: position; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr."position" (
    position_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    department_id uuid,
    job_id uuid NOT NULL,
    job_grade_id uuid,
    reports_to_position_id uuid,
    position_code character varying(60) NOT NULL,
    headcount integer DEFAULT 1 NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr."position" OWNER TO postgres;

--
-- TOC entry 309 (class 1259 OID 17943)
-- Name: salary_component; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.salary_component (
    salary_component_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    component_type character varying(40) NOT NULL,
    calculation_type character varying(40) DEFAULT 'FIXED'::character varying NOT NULL,
    taxable boolean DEFAULT false NOT NULL,
    is_recurring boolean DEFAULT true NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.salary_component OWNER TO postgres;

--
-- TOC entry 313 (class 1259 OID 18045)
-- Name: salary_increment_request; Type: TABLE; Schema: hr; Owner: postgres
--

CREATE TABLE hr.salary_increment_request (
    increment_request_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    increment_policy_id uuid,
    request_type_code character varying(20) NOT NULL,
    increment_type_code character varying(30) NOT NULL,
    current_basic_salary numeric(18,2) NOT NULL,
    percentage numeric(9,4),
    increment_amount numeric(18,2),
    proposed_basic_salary numeric(18,2) NOT NULL,
    effective_date date NOT NULL,
    reason text,
    requested_by uuid,
    requested_at timestamp with time zone DEFAULT now() NOT NULL,
    status_code character varying(30) DEFAULT 'PENDING'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE hr.salary_increment_request OWNER TO postgres;

--
-- TOC entry 414 (class 1259 OID 20814)
-- Name: DistributedCache; Type: TABLE; Schema: infrastructure; Owner: postgres
--

CREATE TABLE infrastructure."DistributedCache" (
    "Id" text NOT NULL,
    "Value" bytea NOT NULL,
    "ExpiresAtTime" timestamp with time zone NOT NULL,
    "SlidingExpirationInSeconds" bigint,
    "AbsoluteExpiration" timestamp with time zone
);


ALTER TABLE infrastructure."DistributedCache" OWNER TO postgres;

--
-- TOC entry 333 (class 1259 OID 18543)
-- Name: item; Type: TABLE; Schema: inventory; Owner: postgres
--

CREATE TABLE inventory.item (
    item_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(60) NOT NULL,
    name character varying(180) NOT NULL,
    unit character varying(30),
    reorder_level numeric(18,3),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE inventory.item OWNER TO postgres;

--
-- TOC entry 330 (class 1259 OID 18476)
-- Name: book; Type: TABLE; Schema: library; Owner: postgres
--

CREATE TABLE library.book (
    book_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    isbn character varying(30),
    title character varying(250) NOT NULL,
    author_text character varying(250),
    publisher_text character varying(250),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE library.book OWNER TO postgres;

--
-- TOC entry 331 (class 1259 OID 18492)
-- Name: book_copy; Type: TABLE; Schema: library; Owner: postgres
--

CREATE TABLE library.book_copy (
    book_copy_id uuid DEFAULT gen_random_uuid() NOT NULL,
    book_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    barcode character varying(100) NOT NULL,
    status character varying(30) DEFAULT 'AVAILABLE'::character varying NOT NULL
);


ALTER TABLE library.book_copy OWNER TO postgres;

--
-- TOC entry 332 (class 1259 OID 18516)
-- Name: book_loan; Type: TABLE; Schema: library; Owner: postgres
--

CREATE TABLE library.book_loan (
    book_loan_id uuid DEFAULT gen_random_uuid() NOT NULL,
    book_copy_id uuid NOT NULL,
    student_id uuid,
    employee_id uuid,
    issued_at timestamp with time zone DEFAULT now() NOT NULL,
    due_at timestamp with time zone NOT NULL,
    returned_at timestamp with time zone,
    CONSTRAINT book_loan_check CHECK (((student_id IS NOT NULL) <> (employee_id IS NOT NULL)))
);


ALTER TABLE library.book_loan OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 17662)
-- Name: academic_assignment; Type: TABLE; Schema: lms; Owner: postgres
--

CREATE TABLE lms.academic_assignment (
    academic_assignment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    class_section_id uuid,
    teaching_group_id uuid,
    teacher_employee_id uuid NOT NULL,
    assignment_type_code character varying(40) NOT NULL,
    title character varying(250) NOT NULL,
    description text,
    instructions text,
    assigned_at timestamp with time zone DEFAULT now() NOT NULL,
    due_at timestamp with time zone,
    total_marks numeric(8,2),
    allow_late_submission boolean DEFAULT false NOT NULL,
    max_attempts integer DEFAULT 1 NOT NULL,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE lms.academic_assignment OWNER TO postgres;

--
-- TOC entry 300 (class 1259 OID 17709)
-- Name: student_assignment_submission; Type: TABLE; Schema: lms; Owner: postgres
--

CREATE TABLE lms.student_assignment_submission (
    submission_id uuid DEFAULT gen_random_uuid() NOT NULL,
    academic_assignment_id uuid NOT NULL,
    student_id uuid NOT NULL,
    attempt_no integer DEFAULT 1 NOT NULL,
    submitted_at timestamp with time zone,
    submission_text text,
    marks_obtained numeric(8,2),
    teacher_feedback text,
    status character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL
);


ALTER TABLE lms.student_assignment_submission OWNER TO postgres;

--
-- TOC entry 523 (class 1259 OID 23202)
-- Name: application_log; Type: TABLE; Schema: observability; Owner: postgres
--

CREATE TABLE observability.application_log (
    id bigint NOT NULL,
    timestamp_utc timestamp with time zone DEFAULT now() NOT NULL,
    level character varying(32) NOT NULL,
    service character varying(128),
    message text NOT NULL,
    message_template text,
    exception text,
    trace_id character varying(64),
    correlation_id character varying(128),
    request_path character varying(1024),
    properties jsonb
);


ALTER TABLE observability.application_log OWNER TO postgres;

--
-- TOC entry 522 (class 1259 OID 23201)
-- Name: application_log_id_seq; Type: SEQUENCE; Schema: observability; Owner: postgres
--

ALTER TABLE observability.application_log ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME observability.application_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 256 (class 1259 OID 16520)
-- Name: campus; Type: TABLE; Schema: org; Owner: postgres
--

CREATE TABLE org.campus (
    campus_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(200) NOT NULL,
    address text,
    phone character varying(50),
    email character varying(200),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE org.campus OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 16541)
-- Name: department; Type: TABLE; Schema: org; Owner: postgres
--

CREATE TABLE org.department (
    department_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    code character varying(50) NOT NULL,
    name character varying(150) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE org.department OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 16563)
-- Name: room; Type: TABLE; Schema: org; Owner: postgres
--

CREATE TABLE org.room (
    room_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(120) NOT NULL,
    capacity integer,
    room_type character varying(40),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE org.room OWNER TO postgres;

--
-- TOC entry 317 (class 1259 OID 18140)
-- Name: employee_payroll; Type: TABLE; Schema: payroll; Owner: postgres
--

CREATE TABLE payroll.employee_payroll (
    employee_payroll_id uuid DEFAULT gen_random_uuid() NOT NULL,
    payroll_run_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    gross_amount numeric(18,2) DEFAULT 0 NOT NULL,
    deduction_amount numeric(18,2) DEFAULT 0 NOT NULL,
    net_amount numeric(18,2) DEFAULT 0 NOT NULL
);


ALTER TABLE payroll.employee_payroll OWNER TO postgres;

--
-- TOC entry 318 (class 1259 OID 18167)
-- Name: payroll_line_item; Type: TABLE; Schema: payroll; Owner: postgres
--

CREATE TABLE payroll.payroll_line_item (
    payroll_line_item_id uuid DEFAULT gen_random_uuid() NOT NULL,
    employee_payroll_id uuid NOT NULL,
    salary_component_id uuid,
    description character varying(200),
    amount numeric(18,2) NOT NULL
);


ALTER TABLE payroll.payroll_line_item OWNER TO postgres;

--
-- TOC entry 315 (class 1259 OID 18097)
-- Name: payroll_period; Type: TABLE; Schema: payroll; Owner: postgres
--

CREATE TABLE payroll.payroll_period (
    payroll_period_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    year integer NOT NULL,
    month integer NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT payroll_period_month_check CHECK (((month >= 1) AND (month <= 12)))
);


ALTER TABLE payroll.payroll_period OWNER TO postgres;

--
-- TOC entry 316 (class 1259 OID 18117)
-- Name: payroll_run; Type: TABLE; Schema: payroll; Owner: postgres
--

CREATE TABLE payroll.payroll_run (
    payroll_run_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    payroll_period_id uuid NOT NULL,
    status_code character varying(30) DEFAULT 'DRAFT'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    approved_by uuid,
    approved_at timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE payroll.payroll_run OWNER TO postgres;

--
-- TOC entry 390 (class 1259 OID 20103)
-- Name: driverdirectoryread; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.driverdirectoryread (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    driverid uuid NOT NULL,
    employeenumber character varying(100) NOT NULL,
    drivername character varying(250) NOT NULL,
    mobilenumber character varying(50),
    licensenumber character varying(100) NOT NULL,
    licenseexpirydate date,
    vehicleregistrationnumber character varying(100),
    routename character varying(250),
    documentcount integer DEFAULT 0 NOT NULL,
    verifieddocumentcount integer DEFAULT 0 NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL
);


ALTER TABLE public.driverdirectoryread OWNER TO postgres;

--
-- TOC entry 387 (class 1259 OID 20021)
-- Name: schooldocument; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.schooldocument (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    schoolid uuid NOT NULL,
    documenttypeid uuid NOT NULL,
    originalfilename character varying(255) NOT NULL,
    contenttype character varying(150) NOT NULL,
    filesizebytes bigint NOT NULL,
    storageprovider character varying(50) NOT NULL,
    storagekey character varying(500) NOT NULL,
    sha256hash character(64) NOT NULL,
    documentnumber character varying(100),
    issuedon date,
    expireson date,
    isverified boolean DEFAULT false NOT NULL,
    verifiedbyuserid uuid,
    verifiedat timestamp with time zone,
    notes character varying(1000),
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL,
    CONSTRAINT ck_schooldocument_dates CHECK (((expireson IS NULL) OR (issuedon IS NULL) OR (expireson >= issuedon))),
    CONSTRAINT schooldocument_filesizebytes_check CHECK ((filesizebytes > 0))
);


ALTER TABLE public.schooldocument OWNER TO postgres;

--
-- TOC entry 388 (class 1259 OID 20055)
-- Name: studentdirectoryread; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.studentdirectoryread (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    studentid uuid NOT NULL,
    admissionnumber character varying(100) NOT NULL,
    studentname character varying(250) NOT NULL,
    programname character varying(250),
    classname character varying(150),
    sectionname character varying(100),
    primaryguardianname character varying(250),
    primaryguardianmobile character varying(50),
    attendancepercentage numeric(5,2),
    latestexampercentage numeric(5,2),
    outstandingbalance numeric(18,2) DEFAULT 0 NOT NULL,
    documentcount integer DEFAULT 0 NOT NULL,
    verifieddocumentcount integer DEFAULT 0 NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL
);


ALTER TABLE public.studentdirectoryread OWNER TO postgres;

--
-- TOC entry 389 (class 1259 OID 20079)
-- Name: teacherdirectoryread; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.teacherdirectoryread (
    id uuid NOT NULL,
    tenantid uuid NOT NULL,
    teacherid uuid NOT NULL,
    employeenumber character varying(100) NOT NULL,
    teachername character varying(250) NOT NULL,
    jobtitle character varying(150),
    jobgrade character varying(100),
    departmentname character varying(150),
    mobilenumber character varying(50),
    activeclassassignments integer DEFAULT 0 NOT NULL,
    documentcount integer DEFAULT 0 NOT NULL,
    verifieddocumentcount integer DEFAULT 0 NOT NULL,
    isactive boolean DEFAULT true NOT NULL,
    createdat timestamp with time zone NOT NULL,
    updatedat timestamp with time zone,
    rowversion bytea NOT NULL
);


ALTER TABLE public.teacherdirectoryread OWNER TO postgres;

--
-- TOC entry 400 (class 1259 OID 20252)
-- Name: AttendanceStatusType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."AttendanceStatusType" (
    "Id" uuid NOT NULL,
    "Code" character varying(30) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."AttendanceStatusType" OWNER TO postgres;

--
-- TOC entry 394 (class 1259 OID 20168)
-- Name: BloodGroupType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."BloodGroupType" (
    "Id" uuid NOT NULL,
    "Code" character varying(10) NOT NULL,
    "Name" character varying(20) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."BloodGroupType" OWNER TO postgres;

--
-- TOC entry 402 (class 1259 OID 20280)
-- Name: DocumentTypeLookup; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."DocumentTypeLookup" (
    "Id" uuid NOT NULL,
    "Code" character varying(80) NOT NULL,
    "Name" character varying(150) NOT NULL,
    "OwnerCategory" character varying(50) NOT NULL,
    "IsIdentityDocument" boolean DEFAULT false NOT NULL,
    "RequiresExpiryDate" boolean DEFAULT false NOT NULL,
    "RequiresVerification" boolean DEFAULT false NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."DocumentTypeLookup" OWNER TO postgres;

--
-- TOC entry 395 (class 1259 OID 20182)
-- Name: EmploymentStatusType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."EmploymentStatusType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."EmploymentStatusType" OWNER TO postgres;

--
-- TOC entry 396 (class 1259 OID 20196)
-- Name: EmploymentType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."EmploymentType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."EmploymentType" OWNER TO postgres;

--
-- TOC entry 401 (class 1259 OID 20266)
-- Name: ExamType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."ExamType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."ExamType" OWNER TO postgres;

--
-- TOC entry 399 (class 1259 OID 20238)
-- Name: FeeStatusType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."FeeStatusType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."FeeStatusType" OWNER TO postgres;

--
-- TOC entry 393 (class 1259 OID 20154)
-- Name: GenderType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."GenderType" (
    "Id" uuid NOT NULL,
    "Code" character varying(30) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."GenderType" OWNER TO postgres;

--
-- TOC entry 404 (class 1259 OID 20315)
-- Name: LicenseCategoryType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."LicenseCategoryType" (
    "Id" uuid NOT NULL,
    "Code" character varying(30) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."LicenseCategoryType" OWNER TO postgres;

--
-- TOC entry 397 (class 1259 OID 20210)
-- Name: MaritalStatusType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."MaritalStatusType" (
    "Id" uuid NOT NULL,
    "Code" character varying(30) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."MaritalStatusType" OWNER TO postgres;

--
-- TOC entry 391 (class 1259 OID 20126)
-- Name: OccupationType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."OccupationType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(150) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."OccupationType" OWNER TO postgres;

--
-- TOC entry 398 (class 1259 OID 20224)
-- Name: PaymentMethodType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."PaymentMethodType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."PaymentMethodType" OWNER TO postgres;

--
-- TOC entry 392 (class 1259 OID 20140)
-- Name: RelationshipType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."RelationshipType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."RelationshipType" OWNER TO postgres;

--
-- TOC entry 403 (class 1259 OID 20301)
-- Name: VehicleType; Type: TABLE; Schema: reference; Owner: postgres
--

CREATE TABLE reference."VehicleType" (
    "Id" uuid NOT NULL,
    "Code" character varying(50) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "DisplayOrder" integer DEFAULT 0 NOT NULL,
    "IsActive" boolean DEFAULT true NOT NULL
);


ALTER TABLE reference."VehicleType" OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 16453)
-- Name: lookup_type; Type: TABLE; Schema: saas; Owner: postgres
--

CREATE TABLE saas.lookup_type (
    lookup_type_id bigint NOT NULL,
    code character varying(80) NOT NULL,
    name character varying(150) NOT NULL
);


ALTER TABLE saas.lookup_type OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 16452)
-- Name: lookup_type_lookup_type_id_seq; Type: SEQUENCE; Schema: saas; Owner: postgres
--

ALTER TABLE saas.lookup_type ALTER COLUMN lookup_type_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME saas.lookup_type_lookup_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 253 (class 1259 OID 16464)
-- Name: lookup_value; Type: TABLE; Schema: saas; Owner: postgres
--

CREATE TABLE saas.lookup_value (
    lookup_value_id bigint NOT NULL,
    lookup_type_id bigint NOT NULL,
    code character varying(80) NOT NULL,
    name character varying(150) NOT NULL,
    sort_order integer DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    metadata jsonb
);


ALTER TABLE saas.lookup_value OWNER TO postgres;

--
-- TOC entry 252 (class 1259 OID 16463)
-- Name: lookup_value_lookup_value_id_seq; Type: SEQUENCE; Schema: saas; Owner: postgres
--

ALTER TABLE saas.lookup_value ALTER COLUMN lookup_value_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME saas.lookup_value_lookup_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 255 (class 1259 OID 16507)
-- Name: school_branding; Type: TABLE; Schema: saas; Owner: postgres
--

CREATE TABLE saas.school_branding (
    tenant_id uuid NOT NULL,
    logo bytea,
    logo_content_type character varying(150),
    logo_file_name character varying(255),
    small_logo bytea,
    small_logo_content_type character varying(150),
    small_logo_file_name character varying(255),
    favicon bytea,
    favicon_content_type character varying(150),
    favicon_file_name character varying(255),
    certificate_logo bytea,
    certificate_logo_content_type character varying(150),
    certificate_logo_file_name character varying(255),
    letterhead bytea,
    letterhead_content_type character varying(150),
    letterhead_file_name character varying(255),
    watermark bytea,
    watermark_content_type character varying(150),
    watermark_file_name character varying(255),
    primary_color character varying(20),
    secondary_color character varying(20),
    accent_color character varying(20),
    footer_text text,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE saas.school_branding OWNER TO postgres;

--
-- TOC entry 254 (class 1259 OID 16486)
-- Name: tenant; Type: TABLE; Schema: saas; Owner: postgres
--

CREATE TABLE saas.tenant (
    tenant_id uuid DEFAULT gen_random_uuid() NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(200) NOT NULL,
    status_code character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    default_language character varying(10) DEFAULT 'en'::character varying NOT NULL,
    timezone character varying(80) DEFAULT 'Asia/Karachi'::character varying NOT NULL,
    currency_code character(3) DEFAULT 'PKR'::bpchar NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE saas.tenant OWNER TO postgres;

--
-- TOC entry 275 (class 1259 OID 16995)
-- Name: guardian; Type: TABLE; Schema: student; Owner: postgres
--

CREATE TABLE student.guardian (
    guardian_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    user_id uuid,
    full_name character varying(200) NOT NULL,
    cnic_number character varying(20),
    email character varying(250),
    phone character varying(50),
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE student.guardian OWNER TO postgres;

--
-- TOC entry 274 (class 1259 OID 16974)
-- Name: student; Type: TABLE; Schema: student; Owner: postgres
--

CREATE TABLE student.student (
    student_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    user_id uuid,
    student_number character varying(60) NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100),
    date_of_birth date,
    gender character varying(30),
    photo bytea,
    photo_content_type character varying(150),
    photo_file_name character varying(255),
    admission_date date,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE student.student OWNER TO postgres;

--
-- TOC entry 278 (class 1259 OID 17076)
-- Name: student_course_enrollment; Type: TABLE; Schema: student; Owner: postgres
--

CREATE TABLE student.student_course_enrollment (
    student_course_enrollment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_enrollment_id uuid NOT NULL,
    course_offering_id uuid NOT NULL,
    enrollment_type_code character varying(30) NOT NULL,
    selected_at timestamp with time zone DEFAULT now() NOT NULL,
    approved_by uuid,
    approved_at timestamp with time zone,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE student.student_course_enrollment OWNER TO postgres;

--
-- TOC entry 277 (class 1259 OID 17039)
-- Name: student_enrollment; Type: TABLE; Schema: student; Owner: postgres
--

CREATE TABLE student.student_enrollment (
    student_enrollment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    student_id uuid NOT NULL,
    academic_year_id uuid NOT NULL,
    class_section_id uuid NOT NULL,
    enrollment_date date DEFAULT CURRENT_DATE NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE student.student_enrollment OWNER TO postgres;

--
-- TOC entry 276 (class 1259 OID 17013)
-- Name: student_guardian; Type: TABLE; Schema: student; Owner: postgres
--

CREATE TABLE student.student_guardian (
    student_id uuid NOT NULL,
    guardian_id uuid NOT NULL,
    relationship character varying(60) NOT NULL,
    is_primary boolean DEFAULT false NOT NULL,
    can_view_academics boolean DEFAULT true NOT NULL,
    can_view_finance boolean DEFAULT true NOT NULL,
    can_pickup boolean DEFAULT false NOT NULL
);


ALTER TABLE student.student_guardian OWNER TO postgres;

--
-- TOC entry 529 (class 1259 OID 23366)
-- Name: leave_request; Type: TABLE; Schema: teacher; Owner: postgres
--

CREATE TABLE teacher.leave_request (
    leave_request_id uuid NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    leave_type character varying(50) NOT NULL,
    from_date date NOT NULL,
    to_date date NOT NULL,
    reason text NOT NULL,
    status character varying(30) DEFAULT 'PENDING'::character varying NOT NULL,
    approved_by uuid,
    decision_at timestamp with time zone,
    decision_note text,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT leave_request_check CHECK ((to_date >= from_date))
);


ALTER TABLE teacher.leave_request OWNER TO postgres;

--
-- TOC entry 528 (class 1259 OID 23326)
-- Name: teacher_actor; Type: TABLE; Schema: teacher; Owner: postgres
--

CREATE TABLE teacher.teacher_actor (
    teacher_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid NOT NULL,
    user_id uuid,
    primary_campus_id uuid,
    qualification character varying(250),
    specialization character varying(250),
    teaching_experience_years integer,
    max_periods_per_week integer DEFAULT 30 NOT NULL,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone
);


ALTER TABLE teacher.teacher_actor OWNER TO postgres;

--
-- TOC entry 334 (class 1259 OID 18560)
-- Name: driver; Type: TABLE; Schema: transport; Owner: postgres
--

CREATE TABLE transport.driver (
    driver_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    employee_id uuid,
    driver_number character varying(60) NOT NULL,
    full_name character varying(200) NOT NULL,
    cnic_number character varying(20) NOT NULL,
    phone character varying(50),
    alternate_phone character varying(50),
    date_of_birth date,
    driving_license_number character varying(100) NOT NULL,
    driving_license_category character varying(50),
    driving_license_issued_on date,
    driving_license_expires_on date,
    picture bytea,
    picture_content_type character varying(150),
    picture_file_name character varying(255),
    emergency_contact_name character varying(200),
    emergency_contact_phone character varying(50),
    address text,
    hire_date date,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE transport.driver OWNER TO postgres;

--
-- TOC entry 337 (class 1259 OID 18651)
-- Name: route; Type: TABLE; Schema: transport; Owner: postgres
--

CREATE TABLE transport.route (
    route_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(150) NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE transport.route OWNER TO postgres;

--
-- TOC entry 335 (class 1259 OID 18597)
-- Name: vehicle; Type: TABLE; Schema: transport; Owner: postgres
--

CREATE TABLE transport.vehicle (
    vehicle_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid NOT NULL,
    registration_no character varying(80) NOT NULL,
    capacity integer,
    status character varying(30) DEFAULT 'ACTIVE'::character varying NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE transport.vehicle OWNER TO postgres;

--
-- TOC entry 336 (class 1259 OID 18621)
-- Name: vehicle_driver_assignment; Type: TABLE; Schema: transport; Owner: postgres
--

CREATE TABLE transport.vehicle_driver_assignment (
    vehicle_driver_assignment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    vehicle_id uuid NOT NULL,
    driver_id uuid NOT NULL,
    effective_from date NOT NULL,
    effective_to date,
    is_primary boolean DEFAULT true NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL,
    CONSTRAINT vehicle_driver_assignment_check CHECK (((effective_to IS NULL) OR (effective_to >= effective_from)))
);


ALTER TABLE transport.vehicle_driver_assignment OWNER TO postgres;

--
-- TOC entry 319 (class 1259 OID 18186)
-- Name: work_assignment; Type: TABLE; Schema: workflow; Owner: postgres
--

CREATE TABLE workflow.work_assignment (
    work_assignment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    campus_id uuid,
    title character varying(250) NOT NULL,
    description text,
    assigned_by_user_id uuid NOT NULL,
    assigned_to_user_id uuid,
    priority character varying(30) DEFAULT 'NORMAL'::character varying NOT NULL,
    status_code character varying(30) DEFAULT 'ASSIGNED'::character varying NOT NULL,
    assigned_at timestamp with time zone DEFAULT now() NOT NULL,
    due_at timestamp with time zone,
    completed_at timestamp with time zone,
    related_entity_type character varying(100),
    related_entity_id uuid,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT public.gen_random_bytes(8) NOT NULL
);


ALTER TABLE workflow.work_assignment OWNER TO postgres;

--
-- TOC entry 5023 (class 2604 OID 23118)
-- Name: aggregatedcounter id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.aggregatedcounter ALTER COLUMN id SET DEFAULT nextval('hangfire.aggregatedcounter_id_seq'::regclass);


--
-- TOC entry 5006 (class 2604 OID 22937)
-- Name: counter id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.counter ALTER COLUMN id SET DEFAULT nextval('hangfire.counter_id_seq'::regclass);


--
-- TOC entry 5007 (class 2604 OID 22947)
-- Name: hash id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.hash ALTER COLUMN id SET DEFAULT nextval('hangfire.hash_id_seq'::regclass);


--
-- TOC entry 5009 (class 2604 OID 22958)
-- Name: job id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.job ALTER COLUMN id SET DEFAULT nextval('hangfire.job_id_seq'::regclass);


--
-- TOC entry 5020 (class 2604 OID 23011)
-- Name: jobparameter id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.jobparameter ALTER COLUMN id SET DEFAULT nextval('hangfire.jobparameter_id_seq'::regclass);


--
-- TOC entry 5013 (class 2604 OID 23036)
-- Name: jobqueue id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.jobqueue ALTER COLUMN id SET DEFAULT nextval('hangfire.jobqueue_id_seq'::regclass);


--
-- TOC entry 5015 (class 2604 OID 23058)
-- Name: list id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.list ALTER COLUMN id SET DEFAULT nextval('hangfire.list_id_seq'::regclass);


--
-- TOC entry 5018 (class 2604 OID 23068)
-- Name: set id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.set ALTER COLUMN id SET DEFAULT nextval('hangfire.set_id_seq'::regclass);


--
-- TOC entry 5011 (class 2604 OID 22986)
-- Name: state id; Type: DEFAULT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.state ALTER COLUMN id SET DEFAULT nextval('hangfire.state_id_seq'::regclass);


--
-- TOC entry 6243 (class 0 OID 16586)
-- Dependencies: 259
-- Data for Name: academic_system; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.academic_system (academic_system_id, tenant_id, code, name, system_type_code, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6253 (class 0 OID 16822)
-- Dependencies: 269
-- Data for Name: academic_year; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.academic_year (academic_year_id, tenant_id, campus_id, name, start_date, end_date, is_current, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6246 (class 0 OID 16650)
-- Dependencies: 262
-- Data for Name: campus_program; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.campus_program (campus_program_id, tenant_id, campus_id, program_id, effective_from, effective_to, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6256 (class 0 OID 16890)
-- Dependencies: 272
-- Data for Name: class_section; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.class_section (class_section_id, tenant_id, campus_id, academic_year_id, program_grade_id, section_id, class_teacher_employee_id, room_id, capacity, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6257 (class 0 OID 16936)
-- Dependencies: 273
-- Data for Name: course_offering; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.course_offering (course_offering_id, tenant_id, campus_id, academic_year_id, term_id, program_subject_id, display_name, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6251 (class 0 OID 16780)
-- Dependencies: 267
-- Data for Name: course_selection_group; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.course_selection_group (selection_group_id, tenant_id, program_grade_id, name, min_selections, max_selections, requires_approval, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6252 (class 0 OID 16805)
-- Dependencies: 268
-- Data for Name: course_selection_group_course; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.course_selection_group_course (selection_group_id, program_subject_id) FROM stdin;
\.


--
-- TOC entry 6244 (class 0 OID 16606)
-- Dependencies: 260
-- Data for Name: education_board; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.education_board (education_board_id, tenant_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6247 (class 0 OID 16679)
-- Dependencies: 263
-- Data for Name: grade_level; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.grade_level (grade_level_id, tenant_id, code, name, sort_order, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6245 (class 0 OID 16623)
-- Dependencies: 261
-- Data for Name: program; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.program (program_id, tenant_id, academic_system_id, code, name, description, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6248 (class 0 OID 16698)
-- Dependencies: 264
-- Data for Name: program_grade; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.program_grade (program_grade_id, tenant_id, program_id, grade_level_id, sort_order, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6250 (class 0 OID 16748)
-- Dependencies: 266
-- Data for Name: program_subject; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.program_subject (program_subject_id, tenant_id, program_grade_id, subject_id, requirement_type_code, periods_per_week, minimum_pass_marks, display_order, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6255 (class 0 OID 16873)
-- Dependencies: 271
-- Data for Name: section; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.section (section_id, tenant_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6249 (class 0 OID 16727)
-- Dependencies: 265
-- Data for Name: subject; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.subject (subject_id, tenant_id, code, name, short_name, is_practical, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6270 (class 0 OID 17306)
-- Dependencies: 286
-- Data for Name: teacher_course_assignment; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.teacher_course_assignment (teacher_course_assignment_id, tenant_id, course_offering_id, employee_id, class_section_id, teaching_group_id, assignment_role, periods_per_week, effective_from, effective_to, is_primary, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6278 (class 0 OID 17496)
-- Dependencies: 294
-- Data for Name: teaching_group; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.teaching_group (teaching_group_id, tenant_id, academic_year_id, term_id, course_offering_id, name, capacity, room_id, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6279 (class 0 OID 17539)
-- Dependencies: 295
-- Data for Name: teaching_group_student; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.teaching_group_student (teaching_group_id, student_course_enrollment_id) FROM stdin;
\.


--
-- TOC entry 6254 (class 0 OID 16848)
-- Dependencies: 270
-- Data for Name: term; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.term (term_id, tenant_id, academic_year_id, code, name, start_date, end_date, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6281 (class 0 OID 17580)
-- Dependencies: 297
-- Data for Name: timetable; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.timetable (timetable_id, tenant_id, campus_id, academic_year_id, term_id, name, effective_from, effective_to, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6282 (class 0 OID 17613)
-- Dependencies: 298
-- Data for Name: timetable_entry; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.timetable_entry (timetable_entry_id, timetable_id, day_of_week, timetable_period_id, class_section_id, teaching_group_id, course_offering_id, teacher_course_assignment_id, room_id, entry_type) FROM stdin;
\.


--
-- TOC entry 6280 (class 0 OID 17556)
-- Dependencies: 296
-- Data for Name: timetable_period; Type: TABLE DATA; Schema: academic; Owner: postgres
--

COPY academic.timetable_period (timetable_period_id, tenant_id, campus_id, period_number, name, start_time, end_time, period_type, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6306 (class 0 OID 18295)
-- Dependencies: 322
-- Data for Name: activity; Type: TABLE DATA; Schema: activity; Owner: postgres
--

COPY activity.activity (activity_id, tenant_id, campus_id, name, category, coordinator_employee_id, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6307 (class 0 OID 18321)
-- Dependencies: 323
-- Data for Name: student_activity; Type: TABLE DATA; Schema: activity; Owner: postgres
--

COPY activity.student_activity (activity_id, student_id, role_name, joined_at, left_at) FROM stdin;
\.


--
-- TOC entry 6308 (class 0 OID 18338)
-- Dependencies: 324
-- Data for Name: student_award; Type: TABLE DATA; Schema: activity; Owner: postgres
--

COPY activity.student_award (student_award_id, tenant_id, student_id, award_type_code, title, description, award_date, approved_by, generated_document_id, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6354 (class 0 OID 19481)
-- Dependencies: 370
-- Data for Name: class_performance_insight; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.class_performance_insight (class_performance_insight_id, tenant_id, academic_year_id, term_id, class_section_id, course_offering_id, teacher_employee_id, students_count, on_track_count, needs_attention_count, high_risk_count, predicted_class_average, current_class_average, trend, summary, generated_at, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6358 (class 0 OID 19663)
-- Dependencies: 374
-- Data for Name: intervention_action; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.intervention_action (intervention_action_id, student_intervention_id, sequence_no, action_type, description, related_entity_type, related_entity_id, due_at, completed_at, status) FROM stdin;
\.


--
-- TOC entry 6359 (class 0 OID 19685)
-- Dependencies: 375
-- Data for Name: intervention_outcome; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.intervention_outcome (intervention_outcome_id, student_intervention_id, measured_at, before_score, after_score, improvement, outcome_status, teacher_notes) FROM stdin;
\.


--
-- TOC entry 6353 (class 0 OID 19463)
-- Dependencies: 369
-- Data for Name: predicted_grade_probability; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.predicted_grade_probability (predicted_grade_probability_id, student_performance_prediction_id, grade, probability) FROM stdin;
\.


--
-- TOC entry 6350 (class 0 OID 19347)
-- Dependencies: 366
-- Data for Name: prediction; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.prediction (prediction_id, tenant_id, prediction_model_id, student_id, prediction_type, score, risk_level, explanation, predicted_at, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6360 (class 0 OID 19702)
-- Dependencies: 376
-- Data for Name: prediction_evaluation; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.prediction_evaluation (prediction_evaluation_id, student_performance_prediction_id, student_exam_result_id, predicted_percentage, actual_percentage, absolute_error, predicted_grade, actual_grade, grade_correct, evaluated_at) FROM stdin;
\.


--
-- TOC entry 6352 (class 0 OID 19447)
-- Dependencies: 368
-- Data for Name: prediction_evidence; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.prediction_evidence (prediction_evidence_id, student_performance_prediction_id, evidence_type, source_entity_type, source_entity_id, numeric_value, text_value, normalized_value, weight, occurred_at, explanation) FROM stdin;
\.


--
-- TOC entry 6349 (class 0 OID 19330)
-- Dependencies: 365
-- Data for Name: prediction_model; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.prediction_model (prediction_model_id, tenant_id, code, name, prediction_type, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6357 (class 0 OID 19612)
-- Dependencies: 373
-- Data for Name: student_intervention; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.student_intervention (student_intervention_id, tenant_id, student_id, subject_id, course_offering_id, teacher_employee_id, source_prediction_id, source_recommendation_id, title, reason, target_outcome, start_date, target_date, status, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6351 (class 0 OID 19382)
-- Dependencies: 367
-- Data for Name: student_performance_prediction; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.student_performance_prediction (student_performance_prediction_id, tenant_id, student_id, academic_year_id, term_id, course_offering_id, subject_id, target_exam_id, target_exam_subject_id, target_exam_type_code, target_date, predicted_marks, predicted_percentage, predicted_grade, lower_bound_percentage, upper_bound_percentage, confidence_score, pass_probability, fail_probability, target_grade, target_grade_probability, trend, risk_level, explanation_summary, explanation, prediction_model_id, model_version, generated_at, expires_at, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6361 (class 0 OID 19725)
-- Dependencies: 377
-- Data for Name: student_progress_recommendation; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.student_progress_recommendation (student_progress_recommendation_id, tenant_id, student_id, prediction_id, audience, title, recommendation_text, priority, status, generated_at, expires_at, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6356 (class 0 OID 19560)
-- Dependencies: 372
-- Data for Name: teaching_recommendation; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.teaching_recommendation (teaching_recommendation_id, tenant_id, class_performance_insight_id, class_section_id, course_offering_id, teacher_employee_id, subject_id, topic, recommendation_type, title, recommendation_text, rationale, priority, status, generated_at, reviewed_at, reviewed_by, teacher_comments, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6355 (class 0 OID 19534)
-- Dependencies: 371
-- Data for Name: topic_performance_insight; Type: TABLE DATA; Schema: ai; Owner: postgres
--

COPY ai.topic_performance_insight (topic_performance_insight_id, class_performance_insight_id, subject_id, topic, average_mastery_score, students_struggling_count, students_mastered_count, risk_level, recommended_focus) FROM stdin;
\.


--
-- TOC entry 6397 (class 0 OID 20792)
-- Dependencies: 413
-- Data for Name: RagKnowledgeChunks; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core."RagKnowledgeChunks" ("Id", "TenantId", "DocumentId", "ChunkIndex", "Content", "CitationLabel", "Embedding") FROM stdin;
\.


--
-- TOC entry 6396 (class 0 OID 20778)
-- Dependencies: 412
-- Data for Name: RagKnowledgeDocuments; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core."RagKnowledgeDocuments" ("Id", "TenantId", "CollectionId", "Title", "SourceName", "Audience", "ContentHash", "IsApproved", "IndexedAt") FROM stdin;
\.


--
-- TOC entry 6330 (class 0 OID 18858)
-- Dependencies: 346
-- Data for Name: ai_execution_log; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.ai_execution_log (ai_execution_log_id, tenant_id, assistant_type, conversation_reference_id, user_id, model_configuration_id, prompt_tokens, completion_tokens, total_tokens, estimated_cost, latency_ms, status, correlation_id, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6329 (class 0 OID 18836)
-- Dependencies: 345
-- Data for Name: assistant_knowledge_collection; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.assistant_knowledge_collection (assistant_knowledge_collection_id, tenant_id, assistant_type, knowledge_collection_id, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6328 (class 0 OID 18813)
-- Dependencies: 344
-- Data for Name: assistant_tool; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.assistant_tool (assistant_tool_id, tenant_id, assistant_type, tool_definition_id, is_enabled, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6326 (class 0 OID 18774)
-- Dependencies: 342
-- Data for Name: knowledge_chunk; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.knowledge_chunk (knowledge_chunk_id, knowledge_document_id, chunk_index, content, metadata, embedding_reference, embedding) FROM stdin;
\.


--
-- TOC entry 6324 (class 0 OID 18719)
-- Dependencies: 340
-- Data for Name: knowledge_collection; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.knowledge_collection (knowledge_collection_id, tenant_id, code, name, description, access_scope, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6325 (class 0 OID 18740)
-- Dependencies: 341
-- Data for Name: knowledge_document; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.knowledge_document (knowledge_document_id, knowledge_collection_id, tenant_id, campus_id, academic_system_id, title, document_type, source_url, metadata, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6322 (class 0 OID 18674)
-- Dependencies: 338
-- Data for Name: model_configuration; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.model_configuration (model_configuration_id, tenant_id, code, provider, model_name, configuration, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6323 (class 0 OID 18695)
-- Dependencies: 339
-- Data for Name: prompt_template; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.prompt_template (prompt_template_id, tenant_id, assistant_type, prompt_type, code, prompt_text, version, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6425 (class 0 OID 23305)
-- Dependencies: 527
-- Data for Name: rag_knowledge_chunk; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.rag_knowledge_chunk (id, tenant_id, collection, document_name, content, embedding, created_at, is_active) FROM stdin;
\.


--
-- TOC entry 6327 (class 0 OID 18793)
-- Dependencies: 343
-- Data for Name: tool_definition; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.tool_definition (tool_definition_id, code, name, description, handler_key, requires_user_authorization, requires_human_approval, is_active) FROM stdin;
ee8cbb19-1d36-42de-872b-6e3490f07270	GET_STUDENT_SUBJECTS	Get Student Subjects	\N	Tutor.GetStudentSubjects	t	f	t
57fd4968-1cde-4040-a86a-1772eadae55d	SEARCH_COURSE_MATERIAL	Search Course Material	\N	Tutor.SearchCourseMaterial	t	f	t
02095f67-5eeb-4d16-ab09-378e4773d7b2	GENERATE_PRACTICE_QUIZ	Generate Practice Quiz	\N	Tutor.GeneratePracticeQuiz	t	f	t
b4372afb-b63f-4c64-9ea9-4e4c13be176d	GET_STUDENT_PROGRESS	Get Student Progress	\N	Tutor.GetStudentProgress	t	f	t
9476acce-5d22-4d86-afab-b5900bc5b74a	GET_PROGRAMS	Get School Programs	\N	Inquiry.GetPrograms	f	f	t
4ac125e6-e496-45aa-a95d-95b849f939f5	GET_ADMISSION_INFO	Get Admission Information	\N	Inquiry.GetAdmissionInfo	f	f	t
0d872c40-504e-46af-9cff-0aa213ec60d3	CREATE_ADMISSION_INQUIRY	Create Admission Inquiry	\N	Inquiry.CreateAdmissionInquiry	f	t	t
f456430b-7cb4-4735-8f92-729eddc2f047	REQUEST_HUMAN_HANDOFF	Request Human Handoff	\N	Inquiry.RequestHumanHandoff	f	f	t
e85f1e63-7abc-40d0-b0c3-c12ad1dfe598	GET_CHILD_ATTENDANCE	Get Child Attendance	\N	Parent.GetChildAttendance	t	f	t
72691771-74e7-4077-aa0f-d1f9c4d84539	GET_CHILD_RESULTS	Get Child Results	\N	Parent.GetChildResults	t	f	t
b04f1959-6ce6-4ded-b920-170d6b0f090b	GET_CHILD_TIMETABLE	Get Child Timetable	\N	Parent.GetChildTimetable	t	f	t
42bd163f-a7de-4bdd-bc38-3681ddb7afb9	GET_CHILD_FEE_BALANCE	Get Child Fee Balance	\N	Parent.GetChildFeeBalance	t	f	t
\.


--
-- TOC entry 6331 (class 0 OID 18880)
-- Dependencies: 347
-- Data for Name: tool_execution; Type: TABLE DATA; Schema: ai_core; Owner: postgres
--

COPY ai_core.tool_execution (tool_execution_id, ai_execution_log_id, tool_definition_id, input_payload, output_payload, status, error_message, started_at, completed_at) FROM stdin;
\.


--
-- TOC entry 6345 (class 0 OID 19235)
-- Dependencies: 361
-- Data for Name: human_handoff; Type: TABLE DATA; Schema: ai_inquiry; Owner: postgres
--

COPY ai_inquiry.human_handoff (human_handoff_id, inquiry_conversation_id, requested_at, reason, assigned_to_user_id, accepted_at, resolved_at, status) FROM stdin;
\.


--
-- TOC entry 6342 (class 0 OID 19154)
-- Dependencies: 358
-- Data for Name: inquiry_conversation; Type: TABLE DATA; Schema: ai_inquiry; Owner: postgres
--

COPY ai_inquiry.inquiry_conversation (inquiry_conversation_id, tenant_id, campus_id, visitor_session_id, user_id, visitor_name, phone, email, interested_program_id, started_at, ended_at, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6343 (class 0 OID 19184)
-- Dependencies: 359
-- Data for Name: inquiry_message; Type: TABLE DATA; Schema: ai_inquiry; Owner: postgres
--

COPY ai_inquiry.inquiry_message (inquiry_message_id, inquiry_conversation_id, role, content, created_at) FROM stdin;
\.


--
-- TOC entry 6344 (class 0 OID 19203)
-- Dependencies: 360
-- Data for Name: lead_capture; Type: TABLE DATA; Schema: ai_inquiry; Owner: postgres
--

COPY ai_inquiry.lead_capture (lead_capture_id, inquiry_conversation_id, name, phone, email, interested_campus_id, interested_program_id, interested_grade_id, notes, captured_at, converted_inquiry_id) FROM stdin;
\.


--
-- TOC entry 6346 (class 0 OID 19254)
-- Dependencies: 362
-- Data for Name: parent_conversation; Type: TABLE DATA; Schema: ai_parent; Owner: postgres
--

COPY ai_parent.parent_conversation (parent_conversation_id, tenant_id, guardian_id, selected_student_id, title, started_at, ended_at, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6347 (class 0 OID 19282)
-- Dependencies: 363
-- Data for Name: parent_message; Type: TABLE DATA; Schema: ai_parent; Owner: postgres
--

COPY ai_parent.parent_message (parent_message_id, parent_conversation_id, role, content, created_at) FROM stdin;
\.


--
-- TOC entry 6348 (class 0 OID 19301)
-- Dependencies: 364
-- Data for Name: parent_tool_execution; Type: TABLE DATA; Schema: ai_parent; Owner: postgres
--

COPY ai_parent.parent_tool_execution (parent_tool_execution_id, parent_conversation_id, tool_definition_id, student_id, input_payload, output_payload, status, executed_at) FROM stdin;
\.


--
-- TOC entry 6339 (class 0 OID 19079)
-- Dependencies: 355
-- Data for Name: generated_quiz; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.generated_quiz (generated_quiz_id, tenant_id, student_id, subject_id, tutor_conversation_id, topic, difficulty, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6340 (class 0 OID 19111)
-- Dependencies: 356
-- Data for Name: generated_quiz_question; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.generated_quiz_question (generated_quiz_question_id, generated_quiz_id, sequence_no, question_text, question_type, options, expected_answer, explanation) FROM stdin;
\.


--
-- TOC entry 6338 (class 0 OID 19051)
-- Dependencies: 354
-- Data for Name: learning_recommendation; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.learning_recommendation (learning_recommendation_id, student_id, subject_id, topic, recommendation_type, recommendation_text, priority, status, created_at) FROM stdin;
\.


--
-- TOC entry 6341 (class 0 OID 19131)
-- Dependencies: 357
-- Data for Name: student_quiz_attempt; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.student_quiz_attempt (student_quiz_attempt_id, generated_quiz_id, student_id, started_at, completed_at, score, answers) FROM stdin;
\.


--
-- TOC entry 6337 (class 0 OID 19019)
-- Dependencies: 353
-- Data for Name: student_topic_mastery; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.student_topic_mastery (student_topic_mastery_id, tenant_id, student_id, subject_id, topic, mastery_score, confidence_score, evidence_count, last_assessed_at, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6332 (class 0 OID 18903)
-- Dependencies: 348
-- Data for Name: tutor_conversation; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.tutor_conversation (tutor_conversation_id, tenant_id, student_id, academic_year_id, course_offering_id, subject_id, title, started_at, ended_at, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6336 (class 0 OID 18995)
-- Dependencies: 352
-- Data for Name: tutor_feedback; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.tutor_feedback (tutor_feedback_id, tutor_message_id, student_id, rating, was_helpful, comments, created_at) FROM stdin;
\.


--
-- TOC entry 6333 (class 0 OID 18941)
-- Dependencies: 349
-- Data for Name: tutor_message; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.tutor_message (tutor_message_id, tutor_conversation_id, role, content, created_at) FROM stdin;
\.


--
-- TOC entry 6334 (class 0 OID 18960)
-- Dependencies: 350
-- Data for Name: tutor_message_reference; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.tutor_message_reference (tutor_message_reference_id, tutor_message_id, knowledge_chunk_id, citation_label, relevance_score) FROM stdin;
\.


--
-- TOC entry 6335 (class 0 OID 18978)
-- Dependencies: 351
-- Data for Name: tutor_session; Type: TABLE DATA; Schema: ai_tutor; Owner: postgres
--

COPY ai_tutor.tutor_session (tutor_session_id, tutor_conversation_id, topic, learning_objective, started_at, ended_at, session_summary) FROM stdin;
\.


--
-- TOC entry 6363 (class 0 OID 19770)
-- Dependencies: 379
-- Data for Name: audit_log; Type: TABLE DATA; Schema: audit; Owner: postgres
--

COPY audit.audit_log (audit_log_id, tenant_id, user_id, action, entity_type, entity_id, old_values, new_values, ip_address, correlation_id, occurred_at) FROM stdin;
\.


--
-- TOC entry 6393 (class 0 OID 20395)
-- Dependencies: 409
-- Data for Name: ChatAttachments; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."ChatAttachments" ("Id", "TenantId", "MessageId", "FileName", "ContentType", "FileSizeBytes", "StorageKey") FROM stdin;
\.


--
-- TOC entry 6390 (class 0 OID 20342)
-- Dependencies: 406
-- Data for Name: ChatConversations; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."ChatConversations" ("Id", "TenantId", "Title", "ConversationType", "CreatedByUserId", "RelatedEntityId", "RelatedEntityType", "IsClosed", "ClosedAt") FROM stdin;
\.


--
-- TOC entry 6392 (class 0 OID 20374)
-- Dependencies: 408
-- Data for Name: ChatMessages; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."ChatMessages" ("Id", "TenantId", "ConversationId", "SenderUserId", "MessageType", "Message", "ReplyToMessageId", "SentAt", "EditedAt", "IsDeleted") FROM stdin;
\.


--
-- TOC entry 6391 (class 0 OID 20354)
-- Dependencies: 407
-- Data for Name: ChatParticipants; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."ChatParticipants" ("Id", "TenantId", "ConversationId", "UserId", "Role", "JoinedAt", "LastReadAt", "IsMuted") FROM stdin;
\.


--
-- TOC entry 6395 (class 0 OID 20431)
-- Dependencies: 411
-- Data for Name: NotificationPreferences; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."NotificationPreferences" ("Id", "TenantId", "UserId", "NotificationType", "InAppEnabled", "PushEnabled", "EmailEnabled", "SmsEnabled") FROM stdin;
\.


--
-- TOC entry 6389 (class 0 OID 20329)
-- Dependencies: 405
-- Data for Name: NotificationTypeLookup; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."NotificationTypeLookup" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
31000000-0000-0000-0000-000000000001	EXAMSCHEDULED	ExamScheduled	1	t
31000000-0000-0000-0000-000000000002	EXAMSTARTINGSOON	ExamStartingSoon	2	t
31000000-0000-0000-0000-000000000003	EXAMRESULTPUBLISHED	ExamResultPublished	3	t
31000000-0000-0000-0000-000000000004	TIMETABLEPUBLISHED	TimetablePublished	4	t
31000000-0000-0000-0000-000000000005	TIMETABLECHANGED	TimetableChanged	5	t
31000000-0000-0000-0000-000000000006	CLASSTIMINGCHANGED	ClassTimingChanged	6	t
31000000-0000-0000-0000-000000000007	EIDHOLIDAY	EidHoliday	7	t
31000000-0000-0000-0000-000000000008	SUMMERVACATION	SummerVacation	8	t
31000000-0000-0000-0000-000000000009	WINTERVACATION	WinterVacation	9	t
31000000-0000-0000-0000-000000000010	PUBLICHOLIDAY	PublicHoliday	10	t
31000000-0000-0000-0000-000000000011	FEEDUE	FeeDue	11	t
31000000-0000-0000-0000-000000000012	FEEOVERDUE	FeeOverdue	12	t
31000000-0000-0000-0000-000000000013	FEESUBMITTED	FeeSubmitted	13	t
31000000-0000-0000-0000-000000000014	FEEPAYMENTCONFIRMED	FeePaymentConfirmed	14	t
31000000-0000-0000-0000-000000000015	FEEWAIVED	FeeWaived	15	t
31000000-0000-0000-0000-000000000016	LEAVESUBMITTED	LeaveSubmitted	16	t
31000000-0000-0000-0000-000000000017	LEAVEACCEPTED	LeaveAccepted	17	t
31000000-0000-0000-0000-000000000018	LEAVEREJECTED	LeaveRejected	18	t
31000000-0000-0000-0000-000000000019	LEAVECANCELLED	LeaveCancelled	19	t
31000000-0000-0000-0000-000000000020	ADMISSIONSUBMITTED	AdmissionSubmitted	20	t
31000000-0000-0000-0000-000000000021	ADMISSIONACCEPTED	AdmissionAccepted	21	t
31000000-0000-0000-0000-000000000022	ADMISSIONREJECTED	AdmissionRejected	22	t
31000000-0000-0000-0000-000000000023	ADMISSIONTERMINATED	AdmissionTerminated	23	t
31000000-0000-0000-0000-000000000024	EVENTCREATED	EventCreated	24	t
31000000-0000-0000-0000-000000000025	EVENTPOSTPONED	EventPostponed	25	t
31000000-0000-0000-0000-000000000026	EVENTCANCELLED	EventCancelled	26	t
31000000-0000-0000-0000-000000000027	EVENTREMINDER	EventReminder	27	t
31000000-0000-0000-0000-000000000028	ATTENDANCEABSENT	AttendanceAbsent	28	t
31000000-0000-0000-0000-000000000029	ATTENDANCELATE	AttendanceLate	29	t
31000000-0000-0000-0000-000000000030	ATTENDANCESUMMARY	AttendanceSummary	30	t
31000000-0000-0000-0000-000000000031	ASSIGNMENTCREATED	AssignmentCreated	31	t
31000000-0000-0000-0000-000000000032	ASSIGNMENTDUE	AssignmentDue	32	t
31000000-0000-0000-0000-000000000033	ASSIGNMENTGRADED	AssignmentGraded	33	t
31000000-0000-0000-0000-000000000034	ANNOUNCEMENTPUBLISHED	AnnouncementPublished	34	t
31000000-0000-0000-0000-000000000035	EMERGENCYALERT	EmergencyAlert	35	t
31000000-0000-0000-0000-000000000036	TRANSPORTDELAY	TransportDelay	36	t
31000000-0000-0000-0000-000000000037	TRANSPORTROUTECHANGED	TransportRouteChanged	37	t
31000000-0000-0000-0000-000000000038	PARENTTEACHERMEETING	ParentTeacherMeeting	38	t
31000000-0000-0000-0000-000000000039	TEACHERSUBSTITUTION	TeacherSubstitution	39	t
31000000-0000-0000-0000-000000000040	REPORTCARDPUBLISHED	ReportCardPublished	40	t
31000000-0000-0000-0000-000000000041	GENERAL	General	41	t
\.


--
-- TOC entry 6394 (class 0 OID 20414)
-- Dependencies: 410
-- Data for Name: Notifications; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication."Notifications" ("Id", "TenantId", "RecipientUserId", "Type", "Title", "Message", "RelatedEntityId", "RelatedEntityType", "ActionUrl", "Priority", "IsRead", "ReadAt", "OccurredAt") FROM stdin;
\.


--
-- TOC entry 6422 (class 0 OID 23238)
-- Dependencies: 524
-- Data for Name: chat_conversation; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.chat_conversation ("Id", "TenantId", "Title", "ConversationType", "CreatedByUserId", "RelatedEntityId", "RelatedEntityType", "IsClosed", "ClosedAt", "IsActive", "CreatedAt", "UpdatedAt", "RowVersion") FROM stdin;
\.


--
-- TOC entry 6424 (class 0 OID 23281)
-- Dependencies: 526
-- Data for Name: chat_message; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.chat_message ("Id", "TenantId", "ConversationId", "SenderUserId", "MessageType", "Message", "ReplyToMessageId", "SentAt", "EditedAt", "IsDeleted", "IsActive", "CreatedAt", "UpdatedAt", "RowVersion") FROM stdin;
\.


--
-- TOC entry 6423 (class 0 OID 23258)
-- Dependencies: 525
-- Data for Name: chat_participant; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.chat_participant ("Id", "TenantId", "ConversationId", "UserId", "Role", "JoinedAt", "LastReadAt", "IsMuted", "IsActive", "CreatedAt", "UpdatedAt", "RowVersion") FROM stdin;
\.


--
-- TOC entry 6309 (class 0 OID 18367)
-- Dependencies: 325
-- Data for Name: conversation; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.conversation (conversation_id, tenant_id, campus_id, conversation_type_code, student_id, class_section_id, subject_id, title, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6310 (class 0 OID 18403)
-- Dependencies: 326
-- Data for Name: conversation_participant; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.conversation_participant (conversation_id, user_id, joined_at, left_at) FROM stdin;
\.


--
-- TOC entry 6311 (class 0 OID 18417)
-- Dependencies: 327
-- Data for Name: message; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.message (message_id, conversation_id, sender_user_id, reply_to_message_id, message_type_code, body, sent_at, edited_at, deleted_at) FROM stdin;
\.


--
-- TOC entry 6312 (class 0 OID 18442)
-- Dependencies: 328
-- Data for Name: message_receipt; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.message_receipt (message_id, user_id, delivered_at, read_at) FROM stdin;
\.


--
-- TOC entry 6313 (class 0 OID 18454)
-- Dependencies: 329
-- Data for Name: notification; Type: TABLE DATA; Schema: communication; Owner: postgres
--

COPY communication.notification (notification_id, tenant_id, user_id, title, body, channel_code, status, created_at, sent_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6369 (class 0 OID 19953)
-- Dependencies: 385
-- Data for Name: candidatedocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.candidatedocument (id, tenantid, candidateid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6304 (class 0 OID 18214)
-- Dependencies: 320
-- Data for Name: document_template; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.document_template (document_template_id, tenant_id, campus_id, academic_system_id, document_type_code, code, name, subject_template, header_html, body_html, footer_html, language_code, version, requires_approval, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6364 (class 0 OID 19793)
-- Dependencies: 380
-- Data for Name: documenttype; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.documenttype (id, tenantid, code, name, ownercategory, isidentitydocument, requiresexpirydate, requiresverification, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6370 (class 0 OID 19987)
-- Dependencies: 386
-- Data for Name: driverdocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.driverdocument (id, tenantid, driverid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6368 (class 0 OID 19919)
-- Dependencies: 384
-- Data for Name: employeedocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.employeedocument (id, tenantid, employeeid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6305 (class 0 OID 18255)
-- Dependencies: 321
-- Data for Name: generated_document; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.generated_document (generated_document_id, tenant_id, document_template_id, template_version, student_id, employee_id, document_number, rendered_content_snapshot, file_url, verification_code, issued_by, approved_by, issued_at, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6366 (class 0 OID 19851)
-- Dependencies: 382
-- Data for Name: parentdocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.parentdocument (id, tenantid, parentid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6365 (class 0 OID 19817)
-- Dependencies: 381
-- Data for Name: studentdocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.studentdocument (id, tenantid, studentid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6367 (class 0 OID 19885)
-- Dependencies: 383
-- Data for Name: teacherdocument; Type: TABLE DATA; Schema: document; Owner: postgres
--

COPY document.teacherdocument (id, tenantid, teacherid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6285 (class 0 OID 17736)
-- Dependencies: 301
-- Data for Name: exam; Type: TABLE DATA; Schema: exam; Owner: postgres
--

COPY exam.exam (exam_id, tenant_id, campus_id, academic_year_id, term_id, academic_system_id, exam_type_code, name, start_date, end_date, result_publish_date, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6286 (class 0 OID 17776)
-- Dependencies: 302
-- Data for Name: exam_subject; Type: TABLE DATA; Schema: exam; Owner: postgres
--

COPY exam.exam_subject (exam_subject_id, exam_id, course_offering_id, exam_date, start_time, duration_minutes, total_marks, passing_marks, room_id) FROM stdin;
\.


--
-- TOC entry 6287 (class 0 OID 17801)
-- Dependencies: 303
-- Data for Name: student_exam_result; Type: TABLE DATA; Schema: exam; Owner: postgres
--

COPY exam.student_exam_result (student_exam_result_id, exam_subject_id, student_id, marks_obtained, percentage, grade, is_absent, remarks, entered_by, verified_by) FROM stdin;
\.


--
-- TOC entry 6288 (class 0 OID 17826)
-- Dependencies: 304
-- Data for Name: fee_type; Type: TABLE DATA; Schema: finance; Owner: postgres
--

COPY finance.fee_type (fee_type_id, tenant_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6292 (class 0 OID 17923)
-- Dependencies: 308
-- Data for Name: payment_allocation; Type: TABLE DATA; Schema: finance; Owner: postgres
--

COPY finance.payment_allocation (payment_allocation_id, student_payment_id, student_invoice_id, amount) FROM stdin;
\.


--
-- TOC entry 6289 (class 0 OID 17843)
-- Dependencies: 305
-- Data for Name: student_invoice; Type: TABLE DATA; Schema: finance; Owner: postgres
--

COPY finance.student_invoice (student_invoice_id, tenant_id, student_id, academic_year_id, invoice_number, invoice_date, due_date, status, total_amount, balance_amount, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6290 (class 0 OID 17877)
-- Dependencies: 306
-- Data for Name: student_invoice_line; Type: TABLE DATA; Schema: finance; Owner: postgres
--

COPY finance.student_invoice_line (student_invoice_line_id, student_invoice_id, fee_type_id, description, amount) FROM stdin;
\.


--
-- TOC entry 6291 (class 0 OID 17897)
-- Dependencies: 307
-- Data for Name: student_payment; Type: TABLE DATA; Schema: finance; Owner: postgres
--

COPY finance.student_payment (student_payment_id, tenant_id, student_id, payment_number, payment_date, amount, payment_method, reference_no, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6419 (class 0 OID 23115)
-- Dependencies: 521
-- Data for Name: aggregatedcounter; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.aggregatedcounter (id, key, value, expireat) FROM stdin;
\.


--
-- TOC entry 6401 (class 0 OID 22768)
-- Dependencies: 503
-- Data for Name: counter; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.counter (id, key, value, expireat) FROM stdin;
\.


--
-- TOC entry 6403 (class 0 OID 22779)
-- Dependencies: 505
-- Data for Name: hash; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.hash (id, key, field, value, expireat, updatecount) FROM stdin;
\.


--
-- TOC entry 6405 (class 0 OID 22793)
-- Dependencies: 507
-- Data for Name: job; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.job (id, stateid, statename, invocationdata, arguments, createdat, expireat, updatecount) FROM stdin;
\.


--
-- TOC entry 6416 (class 0 OID 22872)
-- Dependencies: 518
-- Data for Name: jobparameter; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.jobparameter (id, jobid, name, value, updatecount) FROM stdin;
\.


--
-- TOC entry 6409 (class 0 OID 22826)
-- Dependencies: 511
-- Data for Name: jobqueue; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.jobqueue (id, jobid, queue, fetchedat, updatecount) FROM stdin;
\.


--
-- TOC entry 6411 (class 0 OID 22837)
-- Dependencies: 513
-- Data for Name: list; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.list (id, key, value, expireat, updatecount) FROM stdin;
\.


--
-- TOC entry 6417 (class 0 OID 22889)
-- Dependencies: 519
-- Data for Name: lock; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.lock (resource, updatecount, acquired) FROM stdin;
\.


--
-- TOC entry 6399 (class 0 OID 22761)
-- Dependencies: 501
-- Data for Name: schema; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.schema (version) FROM stdin;
23
\.


--
-- TOC entry 6412 (class 0 OID 22847)
-- Dependencies: 514
-- Data for Name: server; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.server (id, data, lastheartbeat, updatecount) FROM stdin;
\.


--
-- TOC entry 6414 (class 0 OID 22857)
-- Dependencies: 516
-- Data for Name: set; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.set (id, key, score, value, expireat, updatecount) FROM stdin;
\.


--
-- TOC entry 6407 (class 0 OID 22807)
-- Dependencies: 509
-- Data for Name: state; Type: TABLE DATA; Schema: hangfire; Owner: postgres
--

COPY hangfire.state (id, jobid, name, reason, createdat, data, updatecount) FROM stdin;
\.


--
-- TOC entry 6271 (class 0 OID 17340)
-- Dependencies: 287
-- Data for Name: candidate; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.candidate (candidate_id, tenant_id, first_name, last_name, email, phone, current_job_title, current_employer, total_experience_years, highest_qualification, expected_salary, notice_period_days, status_code, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6272 (class 0 OID 17360)
-- Dependencies: 288
-- Data for Name: candidate_document; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.candidate_document (candidate_document_id, candidate_id, document_type, file_name, file_url, mime_type, size_bytes, uploaded_at) FROM stdin;
\.


--
-- TOC entry 6268 (class 0 OID 17245)
-- Dependencies: 284
-- Data for Name: employee; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.employee (employee_id, tenant_id, user_id, employee_number, first_name, last_name, cnic_number, photo, photo_content_type, photo_file_name, email, phone, hire_date, employment_type_code, status, source_candidate_id, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6294 (class 0 OID 17967)
-- Dependencies: 310
-- Data for Name: employee_compensation; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.employee_compensation (employee_compensation_id, tenant_id, employee_id, job_grade_id, effective_from, effective_to, basic_salary, gross_salary, currency_code, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6269 (class 0 OID 17275)
-- Dependencies: 285
-- Data for Name: employee_position; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.employee_position (employee_position_id, tenant_id, employee_id, position_id, effective_from, effective_to, is_primary, change_reason, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6295 (class 0 OID 17997)
-- Dependencies: 311
-- Data for Name: employee_salary_component; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.employee_salary_component (employee_compensation_id, salary_component_id, amount, percentage, formula) FROM stdin;
\.


--
-- TOC entry 6298 (class 0 OID 18080)
-- Dependencies: 314
-- Data for Name: increment_approval; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.increment_approval (increment_approval_id, increment_request_id, approval_level, approver_user_id, decision, comments, decision_at) FROM stdin;
\.


--
-- TOC entry 6296 (class 0 OID 18016)
-- Dependencies: 312
-- Data for Name: increment_policy; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.increment_policy (increment_policy_id, tenant_id, name, frequency, increment_type_code, increment_value, minimum_service_months, minimum_performance_score, requires_hr_approval, requires_finance_approval, requires_principal_approval, is_automatic, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6275 (class 0 OID 17438)
-- Dependencies: 291
-- Data for Name: interview; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.interview (interview_id, job_application_id, interview_type_code, round_number, scheduled_at, duration_minutes, location, meeting_url, status, overall_score, recommendation, notes) FROM stdin;
\.


--
-- TOC entry 6277 (class 0 OID 17475)
-- Dependencies: 293
-- Data for Name: interview_evaluation; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.interview_evaluation (interview_evaluation_id, interview_id, interviewer_employee_id, score, strengths, weaknesses, comments, recommendation, submitted_at) FROM stdin;
\.


--
-- TOC entry 6276 (class 0 OID 17458)
-- Dependencies: 292
-- Data for Name: interview_panel; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.interview_panel (interview_id, employee_id) FROM stdin;
\.


--
-- TOC entry 6265 (class 0 OID 17146)
-- Dependencies: 281
-- Data for Name: job; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job (job_id, tenant_id, department_id, job_family_id, code, title, description, responsibilities, minimum_qualification, minimum_experience_years, is_teaching_position, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6274 (class 0 OID 17403)
-- Dependencies: 290
-- Data for Name: job_application; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job_application (job_application_id, tenant_id, candidate_id, job_vacancy_id, application_date, status_code, screening_score, final_score, rejection_reason, eligible_for_future_opening, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6263 (class 0 OID 17108)
-- Dependencies: 279
-- Data for Name: job_family; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job_family (job_family_id, tenant_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6264 (class 0 OID 17125)
-- Dependencies: 280
-- Data for Name: job_grade; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job_grade (job_grade_id, tenant_id, code, name, grade_level, minimum_salary, midpoint_salary, maximum_salary, currency_code, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6266 (class 0 OID 17179)
-- Dependencies: 282
-- Data for Name: job_grade_mapping; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job_grade_mapping (job_id, job_grade_id, is_default, effective_from, effective_to) FROM stdin;
\.


--
-- TOC entry 6273 (class 0 OID 17380)
-- Dependencies: 289
-- Data for Name: job_vacancy; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.job_vacancy (job_vacancy_id, tenant_id, position_id, number_of_positions, opening_date, closing_date, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6267 (class 0 OID 17198)
-- Dependencies: 283
-- Data for Name: position; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr."position" (position_id, tenant_id, campus_id, department_id, job_id, job_grade_id, reports_to_position_id, position_code, headcount, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6293 (class 0 OID 17943)
-- Dependencies: 309
-- Data for Name: salary_component; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.salary_component (salary_component_id, tenant_id, code, name, component_type, calculation_type, taxable, is_recurring, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6297 (class 0 OID 18045)
-- Dependencies: 313
-- Data for Name: salary_increment_request; Type: TABLE DATA; Schema: hr; Owner: postgres
--

COPY hr.salary_increment_request (increment_request_id, tenant_id, employee_id, increment_policy_id, request_type_code, increment_type_code, current_basic_salary, percentage, increment_amount, proposed_basic_salary, effective_date, reason, requested_by, requested_at, status_code, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6398 (class 0 OID 20814)
-- Dependencies: 414
-- Data for Name: DistributedCache; Type: TABLE DATA; Schema: infrastructure; Owner: postgres
--

COPY infrastructure."DistributedCache" ("Id", "Value", "ExpiresAtTime", "SlidingExpirationInSeconds", "AbsoluteExpiration") FROM stdin;
\.


--
-- TOC entry 6317 (class 0 OID 18543)
-- Dependencies: 333
-- Data for Name: item; Type: TABLE DATA; Schema: inventory; Owner: postgres
--

COPY inventory.item (item_id, tenant_id, code, name, unit, reorder_level, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6314 (class 0 OID 18476)
-- Dependencies: 330
-- Data for Name: book; Type: TABLE DATA; Schema: library; Owner: postgres
--

COPY library.book (book_id, tenant_id, isbn, title, author_text, publisher_text, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6315 (class 0 OID 18492)
-- Dependencies: 331
-- Data for Name: book_copy; Type: TABLE DATA; Schema: library; Owner: postgres
--

COPY library.book_copy (book_copy_id, book_id, campus_id, barcode, status) FROM stdin;
\.


--
-- TOC entry 6316 (class 0 OID 18516)
-- Dependencies: 332
-- Data for Name: book_loan; Type: TABLE DATA; Schema: library; Owner: postgres
--

COPY library.book_loan (book_loan_id, book_copy_id, student_id, employee_id, issued_at, due_at, returned_at) FROM stdin;
\.


--
-- TOC entry 6283 (class 0 OID 17662)
-- Dependencies: 299
-- Data for Name: academic_assignment; Type: TABLE DATA; Schema: lms; Owner: postgres
--

COPY lms.academic_assignment (academic_assignment_id, tenant_id, course_offering_id, class_section_id, teaching_group_id, teacher_employee_id, assignment_type_code, title, description, instructions, assigned_at, due_at, total_marks, allow_late_submission, max_attempts, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6284 (class 0 OID 17709)
-- Dependencies: 300
-- Data for Name: student_assignment_submission; Type: TABLE DATA; Schema: lms; Owner: postgres
--

COPY lms.student_assignment_submission (submission_id, academic_assignment_id, student_id, attempt_no, submitted_at, submission_text, marks_obtained, teacher_feedback, status) FROM stdin;
\.


--
-- TOC entry 6421 (class 0 OID 23202)
-- Dependencies: 523
-- Data for Name: application_log; Type: TABLE DATA; Schema: observability; Owner: postgres
--

COPY observability.application_log (id, timestamp_utc, level, service, message, message_template, exception, trace_id, correlation_id, request_path, properties) FROM stdin;
\.


--
-- TOC entry 6240 (class 0 OID 16520)
-- Dependencies: 256
-- Data for Name: campus; Type: TABLE DATA; Schema: org; Owner: postgres
--

COPY org.campus (campus_id, tenant_id, code, name, address, phone, email, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6241 (class 0 OID 16541)
-- Dependencies: 257
-- Data for Name: department; Type: TABLE DATA; Schema: org; Owner: postgres
--

COPY org.department (department_id, tenant_id, campus_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6242 (class 0 OID 16563)
-- Dependencies: 258
-- Data for Name: room; Type: TABLE DATA; Schema: org; Owner: postgres
--

COPY org.room (room_id, tenant_id, campus_id, code, name, capacity, room_type, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6301 (class 0 OID 18140)
-- Dependencies: 317
-- Data for Name: employee_payroll; Type: TABLE DATA; Schema: payroll; Owner: postgres
--

COPY payroll.employee_payroll (employee_payroll_id, payroll_run_id, employee_id, gross_amount, deduction_amount, net_amount) FROM stdin;
\.


--
-- TOC entry 6302 (class 0 OID 18167)
-- Dependencies: 318
-- Data for Name: payroll_line_item; Type: TABLE DATA; Schema: payroll; Owner: postgres
--

COPY payroll.payroll_line_item (payroll_line_item_id, employee_payroll_id, salary_component_id, description, amount) FROM stdin;
\.


--
-- TOC entry 6299 (class 0 OID 18097)
-- Dependencies: 315
-- Data for Name: payroll_period; Type: TABLE DATA; Schema: payroll; Owner: postgres
--

COPY payroll.payroll_period (payroll_period_id, tenant_id, year, month, start_date, end_date, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6300 (class 0 OID 18117)
-- Dependencies: 316
-- Data for Name: payroll_run; Type: TABLE DATA; Schema: payroll; Owner: postgres
--

COPY payroll.payroll_run (payroll_run_id, tenant_id, payroll_period_id, status_code, created_at, approved_by, approved_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6374 (class 0 OID 20103)
-- Dependencies: 390
-- Data for Name: driverdirectoryread; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.driverdirectoryread (id, tenantid, driverid, employeenumber, drivername, mobilenumber, licensenumber, licenseexpirydate, vehicleregistrationnumber, routename, documentcount, verifieddocumentcount, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6371 (class 0 OID 20021)
-- Dependencies: 387
-- Data for Name: schooldocument; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.schooldocument (id, tenantid, schoolid, documenttypeid, originalfilename, contenttype, filesizebytes, storageprovider, storagekey, sha256hash, documentnumber, issuedon, expireson, isverified, verifiedbyuserid, verifiedat, notes, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6372 (class 0 OID 20055)
-- Dependencies: 388
-- Data for Name: studentdirectoryread; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.studentdirectoryread (id, tenantid, studentid, admissionnumber, studentname, programname, classname, sectionname, primaryguardianname, primaryguardianmobile, attendancepercentage, latestexampercentage, outstandingbalance, documentcount, verifieddocumentcount, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6373 (class 0 OID 20079)
-- Dependencies: 389
-- Data for Name: teacherdirectoryread; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.teacherdirectoryread (id, tenantid, teacherid, employeenumber, teachername, jobtitle, jobgrade, departmentname, mobilenumber, activeclassassignments, documentcount, verifieddocumentcount, isactive, createdat, updatedat, rowversion) FROM stdin;
\.


--
-- TOC entry 6384 (class 0 OID 20252)
-- Dependencies: 400
-- Data for Name: AttendanceStatusType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."AttendanceStatusType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
19000000-0000-0000-0000-000000000001	PRESENT	Present	1	t
19000000-0000-0000-0000-000000000002	ABSENT	Absent	2	t
19000000-0000-0000-0000-000000000003	LATE	Late	3	t
19000000-0000-0000-0000-000000000004	EXCUSED	Excused	4	t
19000000-0000-0000-0000-000000000005	LEAVE	Leave	5	t
\.


--
-- TOC entry 6378 (class 0 OID 20168)
-- Dependencies: 394
-- Data for Name: BloodGroupType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."BloodGroupType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
13000000-0000-0000-0000-000000000001	A+	A+	1	t
13000000-0000-0000-0000-000000000002	A-	A-	2	t
13000000-0000-0000-0000-000000000003	B+	B+	3	t
13000000-0000-0000-0000-000000000004	B-	B-	4	t
13000000-0000-0000-0000-000000000005	AB+	AB+	5	t
13000000-0000-0000-0000-000000000006	AB-	AB-	6	t
13000000-0000-0000-0000-000000000007	O+	O+	7	t
13000000-0000-0000-0000-000000000008	O-	O-	8	t
\.


--
-- TOC entry 6386 (class 0 OID 20280)
-- Dependencies: 402
-- Data for Name: DocumentTypeLookup; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."DocumentTypeLookup" ("Id", "Code", "Name", "OwnerCategory", "IsIdentityDocument", "RequiresExpiryDate", "RequiresVerification", "DisplayOrder", "IsActive") FROM stdin;
21000000-0000-0000-0000-000000000001	PROFILE_PICTURE	Profile Picture	ANY	f	f	f	1	t
21000000-0000-0000-0000-000000000002	BIRTH_CERTIFICATE	Birth Certificate	STUDENT	t	f	t	2	t
21000000-0000-0000-0000-000000000003	B_FORM	B-Form	STUDENT	t	f	t	3	t
21000000-0000-0000-0000-000000000004	CNIC_FRONT	CNIC Front	ADULT	t	t	t	4	t
21000000-0000-0000-0000-000000000005	CNIC_BACK	CNIC Back	ADULT	t	t	t	5	t
21000000-0000-0000-0000-000000000006	PASSPORT	Passport	ANY	t	t	t	6	t
21000000-0000-0000-0000-000000000007	ACADEMIC_CERTIFICATE	Academic Certificate	ANY	f	f	t	7	t
21000000-0000-0000-0000-000000000008	DEGREE	Degree	EMPLOYEE	f	f	t	8	t
21000000-0000-0000-0000-000000000009	EXPERIENCE_CERTIFICATE	Experience Certificate	EMPLOYEE	f	f	t	9	t
21000000-0000-0000-0000-000000000010	DRIVING_LICENSE	Driving License	DRIVER	t	t	t	10	t
21000000-0000-0000-0000-000000000011	POLICE_VERIFICATION	Police Verification	EMPLOYEE	t	t	t	11	t
21000000-0000-0000-0000-000000000012	MEDICAL_CERTIFICATE	Medical Certificate	ANY	t	t	t	12	t
21000000-0000-0000-0000-000000000013	RESUME	Resume	EMPLOYEE	f	f	f	13	t
21000000-0000-0000-0000-000000000014	EMPLOYMENT_CONTRACT	Employment Contract	EMPLOYEE	t	t	t	14	t
21000000-0000-0000-0000-000000000015	OTHER	Other	ANY	f	f	f	99	t
\.


--
-- TOC entry 6379 (class 0 OID 20182)
-- Dependencies: 395
-- Data for Name: EmploymentStatusType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."EmploymentStatusType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
14000000-0000-0000-0000-000000000001	ACTIVE	Active	1	t
14000000-0000-0000-0000-000000000002	PROBATION	Probation	2	t
14000000-0000-0000-0000-000000000003	ON_LEAVE	On Leave	3	t
14000000-0000-0000-0000-000000000004	SUSPENDED	Suspended	4	t
14000000-0000-0000-0000-000000000005	RESIGNED	Resigned	5	t
14000000-0000-0000-0000-000000000006	TERMINATED	Terminated	6	t
14000000-0000-0000-0000-000000000007	RETIRED	Retired	7	t
\.


--
-- TOC entry 6380 (class 0 OID 20196)
-- Dependencies: 396
-- Data for Name: EmploymentType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."EmploymentType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
15000000-0000-0000-0000-000000000001	PERMANENT	Permanent	1	t
15000000-0000-0000-0000-000000000002	CONTRACT	Contract	2	t
15000000-0000-0000-0000-000000000003	PART_TIME	Part Time	3	t
15000000-0000-0000-0000-000000000004	TEMPORARY	Temporary	4	t
15000000-0000-0000-0000-000000000005	INTERN	Intern	5	t
\.


--
-- TOC entry 6385 (class 0 OID 20266)
-- Dependencies: 401
-- Data for Name: ExamType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."ExamType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
20000000-0000-0000-0000-000000000001	CLASS_TEST	Class Test	1	t
20000000-0000-0000-0000-000000000002	MONTHLY_TEST	Monthly Test	2	t
20000000-0000-0000-0000-000000000003	MIDTERM	Midterm	3	t
20000000-0000-0000-0000-000000000004	ANNUAL	Annual Examination	4	t
20000000-0000-0000-0000-000000000005	PRE_BOARD	Pre-Board	5	t
20000000-0000-0000-0000-000000000006	SUPPLEMENTARY	Supplementary	6	t
\.


--
-- TOC entry 6383 (class 0 OID 20238)
-- Dependencies: 399
-- Data for Name: FeeStatusType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."FeeStatusType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
18000000-0000-0000-0000-000000000001	PENDING	Pending	1	t
18000000-0000-0000-0000-000000000002	PARTIALLY_PAID	Partially Paid	2	t
18000000-0000-0000-0000-000000000003	PAID	Paid	3	t
18000000-0000-0000-0000-000000000004	OVERDUE	Overdue	4	t
18000000-0000-0000-0000-000000000005	WAIVED	Waived	5	t
18000000-0000-0000-0000-000000000006	CANCELLED	Cancelled	6	t
\.


--
-- TOC entry 6377 (class 0 OID 20154)
-- Dependencies: 393
-- Data for Name: GenderType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."GenderType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
12000000-0000-0000-0000-000000000001	MALE	Male	1	t
12000000-0000-0000-0000-000000000002	FEMALE	Female	2	t
12000000-0000-0000-0000-000000000003	OTHER	Other	99	t
\.


--
-- TOC entry 6388 (class 0 OID 20315)
-- Dependencies: 404
-- Data for Name: LicenseCategoryType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."LicenseCategoryType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
23000000-0000-0000-0000-000000000001	LTV	Light Transport Vehicle	1	t
23000000-0000-0000-0000-000000000002	HTV	Heavy Transport Vehicle	2	t
23000000-0000-0000-0000-000000000003	PSV	Public Service Vehicle	3	t
23000000-0000-0000-0000-000000000004	OTHER	Other	99	t
\.


--
-- TOC entry 6381 (class 0 OID 20210)
-- Dependencies: 397
-- Data for Name: MaritalStatusType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."MaritalStatusType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
16000000-0000-0000-0000-000000000001	SINGLE	Single	1	t
16000000-0000-0000-0000-000000000002	MARRIED	Married	2	t
16000000-0000-0000-0000-000000000003	DIVORCED	Divorced	3	t
16000000-0000-0000-0000-000000000004	WIDOWED	Widowed	4	t
\.


--
-- TOC entry 6375 (class 0 OID 20126)
-- Dependencies: 391
-- Data for Name: OccupationType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."OccupationType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
10000000-0000-0000-0000-000000000001	BUSINESS	Business Owner	1	t
10000000-0000-0000-0000-000000000002	PRIVATE_EMPLOYEE	Private Sector Employee	2	t
10000000-0000-0000-0000-000000000003	GOVERNMENT_EMPLOYEE	Government Employee	3	t
10000000-0000-0000-0000-000000000004	TEACHER	Teacher	4	t
10000000-0000-0000-0000-000000000005	DOCTOR	Doctor	5	t
10000000-0000-0000-0000-000000000006	ENGINEER	Engineer	6	t
10000000-0000-0000-0000-000000000007	LAWYER	Lawyer	7	t
10000000-0000-0000-0000-000000000008	SELF_EMPLOYED	Self Employed	8	t
10000000-0000-0000-0000-000000000009	HOMEMAKER	Homemaker	9	t
10000000-0000-0000-0000-000000000010	RETIRED	Retired	10	t
10000000-0000-0000-0000-000000000011	UNEMPLOYED	Unemployed	11	t
10000000-0000-0000-0000-000000000012	OTHER	Other	99	t
\.


--
-- TOC entry 6382 (class 0 OID 20224)
-- Dependencies: 398
-- Data for Name: PaymentMethodType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."PaymentMethodType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
17000000-0000-0000-0000-000000000001	CASH	Cash	1	t
17000000-0000-0000-0000-000000000002	BANK_TRANSFER	Bank Transfer	2	t
17000000-0000-0000-0000-000000000003	CARD	Debit/Credit Card	3	t
17000000-0000-0000-0000-000000000004	CHEQUE	Cheque	4	t
17000000-0000-0000-0000-000000000005	ONLINE	Online Payment	5	t
17000000-0000-0000-0000-000000000006	MOBILE_WALLET	Mobile Wallet	6	t
\.


--
-- TOC entry 6376 (class 0 OID 20140)
-- Dependencies: 392
-- Data for Name: RelationshipType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."RelationshipType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
11000000-0000-0000-0000-000000000001	FATHER	Father	1	t
11000000-0000-0000-0000-000000000002	MOTHER	Mother	2	t
11000000-0000-0000-0000-000000000003	GUARDIAN	Guardian	3	t
11000000-0000-0000-0000-000000000004	BROTHER	Brother	4	t
11000000-0000-0000-0000-000000000005	SISTER	Sister	5	t
11000000-0000-0000-0000-000000000006	GRANDFATHER	Grandfather	6	t
11000000-0000-0000-0000-000000000007	GRANDMOTHER	Grandmother	7	t
11000000-0000-0000-0000-000000000008	UNCLE	Uncle	8	t
11000000-0000-0000-0000-000000000009	AUNT	Aunt	9	t
11000000-0000-0000-0000-000000000010	OTHER	Other	99	t
\.


--
-- TOC entry 6387 (class 0 OID 20301)
-- Dependencies: 403
-- Data for Name: VehicleType; Type: TABLE DATA; Schema: reference; Owner: postgres
--

COPY reference."VehicleType" ("Id", "Code", "Name", "DisplayOrder", "IsActive") FROM stdin;
22000000-0000-0000-0000-000000000001	BUS	Bus	1	t
22000000-0000-0000-0000-000000000002	VAN	Van	2	t
22000000-0000-0000-0000-000000000003	COASTER	Coaster	3	t
22000000-0000-0000-0000-000000000004	CAR	Car	4	t
22000000-0000-0000-0000-000000000005	OTHER	Other	99	t
\.


--
-- TOC entry 6235 (class 0 OID 16453)
-- Dependencies: 251
-- Data for Name: lookup_type; Type: TABLE DATA; Schema: saas; Owner: postgres
--

COPY saas.lookup_type (lookup_type_id, code, name) FROM stdin;
1	TENANT_STATUS	Tenant Status
2	ACADEMIC_SYSTEM_TYPE	Academic System Type
3	SUBJECT_REQUIREMENT_TYPE	Subject Requirement Type
4	ENROLLMENT_TYPE	Course Enrollment Type
5	EXAM_TYPE	Exam / Assessment Type
6	ATTENDANCE_STATUS	Attendance Status
7	ASSIGNMENT_TYPE	Academic Assignment Type
8	WORK_ASSIGNMENT_STATUS	Work Assignment Status
9	EMPLOYMENT_TYPE	Employment Type
10	CANDIDATE_STATUS	Candidate Status
11	APPLICATION_STATUS	Job Application Status
12	INTERVIEW_TYPE	Interview Type
13	DOCUMENT_TYPE	Certificate / Letter Type
14	INCREMENT_REQUEST_TYPE	Increment Request Type
15	INCREMENT_TYPE	Increment Type
16	APPROVAL_STATUS	Approval Status
17	PAYROLL_STATUS	Payroll Status
18	MESSAGE_TYPE	Message Type
19	CONVERSATION_TYPE	Conversation Type
20	AWARD_TYPE	Award Type
21	NOTIFICATION_CHANNEL	Notification Channel
\.


--
-- TOC entry 6237 (class 0 OID 16464)
-- Dependencies: 253
-- Data for Name: lookup_value; Type: TABLE DATA; Schema: saas; Owner: postgres
--

COPY saas.lookup_value (lookup_value_id, lookup_type_id, code, name, sort_order, is_active, metadata) FROM stdin;
1	1	TRIAL	Trial	1	t	\N
2	1	ACTIVE	Active	2	t	\N
3	1	SUSPENDED	Suspended	3	t	\N
4	1	CANCELLED	Cancelled	4	t	\N
5	2	CAMBRIDGE	Cambridge	1	t	\N
6	2	MATRIC	Matric / SSC	2	t	\N
7	2	INTERMEDIATE	Intermediate / HSSC	3	t	\N
8	2	IB	International Baccalaureate	4	t	\N
9	2	AMERICAN	American	5	t	\N
10	2	CUSTOM	Custom	99	t	\N
11	3	MANDATORY	Mandatory	1	t	\N
12	3	OPTIONAL	Optional	2	t	\N
13	3	ELECTIVE	Elective	3	t	\N
14	4	MANDATORY	Mandatory	1	t	\N
15	4	ELECTIVE	Elective	2	t	\N
16	4	OPTIONAL	Optional	3	t	\N
17	4	TRANSFERRED	Transferred	4	t	\N
18	5	QUIZ	Quiz	1	t	\N
19	5	CLASS_TEST	Class Test	2	t	\N
20	5	WEEKLY_TEST	Weekly Test	3	t	\N
21	5	MONTHLY_TEST	Monthly Test	4	t	\N
22	5	UNIT_TEST	Unit / Chapter Test	5	t	\N
23	5	MIDTERM	Midterm	6	t	\N
24	5	TERM	Term Examination	7	t	\N
25	5	PREBOARD	Pre-Board	8	t	\N
26	5	MOCK	Mock Examination	9	t	\N
27	5	ANNUAL	Annual Examination	10	t	\N
28	5	FINAL	Final Examination	11	t	\N
29	5	PRACTICAL	Practical	12	t	\N
30	5	VIVA	Oral / Viva	13	t	\N
31	5	PROJECT	Project / Coursework	14	t	\N
32	5	SUPPLEMENTARY	Supplementary	15	t	\N
33	5	RESIT	Re-sit	16	t	\N
34	6	PRESENT	Present	1	t	\N
35	6	ABSENT	Absent	2	t	\N
36	6	LATE	Late	3	t	\N
37	6	EXCUSED	Excused	4	t	\N
38	6	LEAVE	Leave	5	t	\N
39	6	HALF_DAY	Half Day	6	t	\N
40	7	HOMEWORK	Homework	1	t	\N
41	7	CLASSWORK	Classwork	2	t	\N
42	7	PROJECT	Project	3	t	\N
43	7	RESEARCH	Research	4	t	\N
44	7	PRESENTATION	Presentation	5	t	\N
45	7	PRACTICAL	Practical	6	t	\N
46	7	LAB_WORK	Lab Work	7	t	\N
47	7	ESSAY	Essay	8	t	\N
48	7	READING	Reading	9	t	\N
49	7	GROUP_WORK	Group Work	10	t	\N
50	7	HOLIDAY_HOMEWORK	Holiday Homework	11	t	\N
51	7	CUSTOM	Custom	99	t	\N
52	8	DRAFT	Draft	1	t	\N
53	8	ASSIGNED	Assigned	2	t	\N
54	8	ACCEPTED	Accepted	3	t	\N
55	8	IN_PROGRESS	In Progress	4	t	\N
56	8	BLOCKED	Blocked	5	t	\N
57	8	COMPLETED	Completed	6	t	\N
58	8	REJECTED	Rejected	7	t	\N
59	8	CANCELLED	Cancelled	8	t	\N
60	8	OVERDUE	Overdue	9	t	\N
61	9	PERMANENT	Permanent	1	t	\N
62	9	CONTRACT	Contract	2	t	\N
63	9	PART_TIME	Part Time	3	t	\N
64	9	TEMPORARY	Temporary	4	t	\N
65	9	VISITING	Visiting	5	t	\N
66	9	INTERN	Intern	6	t	\N
67	10	NEW	New	1	t	\N
68	10	SCREENING	Screening	2	t	\N
69	10	SHORTLISTED	Shortlisted	3	t	\N
70	10	INTERVIEW	Interview	4	t	\N
71	10	ASSESSMENT	Assessment	5	t	\N
72	10	SELECTED	Selected	6	t	\N
73	10	OFFER	Offer	7	t	\N
74	10	HIRED	Hired	8	t	\N
75	10	REJECTED	Rejected	9	t	\N
76	10	WITHDRAWN	Withdrawn	10	t	\N
77	10	ON_HOLD	On Hold	11	t	\N
78	11	APPLIED	Applied	1	t	\N
79	11	SCREENING	Screening	2	t	\N
80	11	SHORTLISTED	Shortlisted	3	t	\N
81	11	INTERVIEW	Interview	4	t	\N
82	11	OFFERED	Offered	5	t	\N
83	11	HIRED	Hired	6	t	\N
84	11	REJECTED	Rejected	7	t	\N
85	11	WITHDRAWN	Withdrawn	8	t	\N
86	12	HR_SCREENING	HR Screening	1	t	\N
87	12	SUBJECT	Subject / Technical Interview	2	t	\N
88	12	TEACHING_DEMO	Teaching Demo	3	t	\N
89	12	PANEL	Panel Interview	4	t	\N
90	12	PRINCIPAL	Principal Interview	5	t	\N
91	12	FINAL	Final Interview	6	t	\N
92	13	SCHOOL_LEAVING	School Leaving Certificate	1	t	\N
93	13	TRANSFER	Transfer Certificate	2	t	\N
94	13	MIGRATION	Migration Certificate	3	t	\N
95	13	CHARACTER	Character / Conduct Certificate	4	t	\N
96	13	BONAFIDE	Bonafide / Enrollment Certificate	5	t	\N
97	13	APPRECIATION	Appreciation Certificate	6	t	\N
98	13	STUDENT_OF_MONTH	Student of the Month Certificate	7	t	\N
99	13	ACHIEVEMENT	Achievement Certificate	8	t	\N
100	13	SPORTS	Sports Certificate	9	t	\N
101	13	ACTIVITY	Co-curricular Activity Certificate	10	t	\N
102	13	ADMISSION_OFFER	Admission Offer Letter	11	t	\N
103	13	WARNING	Warning Letter	12	t	\N
104	13	EMPLOYMENT	Employment Letter	13	t	\N
105	13	EXPERIENCE	Experience Letter	14	t	\N
106	13	CUSTOM	Custom Document	99	t	\N
107	14	AUTO	Automatic Proposal	1	t	\N
108	14	MANUAL	Manual Proposal	2	t	\N
109	15	PERCENTAGE	Percentage	1	t	\N
110	15	FIXED	Fixed Amount	2	t	\N
111	15	NEW_SALARY	New Salary	3	t	\N
112	15	GRADE_STEP	Grade / Step	4	t	\N
113	16	DRAFT	Draft	1	t	\N
114	16	PENDING	Pending	2	t	\N
115	16	APPROVED	Approved	3	t	\N
116	16	REJECTED	Rejected	4	t	\N
117	16	CANCELLED	Cancelled	5	t	\N
118	17	DRAFT	Draft	1	t	\N
119	17	CALCULATED	Calculated	2	t	\N
120	17	HR_REVIEW	HR Review	3	t	\N
121	17	FINANCE_REVIEW	Finance Review	4	t	\N
122	17	APPROVED	Approved	5	t	\N
123	17	LOCKED	Locked	6	t	\N
124	17	PAID	Paid	7	t	\N
125	18	TEXT	Text	1	t	\N
126	18	IMAGE	Image	2	t	\N
127	18	FILE	File	3	t	\N
128	18	VOICE	Voice Note	4	t	\N
129	18	SYSTEM	System	5	t	\N
130	19	PARENT_TEACHER	Parent / Teacher	1	t	\N
131	19	CLASS	Class Channel	2	t	\N
132	19	SUBJECT	Subject Channel	3	t	\N
133	19	ADMIN	Administration	4	t	\N
134	19	STAFF	Staff	5	t	\N
135	20	STUDENT_OF_MONTH	Student of the Month	1	t	\N
136	20	ACADEMIC_EXCELLENCE	Academic Excellence	2	t	\N
137	20	BEST_ATTENDANCE	Best Attendance	3	t	\N
138	20	MOST_IMPROVED	Most Improved	4	t	\N
139	20	LEADERSHIP	Leadership	5	t	\N
140	20	SPORTS_EXCELLENCE	Sports Excellence	6	t	\N
141	20	COMMUNITY_SERVICE	Community Service	7	t	\N
142	20	APPRECIATION	Appreciation	8	t	\N
143	21	IN_APP	In-App	1	t	\N
144	21	PUSH	Push	2	t	\N
145	21	EMAIL	Email	3	t	\N
146	21	SMS	SMS	4	t	\N
147	21	WHATSAPP	WhatsApp	5	t	\N
\.


--
-- TOC entry 6239 (class 0 OID 16507)
-- Dependencies: 255
-- Data for Name: school_branding; Type: TABLE DATA; Schema: saas; Owner: postgres
--

COPY saas.school_branding (tenant_id, logo, logo_content_type, logo_file_name, small_logo, small_logo_content_type, small_logo_file_name, favicon, favicon_content_type, favicon_file_name, certificate_logo, certificate_logo_content_type, certificate_logo_file_name, letterhead, letterhead_content_type, letterhead_file_name, watermark, watermark_content_type, watermark_file_name, primary_color, secondary_color, accent_color, footer_text, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6238 (class 0 OID 16486)
-- Dependencies: 254
-- Data for Name: tenant; Type: TABLE DATA; Schema: saas; Owner: postgres
--

COPY saas.tenant (tenant_id, code, name, status_code, default_language, timezone, currency_code, created_at, is_active, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6259 (class 0 OID 16995)
-- Dependencies: 275
-- Data for Name: guardian; Type: TABLE DATA; Schema: student; Owner: postgres
--

COPY student.guardian (guardian_id, tenant_id, user_id, full_name, cnic_number, email, phone, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6258 (class 0 OID 16974)
-- Dependencies: 274
-- Data for Name: student; Type: TABLE DATA; Schema: student; Owner: postgres
--

COPY student.student (student_id, tenant_id, user_id, student_number, first_name, last_name, date_of_birth, gender, photo, photo_content_type, photo_file_name, admission_date, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6262 (class 0 OID 17076)
-- Dependencies: 278
-- Data for Name: student_course_enrollment; Type: TABLE DATA; Schema: student; Owner: postgres
--

COPY student.student_course_enrollment (student_course_enrollment_id, tenant_id, student_enrollment_id, course_offering_id, enrollment_type_code, selected_at, approved_by, approved_at, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6261 (class 0 OID 17039)
-- Dependencies: 277
-- Data for Name: student_enrollment; Type: TABLE DATA; Schema: student; Owner: postgres
--

COPY student.student_enrollment (student_enrollment_id, tenant_id, student_id, academic_year_id, class_section_id, enrollment_date, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6260 (class 0 OID 17013)
-- Dependencies: 276
-- Data for Name: student_guardian; Type: TABLE DATA; Schema: student; Owner: postgres
--

COPY student.student_guardian (student_id, guardian_id, relationship, is_primary, can_view_academics, can_view_finance, can_pickup) FROM stdin;
\.


--
-- TOC entry 6427 (class 0 OID 23366)
-- Dependencies: 529
-- Data for Name: leave_request; Type: TABLE DATA; Schema: teacher; Owner: postgres
--

COPY teacher.leave_request (leave_request_id, tenant_id, employee_id, leave_type, from_date, to_date, reason, status, approved_by, decision_at, decision_note, created_at) FROM stdin;
\.


--
-- TOC entry 6426 (class 0 OID 23326)
-- Dependencies: 528
-- Data for Name: teacher_actor; Type: TABLE DATA; Schema: teacher; Owner: postgres
--

COPY teacher.teacher_actor (teacher_id, tenant_id, employee_id, user_id, primary_campus_id, qualification, specialization, teaching_experience_years, max_periods_per_week, status, is_active, created_at, updated_at) FROM stdin;
\.


--
-- TOC entry 6318 (class 0 OID 18560)
-- Dependencies: 334
-- Data for Name: driver; Type: TABLE DATA; Schema: transport; Owner: postgres
--

COPY transport.driver (driver_id, tenant_id, employee_id, driver_number, full_name, cnic_number, phone, alternate_phone, date_of_birth, driving_license_number, driving_license_category, driving_license_issued_on, driving_license_expires_on, picture, picture_content_type, picture_file_name, emergency_contact_name, emergency_contact_phone, address, hire_date, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6321 (class 0 OID 18651)
-- Dependencies: 337
-- Data for Name: route; Type: TABLE DATA; Schema: transport; Owner: postgres
--

COPY transport.route (route_id, tenant_id, campus_id, code, name, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6319 (class 0 OID 18597)
-- Dependencies: 335
-- Data for Name: vehicle; Type: TABLE DATA; Schema: transport; Owner: postgres
--

COPY transport.vehicle (vehicle_id, tenant_id, campus_id, registration_no, capacity, status, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6320 (class 0 OID 18621)
-- Dependencies: 336
-- Data for Name: vehicle_driver_assignment; Type: TABLE DATA; Schema: transport; Owner: postgres
--

COPY transport.vehicle_driver_assignment (vehicle_driver_assignment_id, tenant_id, vehicle_id, driver_id, effective_from, effective_to, is_primary, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6303 (class 0 OID 18186)
-- Dependencies: 319
-- Data for Name: work_assignment; Type: TABLE DATA; Schema: workflow; Owner: postgres
--

COPY workflow.work_assignment (work_assignment_id, tenant_id, campus_id, title, description, assigned_by_user_id, assigned_to_user_id, priority, status_code, assigned_at, due_at, completed_at, related_entity_type, related_entity_id, is_active, created_at, updated_at, row_version) FROM stdin;
\.


--
-- TOC entry 6443 (class 0 OID 0)
-- Dependencies: 378
-- Name: audit_log_audit_log_id_seq; Type: SEQUENCE SET; Schema: audit; Owner: postgres
--

SELECT pg_catalog.setval('audit.audit_log_audit_log_id_seq', 1, false);


--
-- TOC entry 6444 (class 0 OID 0)
-- Dependencies: 520
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.aggregatedcounter_id_seq', 1, false);


--
-- TOC entry 6445 (class 0 OID 0)
-- Dependencies: 502
-- Name: counter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.counter_id_seq', 1, false);


--
-- TOC entry 6446 (class 0 OID 0)
-- Dependencies: 504
-- Name: hash_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.hash_id_seq', 1, false);


--
-- TOC entry 6447 (class 0 OID 0)
-- Dependencies: 506
-- Name: job_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.job_id_seq', 1, false);


--
-- TOC entry 6448 (class 0 OID 0)
-- Dependencies: 517
-- Name: jobparameter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.jobparameter_id_seq', 1, false);


--
-- TOC entry 6449 (class 0 OID 0)
-- Dependencies: 510
-- Name: jobqueue_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.jobqueue_id_seq', 1, false);


--
-- TOC entry 6450 (class 0 OID 0)
-- Dependencies: 512
-- Name: list_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.list_id_seq', 1, false);


--
-- TOC entry 6451 (class 0 OID 0)
-- Dependencies: 515
-- Name: set_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.set_id_seq', 1, false);


--
-- TOC entry 6452 (class 0 OID 0)
-- Dependencies: 508
-- Name: state_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: postgres
--

SELECT pg_catalog.setval('hangfire.state_id_seq', 1, false);


--
-- TOC entry 6453 (class 0 OID 0)
-- Dependencies: 522
-- Name: application_log_id_seq; Type: SEQUENCE SET; Schema: observability; Owner: postgres
--

SELECT pg_catalog.setval('observability.application_log_id_seq', 1, false);


--
-- TOC entry 6454 (class 0 OID 0)
-- Dependencies: 250
-- Name: lookup_type_lookup_type_id_seq; Type: SEQUENCE SET; Schema: saas; Owner: postgres
--

SELECT pg_catalog.setval('saas.lookup_type_lookup_type_id_seq', 21, true);


--
-- TOC entry 6455 (class 0 OID 0)
-- Dependencies: 252
-- Name: lookup_value_lookup_value_id_seq; Type: SEQUENCE SET; Schema: saas; Owner: postgres
--

SELECT pg_catalog.setval('saas.lookup_value_lookup_value_id_seq', 147, true);


--
-- TOC entry 5106 (class 2606 OID 16598)
-- Name: academic_system academic_system_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_system
    ADD CONSTRAINT academic_system_pkey PRIMARY KEY (academic_system_id);


--
-- TOC entry 5108 (class 2606 OID 16600)
-- Name: academic_system academic_system_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_system
    ADD CONSTRAINT academic_system_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5142 (class 2606 OID 16837)
-- Name: academic_year academic_year_campus_id_name_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_year
    ADD CONSTRAINT academic_year_campus_id_name_key UNIQUE (campus_id, name);


--
-- TOC entry 5144 (class 2606 OID 16835)
-- Name: academic_year academic_year_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_year
    ADD CONSTRAINT academic_year_pkey PRIMARY KEY (academic_year_id);


--
-- TOC entry 5118 (class 2606 OID 16663)
-- Name: campus_program campus_program_campus_id_program_id_effective_from_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.campus_program
    ADD CONSTRAINT campus_program_campus_id_program_id_effective_from_key UNIQUE (campus_id, program_id, effective_from);


--
-- TOC entry 5120 (class 2606 OID 16661)
-- Name: campus_program campus_program_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.campus_program
    ADD CONSTRAINT campus_program_pkey PRIMARY KEY (campus_program_id);


--
-- TOC entry 5154 (class 2606 OID 16905)
-- Name: class_section class_section_academic_year_id_program_grade_id_section_id_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_academic_year_id_program_grade_id_section_id_key UNIQUE (academic_year_id, program_grade_id, section_id);


--
-- TOC entry 5156 (class 2606 OID 16903)
-- Name: class_section class_section_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_pkey PRIMARY KEY (class_section_id);


--
-- TOC entry 5158 (class 2606 OID 16948)
-- Name: course_offering course_offering_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_pkey PRIMARY KEY (course_offering_id);


--
-- TOC entry 5140 (class 2606 OID 16811)
-- Name: course_selection_group_course course_selection_group_course_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group_course
    ADD CONSTRAINT course_selection_group_course_pkey PRIMARY KEY (selection_group_id, program_subject_id);


--
-- TOC entry 5138 (class 2606 OID 16794)
-- Name: course_selection_group course_selection_group_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group
    ADD CONSTRAINT course_selection_group_pkey PRIMARY KEY (selection_group_id);


--
-- TOC entry 5110 (class 2606 OID 16615)
-- Name: education_board education_board_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.education_board
    ADD CONSTRAINT education_board_pkey PRIMARY KEY (education_board_id);


--
-- TOC entry 5112 (class 2606 OID 16617)
-- Name: education_board education_board_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.education_board
    ADD CONSTRAINT education_board_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5122 (class 2606 OID 16690)
-- Name: grade_level grade_level_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.grade_level
    ADD CONSTRAINT grade_level_pkey PRIMARY KEY (grade_level_id);


--
-- TOC entry 5124 (class 2606 OID 16692)
-- Name: grade_level grade_level_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.grade_level
    ADD CONSTRAINT grade_level_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5126 (class 2606 OID 16709)
-- Name: program_grade program_grade_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_grade
    ADD CONSTRAINT program_grade_pkey PRIMARY KEY (program_grade_id);


--
-- TOC entry 5128 (class 2606 OID 16711)
-- Name: program_grade program_grade_program_id_grade_level_id_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_grade
    ADD CONSTRAINT program_grade_program_id_grade_level_id_key UNIQUE (program_id, grade_level_id);


--
-- TOC entry 5114 (class 2606 OID 16637)
-- Name: program program_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program
    ADD CONSTRAINT program_pkey PRIMARY KEY (program_id);


--
-- TOC entry 5134 (class 2606 OID 16762)
-- Name: program_subject program_subject_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_subject
    ADD CONSTRAINT program_subject_pkey PRIMARY KEY (program_subject_id);


--
-- TOC entry 5136 (class 2606 OID 16764)
-- Name: program_subject program_subject_program_grade_id_subject_id_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_subject
    ADD CONSTRAINT program_subject_program_grade_id_subject_id_key UNIQUE (program_grade_id, subject_id);


--
-- TOC entry 5116 (class 2606 OID 16639)
-- Name: program program_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program
    ADD CONSTRAINT program_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5150 (class 2606 OID 16882)
-- Name: section section_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.section
    ADD CONSTRAINT section_pkey PRIMARY KEY (section_id);


--
-- TOC entry 5152 (class 2606 OID 16884)
-- Name: section section_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.section
    ADD CONSTRAINT section_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5130 (class 2606 OID 16740)
-- Name: subject subject_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.subject
    ADD CONSTRAINT subject_pkey PRIMARY KEY (subject_id);


--
-- TOC entry 5132 (class 2606 OID 16742)
-- Name: subject subject_tenant_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.subject
    ADD CONSTRAINT subject_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5210 (class 2606 OID 17319)
-- Name: teacher_course_assignment teacher_course_assignment_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT teacher_course_assignment_pkey PRIMARY KEY (teacher_course_assignment_id);


--
-- TOC entry 5229 (class 2606 OID 17508)
-- Name: teaching_group teaching_group_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group
    ADD CONSTRAINT teaching_group_pkey PRIMARY KEY (teaching_group_id);


--
-- TOC entry 5231 (class 2606 OID 17545)
-- Name: teaching_group_student teaching_group_student_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teaching_group_student
    ADD CONSTRAINT teaching_group_student_pkey PRIMARY KEY (teaching_group_id, student_course_enrollment_id);


--
-- TOC entry 5146 (class 2606 OID 16862)
-- Name: term term_academic_year_id_code_key; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.term
    ADD CONSTRAINT term_academic_year_id_code_key UNIQUE (academic_year_id, code);


--
-- TOC entry 5148 (class 2606 OID 16860)
-- Name: term term_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.term
    ADD CONSTRAINT term_pkey PRIMARY KEY (term_id);


--
-- TOC entry 5239 (class 2606 OID 17626)
-- Name: timetable_entry timetable_entry_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_entry
    ADD CONSTRAINT timetable_entry_pkey PRIMARY KEY (timetable_entry_id);


--
-- TOC entry 5233 (class 2606 OID 17569)
-- Name: timetable_period timetable_period_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable_period
    ADD CONSTRAINT timetable_period_pkey PRIMARY KEY (timetable_period_id);


--
-- TOC entry 5235 (class 2606 OID 17592)
-- Name: timetable timetable_pkey; Type: CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.timetable
    ADD CONSTRAINT timetable_pkey PRIMARY KEY (timetable_id);


--
-- TOC entry 5310 (class 2606 OID 18305)
-- Name: activity activity_pkey; Type: CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.activity
    ADD CONSTRAINT activity_pkey PRIMARY KEY (activity_id);


--
-- TOC entry 5312 (class 2606 OID 18327)
-- Name: student_activity student_activity_pkey; Type: CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_activity
    ADD CONSTRAINT student_activity_pkey PRIMARY KEY (activity_id, student_id);


--
-- TOC entry 5314 (class 2606 OID 18351)
-- Name: student_award student_award_pkey; Type: CONSTRAINT; Schema: activity; Owner: postgres
--

ALTER TABLE ONLY activity.student_award
    ADD CONSTRAINT student_award_pkey PRIMARY KEY (student_award_id);


--
-- TOC entry 5454 (class 2606 OID 19503)
-- Name: class_performance_insight class_performance_insight_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.class_performance_insight
    ADD CONSTRAINT class_performance_insight_pkey PRIMARY KEY (class_performance_insight_id);


--
-- TOC entry 5465 (class 2606 OID 19677)
-- Name: intervention_action intervention_action_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.intervention_action
    ADD CONSTRAINT intervention_action_pkey PRIMARY KEY (intervention_action_id);


--
-- TOC entry 5467 (class 2606 OID 19679)
-- Name: intervention_action intervention_action_student_intervention_id_sequence_no_key; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.intervention_action
    ADD CONSTRAINT intervention_action_student_intervention_id_sequence_no_key UNIQUE (student_intervention_id, sequence_no);


--
-- TOC entry 5469 (class 2606 OID 19696)
-- Name: intervention_outcome intervention_outcome_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.intervention_outcome
    ADD CONSTRAINT intervention_outcome_pkey PRIMARY KEY (intervention_outcome_id);


--
-- TOC entry 5450 (class 2606 OID 19473)
-- Name: predicted_grade_probability predicted_grade_probability_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.predicted_grade_probability
    ADD CONSTRAINT predicted_grade_probability_pkey PRIMARY KEY (predicted_grade_probability_id);


--
-- TOC entry 5452 (class 2606 OID 19475)
-- Name: predicted_grade_probability predicted_grade_probability_student_performance_prediction__key; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.predicted_grade_probability
    ADD CONSTRAINT predicted_grade_probability_student_performance_prediction__key UNIQUE (student_performance_prediction_id, grade);


--
-- TOC entry 5471 (class 2606 OID 19712)
-- Name: prediction_evaluation prediction_evaluation_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evaluation
    ADD CONSTRAINT prediction_evaluation_pkey PRIMARY KEY (prediction_evaluation_id);


--
-- TOC entry 5473 (class 2606 OID 19714)
-- Name: prediction_evaluation prediction_evaluation_student_performance_prediction_id_stu_key; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evaluation
    ADD CONSTRAINT prediction_evaluation_student_performance_prediction_id_stu_key UNIQUE (student_performance_prediction_id, student_exam_result_id);


--
-- TOC entry 5448 (class 2606 OID 19457)
-- Name: prediction_evidence prediction_evidence_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_evidence
    ADD CONSTRAINT prediction_evidence_pkey PRIMARY KEY (prediction_evidence_id);


--
-- TOC entry 5437 (class 2606 OID 19341)
-- Name: prediction_model prediction_model_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction_model
    ADD CONSTRAINT prediction_model_pkey PRIMARY KEY (prediction_model_id);


--
-- TOC entry 5440 (class 2606 OID 19360)
-- Name: prediction prediction_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.prediction
    ADD CONSTRAINT prediction_pkey PRIMARY KEY (prediction_id);


--
-- TOC entry 5463 (class 2606 OID 19627)
-- Name: student_intervention student_intervention_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_intervention
    ADD CONSTRAINT student_intervention_pkey PRIMARY KEY (student_intervention_id);


--
-- TOC entry 5445 (class 2606 OID 19401)
-- Name: student_performance_prediction student_performance_prediction_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_performance_prediction
    ADD CONSTRAINT student_performance_prediction_pkey PRIMARY KEY (student_performance_prediction_id);


--
-- TOC entry 5476 (class 2606 OID 19745)
-- Name: student_progress_recommendation student_progress_recommendation_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.student_progress_recommendation
    ADD CONSTRAINT student_progress_recommendation_pkey PRIMARY KEY (student_progress_recommendation_id);


--
-- TOC entry 5460 (class 2606 OID 19581)
-- Name: teaching_recommendation teaching_recommendation_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.teaching_recommendation
    ADD CONSTRAINT teaching_recommendation_pkey PRIMARY KEY (teaching_recommendation_id);


--
-- TOC entry 5457 (class 2606 OID 19549)
-- Name: topic_performance_insight topic_performance_insight_pkey; Type: CONSTRAINT; Schema: ai; Owner: postgres
--

ALTER TABLE ONLY ai.topic_performance_insight
    ADD CONSTRAINT topic_performance_insight_pkey PRIMARY KEY (topic_performance_insight_id);


--
-- TOC entry 5618 (class 2606 OID 20804)
-- Name: RagKnowledgeChunks RagKnowledgeChunks_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core."RagKnowledgeChunks"
    ADD CONSTRAINT "RagKnowledgeChunks_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5614 (class 2606 OID 20791)
-- Name: RagKnowledgeDocuments RagKnowledgeDocuments_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core."RagKnowledgeDocuments"
    ADD CONSTRAINT "RagKnowledgeDocuments_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5620 (class 2606 OID 20806)
-- Name: RagKnowledgeChunks UQ_RagKnowledgeChunk; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core."RagKnowledgeChunks"
    ADD CONSTRAINT "UQ_RagKnowledgeChunk" UNIQUE ("TenantId", "DocumentId", "ChunkIndex");


--
-- TOC entry 5390 (class 2606 OID 18869)
-- Name: ai_execution_log ai_execution_log_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.ai_execution_log
    ADD CONSTRAINT ai_execution_log_pkey PRIMARY KEY (ai_execution_log_id);


--
-- TOC entry 5386 (class 2606 OID 18847)
-- Name: assistant_knowledge_collection assistant_knowledge_collectio_tenant_id_assistant_type_know_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_knowledge_collection
    ADD CONSTRAINT assistant_knowledge_collectio_tenant_id_assistant_type_know_key UNIQUE (tenant_id, assistant_type, knowledge_collection_id);


--
-- TOC entry 5388 (class 2606 OID 18845)
-- Name: assistant_knowledge_collection assistant_knowledge_collection_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_knowledge_collection
    ADD CONSTRAINT assistant_knowledge_collection_pkey PRIMARY KEY (assistant_knowledge_collection_id);


--
-- TOC entry 5382 (class 2606 OID 18823)
-- Name: assistant_tool assistant_tool_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_tool
    ADD CONSTRAINT assistant_tool_pkey PRIMARY KEY (assistant_tool_id);


--
-- TOC entry 5384 (class 2606 OID 18825)
-- Name: assistant_tool assistant_tool_tenant_id_assistant_type_tool_definition_id_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.assistant_tool
    ADD CONSTRAINT assistant_tool_tenant_id_assistant_type_tool_definition_id_key UNIQUE (tenant_id, assistant_type, tool_definition_id);


--
-- TOC entry 5374 (class 2606 OID 18787)
-- Name: knowledge_chunk knowledge_chunk_knowledge_document_id_chunk_index_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_chunk
    ADD CONSTRAINT knowledge_chunk_knowledge_document_id_chunk_index_key UNIQUE (knowledge_document_id, chunk_index);


--
-- TOC entry 5376 (class 2606 OID 18785)
-- Name: knowledge_chunk knowledge_chunk_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_chunk
    ADD CONSTRAINT knowledge_chunk_pkey PRIMARY KEY (knowledge_chunk_id);


--
-- TOC entry 5368 (class 2606 OID 18732)
-- Name: knowledge_collection knowledge_collection_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_collection
    ADD CONSTRAINT knowledge_collection_pkey PRIMARY KEY (knowledge_collection_id);


--
-- TOC entry 5370 (class 2606 OID 18734)
-- Name: knowledge_collection knowledge_collection_tenant_id_code_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_collection
    ADD CONSTRAINT knowledge_collection_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5372 (class 2606 OID 18753)
-- Name: knowledge_document knowledge_document_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.knowledge_document
    ADD CONSTRAINT knowledge_document_pkey PRIMARY KEY (knowledge_document_id);


--
-- TOC entry 5360 (class 2606 OID 18687)
-- Name: model_configuration model_configuration_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.model_configuration
    ADD CONSTRAINT model_configuration_pkey PRIMARY KEY (model_configuration_id);


--
-- TOC entry 5362 (class 2606 OID 18689)
-- Name: model_configuration model_configuration_tenant_id_code_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.model_configuration
    ADD CONSTRAINT model_configuration_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5364 (class 2606 OID 18711)
-- Name: prompt_template prompt_template_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.prompt_template
    ADD CONSTRAINT prompt_template_pkey PRIMARY KEY (prompt_template_id);


--
-- TOC entry 5366 (class 2606 OID 18713)
-- Name: prompt_template prompt_template_tenant_id_code_version_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.prompt_template
    ADD CONSTRAINT prompt_template_tenant_id_code_version_key UNIQUE (tenant_id, code, version);


--
-- TOC entry 5684 (class 2606 OID 23321)
-- Name: rag_knowledge_chunk rag_knowledge_chunk_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.rag_knowledge_chunk
    ADD CONSTRAINT rag_knowledge_chunk_pkey PRIMARY KEY (id);


--
-- TOC entry 5378 (class 2606 OID 18812)
-- Name: tool_definition tool_definition_code_key; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.tool_definition
    ADD CONSTRAINT tool_definition_code_key UNIQUE (code);


--
-- TOC entry 5380 (class 2606 OID 18810)
-- Name: tool_definition tool_definition_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.tool_definition
    ADD CONSTRAINT tool_definition_pkey PRIMARY KEY (tool_definition_id);


--
-- TOC entry 5392 (class 2606 OID 18892)
-- Name: tool_execution tool_execution_pkey; Type: CONSTRAINT; Schema: ai_core; Owner: postgres
--

ALTER TABLE ONLY ai_core.tool_execution
    ADD CONSTRAINT tool_execution_pkey PRIMARY KEY (tool_execution_id);


--
-- TOC entry 5428 (class 2606 OID 19248)
-- Name: human_handoff human_handoff_pkey; Type: CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.human_handoff
    ADD CONSTRAINT human_handoff_pkey PRIMARY KEY (human_handoff_id);


--
-- TOC entry 5420 (class 2606 OID 19168)
-- Name: inquiry_conversation inquiry_conversation_pkey; Type: CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_conversation
    ADD CONSTRAINT inquiry_conversation_pkey PRIMARY KEY (inquiry_conversation_id);


--
-- TOC entry 5423 (class 2606 OID 19197)
-- Name: inquiry_message inquiry_message_pkey; Type: CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.inquiry_message
    ADD CONSTRAINT inquiry_message_pkey PRIMARY KEY (inquiry_message_id);


--
-- TOC entry 5426 (class 2606 OID 19214)
-- Name: lead_capture lead_capture_pkey; Type: CONSTRAINT; Schema: ai_inquiry; Owner: postgres
--

ALTER TABLE ONLY ai_inquiry.lead_capture
    ADD CONSTRAINT lead_capture_pkey PRIMARY KEY (lead_capture_id);


--
-- TOC entry 5431 (class 2606 OID 19266)
-- Name: parent_conversation parent_conversation_pkey; Type: CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_conversation
    ADD CONSTRAINT parent_conversation_pkey PRIMARY KEY (parent_conversation_id);


--
-- TOC entry 5433 (class 2606 OID 19295)
-- Name: parent_message parent_message_pkey; Type: CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_message
    ADD CONSTRAINT parent_message_pkey PRIMARY KEY (parent_message_id);


--
-- TOC entry 5435 (class 2606 OID 19314)
-- Name: parent_tool_execution parent_tool_execution_pkey; Type: CONSTRAINT; Schema: ai_parent; Owner: postgres
--

ALTER TABLE ONLY ai_parent.parent_tool_execution
    ADD CONSTRAINT parent_tool_execution_pkey PRIMARY KEY (parent_tool_execution_id);


--
-- TOC entry 5412 (class 2606 OID 19090)
-- Name: generated_quiz generated_quiz_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz
    ADD CONSTRAINT generated_quiz_pkey PRIMARY KEY (generated_quiz_id);


--
-- TOC entry 5414 (class 2606 OID 19125)
-- Name: generated_quiz_question generated_quiz_question_generated_quiz_id_sequence_no_key; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz_question
    ADD CONSTRAINT generated_quiz_question_generated_quiz_id_sequence_no_key UNIQUE (generated_quiz_id, sequence_no);


--
-- TOC entry 5416 (class 2606 OID 19123)
-- Name: generated_quiz_question generated_quiz_question_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.generated_quiz_question
    ADD CONSTRAINT generated_quiz_question_pkey PRIMARY KEY (generated_quiz_question_id);


--
-- TOC entry 5410 (class 2606 OID 19068)
-- Name: learning_recommendation learning_recommendation_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.learning_recommendation
    ADD CONSTRAINT learning_recommendation_pkey PRIMARY KEY (learning_recommendation_id);


--
-- TOC entry 5418 (class 2606 OID 19143)
-- Name: student_quiz_attempt student_quiz_attempt_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_quiz_attempt
    ADD CONSTRAINT student_quiz_attempt_pkey PRIMARY KEY (student_quiz_attempt_id);


--
-- TOC entry 5406 (class 2606 OID 19033)
-- Name: student_topic_mastery student_topic_mastery_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_topic_mastery
    ADD CONSTRAINT student_topic_mastery_pkey PRIMARY KEY (student_topic_mastery_id);


--
-- TOC entry 5408 (class 2606 OID 19035)
-- Name: student_topic_mastery student_topic_mastery_student_id_subject_id_topic_key; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.student_topic_mastery
    ADD CONSTRAINT student_topic_mastery_student_id_subject_id_topic_key UNIQUE (student_id, subject_id, topic);


--
-- TOC entry 5395 (class 2606 OID 18915)
-- Name: tutor_conversation tutor_conversation_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_conversation
    ADD CONSTRAINT tutor_conversation_pkey PRIMARY KEY (tutor_conversation_id);


--
-- TOC entry 5404 (class 2606 OID 19008)
-- Name: tutor_feedback tutor_feedback_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_feedback
    ADD CONSTRAINT tutor_feedback_pkey PRIMARY KEY (tutor_feedback_id);


--
-- TOC entry 5398 (class 2606 OID 18954)
-- Name: tutor_message tutor_message_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_message
    ADD CONSTRAINT tutor_message_pkey PRIMARY KEY (tutor_message_id);


--
-- TOC entry 5400 (class 2606 OID 18967)
-- Name: tutor_message_reference tutor_message_reference_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_message_reference
    ADD CONSTRAINT tutor_message_reference_pkey PRIMARY KEY (tutor_message_reference_id);


--
-- TOC entry 5402 (class 2606 OID 18989)
-- Name: tutor_session tutor_session_pkey; Type: CONSTRAINT; Schema: ai_tutor; Owner: postgres
--

ALTER TABLE ONLY ai_tutor.tutor_session
    ADD CONSTRAINT tutor_session_pkey PRIMARY KEY (tutor_session_id);


--
-- TOC entry 5478 (class 2606 OID 19781)
-- Name: audit_log audit_log_pkey; Type: CONSTRAINT; Schema: audit; Owner: postgres
--

ALTER TABLE ONLY audit.audit_log
    ADD CONSTRAINT audit_log_pkey PRIMARY KEY (audit_log_id);


--
-- TOC entry 5606 (class 2606 OID 20408)
-- Name: ChatAttachments ChatAttachments_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatAttachments"
    ADD CONSTRAINT "ChatAttachments_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5598 (class 2606 OID 20353)
-- Name: ChatConversations ChatConversations_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatConversations"
    ADD CONSTRAINT "ChatConversations_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5604 (class 2606 OID 20389)
-- Name: ChatMessages ChatMessages_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatMessages"
    ADD CONSTRAINT "ChatMessages_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5600 (class 2606 OID 20366)
-- Name: ChatParticipants ChatParticipants_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatParticipants"
    ADD CONSTRAINT "ChatParticipants_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5610 (class 2606 OID 20447)
-- Name: NotificationPreferences NotificationPreferences_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."NotificationPreferences"
    ADD CONSTRAINT "NotificationPreferences_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5594 (class 2606 OID 20341)
-- Name: NotificationTypeLookup NotificationTypeLookup_Code_key; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."NotificationTypeLookup"
    ADD CONSTRAINT "NotificationTypeLookup_Code_key" UNIQUE ("Code");


--
-- TOC entry 5596 (class 2606 OID 20339)
-- Name: NotificationTypeLookup NotificationTypeLookup_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."NotificationTypeLookup"
    ADD CONSTRAINT "NotificationTypeLookup_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5608 (class 2606 OID 20430)
-- Name: Notifications Notifications_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."Notifications"
    ADD CONSTRAINT "Notifications_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5602 (class 2606 OID 20368)
-- Name: ChatParticipants UQ_ChatParticipant; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."ChatParticipants"
    ADD CONSTRAINT "UQ_ChatParticipant" UNIQUE ("TenantId", "ConversationId", "UserId");


--
-- TOC entry 5612 (class 2606 OID 20449)
-- Name: NotificationPreferences UQ_NotificationPreference; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication."NotificationPreferences"
    ADD CONSTRAINT "UQ_NotificationPreference" UNIQUE ("TenantId", "UserId", "NotificationType");


--
-- TOC entry 5674 (class 2606 OID 23257)
-- Name: chat_conversation chat_conversation_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.chat_conversation
    ADD CONSTRAINT chat_conversation_pkey PRIMARY KEY ("Id");


--
-- TOC entry 5680 (class 2606 OID 23303)
-- Name: chat_message chat_message_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.chat_message
    ADD CONSTRAINT chat_message_pkey PRIMARY KEY ("Id");


--
-- TOC entry 5676 (class 2606 OID 23278)
-- Name: chat_participant chat_participant_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.chat_participant
    ADD CONSTRAINT chat_participant_pkey PRIMARY KEY ("Id");


--
-- TOC entry 5318 (class 2606 OID 18411)
-- Name: conversation_participant conversation_participant_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation_participant
    ADD CONSTRAINT conversation_participant_pkey PRIMARY KEY (conversation_id, user_id);


--
-- TOC entry 5316 (class 2606 OID 18377)
-- Name: conversation conversation_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.conversation
    ADD CONSTRAINT conversation_pkey PRIMARY KEY (conversation_id);


--
-- TOC entry 5321 (class 2606 OID 18431)
-- Name: message message_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.message
    ADD CONSTRAINT message_pkey PRIMARY KEY (message_id);


--
-- TOC entry 5323 (class 2606 OID 18448)
-- Name: message_receipt message_receipt_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.message_receipt
    ADD CONSTRAINT message_receipt_pkey PRIMARY KEY (message_id, user_id);


--
-- TOC entry 5326 (class 2606 OID 18470)
-- Name: notification notification_pkey; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.notification
    ADD CONSTRAINT notification_pkey PRIMARY KEY (notification_id);


--
-- TOC entry 5678 (class 2606 OID 23280)
-- Name: chat_participant uq_chat_participant; Type: CONSTRAINT; Schema: communication; Owner: postgres
--

ALTER TABLE ONLY communication.chat_participant
    ADD CONSTRAINT uq_chat_participant UNIQUE ("TenantId", "ConversationId", "UserId");


--
-- TOC entry 5508 (class 2606 OID 19977)
-- Name: candidatedocument candidatedocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.candidatedocument
    ADD CONSTRAINT candidatedocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5300 (class 2606 OID 18237)
-- Name: document_template document_template_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.document_template
    ADD CONSTRAINT document_template_pkey PRIMARY KEY (document_template_id);


--
-- TOC entry 5302 (class 2606 OID 18239)
-- Name: document_template document_template_tenant_id_code_version_key; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.document_template
    ADD CONSTRAINT document_template_tenant_id_code_version_key UNIQUE (tenant_id, code, version);


--
-- TOC entry 5480 (class 2606 OID 19814)
-- Name: documenttype documenttype_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.documenttype
    ADD CONSTRAINT documenttype_pkey PRIMARY KEY (id);


--
-- TOC entry 5514 (class 2606 OID 20011)
-- Name: driverdocument driverdocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.driverdocument
    ADD CONSTRAINT driverdocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5502 (class 2606 OID 19943)
-- Name: employeedocument employeedocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.employeedocument
    ADD CONSTRAINT employeedocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5304 (class 2606 OID 18270)
-- Name: generated_document generated_document_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_pkey PRIMARY KEY (generated_document_id);


--
-- TOC entry 5306 (class 2606 OID 18272)
-- Name: generated_document generated_document_tenant_id_document_number_key; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_tenant_id_document_number_key UNIQUE (tenant_id, document_number);


--
-- TOC entry 5308 (class 2606 OID 18274)
-- Name: generated_document generated_document_verification_code_key; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.generated_document
    ADD CONSTRAINT generated_document_verification_code_key UNIQUE (verification_code);


--
-- TOC entry 5492 (class 2606 OID 19875)
-- Name: parentdocument parentdocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.parentdocument
    ADD CONSTRAINT parentdocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5486 (class 2606 OID 19841)
-- Name: studentdocument studentdocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.studentdocument
    ADD CONSTRAINT studentdocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5498 (class 2606 OID 19909)
-- Name: teacherdocument teacherdocument_pkey; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.teacherdocument
    ADD CONSTRAINT teacherdocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5512 (class 2606 OID 19979)
-- Name: candidatedocument uq_candidatedocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.candidatedocument
    ADD CONSTRAINT uq_candidatedocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5482 (class 2606 OID 19816)
-- Name: documenttype uq_documenttype_tenant_code; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.documenttype
    ADD CONSTRAINT uq_documenttype_tenant_code UNIQUE (tenantid, code);


--
-- TOC entry 5518 (class 2606 OID 20013)
-- Name: driverdocument uq_driverdocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.driverdocument
    ADD CONSTRAINT uq_driverdocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5506 (class 2606 OID 19945)
-- Name: employeedocument uq_employeedocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.employeedocument
    ADD CONSTRAINT uq_employeedocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5494 (class 2606 OID 19877)
-- Name: parentdocument uq_parentdocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.parentdocument
    ADD CONSTRAINT uq_parentdocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5488 (class 2606 OID 19843)
-- Name: studentdocument uq_studentdocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.studentdocument
    ADD CONSTRAINT uq_studentdocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5500 (class 2606 OID 19911)
-- Name: teacherdocument uq_teacherdocument_storage; Type: CONSTRAINT; Schema: document; Owner: postgres
--

ALTER TABLE ONLY document.teacherdocument
    ADD CONSTRAINT uq_teacherdocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5247 (class 2606 OID 17750)
-- Name: exam exam_pkey; Type: CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam
    ADD CONSTRAINT exam_pkey PRIMARY KEY (exam_id);


--
-- TOC entry 5249 (class 2606 OID 17785)
-- Name: exam_subject exam_subject_pkey; Type: CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.exam_subject
    ADD CONSTRAINT exam_subject_pkey PRIMARY KEY (exam_subject_id);


--
-- TOC entry 5252 (class 2606 OID 17815)
-- Name: student_exam_result student_exam_result_exam_subject_id_student_id_key; Type: CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.student_exam_result
    ADD CONSTRAINT student_exam_result_exam_subject_id_student_id_key UNIQUE (exam_subject_id, student_id);


--
-- TOC entry 5254 (class 2606 OID 17813)
-- Name: student_exam_result student_exam_result_pkey; Type: CONSTRAINT; Schema: exam; Owner: postgres
--

ALTER TABLE ONLY exam.student_exam_result
    ADD CONSTRAINT student_exam_result_pkey PRIMARY KEY (student_exam_result_id);


--
-- TOC entry 5256 (class 2606 OID 17835)
-- Name: fee_type fee_type_pkey; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.fee_type
    ADD CONSTRAINT fee_type_pkey PRIMARY KEY (fee_type_id);


--
-- TOC entry 5258 (class 2606 OID 17837)
-- Name: fee_type fee_type_tenant_id_code_key; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.fee_type
    ADD CONSTRAINT fee_type_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5270 (class 2606 OID 17932)
-- Name: payment_allocation payment_allocation_pkey; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.payment_allocation
    ADD CONSTRAINT payment_allocation_pkey PRIMARY KEY (payment_allocation_id);


--
-- TOC entry 5264 (class 2606 OID 17886)
-- Name: student_invoice_line student_invoice_line_pkey; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice_line
    ADD CONSTRAINT student_invoice_line_pkey PRIMARY KEY (student_invoice_line_id);


--
-- TOC entry 5260 (class 2606 OID 17859)
-- Name: student_invoice student_invoice_pkey; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice
    ADD CONSTRAINT student_invoice_pkey PRIMARY KEY (student_invoice_id);


--
-- TOC entry 5262 (class 2606 OID 17861)
-- Name: student_invoice student_invoice_tenant_id_invoice_number_key; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_invoice
    ADD CONSTRAINT student_invoice_tenant_id_invoice_number_key UNIQUE (tenant_id, invoice_number);


--
-- TOC entry 5266 (class 2606 OID 17910)
-- Name: student_payment student_payment_pkey; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_payment
    ADD CONSTRAINT student_payment_pkey PRIMARY KEY (student_payment_id);


--
-- TOC entry 5268 (class 2606 OID 17912)
-- Name: student_payment student_payment_tenant_id_payment_number_key; Type: CONSTRAINT; Schema: finance; Owner: postgres
--

ALTER TABLE ONLY finance.student_payment
    ADD CONSTRAINT student_payment_tenant_id_payment_number_key UNIQUE (tenant_id, payment_number);


--
-- TOC entry 5665 (class 2606 OID 23127)
-- Name: aggregatedcounter aggregatedcounter_key_key; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_key_key UNIQUE (key);


--
-- TOC entry 5667 (class 2606 OID 23125)
-- Name: aggregatedcounter aggregatedcounter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_pkey PRIMARY KEY (id);


--
-- TOC entry 5627 (class 2606 OID 22939)
-- Name: counter counter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.counter
    ADD CONSTRAINT counter_pkey PRIMARY KEY (id);


--
-- TOC entry 5631 (class 2606 OID 23087)
-- Name: hash hash_key_field_key; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_key_field_key UNIQUE (key, field);


--
-- TOC entry 5633 (class 2606 OID 22949)
-- Name: hash hash_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_pkey PRIMARY KEY (id);


--
-- TOC entry 5639 (class 2606 OID 22960)
-- Name: job job_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (id);


--
-- TOC entry 5661 (class 2606 OID 23013)
-- Name: jobparameter jobparameter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_pkey PRIMARY KEY (id);


--
-- TOC entry 5647 (class 2606 OID 23038)
-- Name: jobqueue jobqueue_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.jobqueue
    ADD CONSTRAINT jobqueue_pkey PRIMARY KEY (id);


--
-- TOC entry 5650 (class 2606 OID 23060)
-- Name: list list_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.list
    ADD CONSTRAINT list_pkey PRIMARY KEY (id);


--
-- TOC entry 5663 (class 2606 OID 22928)
-- Name: lock lock_resource_key; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.lock
    ADD CONSTRAINT lock_resource_key UNIQUE (resource);

ALTER TABLE ONLY hangfire.lock REPLICA IDENTITY USING INDEX lock_resource_key;


--
-- TOC entry 5625 (class 2606 OID 22766)
-- Name: schema schema_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.schema
    ADD CONSTRAINT schema_pkey PRIMARY KEY (version);


--
-- TOC entry 5652 (class 2606 OID 23092)
-- Name: server server_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.server
    ADD CONSTRAINT server_pkey PRIMARY KEY (id);


--
-- TOC entry 5656 (class 2606 OID 23095)
-- Name: set set_key_value_key; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_key_value_key UNIQUE (key, value);


--
-- TOC entry 5658 (class 2606 OID 23070)
-- Name: set set_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_pkey PRIMARY KEY (id);


--
-- TOC entry 5642 (class 2606 OID 22988)
-- Name: state state_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: postgres
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);


--
-- TOC entry 5215 (class 2606 OID 17374)
-- Name: candidate_document candidate_document_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.candidate_document
    ADD CONSTRAINT candidate_document_pkey PRIMARY KEY (candidate_document_id);


--
-- TOC entry 5212 (class 2606 OID 17354)
-- Name: candidate candidate_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.candidate
    ADD CONSTRAINT candidate_pkey PRIMARY KEY (candidate_id);


--
-- TOC entry 5276 (class 2606 OID 17981)
-- Name: employee_compensation employee_compensation_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_compensation
    ADD CONSTRAINT employee_compensation_pkey PRIMARY KEY (employee_compensation_id);


--
-- TOC entry 5199 (class 2606 OID 17260)
-- Name: employee employee_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee
    ADD CONSTRAINT employee_pkey PRIMARY KEY (employee_id);


--
-- TOC entry 5206 (class 2606 OID 17289)
-- Name: employee_position employee_position_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_position
    ADD CONSTRAINT employee_position_pkey PRIMARY KEY (employee_position_id);


--
-- TOC entry 5278 (class 2606 OID 18005)
-- Name: employee_salary_component employee_salary_component_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee_salary_component
    ADD CONSTRAINT employee_salary_component_pkey PRIMARY KEY (employee_compensation_id, salary_component_id);


--
-- TOC entry 5201 (class 2606 OID 17264)
-- Name: employee employee_tenant_id_cnic_number_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee
    ADD CONSTRAINT employee_tenant_id_cnic_number_key UNIQUE (tenant_id, cnic_number);


--
-- TOC entry 5203 (class 2606 OID 17262)
-- Name: employee employee_tenant_id_employee_number_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.employee
    ADD CONSTRAINT employee_tenant_id_employee_number_key UNIQUE (tenant_id, employee_number);


--
-- TOC entry 5284 (class 2606 OID 18091)
-- Name: increment_approval increment_approval_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.increment_approval
    ADD CONSTRAINT increment_approval_pkey PRIMARY KEY (increment_approval_id);


--
-- TOC entry 5280 (class 2606 OID 18039)
-- Name: increment_policy increment_policy_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.increment_policy
    ADD CONSTRAINT increment_policy_pkey PRIMARY KEY (increment_policy_id);


--
-- TOC entry 5227 (class 2606 OID 17485)
-- Name: interview_evaluation interview_evaluation_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_evaluation
    ADD CONSTRAINT interview_evaluation_pkey PRIMARY KEY (interview_evaluation_id);


--
-- TOC entry 5225 (class 2606 OID 17464)
-- Name: interview_panel interview_panel_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview_panel
    ADD CONSTRAINT interview_panel_pkey PRIMARY KEY (interview_id, employee_id);


--
-- TOC entry 5223 (class 2606 OID 17452)
-- Name: interview interview_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.interview
    ADD CONSTRAINT interview_pkey PRIMARY KEY (interview_id);


--
-- TOC entry 5219 (class 2606 OID 17422)
-- Name: job_application job_application_candidate_id_job_vacancy_id_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_application
    ADD CONSTRAINT job_application_candidate_id_job_vacancy_id_key UNIQUE (candidate_id, job_vacancy_id);


--
-- TOC entry 5221 (class 2606 OID 17420)
-- Name: job_application job_application_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_application
    ADD CONSTRAINT job_application_pkey PRIMARY KEY (job_application_id);


--
-- TOC entry 5181 (class 2606 OID 17117)
-- Name: job_family job_family_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_family
    ADD CONSTRAINT job_family_pkey PRIMARY KEY (job_family_id);


--
-- TOC entry 5183 (class 2606 OID 17119)
-- Name: job_family job_family_tenant_id_code_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_family
    ADD CONSTRAINT job_family_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5193 (class 2606 OID 17187)
-- Name: job_grade_mapping job_grade_mapping_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade_mapping
    ADD CONSTRAINT job_grade_mapping_pkey PRIMARY KEY (job_id, job_grade_id);


--
-- TOC entry 5185 (class 2606 OID 17138)
-- Name: job_grade job_grade_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade
    ADD CONSTRAINT job_grade_pkey PRIMARY KEY (job_grade_id);


--
-- TOC entry 5187 (class 2606 OID 17140)
-- Name: job_grade job_grade_tenant_id_code_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_grade
    ADD CONSTRAINT job_grade_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5189 (class 2606 OID 17161)
-- Name: job job_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (job_id);


--
-- TOC entry 5191 (class 2606 OID 17163)
-- Name: job job_tenant_id_code_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job
    ADD CONSTRAINT job_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5217 (class 2606 OID 17392)
-- Name: job_vacancy job_vacancy_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.job_vacancy
    ADD CONSTRAINT job_vacancy_pkey PRIMARY KEY (job_vacancy_id);


--
-- TOC entry 5195 (class 2606 OID 17212)
-- Name: position position_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_pkey PRIMARY KEY (position_id);


--
-- TOC entry 5197 (class 2606 OID 17214)
-- Name: position position_tenant_id_position_code_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr."position"
    ADD CONSTRAINT position_tenant_id_position_code_key UNIQUE (tenant_id, position_code);


--
-- TOC entry 5272 (class 2606 OID 17959)
-- Name: salary_component salary_component_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_component
    ADD CONSTRAINT salary_component_pkey PRIMARY KEY (salary_component_id);


--
-- TOC entry 5274 (class 2606 OID 17961)
-- Name: salary_component salary_component_tenant_id_code_key; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_component
    ADD CONSTRAINT salary_component_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5282 (class 2606 OID 18064)
-- Name: salary_increment_request salary_increment_request_pkey; Type: CONSTRAINT; Schema: hr; Owner: postgres
--

ALTER TABLE ONLY hr.salary_increment_request
    ADD CONSTRAINT salary_increment_request_pkey PRIMARY KEY (increment_request_id);


--
-- TOC entry 5622 (class 2606 OID 20823)
-- Name: DistributedCache DistributedCache_pkey; Type: CONSTRAINT; Schema: infrastructure; Owner: postgres
--

ALTER TABLE ONLY infrastructure."DistributedCache"
    ADD CONSTRAINT "DistributedCache_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5336 (class 2606 OID 18552)
-- Name: item item_pkey; Type: CONSTRAINT; Schema: inventory; Owner: postgres
--

ALTER TABLE ONLY inventory.item
    ADD CONSTRAINT item_pkey PRIMARY KEY (item_id);


--
-- TOC entry 5338 (class 2606 OID 18554)
-- Name: item item_tenant_id_code_key; Type: CONSTRAINT; Schema: inventory; Owner: postgres
--

ALTER TABLE ONLY inventory.item
    ADD CONSTRAINT item_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5330 (class 2606 OID 18505)
-- Name: book_copy book_copy_campus_id_barcode_key; Type: CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_copy
    ADD CONSTRAINT book_copy_campus_id_barcode_key UNIQUE (campus_id, barcode);


--
-- TOC entry 5332 (class 2606 OID 18503)
-- Name: book_copy book_copy_pkey; Type: CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_copy
    ADD CONSTRAINT book_copy_pkey PRIMARY KEY (book_copy_id);


--
-- TOC entry 5334 (class 2606 OID 18527)
-- Name: book_loan book_loan_pkey; Type: CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book_loan
    ADD CONSTRAINT book_loan_pkey PRIMARY KEY (book_loan_id);


--
-- TOC entry 5328 (class 2606 OID 18486)
-- Name: book book_pkey; Type: CONSTRAINT; Schema: library; Owner: postgres
--

ALTER TABLE ONLY library.book
    ADD CONSTRAINT book_pkey PRIMARY KEY (book_id);


--
-- TOC entry 5241 (class 2606 OID 17683)
-- Name: academic_assignment academic_assignment_pkey; Type: CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.academic_assignment
    ADD CONSTRAINT academic_assignment_pkey PRIMARY KEY (academic_assignment_id);


--
-- TOC entry 5243 (class 2606 OID 17725)
-- Name: student_assignment_submission student_assignment_submission_academic_assignment_id_studen_key; Type: CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.student_assignment_submission
    ADD CONSTRAINT student_assignment_submission_academic_assignment_id_studen_key UNIQUE (academic_assignment_id, student_id, attempt_no);


--
-- TOC entry 5245 (class 2606 OID 17723)
-- Name: student_assignment_submission student_assignment_submission_pkey; Type: CONSTRAINT; Schema: lms; Owner: postgres
--

ALTER TABLE ONLY lms.student_assignment_submission
    ADD CONSTRAINT student_assignment_submission_pkey PRIMARY KEY (submission_id);


--
-- TOC entry 5669 (class 2606 OID 23213)
-- Name: application_log application_log_pkey; Type: CONSTRAINT; Schema: observability; Owner: postgres
--

ALTER TABLE ONLY observability.application_log
    ADD CONSTRAINT application_log_pkey PRIMARY KEY (id);


--
-- TOC entry 5094 (class 2606 OID 16533)
-- Name: campus campus_pkey; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.campus
    ADD CONSTRAINT campus_pkey PRIMARY KEY (campus_id);


--
-- TOC entry 5096 (class 2606 OID 16535)
-- Name: campus campus_tenant_id_code_key; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.campus
    ADD CONSTRAINT campus_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5098 (class 2606 OID 16550)
-- Name: department department_pkey; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.department
    ADD CONSTRAINT department_pkey PRIMARY KEY (department_id);


--
-- TOC entry 5100 (class 2606 OID 16552)
-- Name: department department_tenant_id_code_key; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.department
    ADD CONSTRAINT department_tenant_id_code_key UNIQUE (tenant_id, code);


--
-- TOC entry 5102 (class 2606 OID 16575)
-- Name: room room_campus_id_code_key; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.room
    ADD CONSTRAINT room_campus_id_code_key UNIQUE (campus_id, code);


--
-- TOC entry 5104 (class 2606 OID 16573)
-- Name: room room_pkey; Type: CONSTRAINT; Schema: org; Owner: postgres
--

ALTER TABLE ONLY org.room
    ADD CONSTRAINT room_pkey PRIMARY KEY (room_id);


--
-- TOC entry 5292 (class 2606 OID 18156)
-- Name: employee_payroll employee_payroll_payroll_run_id_employee_id_key; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.employee_payroll
    ADD CONSTRAINT employee_payroll_payroll_run_id_employee_id_key UNIQUE (payroll_run_id, employee_id);


--
-- TOC entry 5294 (class 2606 OID 18154)
-- Name: employee_payroll employee_payroll_pkey; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.employee_payroll
    ADD CONSTRAINT employee_payroll_pkey PRIMARY KEY (employee_payroll_id);


--
-- TOC entry 5296 (class 2606 OID 18175)
-- Name: payroll_line_item payroll_line_item_pkey; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_line_item
    ADD CONSTRAINT payroll_line_item_pkey PRIMARY KEY (payroll_line_item_id);


--
-- TOC entry 5286 (class 2606 OID 18109)
-- Name: payroll_period payroll_period_pkey; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_period
    ADD CONSTRAINT payroll_period_pkey PRIMARY KEY (payroll_period_id);


--
-- TOC entry 5288 (class 2606 OID 18111)
-- Name: payroll_period payroll_period_tenant_id_year_month_key; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_period
    ADD CONSTRAINT payroll_period_tenant_id_year_month_key UNIQUE (tenant_id, year, month);


--
-- TOC entry 5290 (class 2606 OID 18129)
-- Name: payroll_run payroll_run_pkey; Type: CONSTRAINT; Schema: payroll; Owner: postgres
--

ALTER TABLE ONLY payroll.payroll_run
    ADD CONSTRAINT payroll_run_pkey PRIMARY KEY (payroll_run_id);


--
-- TOC entry 5534 (class 2606 OID 20123)
-- Name: driverdirectoryread driverdirectoryread_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.driverdirectoryread
    ADD CONSTRAINT driverdirectoryread_pkey PRIMARY KEY (id);


--
-- TOC entry 5522 (class 2606 OID 20045)
-- Name: schooldocument schooldocument_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.schooldocument
    ADD CONSTRAINT schooldocument_pkey PRIMARY KEY (id);


--
-- TOC entry 5526 (class 2606 OID 20076)
-- Name: studentdirectoryread studentdirectoryread_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.studentdirectoryread
    ADD CONSTRAINT studentdirectoryread_pkey PRIMARY KEY (id);


--
-- TOC entry 5530 (class 2606 OID 20100)
-- Name: teacherdirectoryread teacherdirectoryread_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.teacherdirectoryread
    ADD CONSTRAINT teacherdirectoryread_pkey PRIMARY KEY (id);


--
-- TOC entry 5536 (class 2606 OID 20125)
-- Name: driverdirectoryread uq_driverdirectoryread; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.driverdirectoryread
    ADD CONSTRAINT uq_driverdirectoryread UNIQUE (tenantid, driverid);


--
-- TOC entry 5524 (class 2606 OID 20047)
-- Name: schooldocument uq_schooldocument_storage; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.schooldocument
    ADD CONSTRAINT uq_schooldocument_storage UNIQUE (tenantid, storageprovider, storagekey);


--
-- TOC entry 5528 (class 2606 OID 20078)
-- Name: studentdirectoryread uq_studentdirectoryread; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.studentdirectoryread
    ADD CONSTRAINT uq_studentdirectoryread UNIQUE (tenantid, studentid);


--
-- TOC entry 5532 (class 2606 OID 20102)
-- Name: teacherdirectoryread uq_teacherdirectoryread; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.teacherdirectoryread
    ADD CONSTRAINT uq_teacherdirectoryread UNIQUE (tenantid, teacherid);


--
-- TOC entry 5574 (class 2606 OID 20265)
-- Name: AttendanceStatusType AttendanceStatusType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."AttendanceStatusType"
    ADD CONSTRAINT "AttendanceStatusType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5576 (class 2606 OID 20263)
-- Name: AttendanceStatusType AttendanceStatusType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."AttendanceStatusType"
    ADD CONSTRAINT "AttendanceStatusType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5550 (class 2606 OID 20181)
-- Name: BloodGroupType BloodGroupType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."BloodGroupType"
    ADD CONSTRAINT "BloodGroupType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5552 (class 2606 OID 20179)
-- Name: BloodGroupType BloodGroupType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."BloodGroupType"
    ADD CONSTRAINT "BloodGroupType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5582 (class 2606 OID 20300)
-- Name: DocumentTypeLookup DocumentTypeLookup_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."DocumentTypeLookup"
    ADD CONSTRAINT "DocumentTypeLookup_Code_key" UNIQUE ("Code");


--
-- TOC entry 5584 (class 2606 OID 20298)
-- Name: DocumentTypeLookup DocumentTypeLookup_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."DocumentTypeLookup"
    ADD CONSTRAINT "DocumentTypeLookup_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5554 (class 2606 OID 20195)
-- Name: EmploymentStatusType EmploymentStatusType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."EmploymentStatusType"
    ADD CONSTRAINT "EmploymentStatusType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5556 (class 2606 OID 20193)
-- Name: EmploymentStatusType EmploymentStatusType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."EmploymentStatusType"
    ADD CONSTRAINT "EmploymentStatusType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5558 (class 2606 OID 20209)
-- Name: EmploymentType EmploymentType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."EmploymentType"
    ADD CONSTRAINT "EmploymentType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5560 (class 2606 OID 20207)
-- Name: EmploymentType EmploymentType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."EmploymentType"
    ADD CONSTRAINT "EmploymentType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5578 (class 2606 OID 20279)
-- Name: ExamType ExamType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."ExamType"
    ADD CONSTRAINT "ExamType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5580 (class 2606 OID 20277)
-- Name: ExamType ExamType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."ExamType"
    ADD CONSTRAINT "ExamType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5570 (class 2606 OID 20251)
-- Name: FeeStatusType FeeStatusType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."FeeStatusType"
    ADD CONSTRAINT "FeeStatusType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5572 (class 2606 OID 20249)
-- Name: FeeStatusType FeeStatusType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."FeeStatusType"
    ADD CONSTRAINT "FeeStatusType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5546 (class 2606 OID 20167)
-- Name: GenderType GenderType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."GenderType"
    ADD CONSTRAINT "GenderType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5548 (class 2606 OID 20165)
-- Name: GenderType GenderType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."GenderType"
    ADD CONSTRAINT "GenderType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5590 (class 2606 OID 20328)
-- Name: LicenseCategoryType LicenseCategoryType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."LicenseCategoryType"
    ADD CONSTRAINT "LicenseCategoryType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5592 (class 2606 OID 20326)
-- Name: LicenseCategoryType LicenseCategoryType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."LicenseCategoryType"
    ADD CONSTRAINT "LicenseCategoryType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5562 (class 2606 OID 20223)
-- Name: MaritalStatusType MaritalStatusType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."MaritalStatusType"
    ADD CONSTRAINT "MaritalStatusType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5564 (class 2606 OID 20221)
-- Name: MaritalStatusType MaritalStatusType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."MaritalStatusType"
    ADD CONSTRAINT "MaritalStatusType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5538 (class 2606 OID 20139)
-- Name: OccupationType OccupationType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."OccupationType"
    ADD CONSTRAINT "OccupationType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5540 (class 2606 OID 20137)
-- Name: OccupationType OccupationType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."OccupationType"
    ADD CONSTRAINT "OccupationType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5566 (class 2606 OID 20237)
-- Name: PaymentMethodType PaymentMethodType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."PaymentMethodType"
    ADD CONSTRAINT "PaymentMethodType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5568 (class 2606 OID 20235)
-- Name: PaymentMethodType PaymentMethodType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."PaymentMethodType"
    ADD CONSTRAINT "PaymentMethodType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5542 (class 2606 OID 20153)
-- Name: RelationshipType RelationshipType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."RelationshipType"
    ADD CONSTRAINT "RelationshipType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5544 (class 2606 OID 20151)
-- Name: RelationshipType RelationshipType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."RelationshipType"
    ADD CONSTRAINT "RelationshipType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5586 (class 2606 OID 20314)
-- Name: VehicleType VehicleType_Code_key; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."VehicleType"
    ADD CONSTRAINT "VehicleType_Code_key" UNIQUE ("Code");


--
-- TOC entry 5588 (class 2606 OID 20312)
-- Name: VehicleType VehicleType_pkey; Type: CONSTRAINT; Schema: reference; Owner: postgres
--

ALTER TABLE ONLY reference."VehicleType"
    ADD CONSTRAINT "VehicleType_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 5080 (class 2606 OID 16462)
-- Name: lookup_type lookup_type_code_key; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.lookup_type
    ADD CONSTRAINT lookup_type_code_key UNIQUE (code);


--
-- TOC entry 5082 (class 2606 OID 16460)
-- Name: lookup_type lookup_type_pkey; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.lookup_type
    ADD CONSTRAINT lookup_type_pkey PRIMARY KEY (lookup_type_id);


--
-- TOC entry 5084 (class 2606 OID 16480)
-- Name: lookup_value lookup_value_lookup_type_id_code_key; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.lookup_value
    ADD CONSTRAINT lookup_value_lookup_type_id_code_key UNIQUE (lookup_type_id, code);


--
-- TOC entry 5086 (class 2606 OID 16478)
-- Name: lookup_value lookup_value_pkey; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.lookup_value
    ADD CONSTRAINT lookup_value_pkey PRIMARY KEY (lookup_value_id);


--
-- TOC entry 5092 (class 2606 OID 16514)
-- Name: school_branding school_branding_pkey; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.school_branding
    ADD CONSTRAINT school_branding_pkey PRIMARY KEY (tenant_id);


--
-- TOC entry 5088 (class 2606 OID 16506)
-- Name: tenant tenant_code_key; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.tenant
    ADD CONSTRAINT tenant_code_key UNIQUE (code);


--
-- TOC entry 5090 (class 2606 OID 16504)
-- Name: tenant tenant_pkey; Type: CONSTRAINT; Schema: saas; Owner: postgres
--

ALTER TABLE ONLY saas.tenant
    ADD CONSTRAINT tenant_pkey PRIMARY KEY (tenant_id);


--
-- TOC entry 5165 (class 2606 OID 17005)
-- Name: guardian guardian_pkey; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.guardian
    ADD CONSTRAINT guardian_pkey PRIMARY KEY (guardian_id);


--
-- TOC entry 5167 (class 2606 OID 17007)
-- Name: guardian guardian_tenant_id_cnic_number_key; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.guardian
    ADD CONSTRAINT guardian_tenant_id_cnic_number_key UNIQUE (tenant_id, cnic_number);


--
-- TOC entry 5177 (class 2606 OID 17090)
-- Name: student_course_enrollment student_course_enrollment_pkey; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_course_enrollment
    ADD CONSTRAINT student_course_enrollment_pkey PRIMARY KEY (student_course_enrollment_id);


--
-- TOC entry 5179 (class 2606 OID 17092)
-- Name: student_course_enrollment student_course_enrollment_student_enrollment_id_course_offe_key; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_course_enrollment
    ADD CONSTRAINT student_course_enrollment_student_enrollment_id_course_offe_key UNIQUE (student_enrollment_id, course_offering_id);


--
-- TOC entry 5172 (class 2606 OID 17053)
-- Name: student_enrollment student_enrollment_pkey; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_pkey PRIMARY KEY (student_enrollment_id);


--
-- TOC entry 5174 (class 2606 OID 17055)
-- Name: student_enrollment student_enrollment_student_id_academic_year_id_key; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_enrollment
    ADD CONSTRAINT student_enrollment_student_id_academic_year_id_key UNIQUE (student_id, academic_year_id);


--
-- TOC entry 5169 (class 2606 OID 17028)
-- Name: student_guardian student_guardian_pkey; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student_guardian
    ADD CONSTRAINT student_guardian_pkey PRIMARY KEY (student_id, guardian_id);


--
-- TOC entry 5161 (class 2606 OID 16987)
-- Name: student student_pkey; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student
    ADD CONSTRAINT student_pkey PRIMARY KEY (student_id);


--
-- TOC entry 5163 (class 2606 OID 16989)
-- Name: student student_tenant_id_student_number_key; Type: CONSTRAINT; Schema: student; Owner: postgres
--

ALTER TABLE ONLY student.student
    ADD CONSTRAINT student_tenant_id_student_number_key UNIQUE (tenant_id, student_number);


--
-- TOC entry 5695 (class 2606 OID 23384)
-- Name: leave_request leave_request_pkey; Type: CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.leave_request
    ADD CONSTRAINT leave_request_pkey PRIMARY KEY (leave_request_id);


--
-- TOC entry 5688 (class 2606 OID 23344)
-- Name: teacher_actor teacher_actor_pkey; Type: CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_pkey PRIMARY KEY (teacher_id);


--
-- TOC entry 5690 (class 2606 OID 23346)
-- Name: teacher_actor teacher_actor_tenant_id_employee_id_key; Type: CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_tenant_id_employee_id_key UNIQUE (tenant_id, employee_id);


--
-- TOC entry 5692 (class 2606 OID 23348)
-- Name: teacher_actor teacher_actor_tenant_id_user_id_key; Type: CONSTRAINT; Schema: teacher; Owner: postgres
--

ALTER TABLE ONLY teacher.teacher_actor
    ADD CONSTRAINT teacher_actor_tenant_id_user_id_key UNIQUE (tenant_id, user_id);


--
-- TOC entry 5340 (class 2606 OID 18579)
-- Name: driver driver_pkey; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_pkey PRIMARY KEY (driver_id);


--
-- TOC entry 5342 (class 2606 OID 18583)
-- Name: driver driver_tenant_id_cnic_number_key; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_tenant_id_cnic_number_key UNIQUE (tenant_id, cnic_number);


--
-- TOC entry 5344 (class 2606 OID 18581)
-- Name: driver driver_tenant_id_driver_number_key; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_tenant_id_driver_number_key UNIQUE (tenant_id, driver_number);


--
-- TOC entry 5346 (class 2606 OID 18585)
-- Name: driver driver_tenant_id_driving_license_number_key; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.driver
    ADD CONSTRAINT driver_tenant_id_driving_license_number_key UNIQUE (tenant_id, driving_license_number);


--
-- TOC entry 5356 (class 2606 OID 18663)
-- Name: route route_campus_id_code_key; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.route
    ADD CONSTRAINT route_campus_id_code_key UNIQUE (campus_id, code);


--
-- TOC entry 5358 (class 2606 OID 18661)
-- Name: route route_pkey; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.route
    ADD CONSTRAINT route_pkey PRIMARY KEY (route_id);


--
-- TOC entry 5354 (class 2606 OID 18634)
-- Name: vehicle_driver_assignment vehicle_driver_assignment_pkey; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle_driver_assignment
    ADD CONSTRAINT vehicle_driver_assignment_pkey PRIMARY KEY (vehicle_driver_assignment_id);


--
-- TOC entry 5349 (class 2606 OID 18608)
-- Name: vehicle vehicle_pkey; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle
    ADD CONSTRAINT vehicle_pkey PRIMARY KEY (vehicle_id);


--
-- TOC entry 5351 (class 2606 OID 18610)
-- Name: vehicle vehicle_tenant_id_registration_no_key; Type: CONSTRAINT; Schema: transport; Owner: postgres
--

ALTER TABLE ONLY transport.vehicle
    ADD CONSTRAINT vehicle_tenant_id_registration_no_key UNIQUE (tenant_id, registration_no);


--
-- TOC entry 5298 (class 2606 OID 18203)
-- Name: work_assignment work_assignment_pkey; Type: CONSTRAINT; Schema: workflow; Owner: postgres
--

ALTER TABLE ONLY workflow.work_assignment
    ADD CONSTRAINT work_assignment_pkey PRIMARY KEY (work_assignment_id);


--
-- TOC entry 5208 (class 1259 OID 19785)
-- Name: ix_teacher_assignment_employee; Type: INDEX; Schema: academic; Owner: postgres
--

CREATE INDEX ix_teacher_assignment_employee ON academic.teacher_course_assignment USING btree (employee_id, effective_to);


--
-- TOC entry 5236 (class 1259 OID 19786)
-- Name: ix_timetable_section_day; Type: INDEX; Schema: academic; Owner: postgres
--

CREATE INDEX ix_timetable_section_day ON academic.timetable_entry USING btree (class_section_id, day_of_week, timetable_period_id);


--
-- TOC entry 5237 (class 1259 OID 19787)
-- Name: ix_timetable_teacher; Type: INDEX; Schema: academic; Owner: postgres
--

CREATE INDEX ix_timetable_teacher ON academic.timetable_entry USING btree (teacher_course_assignment_id, day_of_week, timetable_period_id);


--
-- TOC entry 5455 (class 1259 OID 19765)
-- Name: ix_class_insight_section_course; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_class_insight_section_course ON ai.class_performance_insight USING btree (class_section_id, course_offering_id, generated_at DESC);


--
-- TOC entry 5441 (class 1259 OID 19763)
-- Name: ix_perf_prediction_risk; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_perf_prediction_risk ON ai.student_performance_prediction USING btree (tenant_id, risk_level, generated_at DESC);


--
-- TOC entry 5442 (class 1259 OID 19761)
-- Name: ix_perf_prediction_student_subject; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_perf_prediction_student_subject ON ai.student_performance_prediction USING btree (student_id, subject_id, generated_at DESC);


--
-- TOC entry 5443 (class 1259 OID 19762)
-- Name: ix_perf_prediction_target_exam; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_perf_prediction_target_exam ON ai.student_performance_prediction USING btree (target_exam_id, student_id);


--
-- TOC entry 5446 (class 1259 OID 19764)
-- Name: ix_prediction_evidence_prediction; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_prediction_evidence_prediction ON ai.prediction_evidence USING btree (student_performance_prediction_id);


--
-- TOC entry 5438 (class 1259 OID 19381)
-- Name: ix_prediction_student_type; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_prediction_student_type ON ai.prediction USING btree (student_id, prediction_type, predicted_at DESC);


--
-- TOC entry 5474 (class 1259 OID 19768)
-- Name: ix_progress_recommendation_student_audience; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_progress_recommendation_student_audience ON ai.student_progress_recommendation USING btree (student_id, audience, status, generated_at DESC);


--
-- TOC entry 5461 (class 1259 OID 19767)
-- Name: ix_student_intervention_student; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_student_intervention_student ON ai.student_intervention USING btree (student_id, status, created_at DESC);


--
-- TOC entry 5458 (class 1259 OID 19766)
-- Name: ix_teaching_recommendation_teacher; Type: INDEX; Schema: ai; Owner: postgres
--

CREATE INDEX ix_teaching_recommendation_teacher ON ai.teaching_recommendation USING btree (teacher_employee_id, status, generated_at DESC);


--
-- TOC entry 5615 (class 1259 OID 20813)
-- Name: IX_RagKnowledgeChunks_Embedding_Hnsw; Type: INDEX; Schema: ai_core; Owner: postgres
--

CREATE INDEX "IX_RagKnowledgeChunks_Embedding_Hnsw" ON ai_core."RagKnowledgeChunks" USING hnsw ("Embedding" public.vector_cosine_ops) WHERE ("Embedding" IS NOT NULL);


--
-- TOC entry 5616 (class 1259 OID 20812)
-- Name: IX_RagKnowledgeChunks_TenantDocument; Type: INDEX; Schema: ai_core; Owner: postgres
--

CREATE INDEX "IX_RagKnowledgeChunks_TenantDocument" ON ai_core."RagKnowledgeChunks" USING btree ("TenantId", "DocumentId");


--
-- TOC entry 5682 (class 1259 OID 23322)
-- Name: ix_rag_chunk_tenant_collection; Type: INDEX; Schema: ai_core; Owner: postgres
--

CREATE INDEX ix_rag_chunk_tenant_collection ON ai_core.rag_knowledge_chunk USING btree (tenant_id, collection, is_active);


--
-- TOC entry 5421 (class 1259 OID 19378)
-- Name: ix_inquiry_conversation_session; Type: INDEX; Schema: ai_inquiry; Owner: postgres
--

CREATE INDEX ix_inquiry_conversation_session ON ai_inquiry.inquiry_conversation USING btree (tenant_id, visitor_session_id, started_at DESC);


--
-- TOC entry 5424 (class 1259 OID 19379)
-- Name: ix_inquiry_message_conversation; Type: INDEX; Schema: ai_inquiry; Owner: postgres
--

CREATE INDEX ix_inquiry_message_conversation ON ai_inquiry.inquiry_message USING btree (inquiry_conversation_id, created_at);


--
-- TOC entry 5429 (class 1259 OID 19380)
-- Name: ix_parent_conversation_guardian; Type: INDEX; Schema: ai_parent; Owner: postgres
--

CREATE INDEX ix_parent_conversation_guardian ON ai_parent.parent_conversation USING btree (guardian_id, started_at DESC);


--
-- TOC entry 5393 (class 1259 OID 19376)
-- Name: ix_tutor_conversation_student; Type: INDEX; Schema: ai_tutor; Owner: postgres
--

CREATE INDEX ix_tutor_conversation_student ON ai_tutor.tutor_conversation USING btree (student_id, started_at DESC);


--
-- TOC entry 5396 (class 1259 OID 19377)
-- Name: ix_tutor_message_conversation; Type: INDEX; Schema: ai_tutor; Owner: postgres
--

CREATE INDEX ix_tutor_message_conversation ON ai_tutor.tutor_message USING btree (tutor_conversation_id, created_at);


--
-- TOC entry 5681 (class 1259 OID 23304)
-- Name: ix_chat_message_conversation; Type: INDEX; Schema: communication; Owner: postgres
--

CREATE INDEX ix_chat_message_conversation ON communication.chat_message USING btree ("TenantId", "ConversationId", "SentAt");


--
-- TOC entry 5319 (class 1259 OID 19789)
-- Name: ix_message_conversation_time; Type: INDEX; Schema: communication; Owner: postgres
--

CREATE INDEX ix_message_conversation_time ON communication.message USING btree (conversation_id, sent_at DESC);


--
-- TOC entry 5324 (class 1259 OID 19790)
-- Name: ix_notification_user_status; Type: INDEX; Schema: communication; Owner: postgres
--

CREATE INDEX ix_notification_user_status ON communication.notification USING btree (user_id, status, created_at DESC);


--
-- TOC entry 5509 (class 1259 OID 19986)
-- Name: ix_candidatedocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_candidatedocument_hash ON document.candidatedocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5510 (class 1259 OID 19985)
-- Name: ix_candidatedocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_candidatedocument_owner_type ON document.candidatedocument USING btree (tenantid, candidateid, documenttypeid);


--
-- TOC entry 5515 (class 1259 OID 20020)
-- Name: ix_driverdocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_driverdocument_hash ON document.driverdocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5516 (class 1259 OID 20019)
-- Name: ix_driverdocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_driverdocument_owner_type ON document.driverdocument USING btree (tenantid, driverid, documenttypeid);


--
-- TOC entry 5503 (class 1259 OID 19952)
-- Name: ix_employeedocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_employeedocument_hash ON document.employeedocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5504 (class 1259 OID 19951)
-- Name: ix_employeedocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_employeedocument_owner_type ON document.employeedocument USING btree (tenantid, employeeid, documenttypeid);


--
-- TOC entry 5489 (class 1259 OID 19884)
-- Name: ix_parentdocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_parentdocument_hash ON document.parentdocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5490 (class 1259 OID 19883)
-- Name: ix_parentdocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_parentdocument_owner_type ON document.parentdocument USING btree (tenantid, parentid, documenttypeid);


--
-- TOC entry 5483 (class 1259 OID 19850)
-- Name: ix_studentdocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_studentdocument_hash ON document.studentdocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5484 (class 1259 OID 19849)
-- Name: ix_studentdocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_studentdocument_owner_type ON document.studentdocument USING btree (tenantid, studentid, documenttypeid);


--
-- TOC entry 5495 (class 1259 OID 19918)
-- Name: ix_teacherdocument_hash; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_teacherdocument_hash ON document.teacherdocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5496 (class 1259 OID 19917)
-- Name: ix_teacherdocument_owner_type; Type: INDEX; Schema: document; Owner: postgres
--

CREATE INDEX ix_teacherdocument_owner_type ON document.teacherdocument USING btree (tenantid, teacherid, documenttypeid);


--
-- TOC entry 5250 (class 1259 OID 19788)
-- Name: ix_exam_result_student; Type: INDEX; Schema: exam; Owner: postgres
--

CREATE INDEX ix_exam_result_student ON exam.student_exam_result USING btree (student_id);


--
-- TOC entry 5628 (class 1259 OID 23128)
-- Name: ix_hangfire_counter_expireat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_counter_expireat ON hangfire.counter USING btree (expireat);


--
-- TOC entry 5629 (class 1259 OID 23079)
-- Name: ix_hangfire_counter_key; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_counter_key ON hangfire.counter USING btree (key);


--
-- TOC entry 5634 (class 1259 OID 23129)
-- Name: ix_hangfire_hash_expireat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_hash_expireat ON hangfire.hash USING btree (expireat);


--
-- TOC entry 5635 (class 1259 OID 23131)
-- Name: ix_hangfire_job_expireat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_job_expireat ON hangfire.job USING btree (expireat);


--
-- TOC entry 5636 (class 1259 OID 23089)
-- Name: ix_hangfire_job_statename; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_job_statename ON hangfire.job USING btree (statename);


--
-- TOC entry 5637 (class 1259 OID 23170)
-- Name: ix_hangfire_job_statename_is_not_null; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_job_statename_is_not_null ON hangfire.job USING btree (statename) INCLUDE (id) WHERE (statename IS NOT NULL);


--
-- TOC entry 5659 (class 1259 OID 23097)
-- Name: ix_hangfire_jobparameter_jobidandname; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_jobparameter_jobidandname ON hangfire.jobparameter USING btree (jobid, name);


--
-- TOC entry 5643 (class 1259 OID 23169)
-- Name: ix_hangfire_jobqueue_fetchedat_queue_jobid; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_jobqueue_fetchedat_queue_jobid ON hangfire.jobqueue USING btree (fetchedat NULLS FIRST, queue, jobid);


--
-- TOC entry 5644 (class 1259 OID 23048)
-- Name: ix_hangfire_jobqueue_jobidandqueue; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_jobqueue_jobidandqueue ON hangfire.jobqueue USING btree (jobid, queue);


--
-- TOC entry 5645 (class 1259 OID 23132)
-- Name: ix_hangfire_jobqueue_queueandfetchedat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_jobqueue_queueandfetchedat ON hangfire.jobqueue USING btree (queue, fetchedat);


--
-- TOC entry 5648 (class 1259 OID 23134)
-- Name: ix_hangfire_list_expireat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_list_expireat ON hangfire.list USING btree (expireat);


--
-- TOC entry 5653 (class 1259 OID 23136)
-- Name: ix_hangfire_set_expireat; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_set_expireat ON hangfire.set USING btree (expireat);


--
-- TOC entry 5654 (class 1259 OID 23113)
-- Name: ix_hangfire_set_key_score; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_set_key_score ON hangfire.set USING btree (key, score);


--
-- TOC entry 5640 (class 1259 OID 22997)
-- Name: ix_hangfire_state_jobid; Type: INDEX; Schema: hangfire; Owner: postgres
--

CREATE INDEX ix_hangfire_state_jobid ON hangfire.state USING btree (jobid);


--
-- TOC entry 5213 (class 1259 OID 19791)
-- Name: ix_candidate_status; Type: INDEX; Schema: hr; Owner: postgres
--

CREATE INDEX ix_candidate_status ON hr.candidate USING btree (tenant_id, status_code);


--
-- TOC entry 5204 (class 1259 OID 19792)
-- Name: ix_employee_tenant_status; Type: INDEX; Schema: hr; Owner: postgres
--

CREATE INDEX ix_employee_tenant_status ON hr.employee USING btree (tenant_id, status);


--
-- TOC entry 5207 (class 1259 OID 17305)
-- Name: ux_employee_primary_current_position; Type: INDEX; Schema: hr; Owner: postgres
--

CREATE UNIQUE INDEX ux_employee_primary_current_position ON hr.employee_position USING btree (employee_id) WHERE ((is_primary = true) AND (effective_to IS NULL));


--
-- TOC entry 5623 (class 1259 OID 20824)
-- Name: IX_DistributedCache_ExpiresAtTime; Type: INDEX; Schema: infrastructure; Owner: postgres
--

CREATE INDEX "IX_DistributedCache_ExpiresAtTime" ON infrastructure."DistributedCache" USING btree ("ExpiresAtTime");


--
-- TOC entry 5670 (class 1259 OID 23216)
-- Name: ix_application_log_correlation; Type: INDEX; Schema: observability; Owner: postgres
--

CREATE INDEX ix_application_log_correlation ON observability.application_log USING btree (correlation_id);


--
-- TOC entry 5671 (class 1259 OID 23214)
-- Name: ix_application_log_timestamp; Type: INDEX; Schema: observability; Owner: postgres
--

CREATE INDEX ix_application_log_timestamp ON observability.application_log USING btree (timestamp_utc DESC);


--
-- TOC entry 5672 (class 1259 OID 23215)
-- Name: ix_application_log_trace; Type: INDEX; Schema: observability; Owner: postgres
--

CREATE INDEX ix_application_log_trace ON observability.application_log USING btree (trace_id);


--
-- TOC entry 5519 (class 1259 OID 20054)
-- Name: ix_schooldocument_hash; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_schooldocument_hash ON public.schooldocument USING btree (tenantid, sha256hash);


--
-- TOC entry 5520 (class 1259 OID 20053)
-- Name: ix_schooldocument_owner_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_schooldocument_owner_type ON public.schooldocument USING btree (tenantid, schoolid, documenttypeid);


--
-- TOC entry 5175 (class 1259 OID 19784)
-- Name: ix_course_enrollment_course; Type: INDEX; Schema: student; Owner: postgres
--

CREATE INDEX ix_course_enrollment_course ON student.student_course_enrollment USING btree (course_offering_id, status);


--
-- TOC entry 5170 (class 1259 OID 19783)
-- Name: ix_enrollment_class; Type: INDEX; Schema: student; Owner: postgres
--

CREATE INDEX ix_enrollment_class ON student.student_enrollment USING btree (class_section_id, status);


--
-- TOC entry 5159 (class 1259 OID 19782)
-- Name: ix_student_tenant_name; Type: INDEX; Schema: student; Owner: postgres
--

CREATE INDEX ix_student_tenant_name ON student.student USING btree (tenant_id, last_name, first_name);


--
-- TOC entry 5685 (class 1259 OID 23364)
-- Name: ix_teacher_actor_tenant_campus; Type: INDEX; Schema: teacher; Owner: postgres
--

CREATE INDEX ix_teacher_actor_tenant_campus ON teacher.teacher_actor USING btree (tenant_id, primary_campus_id, is_active);


--
-- TOC entry 5686 (class 1259 OID 23365)
-- Name: ix_teacher_actor_user; Type: INDEX; Schema: teacher; Owner: postgres
--

CREATE INDEX ix_teacher_actor_user ON teacher.teacher_actor USING btree (user_id) WHERE (user_id IS NOT NULL);


--
-- TOC entry 5693 (class 1259 OID 23395)
-- Name: ix_teacher_leave_employee_status; Type: INDEX; Schema: teacher; Owner: postgres
--

CREATE INDEX ix_teacher_leave_employee_status ON teacher.leave_request USING btree (tenant_id, employee_id, status);


--
-- TOC entry 5347 (class 1259 OID 18596)
-- Name: ix_driver_tenant_status; Type: INDEX; Schema: transport; Owner: postgres
--

CREATE INDEX ix_driver_tenant_status ON transport.driver USING btree (tenant_id, status);


--
-- TOC entry 5352 (class 1259 OID 18650)
-- Name: ix_vehicle_driver_assignment_current; Type: INDEX; Schema: transport; Owner: postgres
--

CREATE INDEX ix_vehicle_driver_assignment_current ON transport.vehicle_driver_assignment USING btree (tenant_id, vehicle_id, effective_to);


--
-- TOC entry 6013 (class 2620 OID 21893)
-- Name: academic_system trg_academic_system_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_academic_system_entity_update BEFORE UPDATE ON academic.academic_system FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6022 (class 2620 OID 21894)
-- Name: academic_year trg_academic_year_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_academic_year_entity_update BEFORE UPDATE ON academic.academic_year FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6016 (class 2620 OID 21895)
-- Name: campus_program trg_campus_program_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_campus_program_entity_update BEFORE UPDATE ON academic.campus_program FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6025 (class 2620 OID 21896)
-- Name: class_section trg_class_section_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_class_section_entity_update BEFORE UPDATE ON academic.class_section FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6026 (class 2620 OID 21897)
-- Name: course_offering trg_course_offering_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_course_offering_entity_update BEFORE UPDATE ON academic.course_offering FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6021 (class 2620 OID 21898)
-- Name: course_selection_group trg_course_selection_group_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_course_selection_group_entity_update BEFORE UPDATE ON academic.course_selection_group FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6014 (class 2620 OID 21899)
-- Name: education_board trg_education_board_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_education_board_entity_update BEFORE UPDATE ON academic.education_board FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6017 (class 2620 OID 21900)
-- Name: grade_level trg_grade_level_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_grade_level_entity_update BEFORE UPDATE ON academic.grade_level FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6015 (class 2620 OID 21901)
-- Name: program trg_program_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_program_entity_update BEFORE UPDATE ON academic.program FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6018 (class 2620 OID 21902)
-- Name: program_grade trg_program_grade_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_program_grade_entity_update BEFORE UPDATE ON academic.program_grade FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6020 (class 2620 OID 21903)
-- Name: program_subject trg_program_subject_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_program_subject_entity_update BEFORE UPDATE ON academic.program_subject FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6024 (class 2620 OID 21904)
-- Name: section trg_section_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_section_entity_update BEFORE UPDATE ON academic.section FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6019 (class 2620 OID 21905)
-- Name: subject trg_subject_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_subject_entity_update BEFORE UPDATE ON academic.subject FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6037 (class 2620 OID 21906)
-- Name: teacher_course_assignment trg_teacher_course_assignment_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_teacher_course_assignment_entity_update BEFORE UPDATE ON academic.teacher_course_assignment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6041 (class 2620 OID 21907)
-- Name: teaching_group trg_teaching_group_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_teaching_group_entity_update BEFORE UPDATE ON academic.teaching_group FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6023 (class 2620 OID 21908)
-- Name: term trg_term_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_term_entity_update BEFORE UPDATE ON academic.term FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6043 (class 2620 OID 21909)
-- Name: timetable trg_timetable_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_timetable_entity_update BEFORE UPDATE ON academic.timetable FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6042 (class 2620 OID 21910)
-- Name: timetable_period trg_timetable_period_entity_update; Type: TRIGGER; Schema: academic; Owner: postgres
--

CREATE TRIGGER trg_timetable_period_entity_update BEFORE UPDATE ON academic.timetable_period FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6058 (class 2620 OID 21911)
-- Name: activity trg_activity_entity_update; Type: TRIGGER; Schema: activity; Owner: postgres
--

CREATE TRIGGER trg_activity_entity_update BEFORE UPDATE ON activity.activity FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6059 (class 2620 OID 21912)
-- Name: student_award trg_student_award_entity_update; Type: TRIGGER; Schema: activity; Owner: postgres
--

CREATE TRIGGER trg_student_award_entity_update BEFORE UPDATE ON activity.student_award FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6083 (class 2620 OID 21913)
-- Name: class_performance_insight trg_class_performance_insight_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_class_performance_insight_entity_update BEFORE UPDATE ON ai.class_performance_insight FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6081 (class 2620 OID 21914)
-- Name: prediction trg_prediction_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_prediction_entity_update BEFORE UPDATE ON ai.prediction FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6080 (class 2620 OID 21915)
-- Name: prediction_model trg_prediction_model_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_prediction_model_entity_update BEFORE UPDATE ON ai.prediction_model FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6085 (class 2620 OID 21916)
-- Name: student_intervention trg_student_intervention_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_student_intervention_entity_update BEFORE UPDATE ON ai.student_intervention FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6082 (class 2620 OID 21917)
-- Name: student_performance_prediction trg_student_performance_prediction_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_student_performance_prediction_entity_update BEFORE UPDATE ON ai.student_performance_prediction FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6086 (class 2620 OID 21918)
-- Name: student_progress_recommendation trg_student_progress_recommendation_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_student_progress_recommendation_entity_update BEFORE UPDATE ON ai.student_progress_recommendation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6084 (class 2620 OID 21919)
-- Name: teaching_recommendation trg_teaching_recommendation_entity_update; Type: TRIGGER; Schema: ai; Owner: postgres
--

CREATE TRIGGER trg_teaching_recommendation_entity_update BEFORE UPDATE ON ai.teaching_recommendation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6074 (class 2620 OID 21920)
-- Name: ai_execution_log trg_ai_execution_log_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_ai_execution_log_entity_update BEFORE UPDATE ON ai_core.ai_execution_log FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6073 (class 2620 OID 21921)
-- Name: assistant_knowledge_collection trg_assistant_knowledge_collection_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_assistant_knowledge_collection_entity_update BEFORE UPDATE ON ai_core.assistant_knowledge_collection FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6072 (class 2620 OID 21922)
-- Name: assistant_tool trg_assistant_tool_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_assistant_tool_entity_update BEFORE UPDATE ON ai_core.assistant_tool FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6070 (class 2620 OID 21923)
-- Name: knowledge_collection trg_knowledge_collection_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_knowledge_collection_entity_update BEFORE UPDATE ON ai_core.knowledge_collection FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6071 (class 2620 OID 21924)
-- Name: knowledge_document trg_knowledge_document_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_knowledge_document_entity_update BEFORE UPDATE ON ai_core.knowledge_document FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6068 (class 2620 OID 21925)
-- Name: model_configuration trg_model_configuration_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_model_configuration_entity_update BEFORE UPDATE ON ai_core.model_configuration FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6069 (class 2620 OID 21926)
-- Name: prompt_template trg_prompt_template_entity_update; Type: TRIGGER; Schema: ai_core; Owner: postgres
--

CREATE TRIGGER trg_prompt_template_entity_update BEFORE UPDATE ON ai_core.prompt_template FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6078 (class 2620 OID 21927)
-- Name: inquiry_conversation trg_inquiry_conversation_entity_update; Type: TRIGGER; Schema: ai_inquiry; Owner: postgres
--

CREATE TRIGGER trg_inquiry_conversation_entity_update BEFORE UPDATE ON ai_inquiry.inquiry_conversation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6079 (class 2620 OID 21928)
-- Name: parent_conversation trg_parent_conversation_entity_update; Type: TRIGGER; Schema: ai_parent; Owner: postgres
--

CREATE TRIGGER trg_parent_conversation_entity_update BEFORE UPDATE ON ai_parent.parent_conversation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6077 (class 2620 OID 21929)
-- Name: generated_quiz trg_generated_quiz_entity_update; Type: TRIGGER; Schema: ai_tutor; Owner: postgres
--

CREATE TRIGGER trg_generated_quiz_entity_update BEFORE UPDATE ON ai_tutor.generated_quiz FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6076 (class 2620 OID 21930)
-- Name: student_topic_mastery trg_student_topic_mastery_entity_update; Type: TRIGGER; Schema: ai_tutor; Owner: postgres
--

CREATE TRIGGER trg_student_topic_mastery_entity_update BEFORE UPDATE ON ai_tutor.student_topic_mastery FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6075 (class 2620 OID 21931)
-- Name: tutor_conversation trg_tutor_conversation_entity_update; Type: TRIGGER; Schema: ai_tutor; Owner: postgres
--

CREATE TRIGGER trg_tutor_conversation_entity_update BEFORE UPDATE ON ai_tutor.tutor_conversation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6060 (class 2620 OID 21932)
-- Name: conversation trg_conversation_entity_update; Type: TRIGGER; Schema: communication; Owner: postgres
--

CREATE TRIGGER trg_conversation_entity_update BEFORE UPDATE ON communication.conversation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6061 (class 2620 OID 21933)
-- Name: notification trg_notification_entity_update; Type: TRIGGER; Schema: communication; Owner: postgres
--

CREATE TRIGGER trg_notification_entity_update BEFORE UPDATE ON communication.notification FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6056 (class 2620 OID 21934)
-- Name: document_template trg_document_template_entity_update; Type: TRIGGER; Schema: document; Owner: postgres
--

CREATE TRIGGER trg_document_template_entity_update BEFORE UPDATE ON document.document_template FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6057 (class 2620 OID 21935)
-- Name: generated_document trg_generated_document_entity_update; Type: TRIGGER; Schema: document; Owner: postgres
--

CREATE TRIGGER trg_generated_document_entity_update BEFORE UPDATE ON document.generated_document FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6045 (class 2620 OID 21936)
-- Name: exam trg_exam_entity_update; Type: TRIGGER; Schema: exam; Owner: postgres
--

CREATE TRIGGER trg_exam_entity_update BEFORE UPDATE ON exam.exam FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6046 (class 2620 OID 21937)
-- Name: fee_type trg_fee_type_entity_update; Type: TRIGGER; Schema: finance; Owner: postgres
--

CREATE TRIGGER trg_fee_type_entity_update BEFORE UPDATE ON finance.fee_type FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6047 (class 2620 OID 21938)
-- Name: student_invoice trg_student_invoice_entity_update; Type: TRIGGER; Schema: finance; Owner: postgres
--

CREATE TRIGGER trg_student_invoice_entity_update BEFORE UPDATE ON finance.student_invoice FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6048 (class 2620 OID 21939)
-- Name: student_payment trg_student_payment_entity_update; Type: TRIGGER; Schema: finance; Owner: postgres
--

CREATE TRIGGER trg_student_payment_entity_update BEFORE UPDATE ON finance.student_payment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6038 (class 2620 OID 21940)
-- Name: candidate trg_candidate_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_candidate_entity_update BEFORE UPDATE ON hr.candidate FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6050 (class 2620 OID 21942)
-- Name: employee_compensation trg_employee_compensation_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_employee_compensation_entity_update BEFORE UPDATE ON hr.employee_compensation FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6035 (class 2620 OID 21941)
-- Name: employee trg_employee_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_employee_entity_update BEFORE UPDATE ON hr.employee FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6036 (class 2620 OID 21943)
-- Name: employee_position trg_employee_position_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_employee_position_entity_update BEFORE UPDATE ON hr.employee_position FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6051 (class 2620 OID 21944)
-- Name: increment_policy trg_increment_policy_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_increment_policy_entity_update BEFORE UPDATE ON hr.increment_policy FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6040 (class 2620 OID 21946)
-- Name: job_application trg_job_application_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_job_application_entity_update BEFORE UPDATE ON hr.job_application FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6033 (class 2620 OID 21945)
-- Name: job trg_job_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_job_entity_update BEFORE UPDATE ON hr.job FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6031 (class 2620 OID 21947)
-- Name: job_family trg_job_family_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_job_family_entity_update BEFORE UPDATE ON hr.job_family FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6032 (class 2620 OID 21948)
-- Name: job_grade trg_job_grade_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_job_grade_entity_update BEFORE UPDATE ON hr.job_grade FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6039 (class 2620 OID 21949)
-- Name: job_vacancy trg_job_vacancy_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_job_vacancy_entity_update BEFORE UPDATE ON hr.job_vacancy FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6034 (class 2620 OID 21950)
-- Name: position trg_position_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_position_entity_update BEFORE UPDATE ON hr."position" FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6049 (class 2620 OID 21951)
-- Name: salary_component trg_salary_component_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_salary_component_entity_update BEFORE UPDATE ON hr.salary_component FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6052 (class 2620 OID 21952)
-- Name: salary_increment_request trg_salary_increment_request_entity_update; Type: TRIGGER; Schema: hr; Owner: postgres
--

CREATE TRIGGER trg_salary_increment_request_entity_update BEFORE UPDATE ON hr.salary_increment_request FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6063 (class 2620 OID 21953)
-- Name: item trg_item_entity_update; Type: TRIGGER; Schema: inventory; Owner: postgres
--

CREATE TRIGGER trg_item_entity_update BEFORE UPDATE ON inventory.item FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6062 (class 2620 OID 21954)
-- Name: book trg_book_entity_update; Type: TRIGGER; Schema: library; Owner: postgres
--

CREATE TRIGGER trg_book_entity_update BEFORE UPDATE ON library.book FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6044 (class 2620 OID 21955)
-- Name: academic_assignment trg_academic_assignment_entity_update; Type: TRIGGER; Schema: lms; Owner: postgres
--

CREATE TRIGGER trg_academic_assignment_entity_update BEFORE UPDATE ON lms.academic_assignment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6010 (class 2620 OID 21956)
-- Name: campus trg_campus_entity_update; Type: TRIGGER; Schema: org; Owner: postgres
--

CREATE TRIGGER trg_campus_entity_update BEFORE UPDATE ON org.campus FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6011 (class 2620 OID 21957)
-- Name: department trg_department_entity_update; Type: TRIGGER; Schema: org; Owner: postgres
--

CREATE TRIGGER trg_department_entity_update BEFORE UPDATE ON org.department FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6012 (class 2620 OID 21958)
-- Name: room trg_room_entity_update; Type: TRIGGER; Schema: org; Owner: postgres
--

CREATE TRIGGER trg_room_entity_update BEFORE UPDATE ON org.room FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6053 (class 2620 OID 21959)
-- Name: payroll_period trg_payroll_period_entity_update; Type: TRIGGER; Schema: payroll; Owner: postgres
--

CREATE TRIGGER trg_payroll_period_entity_update BEFORE UPDATE ON payroll.payroll_period FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6054 (class 2620 OID 21960)
-- Name: payroll_run trg_payroll_run_entity_update; Type: TRIGGER; Schema: payroll; Owner: postgres
--

CREATE TRIGGER trg_payroll_run_entity_update BEFORE UPDATE ON payroll.payroll_run FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6009 (class 2620 OID 21961)
-- Name: school_branding trg_school_branding_entity_update; Type: TRIGGER; Schema: saas; Owner: postgres
--

CREATE TRIGGER trg_school_branding_entity_update BEFORE UPDATE ON saas.school_branding FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6008 (class 2620 OID 21962)
-- Name: tenant trg_tenant_entity_update; Type: TRIGGER; Schema: saas; Owner: postgres
--

CREATE TRIGGER trg_tenant_entity_update BEFORE UPDATE ON saas.tenant FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6028 (class 2620 OID 21963)
-- Name: guardian trg_guardian_entity_update; Type: TRIGGER; Schema: student; Owner: postgres
--

CREATE TRIGGER trg_guardian_entity_update BEFORE UPDATE ON student.guardian FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6030 (class 2620 OID 21965)
-- Name: student_course_enrollment trg_student_course_enrollment_entity_update; Type: TRIGGER; Schema: student; Owner: postgres
--

CREATE TRIGGER trg_student_course_enrollment_entity_update BEFORE UPDATE ON student.student_course_enrollment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6029 (class 2620 OID 21966)
-- Name: student_enrollment trg_student_enrollment_entity_update; Type: TRIGGER; Schema: student; Owner: postgres
--

CREATE TRIGGER trg_student_enrollment_entity_update BEFORE UPDATE ON student.student_enrollment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6027 (class 2620 OID 21964)
-- Name: student trg_student_entity_update; Type: TRIGGER; Schema: student; Owner: postgres
--

CREATE TRIGGER trg_student_entity_update BEFORE UPDATE ON student.student FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6064 (class 2620 OID 21967)
-- Name: driver trg_driver_entity_update; Type: TRIGGER; Schema: transport; Owner: postgres
--

CREATE TRIGGER trg_driver_entity_update BEFORE UPDATE ON transport.driver FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6067 (class 2620 OID 21968)
-- Name: route trg_route_entity_update; Type: TRIGGER; Schema: transport; Owner: postgres
--

CREATE TRIGGER trg_route_entity_update BEFORE UPDATE ON transport.route FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6066 (class 2620 OID 21970)
-- Name: vehicle_driver_assignment trg_vehicle_driver_assignment_entity_update; Type: TRIGGER; Schema: transport; Owner: postgres
--

CREATE TRIGGER trg_vehicle_driver_assignment_entity_update BEFORE UPDATE ON transport.vehicle_driver_assignment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6065 (class 2620 OID 21969)
-- Name: vehicle trg_vehicle_entity_update; Type: TRIGGER; Schema: transport; Owner: postgres
--

CREATE TRIGGER trg_vehicle_entity_update BEFORE UPDATE ON transport.vehicle FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 6055 (class 2620 OID 21971)
-- Name: work_assignment trg_work_assignment_entity_update; Type: TRIGGER; Schema: workflow; Owner: postgres
--

CREATE TRIGGER trg_work_assignment_entity_update BEFORE UPDATE ON workflow.work_assignment FOR EACH ROW EXECUTE FUNCTION infrastructure.smartschool_set_entity_update_fields();


--
-- TOC entry 5703 (class 2606 OID 16601)
-- Name: academic_system academic_system_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_system
    ADD CONSTRAINT academic_system_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5722 (class 2606 OID 16843)
-- Name: academic_year academic_year_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_year
    ADD CONSTRAINT academic_year_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5723 (class 2606 OID 16838)
-- Name: academic_year academic_year_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.academic_year
    ADD CONSTRAINT academic_year_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5707 (class 2606 OID 16669)
-- Name: campus_program campus_program_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.campus_program
    ADD CONSTRAINT campus_program_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5708 (class 2606 OID 16674)
-- Name: campus_program campus_program_program_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.campus_program
    ADD CONSTRAINT campus_program_program_id_fkey FOREIGN KEY (program_id) REFERENCES academic.program(program_id);


--
-- TOC entry 5709 (class 2606 OID 16664)
-- Name: campus_program campus_program_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.campus_program
    ADD CONSTRAINT campus_program_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5727 (class 2606 OID 16916)
-- Name: class_section class_section_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5728 (class 2606 OID 16911)
-- Name: class_section class_section_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5729 (class 2606 OID 16921)
-- Name: class_section class_section_program_grade_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_program_grade_id_fkey FOREIGN KEY (program_grade_id) REFERENCES academic.program_grade(program_grade_id);


--
-- TOC entry 5730 (class 2606 OID 16931)
-- Name: class_section class_section_room_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_room_id_fkey FOREIGN KEY (room_id) REFERENCES org.room(room_id);


--
-- TOC entry 5731 (class 2606 OID 16926)
-- Name: class_section class_section_section_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_section_id_fkey FOREIGN KEY (section_id) REFERENCES academic.section(section_id);


--
-- TOC entry 5732 (class 2606 OID 16906)
-- Name: class_section class_section_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT class_section_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5734 (class 2606 OID 16959)
-- Name: course_offering course_offering_academic_year_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_academic_year_id_fkey FOREIGN KEY (academic_year_id) REFERENCES academic.academic_year(academic_year_id);


--
-- TOC entry 5735 (class 2606 OID 16954)
-- Name: course_offering course_offering_campus_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id);


--
-- TOC entry 5736 (class 2606 OID 16969)
-- Name: course_offering course_offering_program_subject_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_program_subject_id_fkey FOREIGN KEY (program_subject_id) REFERENCES academic.program_subject(program_subject_id);


--
-- TOC entry 5737 (class 2606 OID 16949)
-- Name: course_offering course_offering_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5738 (class 2606 OID 16964)
-- Name: course_offering course_offering_term_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_offering
    ADD CONSTRAINT course_offering_term_id_fkey FOREIGN KEY (term_id) REFERENCES academic.term(term_id);


--
-- TOC entry 5720 (class 2606 OID 16817)
-- Name: course_selection_group_course course_selection_group_course_program_subject_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group_course
    ADD CONSTRAINT course_selection_group_course_program_subject_id_fkey FOREIGN KEY (program_subject_id) REFERENCES academic.program_subject(program_subject_id);


--
-- TOC entry 5721 (class 2606 OID 16812)
-- Name: course_selection_group_course course_selection_group_course_selection_group_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group_course
    ADD CONSTRAINT course_selection_group_course_selection_group_id_fkey FOREIGN KEY (selection_group_id) REFERENCES academic.course_selection_group(selection_group_id);


--
-- TOC entry 5718 (class 2606 OID 16800)
-- Name: course_selection_group course_selection_group_program_grade_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group
    ADD CONSTRAINT course_selection_group_program_grade_id_fkey FOREIGN KEY (program_grade_id) REFERENCES academic.program_grade(program_grade_id);


--
-- TOC entry 5719 (class 2606 OID 16795)
-- Name: course_selection_group course_selection_group_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.course_selection_group
    ADD CONSTRAINT course_selection_group_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5704 (class 2606 OID 16618)
-- Name: education_board education_board_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.education_board
    ADD CONSTRAINT education_board_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5733 (class 2606 OID 17270)
-- Name: class_section fk_class_teacher; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.class_section
    ADD CONSTRAINT fk_class_teacher FOREIGN KEY (class_teacher_employee_id) REFERENCES hr.employee(employee_id);


--
-- TOC entry 5767 (class 2606 OID 17534)
-- Name: teacher_course_assignment fk_teacher_assignment_group; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.teacher_course_assignment
    ADD CONSTRAINT fk_teacher_assignment_group FOREIGN KEY (teaching_group_id) REFERENCES academic.teaching_group(teaching_group_id);


--
-- TOC entry 5710 (class 2606 OID 16693)
-- Name: grade_level grade_level_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.grade_level
    ADD CONSTRAINT grade_level_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5705 (class 2606 OID 16645)
-- Name: program program_academic_system_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program
    ADD CONSTRAINT program_academic_system_id_fkey FOREIGN KEY (academic_system_id) REFERENCES academic.academic_system(academic_system_id);


--
-- TOC entry 5711 (class 2606 OID 16722)
-- Name: program_grade program_grade_grade_level_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_grade
    ADD CONSTRAINT program_grade_grade_level_id_fkey FOREIGN KEY (grade_level_id) REFERENCES academic.grade_level(grade_level_id);


--
-- TOC entry 5712 (class 2606 OID 16717)
-- Name: program_grade program_grade_program_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_grade
    ADD CONSTRAINT program_grade_program_id_fkey FOREIGN KEY (program_id) REFERENCES academic.program(program_id);


--
-- TOC entry 5713 (class 2606 OID 16712)
-- Name: program_grade program_grade_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_grade
    ADD CONSTRAINT program_grade_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5715 (class 2606 OID 16770)
-- Name: program_subject program_subject_program_grade_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_subject
    ADD CONSTRAINT program_subject_program_grade_id_fkey FOREIGN KEY (program_grade_id) REFERENCES academic.program_grade(program_grade_id);


--
-- TOC entry 5716 (class 2606 OID 16775)
-- Name: program_subject program_subject_subject_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_subject
    ADD CONSTRAINT program_subject_subject_id_fkey FOREIGN KEY (subject_id) REFERENCES academic.subject(subject_id);


--
-- TOC entry 5717 (class 2606 OID 16765)
-- Name: program_subject program_subject_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program_subject
    ADD CONSTRAINT program_subject_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5706 (class 2606 OID 16640)
-- Name: program program_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.program
    ADD CONSTRAINT program_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


--
-- TOC entry 5726 (class 2606 OID 16885)
-- Name: section section_tenant_id_fkey; Type: FK CONSTRAINT; Schema: academic; Owner: postgres
--

ALTER TABLE ONLY academic.section
    ADD CONSTRAINT section_tenant_id_fkey FOREIGN KEY (tenant_id) REFERENCES saas.tenant(tenant_id);


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

BEGIN;

-- Align communication.notification with NotificationEntity and Dapper queries.
ALTER TABLE communication.notification
    ADD COLUMN IF NOT EXISTS recipient_user_id uuid,
    ADD COLUMN IF NOT EXISTS type varchar(100),
    ADD COLUMN IF NOT EXISTS message text,
    ADD COLUMN IF NOT EXISTS related_entity_id uuid,
    ADD COLUMN IF NOT EXISTS related_entity_type varchar(100),
    ADD COLUMN IF NOT EXISTS action_url varchar(500),
    ADD COLUMN IF NOT EXISTS priority varchar(50) NOT NULL DEFAULT 'Normal',
    ADD COLUMN IF NOT EXISTS is_read boolean NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS read_at timestamptz,
    ADD COLUMN IF NOT EXISTS occurred_at timestamptz;

-- Preserve data from the previous notification shape when upgrading an existing DB.
UPDATE communication.notification
SET recipient_user_id = COALESCE(recipient_user_id, user_id),
    type = COALESCE(type, channel_code),
    message = COALESCE(message, body, ''),
    occurred_at = COALESCE(occurred_at, created_at)
WHERE recipient_user_id IS NULL
   OR type IS NULL
   OR message IS NULL
   OR occurred_at IS NULL;

ALTER TABLE communication.notification
    ALTER COLUMN recipient_user_id SET NOT NULL,
    ALTER COLUMN type SET NOT NULL,
    ALTER COLUMN message SET NOT NULL,
    ALTER COLUMN occurred_at SET NOT NULL,
    ALTER COLUMN user_id DROP NOT NULL,
    ALTER COLUMN channel_code DROP NOT NULL;

CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient
    ON communication.notification (tenant_id, recipient_user_id);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient_unread
    ON communication.notification (tenant_id, recipient_user_id, is_read);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_recipient_occurred
    ON communication.notification (tenant_id, recipient_user_id, occurred_at DESC);
CREATE INDEX IF NOT EXISTS ix_notification_tenant_type
    ON communication.notification (tenant_id, type);

-- Operations UI reads newest logs and commonly filters by trace/correlation id.
CREATE INDEX IF NOT EXISTS ix_application_log_timestamp_utc
    ON observability.application_log (timestamp_utc DESC);
CREATE INDEX IF NOT EXISTS ix_application_log_level_timestamp
    ON observability.application_log (level, timestamp_utc DESC);
CREATE INDEX IF NOT EXISTS ix_application_log_trace_id
    ON observability.application_log (trace_id);
CREATE INDEX IF NOT EXISTS ix_application_log_correlation_id
    ON observability.application_log (correlation_id);

COMMIT;


-- v71 comprehensive synchronization
-- SmartSchool v71 comprehensive model/schema synchronization
-- Generated 2026-08-24. Idempotent PostgreSQL migration.


CREATE SCHEMA IF NOT EXISTS ai;

CREATE TABLE IF NOT EXISTS ai.ml_prediction_result (
    ml_prediction_result_id uuid DEFAULT gen_random_uuid() NOT NULL,
    tenant_id uuid NOT NULL,
    prediction_type text NOT NULL,
    student_id uuid,
    subject_id uuid,
    related_entity_id uuid,
    score numeric(18,6) NOT NULL,
    probability numeric(18,6) NOT NULL,
    risk_level text NOT NULL,
    outcome text NOT NULL,
    confidence_score numeric(18,6) NOT NULL,
    model_version text NOT NULL,
    used_machine_learning boolean NOT NULL,
    factors_json jsonb,
    generated_at timestamp with time zone NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    CONSTRAINT pk_ml_prediction_result PRIMARY KEY (ml_prediction_result_id)
);

CREATE INDEX IF NOT EXISTS ix_ml_prediction_result_tenant_id ON ai.ml_prediction_result(tenant_id);

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.topic_performance_insight ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.prediction_evidence ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction_model ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.class_performance_insight ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.teaching_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.student_performance_prediction ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.student_intervention ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai.prediction_evaluation ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS subject_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_subject_id uuid;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS target_exam_type_code text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_marks numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS predicted_grade text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS lower_bound_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS upper_bound_percentage numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS confidence_score numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS pass_probability numeric(18,6);

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS trend text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS model_version text;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS historical_result_count integer;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS used_machine_learning boolean;

ALTER TABLE ai.prediction ADD COLUMN IF NOT EXISTS generated_at timestamp with time zone;

ALTER TABLE academic.subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.timetable ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_course_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.course_offering ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.program ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE academic.timetable_entry ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE academic.academic_system ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.term ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.teacher_course_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.academic_year ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.student_topic_mastery ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.tutor_session ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.tutor_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.learning_recommendation ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.generated_quiz ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_tutor.student_quiz_attempt ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_parent.parent_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_parent.parent_tool_execution ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.admissiondecision (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    admission_decision_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_admissiondecision PRIMARY KEY (admission_decision_id)
);

CREATE INDEX IF NOT EXISTS ix_admissiondecision_tenant_id ON admission.admissiondecision(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.application (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    application_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_application PRIMARY KEY (application_id)
);

CREATE INDEX IF NOT EXISTS ix_application_tenant_id ON admission.application(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.applicant (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    applicant_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_applicant PRIMARY KEY (applicant_id)
);

CREATE INDEX IF NOT EXISTS ix_applicant_tenant_id ON admission.applicant(tenant_id);

CREATE SCHEMA IF NOT EXISTS admission;

CREATE TABLE IF NOT EXISTS admission.inquiry (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    inquiry_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_inquiry PRIMARY KEY (inquiry_id)
);

CREATE INDEX IF NOT EXISTS ix_inquiry_tenant_id ON admission.inquiry(tenant_id);

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE exam.exam_subject ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE exam.student_exam_result ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE exam.exam ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS exam;

CREATE TABLE IF NOT EXISTS exam.gradescale (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    grade_scale_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_gradescale PRIMARY KEY (grade_scale_id)
);

CREATE INDEX IF NOT EXISTS ix_gradescale_tenant_id ON exam.gradescale(tenant_id);

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.position ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE teacher.leave_request ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.job ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.job_grade ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.candidate ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE hr.interview ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE audit.audit_log ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE org.department ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS org;

CREATE TABLE IF NOT EXISTS org.school (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    school_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_school PRIMARY KEY (school_id)
);

CREATE INDEX IF NOT EXISTS ix_school_tenant_id ON org.school(tenant_id);

ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.inquiry_conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.lead_capture ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.human_handoff ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_inquiry.inquiry_message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.payslip (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    payslip_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_payslip PRIMARY KEY (payslip_id)
);

CREATE INDEX IF NOT EXISTS ix_payslip_tenant_id ON payroll.payslip(tenant_id);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.increment (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    increment_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_increment PRIMARY KEY (increment_id)
);

CREATE INDEX IF NOT EXISTS ix_increment_tenant_id ON payroll.increment(tenant_id);

CREATE SCHEMA IF NOT EXISTS payroll;

CREATE TABLE IF NOT EXISTS payroll.salarystructure (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    salary_structure_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_salarystructure PRIMARY KEY (salary_structure_id)
);

CREATE INDEX IF NOT EXISTS ix_salarystructure_tenant_id ON payroll.salarystructure(tenant_id);

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE hr.employee_compensation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE payroll.payroll_run ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.attendance (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    attendance_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_attendance PRIMARY KEY (attendance_id)
);

CREATE INDEX IF NOT EXISTS ix_attendance_tenant_id ON student.attendance(tenant_id);

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.parentprofile (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    parent_profile_id uuid DEFAULT gen_random_uuid() NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    cnic text NOT NULL,
    relationship_code text NOT NULL,
    mobile_number text NOT NULL,
    alternate_mobile_number text,
    email_address text,
    occupation text,
    employer_name text,
    work_address text,
    residential_address text,
    is_primary_guardian boolean NOT NULL,
    is_emergency_contact boolean NOT NULL,
    can_collect_student boolean NOT NULL,
    CONSTRAINT pk_parentprofile PRIMARY KEY (parent_profile_id)
);

CREATE INDEX IF NOT EXISTS ix_parentprofile_tenant_id ON student.parentprofile(tenant_id);

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_enrollment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE student.student_guardian ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS student;

CREATE TABLE IF NOT EXISTS student.studentprofile (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    student_profile_id uuid DEFAULT gen_random_uuid() NOT NULL,
    student_id uuid NOT NULL,
    admission_number text NOT NULL,
    first_name text NOT NULL,
    middle_name text,
    last_name text NOT NULL,
    date_of_birth date NOT NULL,
    gender_code text NOT NULL,
    b_form_number text,
    passport_number text,
    blood_group_code text,
    primary_language_code text,
    mobile_number text,
    email_address text,
    address_line1 text,
    address_line2 text,
    city text,
    province text,
    postal_code text,
    country_code text,
    emergency_contact_name text,
    emergency_contact_phone text,
    medical_notes text,
    allergies text,
    admission_date date NOT NULL,
    current_program_id uuid,
    current_class_id uuid,
    current_section_id uuid,
    CONSTRAINT pk_studentprofile PRIMARY KEY (student_profile_id)
);

CREATE INDEX IF NOT EXISTS ix_studentprofile_tenant_id ON student.studentprofile(tenant_id);

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.conversation ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS recipient_user_id uuid;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS type text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS message text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_id uuid;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS related_entity_type text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS action_url text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS priority text;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS is_read boolean;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS read_at timestamp with time zone;

ALTER TABLE communication.notification ADD COLUMN IF NOT EXISTS occurred_at timestamp with time zone;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.conversation_participant ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.message_receipt ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE communication.message ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowinstance (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_instance_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowinstance PRIMARY KEY (workflow_instance_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowinstance_tenant_id ON workflow.workflowinstance(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowstep (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_step_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowstep PRIMARY KEY (workflow_step_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowstep_tenant_id ON workflow.workflowstep(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.approval (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    approval_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_approval PRIMARY KEY (approval_id)
);

CREATE INDEX IF NOT EXISTS ix_approval_tenant_id ON workflow.approval(tenant_id);

CREATE SCHEMA IF NOT EXISTS workflow;

CREATE TABLE IF NOT EXISTS workflow.workflowdefinition (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    workflow_definition_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_workflowdefinition PRIMARY KEY (workflow_definition_id)
);

CREATE INDEX IF NOT EXISTS ix_workflowdefinition_tenant_id ON workflow.workflowdefinition(tenant_id);

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE document.generated_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS document;

CREATE TABLE IF NOT EXISTS document.certificate (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    certificate_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_certificate PRIMARY KEY (certificate_id)
);

CREATE INDEX IF NOT EXISTS ix_certificate_tenant_id ON document.certificate(tenant_id);

CREATE SCHEMA IF NOT EXISTS document;

CREATE TABLE IF NOT EXISTS document.schoollogo (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    school_logo_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_schoollogo PRIMARY KEY (school_logo_id)
);

CREATE INDEX IF NOT EXISTS ix_schoollogo_tenant_id ON document.schoollogo(tenant_id);

ALTER TABLE document.document_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE transport.route ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS transport;

CREATE TABLE IF NOT EXISTS transport.studenttransport (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_transport_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studenttransport PRIMARY KEY (student_transport_id)
);

CREATE INDEX IF NOT EXISTS ix_studenttransport_tenant_id ON transport.studenttransport(tenant_id);

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS employee_number text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS first_name text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS last_name text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS cnic text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS mobile_number text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS license_expiry_date date;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS joining_date date;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS employment_status_code text;

ALTER TABLE transport.driver ADD COLUMN IF NOT EXISTS assigned_vehicle_id uuid;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE transport.vehicle ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS transport;

CREATE TABLE IF NOT EXISTS transport.stop (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    stop_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_stop PRIMARY KEY (stop_id)
);

CREATE INDEX IF NOT EXISTS ix_stop_tenant_id ON transport.stop(tenant_id);

ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.scholarship (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    scholarship_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_scholarship PRIMARY KEY (scholarship_id)
);

CREATE INDEX IF NOT EXISTS ix_scholarship_tenant_id ON finance.scholarship(tenant_id);

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE finance.student_invoice ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.feestructure (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    fee_structure_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_feestructure PRIMARY KEY (fee_structure_id)
);

CREATE INDEX IF NOT EXISTS ix_feestructure_tenant_id ON finance.feestructure(tenant_id);

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE finance.student_payment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.discount (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    discount_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_discount PRIMARY KEY (discount_id)
);

CREATE INDEX IF NOT EXISTS ix_discount_tenant_id ON finance.discount(tenant_id);

CREATE SCHEMA IF NOT EXISTS finance;

CREATE TABLE IF NOT EXISTS finance.studentfee (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_fee_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studentfee PRIMARY KEY (student_fee_id)
);

CREATE INDEX IF NOT EXISTS ix_studentfee_tenant_id ON finance.studentfee(tenant_id);

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE library.book_copy ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE library.book_loan ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS library;

CREATE TABLE IF NOT EXISTS library.reservation (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    reservation_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_reservation PRIMARY KEY (reservation_id)
);

CREATE INDEX IF NOT EXISTS ix_reservation_tenant_id ON library.reservation(tenant_id);

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_core.tool_definition ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.prompt_template ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_collection ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.ai_execution_log ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE ai_core.knowledge_chunk ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS lms;

CREATE TABLE IF NOT EXISTS lms.learningresource (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    learning_resource_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_learningresource PRIMARY KEY (learning_resource_id)
);

CREATE INDEX IF NOT EXISTS ix_learningresource_tenant_id ON lms.learningresource(tenant_id);

CREATE SCHEMA IF NOT EXISTS lms;

CREATE TABLE IF NOT EXISTS lms.lesson (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    lesson_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_lesson PRIMARY KEY (lesson_id)
);

CREATE INDEX IF NOT EXISTS ix_lesson_tenant_id ON lms.lesson(tenant_id);

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE lms.academic_assignment ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE lms.student_assignment_submission ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE activity.student_award ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS tenant_id uuid;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS is_active boolean DEFAULT TRUE;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS created_at timestamp with time zone DEFAULT now();

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS updated_at timestamp with time zone;

ALTER TABLE activity.student_activity ADD COLUMN IF NOT EXISTS row_version bytea DEFAULT gen_random_bytes(8);

CREATE SCHEMA IF NOT EXISTS activity;

CREATE TABLE IF NOT EXISTS activity.studentofmonth (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    student_of_month_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_studentofmonth PRIMARY KEY (student_of_month_id)
);

CREATE INDEX IF NOT EXISTS ix_studentofmonth_tenant_id ON activity.studentofmonth(tenant_id);

ALTER TABLE inventory.item ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE SCHEMA IF NOT EXISTS inventory;

CREATE TABLE IF NOT EXISTS inventory.stocktransaction (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    stock_transaction_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_stocktransaction PRIMARY KEY (stock_transaction_id)
);

CREATE INDEX IF NOT EXISTS ix_stocktransaction_tenant_id ON inventory.stocktransaction(tenant_id);

CREATE SCHEMA IF NOT EXISTS inventory;

CREATE TABLE IF NOT EXISTS inventory.purchaseorder (
    tenant_id uuid NOT NULL,
    is_active boolean DEFAULT TRUE NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone,
    row_version bytea DEFAULT gen_random_bytes(8) NOT NULL,
    code text NOT NULL,
    name text NOT NULL,
    purchase_order_id uuid DEFAULT gen_random_uuid() NOT NULL,
    metadata_json jsonb,
    CONSTRAINT pk_purchaseorder PRIMARY KEY (purchase_order_id)
);

CREATE INDEX IF NOT EXISTS ix_purchaseorder_tenant_id ON inventory.purchaseorder(tenant_id);

ALTER TABLE saas.tenant ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE saas.tenant ADD COLUMN IF NOT EXISTS id uuid;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS code text;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS name text;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS metadata_json jsonb;

ALTER TABLE saas.school_branding ADD COLUMN IF NOT EXISTS id uuid;
