-- Run AFTER Duende EF configuration-store migrations.
-- Seed is intentionally idempotent. Runtime DuendeConfigurationSeeder provides the same logical configuration.
-- Recommended: run the application once in Development or invoke the seeder after schema creation.
-- Resources: openid, profile, email, smartschool.profile
-- API scope/resource: smartschool.api / smartschool-api
-- Clients: smartschool-portal and smartschool-mobile (authorization_code + PKCE, offline access)
SELECT 'Duende configuration schema ready for SmartSchool DB-backed seeding.' AS status;
