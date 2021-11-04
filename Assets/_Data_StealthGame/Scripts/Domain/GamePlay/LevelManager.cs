﻿using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage levels of the gameplay
/// </summary>
public class LevelManager : IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // last gameplay state
    private GamePlayState _lastGameplayState = GamePlayState.None;

    // get player ability related to a specific player level
    private PlayerAbilityDatabase _playerAbilityDatabase;

    // get level data related to a specific player level
    private LevelDatabase _levelDatabase;




    /// <summary>
    /// constructor
    /// </summary>
    public LevelManager()
    {
        InitDataBase();
    }

    /// <summary>
    /// send update of the gameplay state to the lower classes in the execution flow
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        switch (gamePlayState)
        {
            case GamePlayState.None:
                // if the gameplay is terminated, delete loaded level
                DeleteLevel();
                break;

            case GamePlayState.Playing:
                if (_lastGameplayState == GamePlayState.None)
                {
                    // if the gameplay starts, load new level
                    GetNewLevel(0);
                }
                else if(_lastGameplayState == GamePlayState.Pause)
                {
                    // if the gameplay is resumed, resume loaded level
                    ResumeLevel();
                }
                break;

            case GamePlayState.Pause:
                PauseLevel();
                break;

            default:
                break;
        }

        // record gameplay state
        _lastGameplayState = gamePlayState;
    }

    /// <summary>
    /// initialize database
    /// </summary>
    private void InitDataBase()
    {
        // create database reader (= define database type to read)
        JsonDatabaseReader<PlayerAbilityDataSet> playerAbilityJson = new JsonDatabaseReader<PlayerAbilityDataSet>();
        JsonDatabaseReader<LevelDataSet> levelJson = new JsonDatabaseReader<LevelDataSet>();

        // create database class
        _playerAbilityDatabase = new PlayerAbilityDatabase(playerAbilityJson, Config._playerAbilityDataPath);
        _levelDatabase = new LevelDatabase(levelJson, Config._levelDataPath);

    }

    /// <summary>
    /// get and load new level based on the player's status and exercise selection
    /// </summary>
    private void GetNewLevel(int playerLevel)
    {
        LevelDataSet levelData = _levelDatabase.GetData(playerLevel);
        PlayerAbilityDataSet playerAbility = _playerAbilityDatabase.GetData(playerLevel);
    }

    /// <summary>
    /// pause level interactions
    /// </summary>
    private void PauseLevel()
    {

    }

    /// <summary>
    /// resume level interactions
    /// </summary>
    private void ResumeLevel()
    {

    }

    /// <summary>
    /// delete loaded level
    /// </summary>
    private void DeleteLevel()
    {

    }
}
