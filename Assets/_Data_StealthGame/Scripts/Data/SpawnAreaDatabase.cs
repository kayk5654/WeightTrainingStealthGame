using System.Linq;
/// <summary>
/// get data of spawn area
/// </summary>
public class SpawnAreaDatabase : ObjectDatabaseBase<SpawnAreaDataSet>
{
    /// <summary>
    /// constructor
    /// </summary>
    public SpawnAreaDatabase(IDatabaseReader<SpawnAreaDataSet> databaseReader, string dataPath)
    {
        // set reference of a database reader
        _databaseReader = databaseReader;

        // read database
        _database = databaseReader.ReadData(dataPath);
    }

    /// <summary>
    /// get data tied with a specific player's level
    /// </summary>
    /// <param name="exerciseType"></param>
    /// <returns></returns>
    public override SpawnAreaDataSet GetData(int exerciseType)
    {
        // if the database isn't initialized or hasn't been read, do nothing
        if (_database == null || _database.Length < 1) { return null; }

        // if the given exerciseType isn't valid, return null
        if(exerciseType < 0 || exerciseType >= (int)ExerciseType.LENGTH) { return null; }

        return _database.FirstOrDefault(dataset => dataset._exerciseType == (ExerciseType)exerciseType);
    }
}
