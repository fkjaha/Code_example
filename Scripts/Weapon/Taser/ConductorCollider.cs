using System;
using System.Collections.Generic;
using UnityEngine;

public class ConductorCollider : MonoBehaviour, ITaserExecutable
{
    [SerializeField] private ParticleSystem electricityEffect;
    
    private List<ITaserExecutable> _touchingHpContainers;

    private void Start()
    {
        _touchingHpContainers = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITaserExecutable hpContainer))
        {
            _touchingHpContainers.Add(hpContainer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITaserExecutable hpContainer))
        {
            _touchingHpContainers.Remove(hpContainer);
        }
    }

    private void DamageTouching(DeathType deathType, float time)
    {
        foreach (ITaserExecutable touchingHpContainer in _touchingHpContainers)
        {
            if(touchingHpContainer == null) continue;
            touchingHpContainer.GetExecuted(deathType, time);
        }
    }

    private void OnConduct()
    {
        PlayParticles();
    }

    private void PlayParticles()
    {
        if(electricityEffect != null)
            electricityEffect.Play();
    }
    
    public void GetExecuted(DeathType deathType, float time)
    {
        OnConduct();
        DamageTouching(deathType, time);
    }
}
