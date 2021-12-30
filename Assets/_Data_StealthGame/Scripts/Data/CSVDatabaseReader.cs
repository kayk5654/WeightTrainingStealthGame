using System.Collections.Generic;
using System.IO;
public class StringArary
{
    public string[][] _stringArray;
}

/// <summary>
/// read csv files
/// </summary>
public class CSVDatabaseReader<T> : IDatabaseReader<T>
{
    /// <summary>
    /// read data from a file
    /// </summary>
    /// <returns></returns>
    public T[] ReadData(string path)
    {
        // read text from a file
        string csvText = ReadFile(path);

        // parse csv
        string[][] parseData = ParseCsv(csvText);

        // create array of T
        T[] objects = new T[parseData.Length];
        for(int i = 0; i < parseData.Length; i++)
        {
            objects[i] = (T)new object();
        }

        return objects;
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

    /// <summary>
    /// parse csv
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string[][] ParseCsv(string text)
    {
        string[] rows = text.Split('\n');
        List<string[]> parseData = new List<string[]>();

        for (int i = 0; i < rows.Length; i++)
        {
            parseData.Add(rows[i].Split(','));
        }

        return parseData.ToArray();
    }
}
