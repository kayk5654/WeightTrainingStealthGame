/// <summary>
/// enable to control features on the GamePlay phase
/// </summary>
public interface IGamePlayStateManager
{
    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    void EnableGamePlay();

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    void DisableGamePlay();
}
