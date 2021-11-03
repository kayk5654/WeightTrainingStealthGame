using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// set and notify information of the selected exercise
/// </summary>
public class ExerciseInfoSetter : MonoBehaviour, IExerciseInfoSetter
{
    // notify information about the selected exercise
    public event EventHandler<ExerciseInfoEventArgs> _onExerciseSelected;
    
    // selected exercise is stored temporarily
    private ExerciseType _selectedExercise = ExerciseType.None;

    [SerializeField, Tooltip("button to finalize exercise selection")]
    private Button _finalizeButton;

    /// <summary>
    /// reset selection
    /// </summary>
    private void OnEnable()
    {
        _selectedExercise = ExerciseType.None;
    }

    /// <summary>
    /// update finalize button status
    /// </summary>
    private void Update()
    {
        SetFinalizeButtonAvailability();
    }

    /// <summary>
    /// select exercise from the buttons
    /// </summary>
    /// <param name="exerciseType"></param>
    public void SelectExercise(int exerciseType)
    {
        // check whether exerciseType is valid
        if (exerciseType >= (int)ExerciseType.LENGTH || exerciseType < 0) { return; }
        
        _selectedExercise = (ExerciseType)exerciseType;
    }

    /// <summary>
    /// finalize exercise selection and notify it to the upper classes
    /// </summary>
    public void FinalizeSelection()
    {
        // notify exercise selection to the upper classes
        ExerciseInfoEventArgs args = new ExerciseInfoEventArgs(_selectedExercise);
        _onExerciseSelected?.Invoke(this, args);


    }

    /// <summary>
    /// enable to press _finalizeButton if eithre of exercises are selected
    /// </summary>
    private void SetFinalizeButtonAvailability()
    {
        if (_selectedExercise == ExerciseType.None)
        {
            // disable finalize button
            if (_finalizeButton)
            {
                _finalizeButton.interactable = false;
            }
        }
        else
        {
            // enable finalize button
            if (_finalizeButton)
            {
                _finalizeButton.interactable = true;
            }
        }
        
        
    }
}
