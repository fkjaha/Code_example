using UnityEngine;

public abstract class Weapon<T> : Item<T>
{
    [Header("Weapon")]
    [SerializeField] private protected DeathType deathType;
}