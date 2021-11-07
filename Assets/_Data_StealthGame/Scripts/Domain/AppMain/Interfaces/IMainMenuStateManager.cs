/// <summary>
/// enable to control features in the MainMenu phase
/// </summary>
public interface IMainMenuStateManager
{
    /// <summary>
    /// enable features at the beginning of the MainMenu phase
    /// </summary>
    void EnableMainMenu();

    /// <summary>
    /// disable features at the beginning of the MainMenu phase
    /// </summary>
    void DisableMainMenu();
}
