# v46 completion pass
Added dedicated tenant-scoped Dapper dashboard APIs for admin, student, parent, teacher and driver; actor profile projections; liveness/readiness endpoints; React dashboard integration; profile UI; and core actor lifecycle schema synchronization for PostgreSQL and SQL Server.

Endpoints:
- GET /api/dashboard/admin?tenantId=
- GET /api/dashboard/student/{studentId}?tenantId=
- GET /api/dashboard/parent/{guardianId}?tenantId=
- GET /api/dashboard/teacher/{employeeId}?tenantId=
- GET /api/dashboard/driver/{driverId}?tenantId=
- GET /api/profiles/students/{id}?tenantId=
- GET /api/profiles/parents/{id}?tenantId=
- GET /api/profiles/teachers/{id}?tenantId=
- GET /api/profiles/drivers/{id}?tenantId=
- GET /health/live
- GET /health/ready

Read endpoints use Dapper projections/aggregates and do not materialize EF entities.
