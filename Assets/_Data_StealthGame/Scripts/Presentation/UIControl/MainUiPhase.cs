using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// control main menu ui phase
/// </summary>
public class MainUiPhase : MonoBehaviour, IUiPhase
{

    // notify the action to move forward phase
    public event EventHandler _onMoveForward;

    // notify the action to move backward phase
    public event EventHandler _onMoveBackward;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// display this ui phase
    /// </summary>
    public void Display()
    {

    }

    /// <summary>
    /// hide this ui phase
    /// </summary>
    public void Hide()
    {

    }

    /// <summary>
    /// execute process to go to the next phase from a button
    /// </summary>
    public void MoveForward()
    {

    }

    /// <summary>
    /// execute process o go back to the previous phase from a button
    /// </summary>
    public void MoveBackward()
    {

    }
}
