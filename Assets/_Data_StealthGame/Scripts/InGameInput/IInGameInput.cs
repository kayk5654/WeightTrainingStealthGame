/// <summary>
/// interface of input to control in-game objects
/// </summary>
public interface IInGameInput
{
    /// <summary>
    /// movement to push player's body up
    /// used for attack enemies, spawn projectile, etc.
    /// </summary>
    void Push();

    /// <summary>
    /// movement to start keeping the lowest posture
    /// used for activate sheld to protect the player, etc.
    /// </summary>
    void StartHold();

    /// <summary>
    /// movement to stop keeping the lowest posture
    /// used for deactivate shield to protect the player, etc.
    /// </summary>
    void StopHold();
}
