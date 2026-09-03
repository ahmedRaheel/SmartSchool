using SmartSchool.Modules.Organization.Enums;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

public sealed class TenantSettingsEntity : Entity
{
    private TenantSettingsEntity() { }
    public Guid TenantSettingsId { get; private set; } = Guid.NewGuid();
    public short AcademicYearStartMonth { get; private set; } = 4;
    public DefaultLanguage DefaultLanguage { get; private set; } = DefaultLanguage.English;
    public SchoolDateFormat DateFormat { get; private set; } = SchoolDateFormat.DayMonthYear;
    public string TimeZone { get; private set; } = "Asia/Karachi";
    public SchoolWeekStart WeekStart { get; private set; } = SchoolWeekStart.Monday;
    public short FeeWarningDays { get; private set; } = 5;

    public bool AiRagAssistant { get; private set; } = true;
    public bool AiTutor { get; private set; } = true;
    public bool AiQuiz { get; private set; } = true;
    public bool AiPredictions { get; private set; } = true;
    public bool AiAgent { get; private set; }
    public bool AiParentChatbot { get; private set; } = true;
    public bool InternalChat { get; private set; } = true;
    public bool Notifications { get; private set; } = true;
    public bool Broadcast { get; private set; } = true;
    public bool ParentPortal { get; private set; } = true;
    public bool Assignments { get; private set; } = true;
    public bool StudentLeaveApply { get; private set; } = true;
    public bool Library { get; private set; } = true;
    public bool OnlinePayment { get; private set; }
    public bool FeeReminders { get; private set; } = true;
    public bool DigitalReceipts { get; private set; } = true;
    public bool StaffSelfLeave { get; private set; } = true;
    public bool BiometricAttendance { get; private set; }
    public bool QrAttendance { get; private set; }
    public bool TwoFactor { get; private set; }
    public bool SessionTimeout { get; private set; } = true;
    public bool IpRestriction { get; private set; }

    public static TenantSettingsEntity Create(Guid tenantId) => new() { TenantId = tenantId };

    public void Update(short academicYearStartMonth, DefaultLanguage defaultLanguage, SchoolDateFormat dateFormat, string timeZone, SchoolWeekStart weekStart, short feeWarningDays,
        bool aiRagAssistant, bool aiTutor, bool aiQuiz, bool aiPredictions, bool aiAgent, bool aiParentChatbot, bool internalChat, bool notifications, bool broadcast, bool parentPortal,
        bool assignments, bool studentLeaveApply, bool library, bool onlinePayment, bool feeReminders, bool digitalReceipts, bool staffSelfLeave, bool biometricAttendance, bool qrAttendance,
        bool twoFactor, bool sessionTimeout, bool ipRestriction)
    {
        AcademicYearStartMonth = academicYearStartMonth; DefaultLanguage = defaultLanguage; DateFormat = dateFormat; TimeZone = timeZone.Trim(); WeekStart = weekStart; FeeWarningDays = feeWarningDays;
        AiRagAssistant = aiRagAssistant; AiTutor = aiTutor; AiQuiz = aiQuiz; AiPredictions = aiPredictions; AiAgent = aiAgent; AiParentChatbot = aiParentChatbot;
        InternalChat = internalChat; Notifications = notifications; Broadcast = broadcast; ParentPortal = parentPortal; Assignments = assignments; StudentLeaveApply = studentLeaveApply; Library = library;
        OnlinePayment = onlinePayment; FeeReminders = feeReminders; DigitalReceipts = digitalReceipts; StaffSelfLeave = staffSelfLeave; BiometricAttendance = biometricAttendance; QrAttendance = qrAttendance;
        TwoFactor = twoFactor; SessionTimeout = sessionTimeout; IpRestriction = ipRestriction; MarkAsUpdated();
    }
}
