using UnityEngine;

public class PlayerCollider : MonoBehaviour, IGunDamagable, IMeeleDamagable, IGasDamagable
{
    [SerializeField] private HPContainer hpContainer;

    public void GetDamage(int damage, DeathType deathType)
    {
        hpContainer.GetDamage(damage, deathType);
    }
}
