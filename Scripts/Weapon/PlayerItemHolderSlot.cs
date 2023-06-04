using UnityEngine;

public class PlayerItemHolderSlot : ItemHolderSlot
{
    private protected override bool ItemUsePossible(GameObject useTarget, Vector3 useWorldPosition)
    {
        if (!base.ItemUsePossible(useTarget, useWorldPosition)) return false;
        if(!gameObject.activeSelf || _currentItem == null) return false;
        if(!TargetInUseRange(useTarget.transform)) return false;
        return true;
    }

    private bool TargetInUseRange(Transform targetTransform)
    {
        return StaticFunctions.FlatDistance(targetTransform.position, transform.position) <= _currentItem.GetUseRange;
    }
}
