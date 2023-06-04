using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private List<ArmsAnimationSetup> setups;
    [SerializeField] private List<TaskAnimation> taskAnimations;
    [SerializeField] private List<UseItemAnimation> useItemAnimations;
    [SerializeField] private int overlayAnimationsAnimatorLayer;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private string idleAnimationName;
    [SerializeField] private string isMovingBoolName;
    [SerializeField] private string isRunningBoolName;

    private IEnumerator _overlayLayerActiveCoroutine;

    private void Start()
    {
        playerInventory.OnActiveSlotUpdated += AdjustWeaponHoldingAnimation;
    }

    private void AdjustWeaponHoldingAnimation()
    {
        SetArmAnimation(playerInventory.GetActiveWeapon == null ? null : playerInventory.GetActiveWeapon.GetHandHoldingType);
    }

    private void SetArmAnimation(HoldingItemType handHoldingType)
    {
        foreach (ArmsAnimationSetup armsAnimationSetup in setups)
        {
            if (armsAnimationSetup.GetHandHoldingType == handHoldingType)
            {
                EnableArmAnimationSetup(armsAnimationSetup);
            }
            else
            {
                DisableArmAnimationSetup(armsAnimationSetup);
            }
        }
    }

    private void EnableArmAnimationSetup(ArmsAnimationSetup armsAnimationSetup)
    {
        if(armsAnimationSetup.GetRig != null)
            armsAnimationSetup.GetRig.weight = 1;
        animator.SetLayerWeight(armsAnimationSetup.GetLayerIndex,  1);
    }
    
    private void DisableArmAnimationSetup(ArmsAnimationSetup armsAnimationSetup)
    {
        if(armsAnimationSetup.GetRig != null)
            armsAnimationSetup.GetRig.weight = 0;
        animator.SetLayerWeight(armsAnimationSetup.GetLayerIndex,  0);
    }

    public void PlayUseItemAnimation(Vector3 usePosition, UseItemAnimationType deathType, float time)
    {
        UseItemAnimation filteredAnimation = useItemAnimations.Find(animationPair => animationPair.GetSearchKey == deathType);
        if(filteredAnimation == null) return;
        PlayOverlayLayerAnimation(filteredAnimation.GetName, time);
    }
    
    public void PlayTaskPerformingAnimation(TaskInfo taskInfo)
    {
        TaskAnimation filteredAnimation = taskAnimations.Find(taskAnimation => taskAnimation.GetSearchKey == taskInfo);
        if(filteredAnimation == null) return;
        PlayOverlayLayerAnimation(filteredAnimation.GetName, taskInfo.GetPerformTime);
    }

    private void PlayOverlayLayerAnimation(string animationName, float animatingTime, float normalizedTime = 0)
    {
        SetOverlayLayerActiveForTime(animatingTime);
        animator.Play(animationName, overlayAnimationsAnimatorLayer, normalizedTime);
    }

    private void SetOverlayLayerActiveForTime(float time)
    {
        if(_overlayLayerActiveCoroutine != null)
            StopCoroutine(_overlayLayerActiveCoroutine);

        _overlayLayerActiveCoroutine = SetOverlayLayerActiveForTimeCoroutine(time);
        StartCoroutine(_overlayLayerActiveCoroutine);
    }

    private IEnumerator SetOverlayLayerActiveForTimeCoroutine(float time)
    {
        SetOverlayLayerActive(true);
        yield return new WaitForSeconds(time);
        SetOverlayLayerActive(false);
    }
    
    private void SetOverlayLayerActive(bool active)
    {
        animator.SetLayerWeight(overlayAnimationsAnimatorLayer, active ? 1 : 0);
    }

    public void ReturnToDefault()
    {
        animator.Play(idleAnimationName, default, 0);
    }

    private void Update()
    {
        animator.SetBool(isRunningBoolName, playerController.IsRunning);
        animator.SetBool(isMovingBoolName, playerController.IsMoving);
    }
}

[Serializable]
public class ArmsAnimationSetup
{
    public HoldingItemType GetHandHoldingType => handHoldingType;
    public int GetLayerIndex => layerIndex;
    public Rig GetRig => rig;
    public Transform GetVisualsParent => visualsParent;
    
    [SerializeField] private HoldingItemType handHoldingType;
    [Space(10f)]
    [SerializeField] private int layerIndex;
    [SerializeField] private Rig rig;
    [SerializeField] private Transform visualsParent;
}

[Serializable]
public class TaskAnimation : KeyNamePair<TaskInfo>
{
}

[Serializable]
public class ExecutionAnimation : KeyNamePair<DeathType>
{
}

[Serializable]
public class UseItemAnimation : KeyNamePair<UseItemAnimationType>
{
}
