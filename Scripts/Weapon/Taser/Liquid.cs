using DG.Tweening;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float spillTime;
    [SerializeField] private Vector3 startSize;
    [SerializeField] private Vector3 spillSize;
    
    public void Spill()
    {
        targetTransform.localScale = startSize;
        targetTransform.DOScale(spillSize, spillTime);
    }
}
