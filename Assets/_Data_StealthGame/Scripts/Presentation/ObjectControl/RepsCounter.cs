using System;
using UnityEngine;
/// <summary>
/// count how many times the player was able to do the selected exercise during single gameplay
/// </summary>
public class RepsCounter : MonoBehaviour, IActionActivator
{
    [SerializeField, Tooltip("scene object reference")]
    private SceneObjectContainer _sceneObjectContainer;

    // counted number of exercise detected
    private int _reps;

    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        _sceneObjectContainer.GetExerciseInput()._onPush += IncrementReps;
    }

    /// <summary>
    /// remove callback
    /// </summary>
    private void OnDestroy()
    {
        _sceneObjectContainer.GetExerciseInput()._onPush -= IncrementReps;
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// initialize action
    /// </summary>
    public void InitAction()
    {
        ResetReps();
    }

    /// <summary>
    /// enable action
    /// </summary>
    public void StartAction()
    {

    }

    /// <summary>
    /// disable action
    /// </summary>
    public void StopAction()
    {

    }

    /// <summary>
    /// count reps
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void IncrementReps(object sender, EventArgs args)
    {
        _reps++;
        Debug.Log("counted reps: " + _reps);
    }

    /// <summary>
    /// get counted reps
    /// </summary>
    /// <returns></returns>
    public int GetReps()
    {
        return _reps;
    }

    /// <summary>
    /// reset count of reps
    /// </summary>
    private void ResetReps()
    {
        _reps = 0;
    }
}
