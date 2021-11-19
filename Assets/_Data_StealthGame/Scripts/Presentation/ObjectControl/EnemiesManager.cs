using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
using System;
/// <summary>
/// data of single enemy
/// </summary>
public struct Enemy_ComputeShader
{
    // identify each enemies
    public int _id;
    // world space position of an enemy
    public Vector3 _position;
    // whether the enemy is searching player's object (= whether the enemy is animated)
    public int _isSearching;
}

/// <summary>
/// manage enemy objects
/// </summary>
public class EnemiesManager : MonoBehaviour, IItemManager<LevelDataSet, SpawnAreaDataSet>, IGamePlayEndSender
{
    // dictionary of _node instances
    private Dictionary<int, Enemy> _enemies;

    [SerializeField, Tooltip("prefab of enemy object to spawn")]
    private GameObject _enemyPrefab;

    [SerializeField, Tooltip("compute shader for enemy control")]
    private ComputeShader _enemyBehaviourControl;

    [SerializeField, Tooltip("range to search nearby nodes")]
    private float _nodeSearchingRange = 0.6f;

    [SerializeField, Tooltip("base move speed of an enemy")]
    private float _baseMoveSpeed = 1f;

    [SerializeField, Tooltip("range of neighbour enemies which affects single enemy's behaviour")]
    private float _neighbourRadious = 0.4f;

    [SerializeField, Tooltip("weight of the velocity to avoid boundary")]
    private float _avoidBoundaryVelWeight = 1f;

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

    // kernel parameters of GetEnemyForce
    private KernelParamsHandler _getEnemyForceKernel;

    // kernel name of GetEnemyForce
    private string _getEnemyForceKernelName = "GetEnemyForce";

    // contain nearest node id from each enemy instaces
    private ComputeBuffer _nearestNodeBuffer;

    // contain current enemies' positions
    private ComputeBuffer _enemyPositionBuffer;

    // contain enemy data (for reading)
    private ComputeBuffer _enemyBuffer_Read;

    // parameter name of _enemyBufferRead
    private string _enemyBufferName_Read = "_enemyBufferRead";

    // contain enemy force (for writing)
    private ComputeBuffer _enemyForceBuffer;

    // parameter name of _enemyForceBuffer
    private string _enemyForceBufferName = "_enemyForceBuffer";

    // contain enemy data (for reading)
    private Enemy_ComputeShader[] _enemyBufferData_Read;

    // contain calculated enemy force
    private Vector3[] _enemyForceBufferData;

    // contain node id from _nearestNodeBuffer / _id of enemy is index of this array
    private int[] _nearestNodeBufferData;

    // contain enemies' position to set _enemyPositionBuffer / _id of enemy is index of this array
    private Vector3[] _enemyPositionBufferData;

    // the theoretical maximum number of spawned enemies
    private int _maxSpawnedEnemyNum;

    // name of parameter of _maxSpawnedEnemyNum on the compute shader
    private string _maxSpawnedEnemyNumName = "_maxSpawnedEnemyNum";

    // name of parameter of _neighbourRadious on the compute shader
    private string _neighbourRadiousName = "_neighbourRadious";

    // parameter name of _avoidBoundaryVelWeight
    private string _avoidBoundaryVelWeightName = "_avoidBoundaryVelWeight";

    // parameter name of _boundaryCenter
    private string _boundaryCenterName = "_boundaryCenter";

    // parameter name of _boundarySize
    private string _boundarySizeName = "_boundarySize";

    // parameter name of _boundaryRotation
    private string _boundaryRotationName = "_boundaryRotation";

    // parameter name of _deltaTime
    private string _deltaTimeName = "_deltaTime";

    // thread size of a thread group
    private const int SIMULATION_BLOCK_SIZE = 256;

    // notify the end of current gameplay to the upper classes
    public event EventHandler<GamePlayEndArgs> _onGamePlayEnd;


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
        UpdateForce();
        SetNearestNodeOnEnemies();
        CheckGameEndState();
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

        // initialize enemy id
        _lastSpawnedEnemyId = -1;

        // initialize enemy control data
        InitializeEnemyDictionary();
        InitializeBuffers();
        InitializeParams();
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

    /// <summary>
    /// get number of items
    /// </summary>
    /// <returns></returns>
    public int GetItemCount()
    {
        return _enemies.Count;
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

        // _enemyBufferReading contains current status of spawned enemies
        _enemyBuffer_Read = new ComputeBuffer(_maxSpawnedEnemyNum, Marshal.SizeOf(typeof(Enemy_ComputeShader)));
        Enemy_ComputeShader defaultEnemyData = new Enemy_ComputeShader();
        defaultEnemyData._id = -1;
        defaultEnemyData._isSearching = 1;
        defaultEnemyData._position = Vector3.zero;
        _enemyBufferData_Read = Enumerable.Repeat<Enemy_ComputeShader>(defaultEnemyData, _maxSpawnedEnemyNum).ToArray();

        // _enemyForceBuffer contains calculated external force applied for spawned enemies
        _enemyForceBuffer = new ComputeBuffer(_maxSpawnedEnemyNum, Marshal.SizeOf(typeof(Vector3)));
        _enemyForceBufferData = Enumerable.Repeat<Vector3>(Vector3.zero, _maxSpawnedEnemyNum).ToArray();
    }

