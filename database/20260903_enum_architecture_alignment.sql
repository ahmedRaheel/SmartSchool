BEGIN;

-- BranchType: HEAD_OFFICE=1, REGIONAL_HEAD_OFFICE=2, REGIONAL_BRANCH=3
ALTER TABLE org.campus ADD COLUMN IF NOT EXISTS branch_type_v2 smallint;
UPDATE org.campus
SET branch_type_v2 = CASE UPPER(COALESCE(branch_type::text, ''))
    WHEN 'HEAD_OFFICE' THEN 1
    WHEN 'REGIONAL_HEAD_OFFICE' THEN 2
    WHEN 'REGIONAL_BRANCH' THEN 3
    WHEN '1' THEN 1 WHEN '2' THEN 2 WHEN '3' THEN 3
    ELSE 3 END
WHERE branch_type_v2 IS NULL;
ALTER TABLE org.campus ALTER COLUMN branch_type_v2 SET NOT NULL;
ALTER TABLE org.campus DROP COLUMN branch_type;
ALTER TABLE org.campus RENAME COLUMN branch_type_v2 TO branch_type;
ALTER TABLE org.campus ADD CONSTRAINT ck_campus_branch_type CHECK (branch_type BETWEEN 1 AND 3);
CREATE INDEX IF NOT EXISTS ix_campus_tenant_branch_type ON org.campus(tenant_id, branch_type);

COMMIT;
