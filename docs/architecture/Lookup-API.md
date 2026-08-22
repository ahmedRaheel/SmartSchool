# Lookup API

Central tables: `saas.lookup_type`, `saas.lookup_value`.

- `GET /api/lookups/types`
- `GET /api/lookups/{typeCode}`
- `GET /api/lookups`

Reads use Dapper and explicit SQL projections. No EF metadata/reflection is used.
