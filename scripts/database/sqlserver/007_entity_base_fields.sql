/*
 SmartSchool Entity base-field synchronization for SQL Server.
 Adds Entity.IsActive, CreatedAt, UpdatedAt and RowVersion to tenant-owned entity tables.
*/
DECLARE @sql nvarchar(max) = N'';

SELECT @sql = @sql +
N'
IF COL_LENGTH(''' + s.name + '.' + t.name + ''',''IsActive'') IS NULL
	ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' ADD IsActive bit NOT NULL DEFAULT(1);
IF COL_LENGTH(''' + s.name + '.' + t.name + ''',''CreatedAt'') IS NULL
	ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' ADD CreatedAt datetimeoffset NOT NULL DEFAULT(SYSDATETIMEOFFSET());
IF COL_LENGTH(''' + s.name + '.' + t.name + ''',''UpdatedAt'') IS NULL
	ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' ADD UpdatedAt datetimeoffset NULL;
IF COL_LENGTH(''' + s.name + '.' + t.name + ''',''RowVersion'') IS NULL
	ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' ADD RowVersion rowversion NOT NULL;
'
FROM sys.tables t
JOIN sys.schemas s ON s.schema_id=t.schema_id
WHERE EXISTS (
	SELECT 1 FROM sys.columns c
	WHERE c.object_id=t.object_id AND c.name IN ('TenantId','tenant_id')
);

EXEC sys.sp_executesql @sql;
