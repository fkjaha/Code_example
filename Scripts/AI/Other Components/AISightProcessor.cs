using System;
using UnityEngine;
using UnityEngine.Events;

public class AISightProcessor : MonoBehaviour
{
    public bool GetMainTargetInSightState => _mainTargetInSight;
    public event UnityAction<Vector3> OnAdditionalTargetInSight;

    [SerializeField] private AIExecution aiExecution;
    [SerializeField] private FieldOfView fieldOfView;

    private bool _mainTargetInSight;
    private bool _enabled;

    private void Start()
    {
        _enabled = true;
        if (aiExecution != null)
        {
            aiExecution.OnExecutionStarted += _ => { _enabled = false; };
            aiExecution.OnExecutionCancelled += () => { _enabled = true; };
        }
        fieldOfView.OnDestroyEvent += () => Destroy(this);
    }

    private void Update()
    {
        if (!_enabled)
        {
            _mainTargetInSight = false;
            return;
        }
        
        _mainTargetInSight = fieldOfView.IsInView(AISightTargets.Instance.GetMainTarget);
        
        foreach (AdditionalTarget additionalTarget in AISightTargets.Instance.GetAdditionalTargets)
        {
            if (fieldOfView.IsInView(additionalTarget.transform))
            {
                OnAdditionalTargetInSight?.Invoke(additionalTarget.transform.position);
                additionalTarget.GetNoticed();
                return;
            }
        }
    }
}
