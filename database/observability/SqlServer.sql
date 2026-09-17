IF SCHEMA_ID('observability') IS NULL EXEC('CREATE SCHEMA observability');
IF OBJECT_ID('observability.application_log') IS NULL
BEGIN
CREATE TABLE observability.application_log(
    id bigint IDENTITY(1,1) PRIMARY KEY,
    timestamp_utc datetime2 NOT NULL DEFAULT SYSUTCDATETIME(),
    level nvarchar(32) NOT NULL,
    service nvarchar(128) NULL,
    message nvarchar(max) NOT NULL,
    message_template nvarchar(max) NULL,
    exception nvarchar(max) NULL,
    trace_id nvarchar(64) NULL,
    correlation_id nvarchar(128) NULL,
    request_path nvarchar(1024) NULL,
    properties nvarchar(max) NULL
);
CREATE INDEX ix_application_log_timestamp ON observability.application_log(timestamp_utc DESC);
CREATE INDEX ix_application_log_trace ON observability.application_log(trace_id);
CREATE INDEX ix_application_log_correlation ON observability.application_log(correlation_id);
END
