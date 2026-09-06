BEGIN;

-- Finance: align EF FeeTypeEntity with PostgreSQL.
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS frequency varchar(50);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS description varchar(500);
ALTER TABLE finance.fee_type ADD COLUMN IF NOT EXISTS metadata_json jsonb;

-- Academic hierarchy: Campus -> GradeLevel -> ClassSection.
ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS campus_id uuid;
ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS academic_system_id uuid;
ALTER TABLE academic.grade_level ADD COLUMN IF NOT EXISTS metadata_json jsonb;
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS grade_level_id uuid;
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS room_no varchar(50);
ALTER TABLE academic.class_section ADD COLUMN IF NOT EXISTS metadata_json jsonb;

CREATE INDEX IF NOT EXISTS ix_grade_level_tenant_campus ON academic.grade_level(tenant_id, campus_id);
CREATE INDEX IF NOT EXISTS ix_class_section_tenant_campus_grade ON academic.class_section(tenant_id, campus_id, grade_level_id);

DO $$ BEGIN
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_grade_level_campus') THEN
  ALTER TABLE academic.grade_level ADD CONSTRAINT fk_grade_level_campus FOREIGN KEY (campus_id) REFERENCES org.campus(campus_id) ON DELETE CASCADE;
 END IF;
 IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname='fk_class_section_grade_level') THEN
  ALTER TABLE academic.class_section ADD CONSTRAINT fk_class_section_grade_level FOREIGN KEY (grade_level_id) REFERENCES academic.grade_level(grade_level_id) ON DELETE RESTRICT;
 END IF;
END $$;

-- Knowledge document generic API contract used by current vertical slices.
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS code varchar(100);
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE ai_core.knowledge_document ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE ai_core.knowledge_document
SET code = COALESCE(code, 'DOC-' || substr(replace(knowledge_document_id::text, '-', ''), 1, 12)),
    name = COALESCE(name, title),
    metadata_json = COALESCE(metadata_json, metadata);
CREATE UNIQUE INDEX IF NOT EXISTS ux_knowledge_document_tenant_code ON ai_core.knowledge_document(tenant_id, code);

-- AI model configuration: align current EF/API contract with the canonical table.
ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS name varchar(250);
ALTER TABLE ai_core.model_configuration ADD COLUMN IF NOT EXISTS metadata_json jsonb;
UPDATE ai_core.model_configuration SET name = COALESCE(name, model_name, code) WHERE name IS NULL;
ALTER TABLE ai_core.model_configuration ALTER COLUMN name SET NOT NULL;

COMMIT;
