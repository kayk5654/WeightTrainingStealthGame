using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
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

    [SerializeField, Tooltip("get node information")]
    private NodesManager _nodesManager;

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

    // contain nearest node id from each enemy instaces
    private ComputeBuffer _nearestNodeBuffer;

    // contain current enemies' positions
    private ComputeBuffer _enemyPositionBuffer;

    // contain node id from _nearestNodeBuffer / _id of enemy is index of this array
    private int[] _nearestNodeBufferData;

    // contain enemies' position to set _enemyPositionBuffer / _id of enemy is index of this array
    private Vector3[] _enemyPositionBufferData;

    // the theoretical maximum number of spawned enemies
    private int _maxSpawnedEnemyNum;


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
        UpdateBuffers();
        SetNearestNodeOnEnemies();
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

        // calculate theoretical max number of spawned enemies
        _maxSpawnedEnemyNum = Mathf.FloorToInt((dataset._enemySpawnRate / Config._enemySpawnUnitTime) * dataset._duration);

        // initialize spawn area
        _objectSpawnHandler.SetSpawnArea(spawnArea);

        // initialize enemy control data
        InitializeEnemyDictionary();
        InitializeBuffers();
        _nodesManager.InitializeFindNearestNodeKernel(_maxSpawnedEnemyNum, _nodeSearchingRange);

        // start spawning
        StartSpawnEnemies();

        _toUpdate = true;
    }

    /// <summary>
    /// pause update of scene objects
    /// </summary>
    public void Pause()
    {
        StopSpawnEnemies();
        EnableUpdateEnemies(false);

        _toUpdate = false;
    }

    /// <summary>
    /// resume update of scene objects
    /// </summary>
    public void Resume()
    {
        StartSpawnEnemies();
        EnableUpdateEnemies(true);

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
        ReleaseBuffers();
    }

    #endregion

    #region Handle ComputeShader

    /// <summary>
    /// initialize compute buffers for calculation
    /// </summary>
    private void InitializeBuffers()
    {
        // _nearestNodeBuffer contains node id (= int)
        _nearestNodeBuffer = new ComputeBuffer(_maxSpawnedEnemyNum, Marshal.SizeOf(typeof(int)));
        _nearestNodeBufferData = Enumerable.Repeat<int>(-1, _maxSpawnedEnemyNum).ToArray();

        // _enemyPositionBuffer contains each enemies' position (= vector3)
        _enemyPositionBuffer = new ComputeBuffer(_maxSpawnedEnemyNum, Marshal.SizeOf(typeof(Vector3)));
        _enemyPositionBufferData = Enumerable.Repeat<Vector3>(Vector3.zero, _maxSpawnedEnemyNum).ToArray();
    }

    /// <summary>
    /// set latest enemy positions into data container for compute buffers
    /// </summary>
    private void UpdateBuffers()
    {
        if (_enemies == null || _enemies.Count < 1) { return; }

        foreach (Enemy enemy in _enemies.Values)
        {
            _enemyPositionBufferData[enemy.GetId()] = enemy.transform.position;
        }
    }

    /// <summary>
    /// set the nearest nodes on all alive enemies
    /// </summary>
    private void SetNearestNodeOnEnemies()
    {
        if (_enemies == null || _enemies.Count < 1) { return; }

        // calculate the nearest nodes in _nodesManager using compute shader
        _enemyPositionBuffer.SetData(_enemyPositionBufferData);
        _nodesManager.GetNearestNode(_nearestNodeBuffer, _enemyPositionBuffer);

        // receive calculated position
        _nearestNodeBuffer.GetData(_nearestNodeBufferData);

        // pass nearest node reference to each enemiess
        foreach (Enemy enemy in _enemies.Values)
        {
            enemy.SetNearestNode(_nodesManager.GetNode(_nearestNodeBufferData[enemy.GetId()]));
        }
    }

    /// <summary>
    /// release compute buffers
    /// </summary>
    private void ReleaseBuffers()
    {
        _nearestNodeBuffer.Release();
        _enemyPositionBuffer.Release();
    }

    #endregion

    #region OtherFunctions

    /// <summary>
    /// initialize enemies dictionary
    /// </summary>
    private void InitializeEnemyDictionary()
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
                _enemies.Add(newEnemy.GetId(), newEnemy);
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
        newEnemy.InitParams(_lastSpawnedEnemyId + 1, _nodeSearchingRange, _baseMoveSpeed);

        // record id of this enemy
        _lastSpawnedEnemyId = newEnemy.GetId();

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

    /// <summary>
    /// enable/disable updating enemies' transform
    /// </summary>
    /// <param name="state"></param>
    private void EnableUpdateEnemies(bool state)
    {
        foreach(Enemy enemy in _enemies.Values)
        {
            enemy.EnableUpdate(state);
        }
    }

    #endregion
}
