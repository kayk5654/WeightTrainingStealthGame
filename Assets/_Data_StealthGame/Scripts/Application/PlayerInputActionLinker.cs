using UnityEngine;
/// <summary>
/// set scene object reference for PlayerActionManager
/// </summary>
public class PlayerInputActionLinker : ISceneObjectLinker<PlayerActionManager>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    public void LinkObject(PlayerActionManager parentObject)
    {
        // get action activators in the scene
        InGameInputSwitcher inGameInputSwitcher = MonoBehaviour.FindObjectOfType<InGameInputSwitcher>();

        RepsCounter repsCounter = MonoBehaviour.FindObjectOfType<RepsCounter>();


        IActionActivator[] actionActivators = new IActionActivator[] { inGameInputSwitcher, repsCounter };

        // set action activators
        parentObject.SetActionActivators(actionActivators);
        
        // set in-game input
        parentObject.SetInGameInput(inGameInputSwitcher);

        // set offense action
        ProjectileSpawnHandler projectileSpawnHandler = MonoBehaviour.FindObjectOfType<ProjectileSpawnHandler>();
        parentObject.SetOffenseAction(projectileSpawnHandler);

        // set defense action

        // set exercise info setter
        parentObject.SetExerciseInfoSetter(inGameInputSwitcher);

        // initialize relation between in-game input and actions
        parentObject.SetCallback();
    }
}
