/// <summary>
/// data set to detect movement of each available exercises
/// </summary>
[System.Serializable]
public class ExerciseInputDataSet
{
    // type of exercises
    public ExerciseType _exerciseType;

    // type of input data
    public ExerciseInputType _inputType;

    /* FOR POSITON TRACKING INPUT */
    // max difference of height from start position during single movement cycle
    public float _peakHeightOffset;

    // range of height to detect switching movement direction
    public float _heightOffsetMargin;
}
