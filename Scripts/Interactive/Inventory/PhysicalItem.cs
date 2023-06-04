using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalItem : MonoBehaviour
{
    [SerializeField] private Rigidbody itemRigidbody;
    [SerializeField] private List<Collider> colliders;

    private GameObject _selfObject;
    private Transform _selfTransform;

    private void Start()
    {
        _selfObject = gameObject;
        _selfTransform = transform;
    }

    public void DisablePhysicalPart()
    {
        SetCollidersState(false);
        SetRigidbodyState(false);
        // SetVisualState(false);
    }
    
    public void EnablePhysicalPart()
    {
        SetCollidersState(true);
        SetRigidbodyState(true);
        // SetVisualState(true);
    }

    private void SetCollidersState(bool enabledState)
    {
        foreach (Collider itemCollider in colliders)
        {
            itemCollider.enabled = enabledState;
        }
    }

    private void SetRigidbodyState(bool enabledState)
    {
        itemRigidbody.isKinematic = !enabledState;
    }

    private void SetVisualState(bool visibleState)
    {
        _selfObject.SetActive(visibleState);
    }
}
