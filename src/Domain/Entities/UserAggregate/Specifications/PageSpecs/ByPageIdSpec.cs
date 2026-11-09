using BusinessMakerTemplate.Domain.Contracts;

namespace BusinessMakerTemplate.Domain.Entities.UserAggregate.Specifications.PageSpecs;
public class ByPageIdSpec<TEntity> : Specification<TEntity>
    where TEntity : IHasPage
{
    private readonly long _pageId;

    public ByPageIdSpec(long pageId)
    {
        _pageId = pageId;
    }

    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return p => p.PageId == _pageId;
    }
}
