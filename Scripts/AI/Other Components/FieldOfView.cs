using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    public event UnityAction OnDestroyEvent; 

    public Transform GetEyesTransform => eyeTransform;
    public float GetViewAngle => viewAngle;
    public float GetViewDistance => viewDistance;
    
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewHeightLimit;

    public bool IsInView(Transform target)
    {
        Vector3 targetPosition = target.position;
        
        if (Vector3.Distance(targetPosition.Vector3ToFlat(), eyeTransform.position.Vector3ToFlat()) > viewDistance) return false;
        
        if (Vector3.Angle((targetPosition - eyeTransform.position).Vector3ToFlat(),
                (eyeTransform.forward * viewDistance).Vector3ToFlat()) > viewAngle / 2) return false;

        if (targetPosition.y > eyeTransform.position.y + viewHeightLimit ||
            targetPosition.y < eyeTransform.position.y - viewHeightLimit) return false;

        if (!Physics.Raycast(eyeTransform.position, targetPosition - eyeTransform.position, out RaycastHit hit,
                Mathf.Infinity, raycastMask)) return false;

        if (hit.collider.transform != target) return false;
        
        return true;
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }
}
