# ML.NET Exam Performance Prediction

SmartSchool now predicts marks, percentage and grade for class tests, monthly tests,
mid-term, end/final term, pre-board, supplementary and any other exam type stored in
`exam.exam.exam_type_code`.

## Features used
- previous exam percentage
- historical average percentage
- recent-three average
- latest score trend
- number of historical results
- target exam type (one-hot encoded)

The ML.NET SDCA regression model is trained per tenant + subject and cached in-process
for 30 minutes. A minimum of 8 labelled training rows is required. Before that, the
service uses a transparent weighted cold-start estimate and returns
`UsedMachineLearning=false`.

## Endpoint
`POST /api/aiprediction/exam-performance/predict`

Example body:
```json
{
  "tenantId": "11111111-1111-1111-1111-111111111111",
  "studentId": "...",
  "subjectId": "...",
  "targetExamTypeCode": "MID_TERM",
  "targetExamId": null,
  "targetExamSubjectId": null
}
```

Prediction output includes predicted marks, percentage, grade, confidence interval,
pass probability, trend, risk level, model version and historical sample count.
Predictions are persisted through EF Core to `ai.ml_exam_prediction`; historical
training data is read with Dapper explicit SQL projections.
