using UnityEngine;
/// <summary>
/// set scene object reference for LevelManager
/// </summary>
public class LevelObjectLinker : ISceneObjectLinker<LevelManager>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(LevelManager parentObject)
    {
        // link player object manager
        NodesManager nodesManager = MonoBehaviour.FindObjectOfType<NodesManager>();
        if (nodesManager)
        {
            parentObject.SetPlayerObjectManager(nodesManager);
        }
        
        // link enemy manager
        EnemiesManager enemiesManager = MonoBehaviour.FindObjectOfType<EnemiesManager>();
        if (enemiesManager)
        {
            parentObject.SetEnemyObjectManager(enemiesManager);
            parentObject.SetLastRushEventReceiver(new ILastRushEventReceiver[]{ enemiesManager});
        }

        IGamePlayEndSender[] gamePlayEndSenders = { nodesManager, enemiesManager };
        if(nodesManager && enemiesManager)
        {
            parentObject.SetGamePlayEndSender(gamePlayEndSenders);
        }

        TimeLimitCounter timeLimitCounter = MonoBehaviour.FindObjectOfType<TimeLimitCounter>();
        if (timeLimitCounter)
        {
            parentObject.SetPlayTimeCounter(timeLimitCounter);
        }
    }
}
