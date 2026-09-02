# Actor aggregate refinement

Run `migrations/021_actor_aggregate_documents_enrollment.sql` on an existing database.
For a new environment, use `SmartSchoolComplete.v87.actor-aggregate.sql`.

## Aggregate rules
- `document.document` is the document aggregate root and binary/metadata record.
- Typed ownership tables link documents to Tenant, Student, Teacher, Admin Officer, Staff, Driver, Guardian and Campus.
- `document.required_document` is the policy source for both UI and backend approval gates.
- Student approval requires a guardian, required student documents, and pending academic placement.
- `student.student_enrollment` is created only after approval; it receives a 3-digit enrollment number and class/section placement.
- Teacher hiring requires required documents plus at least one structured education record. If experience is declared, experience certificate becomes mandatory.
- Contacts are separated from actor roots (`tenant_contact`, `student_contact`, `employee_contact`) to avoid repeating address/phone columns and to allow multiple contacts.
