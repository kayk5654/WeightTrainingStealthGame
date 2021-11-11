using UnityEngine;
/// <summary>
/// move enemy object
/// </summary>
public class EnemyMover
{
    // base speed of the enemy
    private float _baseSpeed;

    // transform to move
    private Transform _moveTransform;

    // store world space move direction temporarily
    private Vector3 _moveDirection;

    // delay factor of rotation
    private float _rotationLerpFactor = 0.005f;


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
    public void Move(Vector3 target, Vector3 force)
    {
        _moveDirection = Vector3.Normalize(target - _moveTransform.position);
        _moveTransform.position += _moveDirection * _baseSpeed;
        _moveTransform.rotation = Quaternion.Lerp(_moveTransform.rotation, Quaternion.FromToRotation(_moveTransform.forward, _moveDirection) * _moveTransform.rotation, _rotationLerpFactor);
    }
}
