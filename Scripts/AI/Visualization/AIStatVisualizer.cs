using System;
using UnityEngine;

public abstract class AIStatVisualizer : MonoBehaviour
{
    [SerializeField] private protected GameObject visualizationObject;
    [SerializeField] private protected AIMindBase currentTarget;

    private protected Transform _targetTransform;

    private void Start()
    {
        OnTargetChanged();
    }

    public void ChangeVisualizationTarget(AIMindBase target)
    {
        currentTarget = target;
        OnTargetChanged();
    }

    private protected virtual void OnTargetChanged()
    {
        visualizationObject.SetActive(currentTarget != null);
        if (currentTarget != null)
        {
            _targetTransform = currentTarget.transform;
        }
    }
}
