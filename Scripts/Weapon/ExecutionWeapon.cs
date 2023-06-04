
using UnityEngine;

public abstract class ExecutionWeapon<T> : Weapon<T>, IExecutionWeapon
{
    [Header("Execution Weapon")]
    [SerializeField] private float executionTime;
    
    public float GetExecutionTime() => executionTime;
    public DeathType GetExecutionDeathType() => deathType;
}
