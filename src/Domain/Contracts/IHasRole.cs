using Domain.Entities.UserAggregate;

namespace Domain.Contracts;

public interface IHasRole
{
    long RoleId { get; set; }
    Role Role { get; set; }
}
