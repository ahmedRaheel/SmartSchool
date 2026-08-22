# Separate Identity Host

`SmartSchool.Identity.Api` is a separate executable/API host inside the same solution.

Development URLs:
- Identity: https://localhost:7101
- SmartSchool API: configured separately
- Portal: https://localhost:5173

Identity owns ASP.NET Identity + Duende stores and account lifecycle. SmartSchool.Api validates
tokens issued by Identity and calls the internal account-provisioning API for Student, Teacher,
Parent/Guardian, Driver, Employee and other login-enabled actors.

Account lifecycle rule:
1. Business entity is created in SmartSchool.Api.
2. SmartSchool.Api calls Identity internal provisioning endpoint.
3. Returned Identity user id is stored on the business entity.
4. On business deletion/deactivation, SmartSchool.Api calls Identity to delete/deactivate account.
5. For production reliability, execute cross-service lifecycle through an Outbox + Hangfire retry
   workflow rather than pretending the two databases/HTTP calls form one transaction.

Identity UI/API includes login, forgot password, reset password and change password.
Admin endpoints include users, roles, reset, lock/unlock and deactivate.
