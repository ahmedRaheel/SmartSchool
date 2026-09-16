import {PGlite} from '@electric-sql/pglite'; import {pgcrypto} from '@electric-sql/pglite/contrib/pgcrypto'; import {vector} from '@electric-sql/pglite-pgvector'; import fs from 'node:fs'; import path from 'node:path'; import {fileURLToPath} from 'node:url';
const here=path.dirname(fileURLToPath(import.meta.url));
const root=process.env.SMARTSCHOOL_SOURCE_ROOT || path.resolve(here,'../..');
const db=await PGlite.create({extensions:{pgcrypto,vector}});
try {
 const baseline=fs.readFileSync(path.join(root,'database/SmartSchool.FreshInstall.sql'),'utf8').split('-- V123: Persisted school workflows')[0];
 await db.exec(baseline); await db.exec(fs.readFileSync(path.join(here,'legacy-upgrade-fixture.sql'),'utf8'));
 const upgrade=fs.readFileSync(path.join(root,'database/postgresql/V123__persisted_school_workflows.sql'),'utf8');
 await db.exec(upgrade); await db.exec(upgrade);
 const result=(await db.query(`select (select count(*) from academic.grade_level where grade_level_id='aa000000-0000-0000-0000-000000000005') as grades,
 (select count(*) from academic.teacher_course_assignment where teacher_course_assignment_id='aa000000-0000-0000-0000-000000000009') as allocations,
 (select count(*) from admission.student_application a join academic.grade_level g on g.grade_level_id=a.class_id where a.application_id='aa000000-0000-0000-0000-000000000010') as applications,
 (select count(*) from academic.class_section where class_section_id='aa000000-0000-0000-0000-000000000006' and class_teacher_employee_id='aa000000-0000-0000-0000-000000000008' and grade_level_id='aa000000-0000-0000-0000-000000000005') as sections`)).rows[0];
 if(Object.values(result).some(n=>Number(n)!==1))throw new Error('Legacy data migration failed: '+JSON.stringify(result));
 console.log('PASS: Legacy class, section, teacher allocation and admission relationships survive V123 and a repeat application.');
} finally {await db.close();}
