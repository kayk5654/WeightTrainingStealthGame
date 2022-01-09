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

    // store velocity in the last frame to smooth out the movement
    private Vector3 _lastVelocity;

    // delay factor of rotation; original value is 0.005f
    private float _lerpFactor = 0.01f;


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
        _moveDirection = Vector3.Lerp(_lastVelocity, _moveDirection * _baseSpeed + force, _lerpFactor);
        _moveTransform.position += _moveDirection;
        _moveTransform.rotation = Quaternion.Lerp(_moveTransform.rotation, Quaternion.FromToRotation(_moveTransform.forward, _moveDirection) * _moveTransform.rotation, _lerpFactor);

        _lastVelocity = _moveDirection;
    }
}
