using System.Collections;
using UnityEngine;

public class TestKnife : ExecutionWeapon<IMeleeExecutable>
{
    private IEnumerator _attackCoroutine;
    private AIExecution _lastAiExecution;

    private protected override void UseItem(IMeleeExecutable target, Vector3 useWorldPosition = default)
    {
        target.GetExecuted(deathType, GetExecutionTime());
    }
}
