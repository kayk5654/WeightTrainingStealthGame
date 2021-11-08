using UnityEngine;
/// <summary>
/// enable to get different spawn area depending on the exercise type selection
/// </summary>
public class SpawnAreaHandler
{
    // center of the spawn area
    private Vector3 _center;

    // size of the spawn area
    private Vector3 _size;

    // world-space rotation of the spawn area
    private Quaternion _rotation;

    /// <summary>
    /// overrde spawn area
    /// </summary>
    public SpawnAreaDataSet GetSpawnArea()
    {
        // TODO: access database and get appropriate spawn area data
        // if _isScannedDataPreferred is true, get data from the spatial awareness

        return new SpawnAreaDataSet
        {
            _center = this._center,
            _size = this._size,
            _rotation = this._rotation
        };
    }
}
