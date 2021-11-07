using UnityEngine;
/// <summary>
/// reset player's level from the main menu ui
/// </summary>
public class PlayerLevelResetter : MonoBehaviour
{
    // access player's level
    private IPlayerLevelHandler _playerLevelHandler;



    /// <summary>
    /// set reference of IPlayerLevelHandler
    /// </summary>
    /// <param name="playerLevelHandler"></param>
    public void SetPlayerLevelHandler(IPlayerLevelHandler playerLevelHandler)
    {
        _playerLevelHandler = playerLevelHandler;
    }

    /// <summary>
    /// reset player's level
    /// called from a button on the main menu ui
    /// </summary>
    public void ResetPlayerLevel()
    {
        if (_playerLevelHandler == null) { return; }
        _playerLevelHandler.ResetPlayerLevel();
    }
}
