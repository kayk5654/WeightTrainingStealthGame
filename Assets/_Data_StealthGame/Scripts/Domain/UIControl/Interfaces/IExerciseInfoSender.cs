﻿using System;
/// <summary>
/// set information of the selected exercise for the gameplay
/// </summary>
public interface IExerciseInfoSender
{
    // notify information about the selected exercise
    event EventHandler<ExerciseInfoEventArgs> _onExerciseSelected;
}
