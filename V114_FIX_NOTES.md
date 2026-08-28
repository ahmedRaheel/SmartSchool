# v114 workflow/setup alignment

- Employee approval no longer executes SQL inside the handler. Branch code lookup is now `IEmployeeQuery.GetBranchCodeAsync` -> `EmployeeQuery`.
- Added idempotent PostgreSQL V114 alignment for admission workflow fields plus Department and Fee Type setup masters.
- Department and Fee Type feature slices already existed and remain the API boundary used by the React setup workspace.
- If admissions currently returns HTTP 500 because an older database has not applied V100/V103, apply migrations through V114 before retesting.
