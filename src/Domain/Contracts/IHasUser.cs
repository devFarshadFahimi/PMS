using Domain.Entities.UserAggregate;

namespace Domain.Contracts;
public interface IHasUser
{
    long UserId { get; set; }
    User User { get; set; }
}
