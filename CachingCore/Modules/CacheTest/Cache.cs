namespace CachingCore.Modules.CacheTest;

public class Cache
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}