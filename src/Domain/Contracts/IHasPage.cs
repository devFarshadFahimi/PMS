namespace BusinessMakerTemplate.Domain.Contracts;
public interface IHasPage
{
    long PageId { get; set; }
    Page Page { get; set; }
}
