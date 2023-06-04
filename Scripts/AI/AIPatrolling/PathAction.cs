using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PathAction : MonoBehaviour
{
    public bool ActionGotAChance => Random.Range(0, 101) <= actionChance;
    [SerializeField] private int actionChance = 100;
    
    private protected abstract void InitializeActionsList(List<UnityAction<UnityAction>> actionsList, PathActionsEvents events);

    public IEnumerator StartActions(PathActionsEvents events, UnityAction onCompleted = null)
    {
        List<UnityAction<UnityAction>> localActionsList = new();
        InitializeActionsList(localActionsList, events);
        bool actionCompleted = false;
        foreach (UnityAction<UnityAction> action in localActionsList)
        {
            actionCompleted = false;
            action.Invoke(() => actionCompleted = true);
            while (!actionCompleted)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        onCompleted?.Invoke();
    }
}
