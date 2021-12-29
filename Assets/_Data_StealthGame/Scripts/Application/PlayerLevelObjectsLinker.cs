using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// link IPlayerLevelHandler and a PlayerLevelResetter
/// </summary>
public class PlayerLevelObjectsLinker : ISceneObjectLinker<IPlayerLevelHandler>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(IPlayerLevelHandler parentObject)
    {
        // link player object manager
        PlayerLevelResetter[] playerLevelResetters = Resources.FindObjectsOfTypeAll<PlayerLevelResetter>();
        
        if (playerLevelResetters.Length > 0) 
        {
            foreach (PlayerLevelResetter resetter in playerLevelResetters)
            {
                if (string.IsNullOrEmpty(resetter.gameObject.scene.name)) { continue; }
                resetter.SetPlayerLevelHandler(parentObject);
            }
        }

        DifficultySetter[] difficultySetters = Resources.FindObjectsOfTypeAll<DifficultySetter>();

        if(difficultySetters.Length > 0)
        {
            foreach(DifficultySetter setter in difficultySetters)
            {
                if (string.IsNullOrEmpty(setter.gameObject.scene.name)) { continue; }
                setter.SetPlayerLevelHandler(parentObject);
            }
        }
    }
}
