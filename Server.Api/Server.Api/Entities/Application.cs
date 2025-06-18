namespace GovServices.Server.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int CurrentStepId { get; set; }
        public WorkflowStep CurrentStep { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AssignedToUserId { get; set; }
        public ApplicationUser AssignedTo { get; set; }
        public ICollection<ApplicationLog> Logs { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<OutgoingDocument> OutgoingDocuments { get; set; }
        public ICollection<RosreestrRequest> RosreestrRequests { get; set; }
        public ICollection<ZagsRequest> ZagsRequests { get; set; }
        public ICollection<SedDocumentLog> SedLogs { get; set; }
        public ICollection<ApplicationResult> Results { get; set; }
        public ICollection<ApplicationRevision> Revisions { get; set; }
    }
}
