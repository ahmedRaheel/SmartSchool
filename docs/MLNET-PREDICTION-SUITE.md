# SmartSchool ML.NET Prediction Suite

Implemented prediction families:

Academic/student:
- exam marks/grade/pass probability (existing)
- failure risk
- grade improvement/decline
- attendance risk
- dropout/withdrawal risk
- assignment completion risk
- subject difficulty
- promotion risk
- student behavior/engagement risk
- early-warning composite

Finance/admission/operations:
- fee default/late-payment risk
- admission conversion
- admission success
- teacher workload risk
- teacher/student performance relationship
- payroll anomaly
- transport delay
- library overdue risk
- school capacity forecast
- fee collection forecast
- enrollment forecast

Architecture:
- Dapper + explicit SQL for feature extraction/read side.
- EF Core only persists prediction results.
- No EF metadata reflection in the ML read path.
- ML.NET is used when sufficient labelled history is available.
- Cold-start results explicitly set UsedMachineLearning=false.
- Predictions are advisory; workflows should keep teachers/admins as decision makers.
