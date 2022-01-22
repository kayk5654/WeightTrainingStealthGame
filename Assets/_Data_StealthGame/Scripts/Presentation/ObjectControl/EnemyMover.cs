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

    // delay factor of position and rotation
    private float _lerpFactor = 0.01f;

    // rotation lerp factor for "searching" state
    private float _searchLerpFactor = 0.01f;

    // rotation lerp factor for "attack" state
    private float _attackLerpFactor = 0.02f;


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

    /// <summary>
    /// switch lerp factor depending on the state of the enemy
    /// </summary>
    /// <param name="isSearching"></param>
    public void SetLerpFactorType(bool isSearching)
    {
        _lerpFactor = isSearching ? _searchLerpFactor : _attackLerpFactor;
    }
}
