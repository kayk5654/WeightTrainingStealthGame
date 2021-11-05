using System.Collections;
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

    // manager of scene objects
    private List<IItemManagerBase<GameDataSetBase>> _itemManagers = new List<IItemManagerBase<GameDataSetBase>>();


    /// <summary>
    /// constructor
    /// </summary>
    public LevelManager()
    {
        InitDataBase();
    }

    public void SetItemManager(IItemManagerBase<GameDataSetBase> manager)
    {
        _itemManagers.Add(manager);
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
                    int playerLevel = 0;
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

        // load level
        foreach(IItemManagerBase<GameDataSetBase> manager in _itemManagers)
        {
            if(manager is IItemManagerBase<LevelDataSet>)
            {
                manager.Spawn(levelData);
            }
            else if (manager is IItemManagerBase<PlayerAbilityDataSet>)
            {
                manager.Spawn(playerAbility);
            }
        }
    }

    /// <summary>
    /// pause level interactions
    /// </summary>
    private void PauseLevel()
    {
        foreach (IItemManagerBase<GameDataSetBase> manager in _itemManagers)
        {
            manager.Pause();
        }
    }

    /// <summary>
    /// resume level interactions
    /// </summary>
    private void ResumeLevel()
    {
        foreach (IItemManagerBase<GameDataSetBase> manager in _itemManagers)
        {
            manager.Resume();
        }
    }

    /// <summary>
    /// delete loaded level
    /// </summary>
    private void DeleteLevel()
    {
        foreach (IItemManagerBase<GameDataSetBase> manager in _itemManagers)
        {
            manager.Delete();
        }
    }
}
