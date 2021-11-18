/// <summary>
/// enable to control features on the GamePlay phase
/// </summary>
public interface IGamePlayStateManager
{
    /// <summary>
    /// enable features for before actual gameplay
    /// </summary>
    void BeforeGamePlay();
    
    /// <summary>
    /// enable features at the beginning of the GamePlay phase
    /// </summary>
    void EnableGamePlay();

    /// <summary>
    /// disable features at the beginning of the GamePlay phase
    /// </summary>
    void DisableGamePlay();

    /// <summary>
    /// pause gameplay
    /// </summary>
    void PauseGamePlay();

    /// <summary>
    /// resume gameplay
    /// </summary>
    void ResumeGamePlay();

    /// <summary>
    /// enable features for after actual gameplay
    /// </summary>
    void AfterGamePlay();
}
