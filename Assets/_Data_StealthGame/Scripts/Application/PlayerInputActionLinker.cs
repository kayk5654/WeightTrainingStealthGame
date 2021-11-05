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
        //IActionActivator optionMenu = MonoBehaviour.FindObjectOfType<IActionActivator>();
        //parentObject.SetOptionMenuUi(optionMenu);
    }
}
