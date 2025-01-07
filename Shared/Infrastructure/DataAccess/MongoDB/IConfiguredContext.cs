namespace Infrastructure.DataAccess.MongoDB;

/// <summary>
/// Marker for dependency injection
/// </summary>
public interface IConfiguredContext<T>
    where T : MongoOptions
{
}
