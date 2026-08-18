# SmartSchool API response model policy

## Paged/list/search endpoints

Paged endpoints return lightweight summary DTOs. They must not return:

- document collections
- document metadata
- image/photo/avatar binary data
- image/document storage keys
- attachment content
- large child collections
- audit fields (`CreatedAt`, `UpdatedAt`)
- `IsActive`
- `RowVersion`

Paged responses contain `TenantId`, entity `Id`, identifying/display fields, and the small set of business fields needed to render the grid/list.

Example:

```csharp
public sealed record Response(
    Guid TenantId,
    Guid Id,
    string AdmissionNumber,
    string FirstName,
    string LastName,
    Guid? CurrentClassId,
    Guid? CurrentSectionId);
```

## Get-by-id endpoints

Get-by-id returns the realistic detail DTO for the aggregate. It can contain document/image metadata needed by the detail screen, but file bytes are not embedded in the JSON response.

Documents should be represented as metadata such as:

```csharp
public sealed record DocumentResponse(
    Guid Id,
    Guid DocumentTypeId,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    bool IsVerified,
    DateOnly? IssuedOn,
    DateOnly? ExpiresOn);
```

Actual files/images are retrieved from a dedicated authorized download/content endpoint.

## Security

CNIC, B-Form, passport numbers, payroll/banking information and document storage keys are sensitive. Detail responses should expose them only to roles/permissions that require them. Storage keys must not be treated as public URLs.

## Mapping

Handlers map entities/read models to feature-local response records. Entities are never returned directly.

- `Get...Page` -> summary response/read projection
- `Get...ById` -> detail response
- document download -> dedicated document endpoint

This keeps list endpoints fast and avoids N+1 document queries and oversized payloads.
