using System.Collections;
using UnityEngine;

public abstract class ACooldown : MonoBehaviour
{
    private protected bool GetCooldownActive => _cooldownActive;

    [SerializeField] private float cooldown;
    
    private bool _cooldownActive;

    private protected IEnumerator ActivateCooldown(float cooldownTime)
    {
        _cooldownActive = true;
        yield return new WaitForSeconds(cooldownTime);
        _cooldownActive = false;
    }
}
