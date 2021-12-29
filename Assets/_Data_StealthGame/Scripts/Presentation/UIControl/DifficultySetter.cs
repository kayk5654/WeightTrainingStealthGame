using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// set difficulty of the level using IPlayerLevelHandler
/// </summary>
public class DifficultySetter : MonoBehaviour
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
    /// set difficulty of the gameplay
    /// </summary>
    public void SetDifficulty(int difficulty)
    {
        if (_playerLevelHandler == null) { return; }
        _playerLevelHandler.SetPlayerLevel(difficulty);
    }
}
