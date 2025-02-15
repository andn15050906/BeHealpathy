namespace Core.Helpers;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static readonly Lazy<T> _instance = new(() => new T());

    protected Singleton() { }

    public static T Instance => _instance.Value;
}
