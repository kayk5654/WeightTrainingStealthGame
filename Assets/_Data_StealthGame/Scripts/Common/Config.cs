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

    // path of database of the exercise input
    public static readonly string _exerciseInputDataPath = Application.streamingAssetsPath + "/" + "exerciseInputData.json";

    // path of credit text
    public static readonly string _creditsTextPath = Application.streamingAssetsPath + "/" + "Credits.txt";

    // path of database of the text translation
    public static readonly string _textTranslationDataPath = Application.streamingAssetsPath + "/" + "TextTranslationData.json";

    // player's level as a default
    public static readonly int _defaultPlayerLevel = 0;

    // unit time for spawning enemies
    public static readonly float _enemySpawnUnitTime = 10f;

    // layer name for searchable objects
    public static readonly string _searchableLayerName = "Searchable";

    // layer name for unsearchable objects
    public static readonly string _unsearchableLayerName = "Unsearchable";

    // max distance of the eye gaze cursor
    public static readonly float _cursorMaxDistance = 3.0f;

    /* parameters of scene objects for test */
    
    // HP of nodes
    public static readonly float _nodeHp = 30f;

    // Attack of nodes
    public static readonly float _nodeAttack = 40f;

    // Defense (multiplier) of nodes
    public static readonly float _nodeDefense = 1f;

    // HP of enemies
    public static readonly float _enemyHp = 30f;

    // Attack of enemies per second
    public static readonly float _enemyAttack = 3f;

    // Defense (multiplier) of enemies
    public static readonly float _enemyDefense = 1f;

    // material property name of damage area range
    public static readonly string _damageAreaRangeProperty = "_DamageRange";

    // material property name of hit effect level
    public static readonly string _hitProperty = "_HitEffect";

    // material property name of local position of the attacked point
    public static readonly string _attackPointProperty = "_AttackPosLocal";

    public static readonly float _ringGageFillDuration = 1f;

    // speed of projectiles
    public static readonly float _projectileSpeed = 4.0f;

    // maximum distance projectiles can travel
    public static readonly float _maxProjectileDistance = 6.0f;

    // time to activate features in the remained time defined by its argument
    public static readonly float _lastRushTime = 25f;

    // language identifier for English
    public static readonly string _english = "EN";

    // language identifier for Japanese
    public static readonly string _japanese = "JP";
}
