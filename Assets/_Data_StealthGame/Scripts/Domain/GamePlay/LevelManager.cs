﻿using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manage levels of the gameplay
/// </summary>
public class LevelManager : IGamePlayStateSetter, IExerciseInfoSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;

    // last gameplay state
    private GamePlayState _lastGameplayState = GamePlayState.None;

    // current exercise type
    private ExerciseType _currentExerciseType = ExerciseType.None;

    // get player ability related to a specific player level
    private PlayerAbilityDatabase _playerAbilityDatabase;

    // get level data related to a specific player level
    private LevelDatabase _levelDatabase;

    // get spawn area data of the scene objects
    private SpawnAreaDatabase _spawnAreaDatabase;

    // manager of player's objects
    private IItemManager<PlayerAbilityDataSet, SpawnAreaDataSet> _playerObjectManagers;

    // manager of enemy objects
    private IItemManager<LevelDataSet, SpawnAreaDataSet> _enemyObjectManager;

    // get current player's level
    private IPlayerLevelHandler _playerLevelHandler;

    // get notification that the gameplay ends
    private IGamePlayEndSender[] _gameplayEndSenders;

    // count play time
    private IPlayTimeCounter _timeLimitCounter;

    // receivers to event to activate features for last several seconds of current gameplay
    private ILastRushEventReceiver[] _lastRushEventReceivers;

    // whether the player won the last gameplay
    private bool _didPlayerWin;


    /// <summary>
    /// constructor
    /// </summary>
    public LevelManager()
    {
        InitDataBase();
    }

    /// <summary>
    /// destructor
    /// </summary>
    ~LevelManager()
    {
        _timeLimitCounter._onGamePlayEnd -= EndGamePlayByTimeLimit;
        _timeLimitCounter._lastRushTimeEvent -= ActivateLastRushTimeFeature;
        if (_gameplayEndSenders == null || _gameplayEndSenders.Length < 1) { return; }
        
        foreach (IGamePlayEndSender sender in _gameplayEndSenders)
        {
            sender._onGamePlayEnd -= NotifyGamePlayEnd;
        }
    }

    /// <summary>
    /// set reference of player's object manager
    /// </summary>
    /// <param name="manager"></param>
    public void SetPlayerObjectManager(IItemManager<PlayerAbilityDataSet, SpawnAreaDataSet> manager)
    {
        _playerObjectManagers = manager;
    }

    /// <summary>
    /// set reference of enemy object manager
    /// </summary>
    /// <param name="manager"></param>
    public void SetEnemyObjectManager(IItemManager<LevelDataSet, SpawnAreaDataSet> manager)
    {
        _enemyObjectManager = manager;
    }

    /// <summary>
    /// set reference of player level handler
    /// </summary>
    /// <param name="playerLevelHandler"></param>
    public void SetPlayerLevelHandler(IPlayerLevelHandler playerLevelHandler)
    {
        _playerLevelHandler = playerLevelHandler;
    }

    /// <summary>
    /// set reference of gameplay end sender
    /// </summary>
    /// <param name="gamePlayEndSenders"></param>
    public void SetGamePlayEndSender(IGamePlayEndSender[] gamePlayEndSenders)
    {
        _gameplayEndSenders = gamePlayEndSenders;

        foreach(IGamePlayEndSender sender in _gameplayEndSenders)
        {
            sender._onGamePlayEnd += NotifyGamePlayEnd;
        }
    }

    /// <summary>
    /// set reference of time limit counter
    /// </summary>
    /// <param name="counter"></param>
    public void SetPlayTimeCounter(IPlayTimeCounter counter)
    {
        _timeLimitCounter = counter;
        _timeLimitCounter._onGamePlayEnd += EndGamePlayByTimeLimit;
        _timeLimitCounter._lastRushTimeEvent += ActivateLastRushTimeFeature;
    }

    /// <summary>
    /// set reference of last rush event receiver
    /// </summary>
    /// <param name="receivers"></param>
    public void SetLastRushEventReceiver(ILastRushEventReceiver[] receivers)
    {
        _lastRushEventReceivers = receivers;
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

            case GamePlayState.BeforePlay:
                break;

            case GamePlayState.Playing:
                if (_lastGameplayState == GamePlayState.BeforePlay)
                {
                    // if the gameplay starts, load new level
                    int playerLevel = _playerLevelHandler.GetPlayerLevel();
                    GetNewLevel(playerLevel);
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

            case GamePlayState.AfterPlay:
                // disable attack target finding features
                // show "after play" condition of the level
                SetLevelAfterPlay();
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
        JsonDatabaseReader<SpawnAreaDataSet> spawnAreaJson = new JsonDatabaseReader<SpawnAreaDataSet>();

        // create database class
        _playerAbilityDatabase = new PlayerAbilityDatabase(playerAbilityJson, Config._playerAbilityDataPath);
        _levelDatabase = new LevelDatabase(levelJson, Config._levelDataPath);
        _spawnAreaDatabase = new SpawnAreaDatabase(spawnAreaJson, Config._spawnAreaDataPath);

    }

    /// <summary>
    /// get and load new level based on the player's status and exercise selection
    /// </summary>
    private void GetNewLevel(int playerLevel)
    {
        LevelDataSet levelData = _levelDatabase.GetData(playerLevel);
        PlayerAbilityDataSet playerAbility = _playerAbilityDatabase.GetData(playerLevel);
        SpawnAreaDataSet spawnArea = _spawnAreaDatabase.GetData((int)_currentExerciseType);
        DebugLog.Info(this.ToString(), "_currentExerciseType: " + _currentExerciseType);
        // load level
        _enemyObjectManager.Spawn(levelData, spawnArea);
        _playerObjectManagers.Spawn(playerAbility, spawnArea);

        // start counting playtime
        _timeLimitCounter.SetTimeLimit(levelData._duration);
        _timeLimitCounter.SetLastRushTime(Config._lastRushTime);
        _timeLimitCounter.StartCount();
    }

    /// <summary>
    /// pause level interactions
    /// </summary>
    private void PauseLevel()
    {
        _enemyObjectManager.Pause();
        _playerObjectManagers.Pause();
        _timeLimitCounter.PauseCount();
    }

    /// <summary>
    /// resume level interactions
    /// </summary>
    private void ResumeLevel()
    {
        _enemyObjectManager.Resume();
        _playerObjectManagers.Resume();
        _timeLimitCounter.ResumeCount();
    }

    /// <summary>
    /// delete loaded level
    /// </summary>
    private void DeleteLevel()
    {
        _enemyObjectManager.Delete();
        _playerObjectManagers.Delete();
        _timeLimitCounter.QuitCount();
    }

    /// <summary>
    /// show "after play" condition of the level
    /// </summary>
    private void SetLevelAfterPlay()
    {
        _enemyObjectManager.AfterPlay(_didPlayerWin);
        _playerObjectManagers.AfterPlay(_didPlayerWin);
        _timeLimitCounter.PauseCount();
    }

    /// <summary>
    /// set exercise type
    /// </summary>
    /// <param name="exerciseType"></param>
    public void ChangeExerciseType(ExerciseType exerciseType)
    {
        _currentExerciseType = exerciseType;
    }

    /// <summary>
    /// notify the end of the gameplay to the upper classes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NotifyGamePlayEnd(object sender, GamePlayEndArgs args)
    {
        // set game play state "AfterPlay" to show result of this play
        GamePlayStateEventArgs stateArgs = new GamePlayStateEventArgs(GamePlayState.AfterPlay);
        stateArgs._optionalArgs = args;
        _onGamePlayStateChange?.Invoke(this, stateArgs);
        _didPlayerWin = args._didPlayerWin;
    }

    /// <summary>
    /// terminate gameplay by time limit of the level
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void EndGamePlayByTimeLimit(object sender, GamePlayStateEventArgs args)
    {
        // get number of alive nodes; if at least 1 node is alive, player wins.
        _didPlayerWin = _playerObjectManagers.GetItemCount() > 0;
        GamePlayEndArgs gameplayEndArgs = new GamePlayEndArgs(_didPlayerWin);

        // set game play state "AfterPlay" to show result of this play
        GamePlayStateEventArgs stateArgs = new GamePlayStateEventArgs(GamePlayState.AfterPlay);
        stateArgs._optionalArgs = gameplayEndArgs;
        _onGamePlayStateChange?.Invoke(this, stateArgs);

        
    }

    /// <summary>
    /// activate features in the remained time defined by its argument
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ActivateLastRushTimeFeature(object sender, EventArgs args)
    {
        foreach(ILastRushEventReceiver receiver in _lastRushEventReceivers)
        {
            receiver.LastRushEventCallback(sender, args);
        }
    }
}
