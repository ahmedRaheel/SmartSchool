# Database design notes

## Transactional model
The normalized OLTP model owns writes. Every tenant-owned table carries `TenantId`.
Owner-specific attachment tables reference one owner and one `DocumentType`.

## Document security
CNIC scans, B-Forms, passports, birth certificates and licenses are high-sensitivity documents.
Do not log `DocumentNumber`, `StorageKey`, CNIC values or file contents.
Authorize uploads/downloads by tenant + owner relationship + role/permission.
Virus/malware scanning should occur before a document becomes verified/available.
Validate MIME type using file signatures, not only the client-provided extension.

## Storage
The database stores metadata. `IDocumentStorage` stores bytes.
This avoids database bloat and makes S3/Azure Blob/local development configurable.
Use immutable object keys and checksum validation.

## Materialized read tables
`StudentDirectoryRead`, `TeacherDirectoryRead`, and `DriverDirectoryRead` are physical projection tables.
They are intentionally not vendor-specific PostgreSQL materialized views because SmartSchool supports PostgreSQL and SQL Server.
Refresh them transactionally for simple same-module changes or asynchronously through Outbox/Kafka for cross-module aggregates such as fees, attendance and exam summaries.

## Consistency
OLTP data is strongly consistent.
Cross-module read projections are eventually consistent and must expose `UpdatedAt`.
Consumers must never use a read projection as the source of truth for a write decision.

## Indexing
Document tables index:
- `(TenantId, OwnerId, DocumentTypeId)`
- `(TenantId, Sha256Hash)`
- unique `(TenantId, StorageProvider, StorageKey)`

Directory tables have a unique `(TenantId, SourceId)` key and should receive additional indexes only from measured query patterns.

## Retention
Prefer soft-delete/deactivation for business metadata and an explicit retention workflow for file objects.
Deletion must remove/revoke the object in storage and retain the minimum audit record required by policy.
