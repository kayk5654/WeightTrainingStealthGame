using System;
/// <summary>
/// control option menu in the scene during gameplay
/// </summary>
public interface IOptionMenuUi
{
    // event called when the gameplay is paused/resumed/terminated
    event EventHandler<GamePlayStateEventArgs> _onGameplayStateChange;

    // enalble option menu ui
    void EnableUi(bool state);
}
