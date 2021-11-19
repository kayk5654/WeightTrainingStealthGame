using System;
using UnityEngine;
/// <summary>
/// end gameplay in the AfterPlay phase, then go back to the main menu
/// </summary>
public class GamePlayEndHandler : MonoBehaviour, IGamePlayStateSetter
{
    // event to notify the start of GamePlay phase
    public event EventHandler<GamePlayStateEventArgs> _onGamePlayStateChange;
 
   
    /// <summary>
    /// update the gameplay state from the classes refer this
    /// </summary>
    /// <param name="gamePlayState"></param>
    public void SetGamePlayState(GamePlayState gamePlayState)
    {

    }

    /// <summary>
    /// end gameplay and baok to the main menu
    /// </summary>
    public void EndGamePlay()
    {
        GamePlayStateEventArgs args = new GamePlayStateEventArgs(GamePlayState.None);
        _onGamePlayStateChange?.Invoke(this, args);
    }
}
