namespace GovServices.Server.Entities
{
    public class RosreestrRequest
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string RequestId { get; set; }
        public string Status { get; set; }
        public string ResponseData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
