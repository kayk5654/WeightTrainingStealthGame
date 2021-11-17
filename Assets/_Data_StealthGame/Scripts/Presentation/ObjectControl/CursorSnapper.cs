using UnityEngine;
/// <summary>
/// snap cursor object on the scene objects
/// </summary>
public class CursorSnapper
{
    // cursor position after calculating snap
    private Vector3 _snappedCursorPosition;

    // cursor position in the last frame forinterpolation
    private Vector3 _lastCursorPosition;

    // interpolation factor for the snapped cursor position
    private float _positionLerpFactor = 0.7f;


    /// <summary>
    /// initialize cursor position data
    /// </summary>
    /// <param name="initCursorPosition"></param>
    public void Init(Vector3 initCursorPosition)
    {
        _lastCursorPosition = initCursorPosition;
    }

    /// <summary>
    /// set latest information of raycasting
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="hit"></param>
    public void SetRayCastingInfo(Vector3 origin, Vector3 direction, bool isHit, RaycastHit hit)
    {
        if (isHit)
        {
            // if the ray hits something, snap cursor on the hit object
            _snappedCursorPosition = hit.collider.ClosestPoint(origin);
        }
        else
        {
            _snappedCursorPosition = origin + direction.normalized * Config._cursorMaxDistance;
        }

        // interpolate cursor position
        _snappedCursorPosition = Vector3.Lerp(_lastCursorPosition, _snappedCursorPosition, _positionLerpFactor);

        // record calculation result
        _lastCursorPosition = _snappedCursorPosition;
    }

    /// <summary>
    /// get cursor position snapped on a scene object 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSnappedCursorPosition()
    {
        return _snappedCursorPosition;
    }
}
