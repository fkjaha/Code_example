using UnityEngine;

public class HostileAIAnimator : AIAnimator
{
    [Header("Hostile")]
    [SerializeField] private int aimingLayerIndex;

    private protected override void UpdateCycle()
    {
        base.UpdateCycle();
        animator.SetLayerWeight(aimingLayerIndex, aiSightProcessor.GetMainTargetInSightState ? 1 : 0);
    }
}
