using System.Collections;
using System.Collections.Generic;
/// <summary>
/// manage player's actions during the gameplay
/// </summary>
public class PlayerActionManager
{
    // root class of the gameplay
    private GamePlayManager _gamePlayManager;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="gamePlayManager"></param>
    public PlayerActionManager(GamePlayManager gamePlayManager)
    {
        _gamePlayManager = gamePlayManager;
    }
}
