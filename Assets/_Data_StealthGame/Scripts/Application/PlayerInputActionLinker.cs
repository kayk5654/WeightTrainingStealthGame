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
        InGameInputSwitcher inGameInputManagre = MonoBehaviour.FindObjectOfType<InGameInputSwitcher>();

        //IActionActivator optionMenu = MonoBehaviour.FindObjectOfType<IActionActivator>();
        //parentObject.SetOptionMenuUi(optionMenu);

        IActionActivator[] actionActivators = new IActionActivator[] { inGameInputManagre };

        // set action activators
        parentObject.SetActionActivators(actionActivators);
        
        // set in-game input
        parentObject.SetInGameInput(inGameInputManagre);

        // set offense action
        ProjectileSpawnHandler projectileSpawnHandler = MonoBehaviour.FindObjectOfType<ProjectileSpawnHandler>();
        parentObject.SetOffenseAction(projectileSpawnHandler);

        // set defense action

        // initialize relation between in-game input and actions
        parentObject.SetCallback();
    }
}
