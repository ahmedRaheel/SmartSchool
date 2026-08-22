IF SCHEMA_ID('Infrastructure') IS NULL
    EXEC('CREATE SCHEMA Infrastructure');
GO

IF OBJECT_ID('Infrastructure.DistributedCache', 'U') IS NULL
BEGIN
    CREATE TABLE Infrastructure.DistributedCache
    (
        Id nvarchar(449) NOT NULL PRIMARY KEY,
        Value varbinary(max) NOT NULL,
        ExpiresAtTime datetimeoffset NOT NULL,
        SlidingExpirationInSeconds bigint NULL,
        AbsoluteExpiration datetimeoffset NULL
    );

    CREATE INDEX IX_DistributedCache_ExpiresAtTime
        ON Infrastructure.DistributedCache(ExpiresAtTime);
END;
GO
