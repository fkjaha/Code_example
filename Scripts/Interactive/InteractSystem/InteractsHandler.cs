using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractsHandler : MonoBehaviour
{
    [SerializeField] private List<InteractPair> interactivePairs;
    [Space(10f)]
    [SerializeField] private string tagThatMeansThatCanBeInteractedWithAnything;

    private InteractiveObject _lastInteractedObject;
    private string _lastItemTag;
    private InteractPair _lastPair;

    public void HandleInteraction(InteractiveObject interactiveObject, string currentItemTag = "")
    {
        if (_lastInteractedObject == interactiveObject && _lastItemTag == currentItemTag)
        {
            InteractObject(_lastInteractedObject, _lastItemTag, _lastPair);
            return;
        }

        // get suitable pairs
        List<InteractPair> possiblePairs =
            interactivePairs.Where(pair => pair.InteractiveObjectTag == interactiveObject.GetInteractionTag).ToList();
        // check if interaction can be passed
        foreach (InteractPair possiblePair in possiblePairs)
        {
            if (possiblePair.InteractByObjectWithTag == tagThatMeansThatCanBeInteractedWithAnything ||
                currentItemTag == possiblePair.InteractByObjectWithTag)
            {
                InteractObject(interactiveObject, currentItemTag, possiblePair);
                break;
            }
        }
    }

    private void InteractObject(InteractiveObject interactiveObject, string currentItemTag , InteractPair interactPair)
    {
        _lastInteractedObject = interactiveObject;
        _lastItemTag = currentItemTag;
        _lastPair = interactPair;
        if (interactPair.GetInteractionComponent != null)
        {
            interactPair.GetInteractionComponent.CallInteraction(interactiveObject);
            return;
        }
        interactiveObject.Interact();
    }
    
    [Serializable]
    private struct InteractPair
    {
        public string InteractiveObjectTag => interactiveObjectTag;
        public string InteractByObjectWithTag => interactByObjectWithTag;
        public InteractionComponent GetInteractionComponent => interactionComponent;
        
        [SerializeField] private string interactiveObjectTag;
        [SerializeField] private string interactByObjectWithTag;
        [SerializeField] private InteractionComponent interactionComponent;
    }
}

