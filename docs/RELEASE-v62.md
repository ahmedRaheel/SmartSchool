# v62 Authorization Policies + SignalR Communication

- Added named actor-composition policies: SuperAdminOnly, SuperAdminTenantOnly, SuperAdminTenantTeacher, SuperAdminTenantStudent, SuperAdminTenantParent, SuperAdminTenantAdmin, SuperAdminTenantDriver.
- Added AllAuthenticatedActors for cross-role features such as receiving/reading personal notifications.
- Platform-only endpoints use SuperAdminOnly. Notification creation/administration uses SuperAdminTenantAdmin. Notification self-service uses AllAuthenticatedActors.
- Added authenticated NotificationHub and ChatHub.
- JWT bearer authentication accepts SignalR access_token only for the two hub paths.
- Notification creation pushes NotificationReceived to the recipient's tenant/user SignalR group.
- React performs one notification hydration request after login, then receives live updates through SignalR. The 30-second polling loop was removed.
- Mark-read and mark-all-read update local state rather than refetching the notification API.
- ChatHub supports authenticated conversation groups. Existing chat persistence APIs remain the source for conversation history; live messages are delivered through SignalR as chat send operations are migrated to the conversation command.

## Policy application
- Tenancy platform operations -> SuperAdminOnly.
- Organization administration -> SuperAdminTenantAdmin.
- Student operations -> SuperAdminTenantStudent.
- Academic teacher operations -> SuperAdminTenantTeacher.
- Transport/driver operations -> SuperAdminTenantDriver.
- Notification administration -> SuperAdminTenantAdmin.
- Notification self-service -> AllAuthenticatedActors because notification delivery is shared by every actor.

## Realtime behavior
The portal performs one notification history/unread hydration after authentication. It does not poll. NotificationReceived events update the bell/list in memory. Mark-read operations update local state without a GET refresh. ChatHub exposes JoinConversation, LeaveConversation and SendMessage and broadcasts MessageReceived to the conversation group.
