/// <summary>
/// activate/deactivate player input action
/// </summary>
public interface IActionActivator
{
    /// <summary>
    /// initialize action
    /// </summary>
    void InitAction();

    /// <summary>
    /// enable action
    /// </summary>
    void StartAction();

    /// <summary>
    /// disable action
    /// </summary>
    void StopAction();
}
