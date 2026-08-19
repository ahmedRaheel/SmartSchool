# Database/code synchronization

Canonical database: `scripts/database/postgresql/SmartSchool_Complete.sql`

Changes applied:
- Added explicit schema argument to entity `ToTable` mappings across module configuration files.
- Configuration files updated: 131.
- Student entity/request/response/configuration aligned to `student.student`.
- Guardian/parent entity/request/response/configuration aligned to `student.guardian`, including CNIC.
- Employee entity/request/response/configuration aligned to `hr.employee`, including CNIC and binary photograph.
- Driver entity/request/response/configuration aligned to `transport.driver`, including CNIC and binary picture.
- Notification/chat configurations use the `communication` schema.
- RAG remains under `ai_core`; PostgreSQL distributed cache remains under `infrastructure`.
- Page response models intentionally exclude image/blob bytes; detailed response models include them.
- Identity tables are not part of this DDL.

Important: The legacy solution contains many older generic Code/Name/MetadataJson feature handlers.
The core person/driver contracts above are the canonical models for migrating those handlers.
