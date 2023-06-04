using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private protected DeathType deathType;
    [SerializeField] private Rigidbody bulletRigidbody;
    [SerializeField] private float lifetime;
    [SerializeField] private float speed;
    
    private Vector3 _moveDirection;
    
    public void Initialize(Vector3 direction)
    {
        StartCoroutine(AutoDeath());
        _moveDirection = direction;
        bulletRigidbody.velocity = _moveDirection * speed;
    }

    private IEnumerator AutoDeath()
    {
        yield return new WaitForSeconds(lifetime);
        Death();
    }

    private protected virtual void OnTriggerEnter(Collider other)
    {
        AcceptCollision(other.gameObject);
    }

    private protected virtual void OnCollisionEnter(Collision collision)
    {
        AcceptCollision(collision.gameObject);
    }

    private void AcceptCollision(GameObject collisionObject)
    {
        if (collisionObject.TryGetComponent(out IGunDamagable gunDamageable))
        {
            gunDamageable.GetDamage(10, deathType);
        }
        Death();
    }

    private protected void Death()
    {
        Destroy(gameObject);
    }
}
