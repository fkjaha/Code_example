using System;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public string GetInteractionTag => interactionTag;
    public bool InteractionsEnabled => _interactionsEnabled;
    public bool RequireHoldingBeforeInteract => requireHoldingBeforeInteract;
    public bool RequireHoldingToInteract => requireHoldingToInteract;
    public float RequiredHoldingTimeBeforeInteract => requiredHoldTimeBeforeInteract;
    
    [Header("Main")]
    [SerializeField] private protected string interactionTag;

    [Header("Holding Button")]
    [SerializeField] private protected bool requireHoldingBeforeInteract;
    [SerializeField] private protected float requiredHoldTimeBeforeInteract;
    [Space(20f)]
    [SerializeField] private protected bool requireHoldingToInteract;

    private bool _interactionsEnabled = true;
    
    public virtual void Interact()
    {
        
    }
    
    public virtual void Interact(GameObject client)
    {
        
    }

    public void EnableInteractions() => _interactionsEnabled = true;
    
    public void DisableInteractions() => _interactionsEnabled = false;
}
