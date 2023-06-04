using System.Collections;
using UnityEngine;

public abstract class Item<T> : BaseItem
{
    [Header("Player Usable Item")]
    [SerializeField] private float timeBeforeActualUse;
    [SerializeField] private SoundPresetBase useSound;

    private bool _isBeingUsed;
    
    public override bool TryUse(GameObject useTarget, Vector3 useWorldPosition = default)
    {
        if (_isBeingUsed) return false;
        if (useTarget.TryGetComponent(out T tTarget))
        {
            // UseItem(tTarget, useWorldPosition);
            StartCoroutine(UseItemProcess(tTarget, useWorldPosition));
            return true;
        }
        return false;
    }

    private IEnumerator UseItemProcess(T target, Vector3 useWorldPosition = default)
    {
        _isBeingUsed = true;
        yield return new WaitForSeconds(timeBeforeActualUse);
        UseItem(target, useWorldPosition);
        SoundsPlayer.Instance.PlaySoundOnly(useSound, transform.position);
        yield return new WaitForSeconds(Mathf.Clamp(GetUseTime - timeBeforeActualUse, 0.01f, Mathf.Infinity));
        _isBeingUsed = false;
        if(GetIsConsumable) Destroy(gameObject);
    }

    private protected abstract void UseItem(T target, Vector3 useWorldPosition = default);
}
