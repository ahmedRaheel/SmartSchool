BEGIN;

-- Legacy EF read-model tables were created in public by older generated code.
-- They are not referenced by the current application and duplicate canonical module data.
DROP TABLE IF EXISTS public.driverdirectoryread;
DROP TABLE IF EXISTS public.studentdirectoryread;
DROP TABLE IF EXISTS public.teacherdirectoryread;
DROP TABLE IF EXISTS public.schooldocument;

COMMIT;
