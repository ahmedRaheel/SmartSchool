# GlobalExceptionHandler lifetime fix

`IExceptionHandler` is registered by ASP.NET Core as a singleton. `IDbConnectionFactory` is intentionally scoped.

The handler no longer constructor-injects `IDbConnectionFactory`. During `TryHandleAsync`, persistence resolves it from `HttpContext.RequestServices`, which is the active request scope. This keeps the database factory scoped and avoids the startup validation error:

`Cannot consume scoped service IDbConnectionFactory from singleton IExceptionHandler`.

Do not change `IDbConnectionFactory` to singleton just to satisfy this handler.
