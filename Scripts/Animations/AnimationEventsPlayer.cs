using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsPlayer : MonoBehaviour
{
    [SerializeField] private List<FXAnimationEvent> fxAnimationEvents;

    public void PlayFXWithName(string fxName)
    {
        FXAnimationEvent foundFx = fxAnimationEvents.Find(fx => fx.GetSearchName == fxName);
        if (foundFx == null)
        {
            Debug.LogError($"Unable to find FX with name: {fxName}");
            return;
        }
        foundFx.InvokeEvent();
    }
}

[Serializable]
public class FXAnimationEvent
{
    public string GetSearchName => searchName;

    [SerializeField] private string searchName;
    [SerializeField] private UnityEvent animationEvent;

    public void InvokeEvent()
    {
        animationEvent?.Invoke();
    }
}
