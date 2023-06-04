using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateTowardsAction : PathAction
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float waitTime;
    
    private protected override void InitializeActionsList(List<UnityAction<UnityAction>> actionsList, PathActionsEvents events)
    {
        actionsList.Add(onCompleted => events.RotationLookAction(lookTarget.position, onCompleted));
        actionsList.Add(onCompleted => events.WaitAction(waitTime, onCompleted));
    }
}