    /// <summary>
    /// initialize parameters of the compute shader
    /// </summary>
    private void InitializeParams()
    {
        // calculate thread group size
        int getEnemyForceKernelThreadGroupSize = Mathf.CeilToInt((float)_maxSpawnedEnemyNum / (float)SIMULATION_BLOCK_SIZE);

        // contain data of kernel in KernelParamsHandler
        _getEnemyForceKernel = new KernelParamsHandler(_enemyBehaviourControl, _getEnemyForceKernelName, getEnemyForceKernelThreadGroupSize, 1, 1);

        // set constant parameters for simulation
        _enemyBehaviourControl.SetInt(_maxSpawnedEnemyNumName, _maxSpawnedEnemyNum);
        _enemyBehaviourControl.SetFloat(_neighbourRadiousName, _neighbourRadious);
        _enemyBehaviourControl.SetFloat(_avoidBoundaryVelWeightName, _avoidBoundaryVelWeight);
        _enemyBehaviourControl.SetVector(_boundaryCenterName, _objectSpawnHandler.GetSpawnAreaCenter());
        _enemyBehaviourControl.SetVector(_boundarySizeName, _objectSpawnHandler.GetSpawnAreaSize());
        _enemyBehaviourControl.SetMatrix(_boundaryRotationName, _objectSpawnHandler.GetSpawnAreaTransformMatrix());
    }

    /// <summary>
    /// set latest enemy positions into data container for compute buffers
    /// </summary>
    private void UpdateBuffers()
    {
        if (_enemies == null || _enemies.Count < 1) { return; }

        foreach (Enemy enemy in _enemies.Values)
        {
            int id = enemy.GetId();
            _enemyPositionBufferData[id] = enemy.transform.position;
            _enemyBufferData_Read[id]._position = enemy.transform.position;
            _enemyBufferData_Read[id]._isSearching = enemy.GetCurrentState() == EnemyState.Search ? 1 : 0;
        }
    }

    /// <summary>
    /// update force applied for the spawned enemies
    /// </summary>
    private void UpdateForce()
    {
        // set buffer data on compute buffers
        _enemyBuffer_Read.SetData(_enemyBufferData_Read);

        // set compute buffers
        _enemyBehaviourControl.SetFloat(_deltaTimeName, Time.deltaTime);
        _enemyBehaviourControl.SetBuffer(_getEnemyForceKernel._index, _enemyForceBufferName, _enemyForceBuffer);
        _enemyBehaviourControl.SetBuffer(_getEnemyForceKernel._index, _enemyBufferName_Read, _enemyBuffer_Read);

        // execute GetEnemyForce kernel
        _enemyBehaviourControl.Dispatch(_getEnemyForceKernel._index, _getEnemyForceKernel._x, _getEnemyForceKernel._y, _getEnemyForceKernel._z);
        _enemyForceBuffer.GetData(_enemyForceBufferData);

        // set calculation result on the enemies
        foreach (Enemy enemy in _enemies.Values)
        {
            enemy.SetForce(_enemyForceBufferData[enemy.GetId()]);
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
        _enemyForceBuffer.Release();
        _enemyBuffer_Read.Release();
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

        // limit max spawned enemy number
        if(_lastSpawnedEnemyId >= _maxSpawnedEnemyNum - 1) { return; }

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
            // limit number of total spawned enemy number
            if(_lastSpawnedEnemyId >= _maxSpawnedEnemyNum - 1) 
            {
                _spawnEnemySequence = null;
                yield break; 
            }
            
            // if the number of enemies in the field is less than max number, spawn enemies
            if(_enemies.Count < _currentLevelData._maxEnemyNumberInField)
            {
                Enemy newEnemy = SpawnSingleEnemy();
                newEnemy.EnableUpdate(true);
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
        int id = _lastSpawnedEnemyId + 1;
        newEnemy.InitParams(id, _nodeSearchingRange, _baseMoveSpeed);
        newEnemy._onDestroyed += OnDestroyEnemy;

        // record id of this enemy
        _lastSpawnedEnemyId = id;

        // contain enemy info into the compute buffer
        _enemyBufferData_Read[id]._position = newEnemy.transform.position;
        _enemyBufferData_Read[id]._isSearching = newEnemy.GetCurrentState() == EnemyState.Search ? 1 : 0;

        return newEnemy;
    }

    /// <summary>
    /// delete all enemies
    /// </summary>
    private void DeleteAllEnemies()
    {
        foreach(Enemy enemy in _enemies.Values)
        {
            // remove callback
            enemy._onDestroyed -= OnDestroyEnemy;
            
            // destroy enemy object
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

    /// <summary>
    /// callback when an enemy is destroyed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnDestroyEnemy(object sender, InGameObjectEventArgs args)
    {
        if (!_enemies.ContainsKey(args._id)) { return; }
        _enemies.Remove(args._id);
    }

    /// <summary>
    /// check number of rest enemies; if all enemies are destroyed, end gameplay
    /// </summary>
    private void CheckGameEndState()
    {
        // check number of rest enemies
        // check id of the newesst enemy
        if(_lastSpawnedEnemyId < _maxSpawnedEnemyNum - 1) { return; }

        // check whether there're any destroyed enemies
        foreach(int key in _enemies.Keys)
        {
            if(_enemies[key] == null)
            {
                _enemies.Remove(key);
            }
        }

        // check number of the enemies in the field
        if(_enemies.Count > 0) { return; }

        // notify the end of this gameplay
        GamePlayEndArgs args = new GamePlayEndArgs(true);
        _onGamePlayEnd?.Invoke(this, args);
    }

    #endregion
}
