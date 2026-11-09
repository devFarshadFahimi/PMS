namespace BusinessMakerTemplate.Domain.Entities.UserAggregate.Specifications.PageSpecs;

public class ByNameSpec : Specification<Page>
{
    private readonly string _name;

    public ByNameSpec(string name)
    {
        _name = name;
    }

    public override Expression<Func<Page, bool>> IsSatisfiedBy() => p => p.Name.Trim() == _name.Trim();
}
