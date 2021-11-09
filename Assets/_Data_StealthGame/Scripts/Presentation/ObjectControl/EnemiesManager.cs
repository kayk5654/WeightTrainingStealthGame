using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage enemy objects
/// </summary>
public class EnemiesManager : MonoBehaviour, IItemManager<LevelDataSet, SpawnAreaDataSet>
{
    // dictionary of _node instances
    private Dictionary<int, Enemy> _enemies;

    [SerializeField, Tooltip("prefab of enemy object to spawn")]
    private GameObject _enemyPrefab;

    [SerializeField, Tooltip("compute shader for node control")]
    private ComputeShader _nodeConnectionControl;

    [SerializeField, Tooltip("range to search nearby nodes")]
    private float _nodeSearchingRange = 1f;

    [SerializeField, Tooltip("base move speed of an enemy")]
    private float _baseMoveSpeed = 1f;

    [SerializeField, Tooltip("spawn enemy objects in the spawn area")]
    private ObjectSpawnHandler _objectSpawnHandler;

    // process to keep spawning enemies
    private IEnumerator _spawnEnemySequence;

    // current level data required to spawn enemies
    private LevelDataSet _currentLevelData;

    // whether the nodes must be updated in this frame
    private bool _toUpdate = false;

    // for calculation, store position of spawned enemy temporarily 
    private Vector3 _positionTemp;

    // recordd the id of the enemy spawned lasttime
    private int _lastSpawnedEnemyId = -1;


    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// update status of the enemies
    /// </summary>
    private void Update()
    {
        if (!_toUpdate) { return; }
    }

    #endregion

    #region IItemManager

    /// <summary>
    /// spawn scene objects
    /// </summary>
    /// <param name="dataset"></param>
    public void Spawn(LevelDataSet dataset, SpawnAreaDataSet spawnArea = null)
    {
        _currentLevelData = dataset;

        // initialize spawn area
        _objectSpawnHandler.SetSpawnArea(spawnArea);

        InitEnemyDictionary();
        StartSpawnEnemies();

        _toUpdate = true;
    }

    /// <summary>
    /// pause update of scene objects
    /// </summary>
    public void Pause()
    {
        StopSpawnEnemies();


        _toUpdate = false;
    }

    /// <summary>
    /// resume update of scene objects
    /// </summary>
    public void Resume()
    {
        StartSpawnEnemies();
        
        _toUpdate = true;
    }

    /// <summary>
    /// delete scene objects when the gameplay ends
    /// </summary>
    public void Delete()
    {
        _toUpdate = false;
        StopSpawnEnemies();
        DeleteAllEnemies();
    }

    #endregion

    #region OtherFunctions

    /// <summary>
    /// initialize enemies dictionary
    /// </summary>
    private void InitEnemyDictionary()
    {
        // initialize dictionary
        _enemies = new Dictionary<int, Enemy>();
    }

    /// <summary>
    /// start spawn enemy sequence
    /// </summary>
    private void StartSpawnEnemies()
    {
        if(_spawnEnemySequence != null || _currentLevelData == null)
        {
            return;
        }

        _spawnEnemySequence = SpawnEnemySequence();
        StartCoroutine(_spawnEnemySequence);
    }

    /// <summary>
    /// stop spawn enemy sequence
    /// </summary>
    private void StopSpawnEnemies()
    {
        if(_spawnEnemySequence == null)
        {
            return;
        }

        StopCoroutine(_spawnEnemySequence);
        _spawnEnemySequence = null;
    }

    /// <summary>
    /// keep spawning enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemySequence()
    {
        // spawn interval
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(Config._enemySpawnUnitTime / _currentLevelData._enemySpawnRate);

        while (true)
        {
            // if the number of enemies in the field is less than max number, spawn enemies
            if(_enemies.Count < _currentLevelData._maxEnemyNumberInField)
            {
                Enemy newEnemy = SpawnSingleEnemy();
                _enemies.Add(newEnemy._id, newEnemy);
            }
            
            yield return waitForSpawnInterval;
        }
    }

    /// <summary>
    /// spawn single enemy
    /// </summary>
    /// <returns></returns>
    private Enemy SpawnSingleEnemy()
    {
        // instantiate new enemy
        Enemy newEnemy = _objectSpawnHandler.Spawn(_enemyPrefab, transform).GetComponent<Enemy>();

         // set parameter
        newEnemy._enemiesManager = this;
        newEnemy._id = _lastSpawnedEnemyId + 1;
        newEnemy._range = _nodeSearchingRange;
        newEnemy._speed = _baseMoveSpeed;

        // record id of this enemy
        _lastSpawnedEnemyId = newEnemy._id;

        return newEnemy;
    }

    /// <summary>
    /// delete all enemies
    /// </summary>
    private void DeleteAllEnemies()
    {
        foreach(Enemy enemy in _enemies.Values)
        {
            Destroy(enemy.gameObject);
        }

        _enemies.Clear();
    }

    #endregion
}
