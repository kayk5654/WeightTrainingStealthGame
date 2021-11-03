/// <summary>
/// defense action during gameplay by the player
/// </summary>
public interface IInGameDefenseAction
{
    /// <summary>
    /// enable defense action
    /// </summary>
    void StartDefense();

    /// <summary>
    /// disable defense action
    /// </summary>
    void StopDefense();
}
