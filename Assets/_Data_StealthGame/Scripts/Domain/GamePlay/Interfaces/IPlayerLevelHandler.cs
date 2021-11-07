/// <summary>
/// set and get player's level
/// </summary>
public interface IPlayerLevelHandler
{
    /// <summary>
    /// get current player's level
    /// </summary>
    int GetPlayerLevel();

    /// <summary>
    /// set player's level
    /// </summary>
    /// <param name="newLevel"></param>
    void SetPlayerLevel(int newLevel);

    /// <summary>
    /// reset player's level
    /// </summary>
    void ResetPlayerLevel();
}
