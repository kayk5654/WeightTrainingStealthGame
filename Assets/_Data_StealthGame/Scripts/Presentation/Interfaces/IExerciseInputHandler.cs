using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// get exercise input
/// </summary>
public interface IExerciseInputHandler
{
    /// <summary>
    /// set current movement phase from ExerciseInput
    /// </summary>
    /// <param name="movementPhase"></param>
    void SetCurrentMovementPhase(MovementPhase movementPhase);

    /// <summary>
    /// initialization
    /// </summary>
    /// <param name="exerciseInputData"></param>
    void Init(ExerciseInputDataSet exerciseInputData);

    /// <summary>
    /// enable/disable this exercise input handler
    /// </summary>
    /// <param name="toEnable"></param>
    void SetEnabled(bool toEnable);

    /// <summary>
    /// return end of negative movement
    /// </summary>
    /// <returns></returns>
    bool IsNegativePeak();

    /// <summary>
    /// return start of positive movement
    /// </summary>
    /// <returns></returns>
    bool IsStartOfPositiveMove();

    /// <summary>
    /// return end of positive movement
    /// </summary>
    /// <returns></returns>

    bool IsEndOfPositivePeak();
}
