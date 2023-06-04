using DG.Tweening;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{
    [SerializeField] private Ease ease;
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private float moveTime;

    [ContextMenu("Rotate")]
    public void Rotate()
    {
        rotateTarget.DOLocalRotate(Vector3.up * 180, moveTime).SetEase(ease).onComplete += () =>
            rotateTarget.DOLocalRotate(Vector3.up * 360, moveTime).SetEase(ease);
    }
}
