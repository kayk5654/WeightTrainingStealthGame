/// <summary>
/// manage scene objects
/// </summary>
/// <typeparam name="T">valid type of dataset</typeparam>
public interface IItemManagerBase<T>
{
    /// <summary>
    /// spawn scene objects
    /// </summary>
    /// <param name="dataset"></param>
    void Spawn(T dataset);

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
}
