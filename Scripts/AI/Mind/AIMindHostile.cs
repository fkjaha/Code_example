using UnityEngine;

public class AIMindHostile : AIMindBase
{
    [Space(20f)]
    [SerializeField] private ItemHolderSlot itemHolderSlot;

    private protected override void InSightAndConcerned()
    {
        base.InSightAndConcerned();
        AttackTarget();
    }

    private void AttackTarget()
    {
        itemHolderSlot.TryToUse(AISightTargets.Instance.GetMainTarget.gameObject);
    }
}
