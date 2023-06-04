using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIAnimator : MonoBehaviour
{
    [SerializeField] private protected AIWalkController aiWalkController;
    [SerializeField] private protected AISightProcessor aiSightProcessor;
    [SerializeField] private protected Animator animator;
    [SerializeField] private protected HPContainer hpContainer;
    [SerializeField] private protected Transform gfxTransform;

    [SerializeField] private string idleAnimName;
    [SerializeField] private string isMoving;
    [SerializeField] private string isRunning;
    [SerializeField] private string isInSight;
    [SerializeField] private List<DeathAnimation> deathAnimations;
    [SerializeField] private DeathAnimation defaultDeathAnimation;
    
    private bool _enabled;
    
    private void Start()
    {
        _enabled = true;
        hpContainer.OnDeath += () =>
        {
            _enabled = false;
            PlayDeathAnimation(hpContainer.GetDeathType);
        };
    }

    private void PlayDeathAnimation(DeathType deathType)
    {
        DeathAnimation deathAnimation = GetDeathAnimation(deathType) ?? defaultDeathAnimation;
        if(deathAnimation.GetHasRelativeExecutorYRotation)
            RotateAnimationTargetRelativeExecutor(deathAnimation.GetRelativeExecutorYRotation);
        if(deathAnimation.GetHasRelativeExecutorOffset)
            PlaceAnimationTargetRelatively(deathAnimation.GetRelativeExecutorOffset);
        animator.Play(deathAnimation.GetAnimationName);
    }

    private void RotateAnimationTargetRelativeExecutor(float degrees)
    {
        // aiWalkController.LookAtPosition(AISightTargets.Instance.GetMainTarget.position);
        Vector3 gfxPosition = gfxTransform.position;
        Vector3 testVector = AISightTargets.Instance.GetMainTarget.position - gfxPosition;
        testVector = Quaternion.AngleAxis(degrees, gfxTransform.up) * testVector;
        aiWalkController.LookAtPositionInstantly(gfxPosition + testVector);
    }

    private void PlaceAnimationTargetRelatively(Vector3 offset)
    {
        Vector3 testVector = (gfxTransform.position.Vector3ToFlat() - AISightTargets.Instance.GetMainTarget.position.Vector3ToFlat()).normalized;
        gfxTransform.position = gfxTransform.position.y * Vector3.up 
                                    + AISightTargets.Instance.GetMainTarget.position.Vector3ToFlat()
                                    + testVector * offset.x 
                                    + Quaternion.AngleAxis(90, Vector3.up) * testVector * offset.z;
    }

    public void ReturnToDefaultAnimationsCycle()
    {
        animator.Play(idleAnimName);
    }

    private DeathAnimation GetDeathAnimation(DeathType deathType)
    {
        return deathAnimations.Find(deathAnimation => deathAnimation.GetDeathType == deathType);
    }

    private void Update()
    {
        if(!_enabled) return;
        
        UpdateCycle();
    }

    private protected virtual void UpdateCycle()
    {
        animator.SetBool(isMoving, aiWalkController.GetVelocity != Vector3.zero);
        animator.SetBool(isRunning, aiWalkController.IsRunning);
        animator.SetBool(isInSight, aiSightProcessor.GetMainTargetInSightState);
    }

    public void PlayAnimation(string animationName, UnityAction onCompleted = null)
    {
        Debug.Log("Animation played: " + animationName);
        animator.Play(animationName);
        onCompleted?.Invoke();
    }
}

[Serializable]
public class DeathAnimation
{
    public string GetAnimationName => animationName;
    public DeathType GetDeathType => deathType;
    public bool GetHasRelativeExecutorYRotation => hasRelativeExecutorYRotation;
    public float GetRelativeExecutorYRotation => relativeExecutorYRotation;
    public bool GetHasRelativeExecutorOffset => hasRelativeExecutorOffset;
    public Vector3 GetRelativeExecutorOffset => relativeExecutorOffset;
    
    [SerializeField] private string animationName;
    [SerializeField] private DeathType deathType;
    [SerializeField] private bool hasRelativeExecutorYRotation;
    [SerializeField] private float relativeExecutorYRotation;
    [SerializeField] private bool hasRelativeExecutorOffset;
    [SerializeField] private Vector3 relativeExecutorOffset;
}