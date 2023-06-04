using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractiveObject))]
public abstract class BaseItem : MonoBehaviour
{
    public event UnityAction OnItemDestroyed;
    
    public float GetUseSoundForce => useSoundForce;
    public HoldingItemType GetHandHoldingType => handHoldingType;
    public UseItemAnimationType GetItemAnimationType => useItemAnimationType;
    public InteractiveObject InteractiveObject => _interactiveObject;
    public float GetUseRange => useRange;
    public bool GetCanBeUsedOnSelf => canBeUsedOnSelf;
    public bool GetIsConsumable => isConsumable;
    public bool GetUseWorldPointAsAnimationAnchor => useWorldPointAsAnimationAnchor;
    public float GetUseTime => useTime;
    public bool GetRequiresClearWayToUse => requiresClearWayToUse;


    [Header("Base Item")]
    [SerializeField] private PhysicalItem physicalItem;
    [SerializeField] private float useRange;
    [SerializeField] private bool canBeUsedOnSelf;
    [SerializeField] private bool isConsumable;
    [SerializeField] private bool useWorldPointAsAnimationAnchor;
    [SerializeField] private bool requiresClearWayToUse;
    [Space(15)]
    [SerializeField] private protected Transform visualsTransform;
    [SerializeField] private Vector3 visualsDefaultPosition;
    [Space(15)]
    [SerializeField] private float useSoundForce;
    [SerializeField] private float useTime;
    [Space(15)]
    [SerializeField] private HoldingItemType handHoldingType;
    [SerializeField] private UseItemAnimationType useItemAnimationType;
    [SerializeField] private UnityEvent onUsed;
    
    private InteractiveObject _interactiveObject;

    public void OnUsed()
    {
        onUsed.Invoke();
    }

    private void OnDestroy()
    {
        if(visualsTransform != null)
            Destroy(visualsTransform.gameObject);
        OnItemDestroyed?.Invoke();
    }

    private void Awake()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
    }
    
    public void DisableObjectFunctions()
    {
        physicalItem.DisablePhysicalPart();
    }
    
    public void EnableObjectFunctions()
    {
        physicalItem.EnablePhysicalPart();
    }
    
    public void SetVisualsParent(Transform parent, Vector3 offset, Vector3 rotation)
    {
        if(visualsTransform == null) return;
        
        visualsTransform.parent = parent;
        visualsTransform.localPosition = offset;
        visualsTransform.localRotation = Quaternion.Euler(rotation);
    }

    public void ResetVisualsParent()
    {
        SetVisualsParent(transform, visualsDefaultPosition, Vector3.zero);
    }
    
    public void SetVisualizationState(bool newState)
    {
        if(visualsTransform == null) return;
        visualsTransform.gameObject.SetActive(newState);
    }
    
    public abstract bool TryUse(GameObject useTarget, Vector3 useWorldPosition = default);
}