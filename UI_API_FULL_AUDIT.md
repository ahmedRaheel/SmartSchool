# UI → Backend API full read-model audit

Source UI: SmartSchool_RBAC_v1.

- UI route constants scanned: 103
- Backend feature files scanned: 1435
- Dapper read-model column mismatches detected after PK alias repair: 249
- Compatibility database columns added by migration: 182
- Dapper files with conventional primary-key alias repaired: 6

## Rules applied

1. Grid/list endpoints remain server-paged (`page`, `pageSize`).
2. Read APIs use Dapper and return feature response DTOs; writes remain EF Core.
3. The UI mock contracts are treated as read-model contracts. Related display data belongs in read queries/joins, not in React reconstruction.
4. Existing real primary keys are used instead of inventing a generic `id` database column.
5. Missing read-model columns referenced by API SQL are added by `database/20260902_all_ui_read_models_alignment.sql`.
6. Tenant ownership columns are not blindly invented on child/junction tables; those must be resolved from their owning parent in queries.

## Primary-key aliases repaired

- `src/Modules/Organization/Features/Department/GetDepartmentPage.cs`
- `src/Modules/Organization/Features/School/GetSchoolPage.cs`
- `src/Modules/Organization/Features/AcademicYear/GetAcademicYearPage.cs`
- `src/Modules/Communication/Features/Conversation/GetConversationPage.cs`
- `src/Modules/Communication/Features/Notification/GetNotificationPage.cs`
- `src/Modules/Communication/Features/Message/GetMessagePage.cs`

## Remaining ownership/query issues

The audit intentionally does not add fake `tenant_id` columns to tables whose tenant is derivable through a parent FK. Those queries must join the owning table. This prevents hiding incorrect domain relationships behind duplicate tenant data.
