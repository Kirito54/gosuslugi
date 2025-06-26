namespace GovServices.Server.Entities;

public class PageAccess
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public string PageUrl { get; set; } = string.Empty;
}
