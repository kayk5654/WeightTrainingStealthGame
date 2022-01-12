using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// get exercise type and assign it on text handlers
/// </summary>
public class ExerciseTypeTextManager : MonoBehaviour
{
    [SerializeField, Tooltip("get exercise info")]
    private ExerciseInfoSetter _exerciseInfoSetter;

    [SerializeField, Tooltip("exercise type texts")]
    private ExerciseTypeTextHandler[] _exerciseTypeTextHandlers;

    // selected exercise
    private ExerciseType _selectedExercise = ExerciseType.None;

    // dictionary of text number of the exercise names
    private Dictionary<ExerciseType, int> _exerciseTextNumDictionary;

    // text number of push up
    private int _pushUpTextNum = 36;

    // text number of sit up
    private int _sitUpTextNum = 37;

    // text number of squat
    private int _squatTextNum = 38;

    // text number of chin up
    private int _chinUpTextNum = 39;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _exerciseInfoSetter._onExerciseSelected += SetExerciseType;
        IniTextNumtDictionary();
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _exerciseInfoSetter._onExerciseSelected -= SetExerciseType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IniTextNumtDictionary()
    {
        _exerciseTextNumDictionary = new Dictionary<ExerciseType, int>();
        _exerciseTextNumDictionary.Add(ExerciseType.PushUp, _pushUpTextNum);
        _exerciseTextNumDictionary.Add(ExerciseType.SitUp, _sitUpTextNum);
        _exerciseTextNumDictionary.Add(ExerciseType.Squat, _squatTextNum);
        _exerciseTextNumDictionary.Add(ExerciseType.ChinUp, _chinUpTextNum);
    }

    /// <summary>
    /// set exercise type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void SetExerciseType(object sender, ExerciseInfoEventArgs args)
    {
        _selectedExercise = args._selectedExercise;

        foreach(ExerciseTypeTextHandler textHandler in _exerciseTypeTextHandlers)
        {
            textHandler.SetExerciseTypeTextNum(_exerciseTextNumDictionary[_selectedExercise]);
        }
    }
}
