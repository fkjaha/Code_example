using UnityEngine;

public class TaserProjectile : Bullet
{
    // private protected override void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.TryGetComponent(out ConductorCollider conductorCollider))
    //     {
    //         conductorCollider.DamageTouching(10);
    //     }
    //     else if (other.gameObject.TryGetComponent(out HPContainer hpContainer))
    //     {
    //         hpContainer.GetDamage(10, deathType);
    //     }
    //     Death();
    // }
    //
    // private protected override void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.TryGetComponent(out ConductorCollider conductorCollider))
    //     {
    //         conductorCollider.DamageTouching(10);
    //     }
    //     else if (collision.gameObject.TryGetComponent(out HPContainer hpContainer))
    //     {
    //         hpContainer.GetDamage(10, deathType);
    //     }
    //     Death();
    // }
}
