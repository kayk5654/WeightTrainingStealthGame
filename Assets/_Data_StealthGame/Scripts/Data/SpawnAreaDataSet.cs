using UnityEngine;
/// <summary>
/// dataset of spawn area depending on the exercise types
/// </summary>
[System.Serializable]
public class SpawnAreaDataSet
{
    // type of exercise
    public ExerciseType _exerciseType;

    // whether the area contained in this dataset should be overriden by the area gained from scanned data of the room
    public bool _isScannedDataPreferred;

    // center of the spawn area
    public Vector3 _center;

    // size of the spawn area
    public Vector3 _size;

    // world-space rotation of the spawn area
    public Quaternion _rotation;
}
