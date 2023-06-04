using System.Collections;
using UnityEngine;

public class TestShootWeapon<T> : Weapon<T>
{
    [Header("Shooting Weapon")]
    [SerializeField] private SoundPresetBase shootClipPreset;
    [SerializeField] private Transform bulletShootDirectionTransform;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float shootRate;
    [SerializeField] private int bulletsPerMag;
    [SerializeField] private float reloadTime;

    private bool _isReloading;
    private bool _cooldownActive;
    private int _bulletsLeft;

    private void Start()
    {
        _bulletsLeft = bulletsPerMag;
    }

    private void TryToExecuteShoot(Transform target)
    {
        if (!_isReloading && !_cooldownActive)
        {
            if (BulletMagIsEmpty())
            {
                StartCoroutine(Reload());
                return;
            }
            ExecuteShoot(target);
        }
    }

    private bool BulletMagIsEmpty() => _bulletsLeft <= 0;

    private void ExecuteShoot(Transform target)
    {
        LaunchBullet(target);
        ConsumeBullet();
        TryPlayBulletSound();
        StartCoroutine(Cooldown());
    }

    private void LaunchBullet(Transform target)
    {
        Vector3 bulletShootDirectionPosition = bulletShootDirectionTransform.position;
        Bullet bullet = Instantiate(bulletPrefab, bulletShootDirectionPosition, Quaternion.identity);
        bullet.Initialize((target.position - bullet.transform.position).normalized.Vector3ToFlat());
    }
    
    private void ConsumeBullet()
    {
        _bulletsLeft--;
    }

    private void TryPlayBulletSound()
    {
        if(shootClipPreset != null)
            SoundsPlayer.Instance.PlaySoundOnly(shootClipPreset, transform.position);
    }

    private IEnumerator Cooldown()
    {
        _cooldownActive = true;
        
        yield return new WaitForSeconds(shootRate);
        
        _cooldownActive = false;
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        
        yield return new WaitForSeconds(reloadTime);
        
        _bulletsLeft = bulletsPerMag;
        _isReloading = false;
    }

    private protected override void UseItem(T target, Vector3 useWorldPosition = default)
    {
        if(target is Component targetAsComponent)
            TryToExecuteShoot(targetAsComponent.transform);
    }
}
