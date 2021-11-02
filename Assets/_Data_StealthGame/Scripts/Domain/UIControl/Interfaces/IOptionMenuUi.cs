using System;
/// <summary>
/// control option menu in the scene during gameplay
/// </summary>
public interface IOptionMenuUi
{
    // event called when the gameplay is paused
    event EventHandler _onPause;

    // event called when the gameplay is resumed
    event EventHandler _onResume;

    // event called when the gameplay is terminated and go back to the main menu
    event EventHandler _onBackToMenu;

    // enalble option menu ui
    void EnableUi();
}
