# Sample create flows

The `.http` file under `src/SmartSchool.Api/Samples` contains authenticated example requests for:
- User
- Student + enrollment intent
- Teacher/employee + job/grade intent
- Parent/guardian + student relationship intent

These are deliberately API examples rather than direct production database inserts. Identity users should be provisioned through IdentityServer, and aggregate creation should run through application handlers so validation, tenant isolation, audit, Outbox events, and authorization are not bypassed.
