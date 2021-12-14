using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control player's action for tutorial phases
/// </summary>
public class TutorialActionHandler : MonoBehaviour
{
    [SerializeField, Tooltip("exercise input for tutorial")]
    private ExerciseInput _exerciseInput;

    [SerializeField, Tooltip("spawn projectile")]
    private ProjectileSpawnHandler _projectileSpawnHandler;

    [SerializeField, Tooltip("cursor manager")]
    private CursorManager _cursorManager;

    // callback of attack action
    public delegate void TutorialActionCallback();
    public TutorialActionCallback _tutorialActionCallback;


    /// <summary>
    /// initialization
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// initialize input
    /// </summary>
    public void InitAction()
    {
        _exerciseInput.InitAction();
    }

    /// <summary>
    /// enable player's action for tutorial
    /// </summary>
    public void EnableAction()
    {
        _exerciseInput._onPush += Attack;
        _exerciseInput.StartAction();
        _cursorManager.SetActive(true);
    }

    /// <summary>
    /// disable player's action for tutorial
    /// </summary>
    public void DisableAction()
    {
        _exerciseInput.StopAction();
        _exerciseInput._onPush -= Attack;
        _cursorManager.SetActive(false);
    }

    /// <summary>
    /// enable exercise input to load squat preset
    /// </summary>
    public void SelectSquat()
    {
        _exerciseInput.ChangeExerciseType(ExerciseType.Squat);
    }

    /// <summary>
    /// trigger attack action
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Attack(object sender, EventArgs args)
    {
        _projectileSpawnHandler.Attack();
        _tutorialActionCallback?.Invoke();
    }
}
