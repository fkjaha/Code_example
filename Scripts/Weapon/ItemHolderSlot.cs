using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemHolderSlot : MonoBehaviour
{
    public event UnityAction OnSlotUpdated;
    
    public bool SlotFilled => _currentItem != null;
    public BaseItem GetItem => _currentItem;

    [SerializeField] private protected Transform gunParent;
    [SerializeField] private protected Transform gunVisualsParent;
    [SerializeField] private LayerMask useWayObstaclesLayers;
    [SerializeField] private protected Vector3 gunVisualsParentOffset;
    [SerializeField] private protected Vector3 gunVisualsParentRotation;

    private protected BaseItem _currentItem;
    private bool _currentState;
    
    public bool HoldsThisItem(BaseItem item)
    {
        return _currentItem == item;
    }
    
    public void SetActive(bool newState)
    {
        _currentState = newState;
        gunParent.gameObject.SetActive(newState);
        
        if(_currentItem != null)
            _currentItem.SetVisualizationState(newState);
        UpdateSlot();
    }

    public void SetItem(BaseItem item)
    {
        ReleaseCurrentItem();
        item.DisableObjectFunctions();
        item.SetVisualsParent(gunVisualsParent, gunVisualsParentOffset, gunVisualsParentRotation);
        Transform itemTransform = item.transform;
        itemTransform.parent = gunParent;
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localEulerAngles = Vector3.zero;
        _currentItem = item;
        _currentItem.OnItemDestroyed += OnCurrentItemDestroyed;
        _currentItem.SetVisualizationState(_currentState);
        
        UpdateSlot();
    }

    private void ReleaseCurrentItem()
    {
        if(_currentItem == null) return;
        
        _currentItem.EnableObjectFunctions();
        _currentItem.ResetVisualsParent();
        _currentItem.transform.parent = null;
        _currentItem.OnItemDestroyed -= OnCurrentItemDestroyed;
        NullCurrentItem();
        UpdateSlot();
    }
    
    private void OnCurrentItemDestroyed()
    {
        NullCurrentItem();
        UpdateSlot();
    }

    private void NullCurrentItem()
    {
        _currentItem = null;
    }

    public bool TryToUse(GameObject useTarget, Vector3 useWorldPosition = default)
    {
        if(!ItemUsePossible(useTarget, useWorldPosition)) return false;

        return _currentItem.TryUse(useTarget, useWorldPosition);
    }

    private protected virtual bool ItemUsePossible(GameObject useTarget, Vector3 useWorldPosition)
    {
        if(_currentItem == null) return false;
        if(_currentItem.GetRequiresClearWayToUse && !UseWayIsClear(useWorldPosition)) return false;
        return true;
    }

    private bool UseWayIsClear(Vector3 usePosition)
    {
        Vector3 transformPosition = transform.position;
        Vector3 useWayVector = usePosition - transformPosition;
        return !Physics.Raycast(transformPosition, useWayVector, useWayVector.magnitude, useWayObstaclesLayers);
    }

    private void UpdateSlot()
    {
        OnSlotUpdated?.Invoke();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if(_currentItem == null) return;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _currentItem.GetUseRange);
#endif
    }
}
