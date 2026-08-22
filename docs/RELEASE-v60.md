# v60 Result Contract + Premium Actor UI

## API
- Result and Result<T> are the only SmartSchool API response models.
- Removed ApiResponse<T>.
- ToHttpResult serializes Result itself for success and failure.
- Added Forbidden and InternalServerError errors.
- Added ResultResponseMiddleware as a transitional safety net so legacy raw /api JSON responses, including framework 401/403 responses, are returned in Result shape.
- Explicitly migrated /api/lookups/types from raw array to Result.
- New/changed endpoints must return Result<T> directly; the middleware exists to keep old endpoints consistent while handlers are incrementally refactored.

## Portal
- ApiClient consumes Result<T> and unwraps Value only after checking IsSuccess.
- SuperAdmin sidebar is platform-oriented.
- Added premium Tenant Management screen with metrics, search, Add Tenant, View, Impersonate, Delete and More actions.
- No JSON editor is used for tenant creation.
- Role navigation remains actor-specific.
