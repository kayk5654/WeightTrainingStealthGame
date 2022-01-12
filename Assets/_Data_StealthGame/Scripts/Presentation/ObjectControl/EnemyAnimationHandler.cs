using UnityEngine;
/// <summary>
/// handle animation state of the enemies
/// </summary>
public class EnemyAnimationHandler
{
    // enemy's animator component
    private Animator _animator;

    // animation state for searching player's object
    private string _searchAnimProperty = "Search";

    // animation state for attacking player's object
    private string _attackAnimProperty = "Attack";


    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="animator"></param>
    public EnemyAnimationHandler(Animator animator)
    {
        _animator = animator;
    }
    
    /// <summary>
    /// set animation state for searching player's object
    /// </summary>
    public void SetSearch()
    {
        if (!_animator) { return; }
        _animator.SetTrigger(_searchAnimProperty);
    }

    /// <summary>
    /// set animation state for attacking player's object
    /// </summary>
    public void SetAttack()
    {
        if (!_animator) { return; }
        _animator.SetTrigger(_attackAnimProperty);
    }

    /// <summary>
    /// pause animation
    /// </summary>
    public void Pause()
    {
        _animator.speed = 0f;
    }

    /// <summary>
    /// resume animation
    /// </summary>
    public void Resume()
    {
        _animator.speed = 1f;
    }
}
