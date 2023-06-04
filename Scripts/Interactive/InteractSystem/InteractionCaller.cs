using System;
using System.Collections;
using UnityEngine;

public class InteractionCaller : MonoBehaviour
{
    [SerializeField] private InteractionVisualizer interactionVisualizer;
    [SerializeField] private string holdingInputName;
    [SerializeField] private string sceneInputName;

    [Space(30f)] 
    [SerializeField] private InteractsHandler handler;
    [SerializeField] private PlayerInventory inventory;

    private bool _holdingButton;

    private void Start()
    {
        InputDetector.Instance.AddInputListener(holdingInputName, 
            () => _holdingButton = true, 
            () => _holdingButton = false);
        SceneInputDetector.Instance.AddSceneInputListener<InteractiveObject>(sceneInputName, TryCallInteraction);
    }

    private void TryCallInteraction(InteractiveObject interactiveObject)
    {
        if(!interactiveObject.InteractionsEnabled) return;

        if (interactiveObject.RequireHoldingBeforeInteract)
            StartCoroutine(CallHandleInteractionAfterTime(interactiveObject));
        else
            CallAppropriateInteractMethod(interactiveObject);
    }

    private IEnumerator CallHandleInteractionAfterTime(InteractiveObject interactiveObject)
    {
        float holdingTime = 0;
        
        if(interactionVisualizer != null)
            interactionVisualizer.UpdateHoldingIndicatorImage(true);
        
        float requiredHoldingTime = interactiveObject.RequiredHoldingTimeBeforeInteract;
        
        while (_holdingButton && holdingTime < requiredHoldingTime)
        {
            holdingTime += Time.deltaTime;
            if(interactionVisualizer != null)
                interactionVisualizer.UpdateHoldingIndicatorImage(holdingTime/requiredHoldingTime);
            yield return new WaitForEndOfFrame();
        }
        
        if(interactionVisualizer != null)
            interactionVisualizer.UpdateHoldingIndicatorImage(false);

        if(holdingTime >= requiredHoldingTime) CallAppropriateInteractMethod(interactiveObject);
    }
    
    private IEnumerator CallHandleInteractionWhileHolding(InteractiveObject interactiveObject)
    {
        while (_holdingButton)
        {
            CallInteractionHandling(interactiveObject);
            yield return new WaitForEndOfFrame();
        }
    }

    private void CallAppropriateInteractMethod(InteractiveObject interactiveObject)
    {
        if (interactiveObject.RequireHoldingToInteract)
            StartCoroutine(CallHandleInteractionWhileHolding(interactiveObject));
        else 
            CallInteractionHandling(interactiveObject);
    }

    private void CallInteractionHandling(InteractiveObject interactiveObject)
    {
        if (inventory.TryGetCurrentItemInteractByObjectTag(out string interactTag))
        {
            handler.HandleInteraction(interactiveObject, interactTag);
            // inventory.OnCurrentItemUsed();
        }
        else
            handler.HandleInteraction(interactiveObject);
    }
}
