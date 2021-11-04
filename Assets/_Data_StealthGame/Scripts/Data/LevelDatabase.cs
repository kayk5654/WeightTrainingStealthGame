using System.Linq;

public class LevelDatabase : ObjectDatabaseBase<LevelDataSet>
{
    /// <summary>
    /// constructor
    /// </summary>
    public LevelDatabase(IDatabaseReader<LevelDataSet> databaseReader, string dataPath)
    {
        // set reference of a database reader
        _databaseReader = databaseReader;

        // read database
        _database = databaseReader.ReadData(dataPath);
    }

    /// <summary>
    /// get player ability data tied with a specific player's level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public override LevelDataSet GetData(int level)
    {
        // if the database isn't initialized or hasn't been read, do nothing
        if (_database == null || _database.Length < 1) { return null; }

        return _database.FirstOrDefault(dataset => dataset._level == level);
    }
}
