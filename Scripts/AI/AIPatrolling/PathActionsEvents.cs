using UnityEngine;
using UnityEngine.Events;

public class PathActionsEvents
{
    public UnityAction<Vector3, UnityAction> GoAction { get; }
    public UnityAction<string, UnityAction> AnimationAction { get; }
    public UnityAction<float, UnityAction> WaitAction { get; }
    // public UnityAction<Vector3, UnityAction> LookAction { get; }
    public UnityAction<Vector3, UnityAction> RotationLookAction { get; }

    public PathActionsEvents(
        UnityAction<Vector3, UnityAction> goAction = null,
        UnityAction<string, UnityAction> animationAction = null,
        UnityAction<float, UnityAction> waitAction = null,
        // UnityAction<Vector3, UnityAction> lookAction = null,
        UnityAction<Vector3, UnityAction> rotationLookAction = null)
    {
        GoAction = goAction;
        AnimationAction = animationAction;
        WaitAction = waitAction;
        // LookAction = lookAction;
        RotationLookAction = rotationLookAction;
    }
}
