using UnityEngine;
/// <summary>
/// handle animation state of the enemies
/// </summary>
public class EnemyAnimationHandler
{
    // enemy's animator component
    private Animator _animator;


    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="animator"></param>
    public EnemyAnimationHandler(Animator animator)
    {
        _animator = animator;
    }

    
}
