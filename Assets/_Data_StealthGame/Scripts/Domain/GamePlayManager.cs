using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// manages features during the gameplay except ui
/// </summary>
public class GamePlayManager : IGamePlayStateManager
{
    // event to notify the start of GamePlay phse
    public EventHandler<AppStateEventArgs> _onStartGamePlayState { get; }

    public void EnableGamePlay()
    {

    }

    public void DisableGamePlay()
    {

    }
}
