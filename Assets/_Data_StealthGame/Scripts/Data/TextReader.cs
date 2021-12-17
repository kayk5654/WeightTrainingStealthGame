using System.IO;
/// <summary>
/// read text fron external files
/// </summary>
public class TextReader
{
    /// <summary>
    /// read data from a file
    /// </summary>
    /// <returns></returns>
    public string ReadData(string path)
    {
        string text = File.ReadAllText(path);

        return text;
    }
}
