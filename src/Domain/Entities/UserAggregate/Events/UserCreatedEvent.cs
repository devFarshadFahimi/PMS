namespace Domain.Entities.UserAggregate.Events;
public class UserCreatedEvent
{
    public string UserName { get; set; } = null!;
    public DateTime SubmitDate { get; set; } = DateTime.Now;
}
