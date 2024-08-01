public abstract class Singleton<T> where T : class
{
    public static T Instance;

    public static T _instacene
    {
        get { return Instance; }
    }

    public Singleton()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Instance = null;
        }

    }
}