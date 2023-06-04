using System;
using UnityEngine;
using UnityEngine.Events;

public class HPContainer : MonoBehaviour
{
    public DeathType GetDeathType => deathType;
    
    public event UnityAction OnDeath;
    
    [Header("Base")]

    [SerializeField] private int startHp;
    [SerializeField] private int hp;
    [SerializeField] private bool destroySelfOnDeath;

    private DeathType deathType;
    private bool _deathHappened;

    private protected virtual void Start()
    {
        hp = startHp;
        OnDeath += () => _deathHappened = true;
    }

    public void GetDamage(int damage, DeathType damageDeathType)
    {
        hp -= damage;
        CheckForAlive(damageDeathType);
    }

    public void InstantKill(DeathType damageDeathType)
    {
        hp = 0;
        CheckForAlive(damageDeathType);
    }

    private protected void CheckForAlive(DeathType damageDeathType)
    {
        if (hp <= 0 && !_deathHappened)
        {
            deathType = damageDeathType;
            OnDeath?.Invoke();
            if(destroySelfOnDeath)
                Destroy(gameObject);
        }
    }
}
