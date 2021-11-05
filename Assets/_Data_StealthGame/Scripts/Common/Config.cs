using UnityEngine;
/// <summary>
/// common variables to read
/// </summary>
public static class Config
{
    // path of database of the player's ability
    public static readonly string _playerAbilityDataPath = Application.streamingAssetsPath + "/" + "playerAbilityData.json";

    // path of database of the levels
    public static readonly string _levelDataPath = Application.streamingAssetsPath + "/" + "levelData.json";
}
