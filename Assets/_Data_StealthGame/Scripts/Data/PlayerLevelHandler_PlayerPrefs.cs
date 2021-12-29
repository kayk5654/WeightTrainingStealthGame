using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// set and get player's level stored in the PlayerPrefs
/// </summary>
public class PlayerLevelHandler_PlayerPrefs : IPlayerLevelHandler
{
    // key of the PlayerPrefs to store player's level
    private string _playerLevelKey = "PlayerLevel";
    
    
    /// <summary>
    /// get current player's level
    /// </summary>
    public int GetPlayerLevel()
    {
        // if the key isn't created, initialize key with level 0
        if (!PlayerPrefs.HasKey(_playerLevelKey))
        {
            SetPlayerLevel(Config._defaultPlayerLevel);
        }

        Debug.Log("selected level: " + PlayerPrefs.GetInt(_playerLevelKey));
        return PlayerPrefs.GetInt(_playerLevelKey);
    }

    /// <summary>
    /// set player's level
    /// </summary>
    /// <param name="newLevel"></param>
    public void SetPlayerLevel(int newLevel)
    {
        PlayerPrefs.SetInt(_playerLevelKey, newLevel);
    }

    /// <summary>
    /// reset player's level
    /// </summary>
    public void ResetPlayerLevel()
    {
        SetPlayerLevel(Config._defaultPlayerLevel);
        DebugLog.Info(this.ToString(), "ResetPlayerLevel is called");
    }
}
