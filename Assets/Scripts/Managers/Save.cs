using UnityEngine;
using System.IO;

public static class Save
{
    public static string directory = "/Data/";
    public static string fileName = "data.txt";

    public static void SaveData<T>(T[] saveObjects)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonHelper.ToJson<T>(saveObjects);
        File.WriteAllText(FullPath(), json);
    }

    public static T[] LoadData<T>()
    {
        if (DoesPathExist())
        {
            return JsonHelper.FromJson<T>(File.ReadAllText(FullPath()));
        }
        return default;
        
    }

    public static string FullPath()
    {
        return Application.persistentDataPath + directory + fileName;
    }

    public static bool DoesPathExist()
    {
        return File.Exists(FullPath());
    }
}
