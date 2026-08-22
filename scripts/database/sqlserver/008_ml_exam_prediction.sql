IF SCHEMA_ID('ai') IS NULL EXEC('CREATE SCHEMA ai');
GO
IF OBJECT_ID('ai.MlExamPrediction','U') IS NULL
BEGIN
CREATE TABLE ai.MlExamPrediction (
    MlExamPredictionId uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
    TenantId uniqueidentifier NOT NULL,
    StudentId uniqueidentifier NOT NULL,
    SubjectId uniqueidentifier NOT NULL,
    TargetExamId uniqueidentifier NULL,
    TargetExamSubjectId uniqueidentifier NULL,
    TargetExamTypeCode varchar(40) NOT NULL,
    PredictedMarks decimal(8,2) NOT NULL,
    PredictedPercentage decimal(7,3) NOT NULL,
    PredictedGrade varchar(20) NOT NULL,
    LowerBoundPercentage decimal(7,3) NOT NULL,
    UpperBoundPercentage decimal(7,3) NOT NULL,
    ConfidenceScore decimal(7,4) NOT NULL,
    PassProbability decimal(7,4) NOT NULL,
    Trend varchar(30) NOT NULL,
    RiskLevel varchar(30) NOT NULL,
    ModelVersion varchar(80) NOT NULL,
    HistoricalResultCount int NOT NULL,
    UsedMachineLearning bit NOT NULL,
    GeneratedAt datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    IsActive bit NOT NULL DEFAULT 1,
    CreatedAt datetimeoffset NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    UpdatedAt datetimeoffset NULL,
    RowVersion rowversion NOT NULL
);
CREATE INDEX IX_MlExamPrediction_StudentSubject
ON ai.MlExamPrediction(TenantId,StudentId,SubjectId,GeneratedAt DESC);
END
GO
