using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StayPathAction : PathAction
{
    [SerializeField] private float stayTime;

    private protected override void InitializeActionsList(List<UnityAction<UnityAction>> actionsList, PathActionsEvents events)
    {
        actionsList.Add(onCompleted => events.WaitAction(stayTime, onCompleted));
    }
}
