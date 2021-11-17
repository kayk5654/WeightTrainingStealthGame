using UnityEngine;
/// <summary>
/// get look direction in the scene
/// </summary>
public interface ILookDirectionGetter
{
    /// <summary>
    /// looking direction
    /// </summary>
    /// <returns></returns>
    Vector3 GetDirection();

    /// <summary>
    /// position of the viewpoint
    /// </summary>
    /// <returns></returns>
    Vector3 GetOrigin();
}
