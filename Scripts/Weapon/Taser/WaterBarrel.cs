using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class WaterBarrel : InteractiveObject
{
    [Header("Barrel")]
    [SerializeField] private Liquid liquidPrefab;
    [SerializeField] private Transform rotationObject;
    [SerializeField] private Vector3 fallLocalRotation;
    [SerializeField] private Transform spillTransform;
    [SerializeField] private float animationTime;
    [SerializeField] private Ease fallingEase;
    
    public override void Interact()
    {
        Push();
    }

    private void Push()
    {
        AnimateBarrel(SpillLiquid);
        DisableInteractions();
    }

    private void AnimateBarrel(UnityAction onCompleted = null)
    {
        rotationObject.DOLocalRotate(fallLocalRotation, animationTime).SetEase(fallingEase).onComplete += () =>
        onCompleted?.Invoke();
    }
    
    private void SpillLiquid()
    {
        Liquid liquid = Instantiate(liquidPrefab, spillTransform.position, quaternion.identity, spillTransform);
        liquid.Spill();
    }
}
