CREATE SCHEMA IF NOT EXISTS observability;
CREATE TABLE IF NOT EXISTS observability.application_log (
    id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    timestamp_utc timestamptz NOT NULL DEFAULT now(),
    level varchar(32) NOT NULL,
    service varchar(128) NULL,
    message text NOT NULL,
    message_template text NULL,
    exception text NULL,
    trace_id varchar(64) NULL,
    correlation_id varchar(128) NULL,
    request_path varchar(1024) NULL,
    properties jsonb NULL
);
CREATE INDEX IF NOT EXISTS ix_application_log_timestamp ON observability.application_log(timestamp_utc DESC);
CREATE INDEX IF NOT EXISTS ix_application_log_trace ON observability.application_log(trace_id);
CREATE INDEX IF NOT EXISTS ix_application_log_correlation ON observability.application_log(correlation_id);
