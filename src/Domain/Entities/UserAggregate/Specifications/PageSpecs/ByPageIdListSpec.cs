using BusinessMakerTemplate.Domain.Contracts;

namespace BusinessMakerTemplate.Domain.Entities.UserAggregate.Specifications.PageSpecs;

public class ByPageIdListSpec<TEntity> : Specification<TEntity>
    where TEntity : IHasPage
{
    private readonly List<long> _ids;

    public ByPageIdListSpec(List<long> ids)
    {
        _ids = ids;
    }
    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return p => _ids.Contains(p.PageId);
    }
}
