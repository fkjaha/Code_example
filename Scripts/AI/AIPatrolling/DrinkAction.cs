using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DrinkAction : PathAction
{
    [SerializeField] private Transform drinkTransformPosition;
    [SerializeField] private string drinkAnimationName;
    [SerializeField] private float timeBeforeLeave;
    
    private protected override void InitializeActionsList(List<UnityAction<UnityAction>> actionsList, PathActionsEvents events)
    {
        actionsList.Add(onComplete => events.GoAction?.Invoke(drinkTransformPosition.position, onComplete));
        actionsList.Add(onComplete => events.AnimationAction?.Invoke(drinkAnimationName, onComplete));
        actionsList.Add(onComplete => events.WaitAction?.Invoke(timeBeforeLeave, onComplete));
    }
}
