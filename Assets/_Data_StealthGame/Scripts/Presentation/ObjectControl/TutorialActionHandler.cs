﻿using System.Collections;
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

    [SerializeField, Tooltip("enemy object")]
    private Enemy _enemyPrefab;

    [SerializeField, Tooltip("enemy spawn guide")]
    private Transform _enemySpawnGuide;

    // enemy sample for tutorial
    private Enemy _sampleEnemy;

    // callback of attack action
    public delegate void TutorialActionCallback();
    public TutorialActionCallback _tutorialActionCallback;

    // whether the sample enemy is destroyed;
    private bool _isEnemyDestroyed;


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
        //_projectileSpawnHandler.Attack();
        _tutorialActionCallback?.Invoke();
        Debug.Log(this.ToString() + " / Attack");
    }

    /// <summary>
    /// show/hide enemy
    /// </summary>
    /// <param name="toDisplay"></param>
    public void DisplayEnemy(bool toDisplay)
    {
        if (toDisplay)
        {
            _sampleEnemy = Instantiate(_enemyPrefab, _enemySpawnGuide);
            _sampleEnemy.transform.localPosition = Vector3.zero;
            _sampleEnemy.transform.localRotation = Quaternion.identity;
            _sampleEnemy._onDestroyed += SetIsEnemyDestroyed;
            _isEnemyDestroyed = false;
        }
        else
        {
            if (_sampleEnemy)
            {
                _sampleEnemy._onDestroyed -= SetIsEnemyDestroyed;
                Destroy(_sampleEnemy.gameObject);
                _sampleEnemy = null;
            }
        }
    }

    /// <summary>
    /// check whether the enemy is found
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyFound()
    {
        if (_sampleEnemy)
        {
            return _sampleEnemy.GetIsFound();
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// set flag of _isEnemyDestroyed
    /// </summary>
    /// <returns></returns>
    public void SetIsEnemyDestroyed(object sender, EventArgs args)
    {
        _isEnemyDestroyed = true;
    }

    /// <summary>
    /// check whether the enemy is destroyed
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyDestroyed()
    {
        return _isEnemyDestroyed;
    }
}
