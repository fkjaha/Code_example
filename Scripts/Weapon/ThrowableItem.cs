using DG.Tweening;
using UnityEngine;

public class ThrowableItem : Item<Transform>
{
    [Header("Throwable Item")]
    [SerializeField] private GameObject projectilePrefab;
    [Space(10f)]
    [SerializeField] private float flyTime;
    [SerializeField] private float jumpPower;
    [SerializeField] private AnimationCurve flyEase;
    [Space(10f)]
    [SerializeField] private SoundPresetBase landSound;
    [SerializeField] private float landSoundForce;
    [Space(10f)] 
    [SerializeField] private bool breakOnLand;
    [SerializeField] private ParticleSystem breakingParticleSystem;
    
    private protected override void UseItem(Transform target, Vector3 useWorldPosition = default)
    {
        LaunchItem(useWorldPosition);
    }

    private void LaunchItem(Vector3 targetPosition)
    {
        Transform projectile = Instantiate(projectilePrefab, visualsTransform.position, Quaternion.identity).transform;
        projectile.DOJump(targetPosition, jumpPower, 1, flyTime).SetEase(flyEase).onComplete += () => OnProjectileReachedTargetPosition(projectile);
    }

    private void OnProjectileReachedTargetPosition(Transform projectile)
    {
        PlayLandSound(projectile.position);
        if (breakOnLand) 
            BreakProjectile(projectile);
    }

    private void PlayLandSound(Vector3 landPosition)
    {
        SoundsPlayer.Instance.PlaySoundAndAudibleSound(landPosition, landSound, landSoundForce);
    }

    private void BreakProjectile(Transform projectile)
    {
        Destroy(projectile.gameObject);
        if (breakingParticleSystem == null)
        {
            Debug.LogWarning($"Breaking particle system wasn't assigned in throwable object ({name})", this);
            return;
        }
        ParticleSystem psInstance = Instantiate(breakingParticleSystem, projectile.position, Quaternion.identity);
        psInstance.Play(true);
    }
}
