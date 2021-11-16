﻿using UnityEngine;
/// <summary>
/// common variables to read
/// </summary>
public static class Config
{
    // path of database of the player's ability
    public static readonly string _playerAbilityDataPath = Application.streamingAssetsPath + "/" + "playerAbilityData.json";

    // path of database of the levels
    public static readonly string _levelDataPath = Application.streamingAssetsPath + "/" + "levelData.json";

    // path of database of the spawn area
    public static readonly string _spawnAreaDataPath = Application.streamingAssetsPath + "/" + "spawnAreaData.json";

    public static readonly string _exerciseInputDataPath = Application.streamingAssetsPath + "/" + "exerciseInputData.json";

    // player's level as a default
    public static readonly int _defaultPlayerLevel = 1;

    // unit time for spawning enemies
    public static readonly float _enemySpawnUnitTime = 10f;

    // layer name for searchable objects
    public static readonly string _searchableLayerName = "Searchable";

    // layer name for unsearchable objects
    public static readonly string _unsearchableLayerName = "Unsearchable";
}
