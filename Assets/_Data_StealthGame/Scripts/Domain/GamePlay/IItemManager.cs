/// <summary>
/// manage scene objects
/// </summary>
/// <typeparam name="T">valid type of dataset</typeparam>
public interface IItemManager<T, U>
{
    /// <summary>
    /// spawn scene objects
    /// </summary>
    /// <param name="dataset"></param>
    void Spawn(T dataset1, U dataset2);

    /// <summary>
    /// pause update of scene objects
    /// </summary>
    void Pause();

    /// <summary>
    /// resume update of scene objects
    /// </summary>
    void Resume();

    /// <summary>
    /// delete scene objects when the gameplay ends
    /// </summary>
    void Delete();

    /// <summary>
    /// get number of items
    /// </summary>
    /// <returns></returns>
    int GetItemCount();
}
