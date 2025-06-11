namespace GovServices.Server.Entities
{
    public class PasswordChangeLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; } // "Changed" или "ReminderSent"
        public DateTime Timestamp { get; set; }
    }
}
