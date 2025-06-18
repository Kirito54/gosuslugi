namespace GovServices.Server.Entities;

/// <summary>
/// Controls who can see the document.
/// </summary>
public enum VisibilityType
{
    InternalOnly,
    ApplicantVisible,
    ExecutorOnly,
    Closed
}
