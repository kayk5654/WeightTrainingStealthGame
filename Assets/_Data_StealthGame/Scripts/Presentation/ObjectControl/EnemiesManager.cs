using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// manage enemy objects
/// </summary>
public class EnemiesManager : MonoBehaviour, IItemManager<LevelDataSet>
{
    // dictionary of _node instances
    private Dictionary<int, Enemy> _enemies;

    [SerializeField, Tooltip("prefab of enemy object to spawn")]
    private Enemy _enemyPrefab;

    // whether the nodes must be updated in this frame
    private bool toUpdate = false;


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
        if (!toUpdate) { return; }
    }

    #endregion

    #region IItemManager

    /// <summary>
    /// spawn scene objects
    /// </summary>
    /// <param name="dataset"></param>
    public void Spawn(LevelDataSet dataset)
    {
        toUpdate = true;
    }

    /// <summary>
    /// pause update of scene objects
    /// </summary>
    public void Pause()
    {
        toUpdate = false;
    }

    /// <summary>
    /// resume update of scene objects
    /// </summary>
    public void Resume()
    {
        toUpdate = true;
    }

    /// <summary>
    /// delete scene objects when the gameplay ends
    /// </summary>
    public void Delete()
    {
        toUpdate = false;
    }

    #endregion

    #region OtherFunctions



    #endregion
}
