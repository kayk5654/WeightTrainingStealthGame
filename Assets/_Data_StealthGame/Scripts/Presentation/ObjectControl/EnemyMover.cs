using UnityEngine;
/// <summary>
/// move enemy object
/// </summary>
public class EnemyMover
{
    // base speed of the enemy
    private float _baseSpeed;
    private Transform _moveTransform;

    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="enemyTransform"></param>
    public EnemyMover(float speed, Transform enemyTransform)
    {
        _baseSpeed = speed;
        _moveTransform = enemyTransform;
    }

    /// <summary>
    /// move enemy transform
    /// </summary>
    /// <param name="target"></param>
    public void Move(Vector3 target)
    {
        _moveTransform.position += Vector3.Normalize(target - _moveTransform.position) * _baseSpeed;
    }
}
