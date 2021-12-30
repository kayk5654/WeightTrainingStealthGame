using System.Linq;
/// <summary>
/// enable to access database of texts to be translated by the language setting
/// </summary>
public class TextTranslationDatabase : ObjectDatabaseBase<TextTranslationDataSet>
{
    /// <summary>
    /// constructor
    /// </summary>
    public TextTranslationDatabase(IDatabaseReader<TextTranslationDataSet> databaseReader, string dataPath)
    {
        // set reference of a database reader
        _databaseReader = databaseReader;

        // read database
        _database = databaseReader.ReadData(dataPath);
    }

    /// <summary>
    /// get player ability data tied with a specific player's level
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override TextTranslationDataSet GetData(int id)
    {
        // if the database isn't initialized or hasn't been read, do nothing
        if (_database == null || _database.Length < 1) { return null; }

        return _database.FirstOrDefault(dataset => dataset._id == id);
    }
}
