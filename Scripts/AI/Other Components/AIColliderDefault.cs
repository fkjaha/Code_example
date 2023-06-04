using UnityEngine;

public class AIColliderDefault : AIColliderBase, IGasDamagable, IMeeleDamagable, IGunDamagable, ITaserExecutable, IMeleeExecutable
{
    [Header("Mod: Default")]
    [SerializeField] private HPContainer hpContainer;
    [SerializeField] private AIExecution aiExecution;

    public void GetExecuted(DeathType deathType, float time)
    {
        aiExecution.Execute(deathType, time);
    }

    public void GetDamage(int damage, DeathType deathType)
    {
        hpContainer.GetDamage(damage, deathType);
    }
}
