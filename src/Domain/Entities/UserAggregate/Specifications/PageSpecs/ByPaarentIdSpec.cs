namespace BusinessMakerTemplate.Domain.Entities.UserAggregate.Specifications.PageSpecs;

public class ByPaarentIdSpec : Specification<Page>
{
    private readonly long? _parentId;

    public ByPaarentIdSpec(long? parentId = null)
    {
        _parentId = parentId;
        Includes.Add(p => p.Parent);
        Includes.Add(p => p.Children);
    }

    public override Expression<Func<Page, bool>> IsSatisfiedBy()
    {
        return p => p.ParentId == _parentId;
    }
}
