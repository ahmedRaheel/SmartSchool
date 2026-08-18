# SmartSchool cache packages

The Infrastructure project explicitly references:

- Microsoft.Extensions.Caching.Hybrid
- Microsoft.Extensions.Caching.StackExchangeRedis
- StackExchange.Redis

Versions are controlled centrally through `Directory.Packages.props`.

Application code should consume `HybridCache`. Configuration selects the
distributed backing store:

- `Caching:Provider = Memory` -> distributed memory backing
- `Caching:Provider = Redis` -> StackExchangeRedis distributed backing

Redis-specific APIs should remain in Infrastructure rather than feature handlers.
