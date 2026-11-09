using Bogus;
using Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

/// <summary>
/// https://stenbrinke.nl/blog/taking-ef-core-data-seeding-to-the-next-level-with-bogus/.
/// </summary>
public class DatabaseSeeder
{
    private static int _nameIndex = 0;

    public IReadOnlyCollection<User> Users { get; } = [];
    public IReadOnlyCollection<UserRole> UserRoles { get; } = [];
    public IReadOnlyCollection<Role> Roles { get; } = [];
    private static readonly string[] RoleNames = ["Admin", "User", "Customer", "SuperAdmin", "Manager"];

    public DatabaseSeeder(IPasswordHasher<User> passwordHasher)
    {
        var (users, roles, userRoles) = GenerateUsers(amount: 10, passwordHasher);
        Users = users;
        Roles = roles;
        UserRoles = userRoles;
    }
    private static string GetNextName(string[] roleNames)
    {
        if (_nameIndex >= roleNames.Length)
        {
            _nameIndex = 0;
        }

        return roleNames[_nameIndex++];
    }

    private static (IReadOnlyCollection<User> users, IReadOnlyCollection<Role> roles, IReadOnlyCollection<UserRole> userRoles) GenerateUsers(int amount, IPasswordHasher<User> _passwordHasher)
    {
        var roleId = 1;
        var userId = 1;

        var roleFaker = new Faker<Role>()
            .RuleFor(h => h.Id, f => roleId++)
            .RuleFor(h => h.Name, f => GetNextName(RoleNames))
            .RuleFor(h => h.NormalizedName, (f, role) => role.Name?.ToUpper())
            .RuleFor(h => h.ConcurrencyStamp, f => "3e13d694-9ca5-4665-ad94-825ab2b25057")
        ;

        var userFaker = new Faker<User>()
            .RuleFor(x => x.Id, f => userId++) // Each User will have an incrementing id.
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.NormalizedEmail, f => f.Person.Email.ToUpper())
            .RuleFor(x => x.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.PasswordHash, (f, user) => _passwordHasher.HashPassword(user, "123456aA@"))
            .RuleFor(x => x.NormalizedUserName, f => f.Person.UserName.ToUpper())
            .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("+(98) ###-####-###"))
            .RuleFor(x => x.PhoneNumberConfirmed, f => f.Random.Bool())
            .RuleFor(x => x.AccessFailedCount, f => 0)
            .RuleFor(x => x.TwoFactorEnabled, f => f.Random.Bool())
            .RuleFor(x => x.SecurityStamp, f => "3e13d694-9ca5-4665-ad94-825ab2b25057")
            .RuleFor(x => x.ConcurrencyStamp, f => "3e13d694-9ca5-4665-ad94-825ab2b25057")
            .RuleFor(x => x.IsSystem, f => f.Random.Bool())
            //.RuleFor(x => x.UserRoles, f => userRole.Generate(1))
            ;

        //// The refDate is very important! Without it, it will generate a random date based
        //// on the CURRENT date on your system.
        //// Generating a date based on the system date is not deterministic!
        //// So the solution is to pass in a constant date instead
        //// which will be used to generate a random date
        //.RuleFor(x => x., f => f.Date.FutureOffset(
        //    refDate: new DateTimeOffset(2023, 1, 16, 15, 15, 0, TimeSpan.FromHours(1))))

        var users = Enumerable.Range(1, amount)
            .Select(i => SeedRow(userFaker, i))
            .ToList();


        var roles = Enumerable.Range(1, RoleNames.Length)
            .Select(i => SeedRow(roleFaker, i))
            .ToList();

        var fakerUserRole = new Faker<UserRole>()
            .RuleFor(h => h.Id, f => userId++)
            .RuleFor(h => h.UserId, f => f.PickRandom(users.Select(p => p.Id).ToList()))
            .RuleFor(h => h.RoleId, f => f.PickRandom(roles.Select(p => p.Id).ToList()))
        ;

        var userRoles = Enumerable.Range(1, users.Count)
            .Select(i => SeedRow(fakerUserRole, i))
            .ToList();

        return (users, roles, userRoles);
    }

    private static IReadOnlyCollection<UserRole> GenerateUserRoles(int amount, Faker<User> userFaker, Faker<Role> roleFaker)
    {
        int userId = 1;
        var fakerUserRole = new Faker<UserRole>()
           .RuleFor(h => h.Id, f => userId++)
           .RuleFor(h => h.User, f => f.PickRandom(userFaker))
           .RuleFor(h => h.Role, f => f.PickRandom(roleFaker))
       ;

        var roles = Enumerable.Range(1, amount)
            .Select(i => SeedRow(fakerUserRole, i))
            .ToList();


        return roles;
    }

    private static T SeedRow<T>(Faker<T> faker, int rowId)
        where T : class
    {
        var recordRow = faker.UseSeed(rowId).Generate();
        return recordRow;
    }
}