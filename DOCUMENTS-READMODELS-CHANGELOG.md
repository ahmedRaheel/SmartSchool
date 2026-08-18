# Documents and read-model revision

Added:
- normalized `DocumentType`
- `StudentDocument`
- `ParentDocument`
- `TeacherDocument`
- `EmployeeDocument`
- `CandidateDocument`
- `DriverDocument`
- `SchoolDocument`
- 7 separate EF Core entity configurations
- document storage abstraction and options
- `StudentDirectoryRead`
- `TeacherDirectoryRead`
- `DriverDirectoryRead`
- 3 separate EF Core read-model configurations
- materialized-read refresh contract
- PostgreSQL DDL extension
- SQL Server design/migration companion
- Mermaid ERD
- database/security/indexing guidance

The document tables store metadata only. File bytes are deliberately externalized through
`IDocumentStorage`. This is safer and more scalable for pictures, certificates, CNIC scans,
licenses, resumes and other attachments.
