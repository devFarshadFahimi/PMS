namespace Application.Common.Utils;
public class Consts
{
    public struct ProtectorKeys
    {
        public const string NotificationKey = "NotificationKey";
    }

    public struct EventStreamPrefix
    {
        public const string AuditStreamIdPrefix = "Audit-Stream";

        public static string MakeStreamId(params string[] items) => $"{AuditStreamIdPrefix}-{string.Join('-', items)}";
    }
}