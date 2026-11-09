namespace Domain.Contracts;

public class BaseSnapshot(int version)
{
    public int Version { get; set; } = version;
}
