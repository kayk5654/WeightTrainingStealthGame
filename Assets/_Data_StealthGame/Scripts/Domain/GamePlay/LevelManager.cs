using System.Collections;
using System.Collections.Generic;
/// <summary>
/// manage levels of the gameplay
/// </summary>
public class LevelManager
{
    // root class of the gameplay
    private GamePlayManager _gamePlayManager;
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="gamePlayManager"></param>
    public LevelManager (GamePlayManager gamePlayManager)
    {
        _gamePlayManager = gamePlayManager;
    }
}
