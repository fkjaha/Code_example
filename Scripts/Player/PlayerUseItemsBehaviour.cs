using System.Collections;
using UnityEngine;

public class PlayerUseItemsBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimator playerAnimator;

    private IEnumerator _behaviourExecutionCoroutine;
    private bool _executing;

    private void Awake()
    {
        playerInventory.OnItemUsed += OnItemUsedBehaviour;
    }

    private void OnItemUsedBehaviour(BaseItem usedItem, Transform useTarget, Vector3 inputWorldPosition)
    {
        StartBehaviour(
            usedItem.GetUseWorldPointAsAnimationAnchor ? inputWorldPosition : useTarget.position,
            usedItem.GetUseTime,
            usedItem.GetItemAnimationType);
    }

    private void StartBehaviour(Vector3 anchorPosition, float time, UseItemAnimationType useItemAnimationType)
    {
        _executing = true;
        playerController.Disable(this);
        playerController.RotateInDirection(anchorPosition - playerController.GetPosition);
        playerAnimator.PlayUseItemAnimation(anchorPosition, useItemAnimationType, time);

        StartBehaviourExecutionCoroutine(time);
    }

    private void StartBehaviourExecutionCoroutine(float time)
    {
        CancelBehaviourExecutionCoroutine();
        _behaviourExecutionCoroutine = BehaviourExecution(time);
        StartCoroutine(_behaviourExecutionCoroutine);
    }

    private void FinishBehaviour(bool endByCancellation = false)
    {
        if (!_executing) return;

        _executing = false;
        playerController.Enable(this);
        if (endByCancellation)
            playerAnimator.ReturnToDefault();
    }

    private void CancelBehaviourExecutionCoroutine()
    {
        if (_behaviourExecutionCoroutine != null)
            StopCoroutine(_behaviourExecutionCoroutine);
    }

    private IEnumerator BehaviourExecution(float time)
    {
        yield return new WaitForSeconds(time);
        FinishBehaviour();
    }
}