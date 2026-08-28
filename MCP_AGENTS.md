# SmartSchool MCP Agents

## Runtime

- HTTP MCP endpoint: `/mcp` (authenticated)
- Agent endpoint: `POST /api/ai/agents/run`
- Ollama remains the model runtime.
- MCP 2.0 exposes SmartSchool tools.
- Tools call existing module `I*Query` abstractions. They never execute SQL or use `IDbConnectionFactory`.
- `ICurrentUser` and `ITenantScope` enforce token tenant/actor context.
- Agent runs are written to the existing `ai_execution_log` persistence through `IAiExecutionLogCommand`.

## Initial tools

- `get_student_profile`
- `get_student_exam_results`
- `get_student_predictions`

## Example

```http
POST /api/ai/agents/run
Authorization: Bearer <access-token>
Content-Type: application/json

{
  "agent": "TeacherCopilot",
  "message": "Summarize this student's performance and identify the main risk areas.",
  "studentId": "<student-guid>"
}
```

Student tokens do not need to send `studentId`; their `student_id` claim is used automatically. If they send a different id, the request is rejected.

## Architecture rule

`MCP Tool -> module I*Query/I*Command -> persistence -> database`

Do not add Dapper, EF, or SQL to MCP tool classes or agent workflow classes.
