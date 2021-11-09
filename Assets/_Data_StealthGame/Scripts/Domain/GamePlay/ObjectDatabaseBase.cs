/// <summary>
/// get data tied with player's level from a database
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ObjectDatabaseBase<T>
{
    // array of the specified dataset
    protected T[] _database;

    // read database from an external file
    protected IDatabaseReader<T> _databaseReader;

    /// <summary>
    /// get data tied with a specific player's level
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public abstract T GetData(int key);
}
