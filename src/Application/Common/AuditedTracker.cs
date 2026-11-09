using BusinessMakerFramework.Domain.Core.Entities;

namespace Application.Common;

public class AuditedTracker : IAuditableMarker
{
    public string OperatedBy { get; set; } = string.Empty;
    public DateTime SubmitDate { get; set; }
    public string State { get; set; } = string.Empty;
    public List<Dictionary<string, object>> ChangedProps { get; set; } = [];
}