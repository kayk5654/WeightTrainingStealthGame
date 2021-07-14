using System.Collections;
using UnityEngine;
/// <summary>
/// base class of projectiles to be spawn by items of player and enemy
/// </summary>
public class ProjectileBase : MonoBehaviour
{
    // move direction for calculation
    protected Vector3 _moveDirection;

    protected Vector3 _spawnPosition;

    // a coroutine to delete this projectile when it lost attack target
    protected IEnumerator _destroySelfSequence;

    protected MaterialRevealHandler _materialRevealHandler;

    /// <summary>
    /// initialization
    /// </summary>
    protected virtual void Start()
    {

    }

    /// <summary>
    /// update transform of projectile
    /// </summary>
    protected virtual void Update()
    {
        // if attack target is loast and lost target sequence isn't started yet, start sequence
        StartLostTaretSeuqnce();

        // update transform of this projectile
        UpdateTransform();
    }

    /// <summary>
    /// detect collision on the scene object
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // kick revealing feature for testing
        _materialRevealHandler._revealOrigin = transform.position;
        _materialRevealHandler.TestRevealing();

        // destroy itself
        Destroy(this.gameObject);
    }

    /// <summary>
    /// initialize move direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    /// <summary>
    /// initialize spawn position
    /// </summary>
    /// <param name="position"></param>
    public void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    /// <summary>
    /// initialize reference of MaterialRevealHandler
    /// </summary>
    /// <param name="handler"></param>
    public void SetMaterialRevealHandler(MaterialRevealHandler handler)
    {
        _materialRevealHandler = handler;
    }

    /// <summary>
    /// update transform of this projectile
    /// </summary>
    protected virtual void UpdateTransform()
    {
        // if lost target sequence is already started, never calculate inside of update
        if (_destroySelfSequence != null) { return; }

        this.transform.position += _moveDirection * ItemConfig._projectileSpeed;
        this.transform.rotation *= Quaternion.FromToRotation(this.transform.forward, _moveDirection);
    }

    /// <summary>
    /// if attack target is loast and lost target sequence isn't started yet, start sequence
    /// </summary>
    protected void StartLostTaretSeuqnce()
    {
        if (Vector3.Distance(_spawnPosition, transform.position) > ItemConfig._maxProjectileDistance && _destroySelfSequence == null)
        {
            _destroySelfSequence = DestroySelfSequence();
            StartCoroutine(_destroySelfSequence);
            return;
        }
    }

    /// <summary>
    /// process to delete this projectile when it lost attack target
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DestroySelfSequence()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        
        // duration to fly without attack target
        float duration = 5f;

        // phase of this sequence
        float phase = 0f;

        _moveDirection = this.transform.forward;

        // make this fly for a period defined by duration
        while (phase < 1f)
        {
            phase += Time.deltaTime / duration;
            this.transform.position += _moveDirection * ItemConfig._projectileSpeed;
            yield return wait;
        }

        Destroy(this.gameObject);
    }
}
