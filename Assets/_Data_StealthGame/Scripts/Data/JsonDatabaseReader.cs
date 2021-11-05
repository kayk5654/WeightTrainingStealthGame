using UnityEngine;
using System.IO;
/// <summary>
/// read json containing datasets
/// </summary>
public class JsonDatabaseReader<T> : IDatabaseReader<T>
{
    /// <summary>
    /// read data from a file
    /// </summary>
    /// <returns></returns>
    public T[] ReadData(string path)
    {
        // read text from a file
        string jsonText = ReadFile(path);
        
        // deserialize json
        return JsonHelper.FromJson<T>(jsonText);
    }

    /// <summary>
    /// read text from a file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string ReadFile(string path)
    {
        string text = File.ReadAllText(path);

        return text;
    }
}
