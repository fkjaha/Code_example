using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIWalkController : MonoBehaviour
{
    public Vector3 GetVelocity => navMeshAgent.velocity;
    public bool IsRunning => _isRunning;
    
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private AIMindBase aiMindBase;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;

    private bool _onPathCompletedActionInvoked = true;
    private UnityAction _onPathCompletedAction;
    private Quaternion _targetRotation;
    private Vector3 _currentLookTarget;
    private bool _isRunning;
    private bool _pathCompleted;

    private void Start()
    {
        aiMindBase.OnConcernedStateChanged += UpdateSpeed;
        UpdateSpeed();
        navMeshAgent.updateRotation = false;
        StartCoroutine(ListenToPathCompletion());
    }

    private void UpdateSpeed()
    {
        _isRunning = aiMindBase.IsConcerned;
        navMeshAgent.speed = _isRunning ? runSpeed : walkSpeed;
    }

    public void EnableWalking()
    {
        navMeshAgent.speed = walkSpeed;
    }

    public void DisableWalking()
    {
        navMeshAgent.speed = 0;
    }
    
    private void PathCompleted()
    {
        _pathCompleted = true;
    }

    private void Update()
    {
        if (!_pathCompleted)
        {
            _currentLookTarget = navMeshAgent.steeringTarget;
        } 
        RotateTowardsTargetOverTime();
    }

    public void GoTo(Vector3 position, UnityAction onCompleted = null)
    {
        _pathCompleted = false;
        _onPathCompletedAction = onCompleted;
        _onPathCompletedAction += PathCompleted;
        _onPathCompletedActionInvoked = false;
        navMeshAgent.SetDestination(position);
    }

    public void LookAtPosition(Vector3 position, UnityAction onCompleted = null)
    {
        if (navMeshAgent.velocity == Vector3.zero)
        {
            _currentLookTarget = position;
        }
        onCompleted?.Invoke();
    }
    
    public void LookAtPositionInstantly(Vector3 position, UnityAction onCompleted = null)
    {
        _currentLookTarget = position;
        RotateTowardsTargetInstantly();
        onCompleted?.Invoke();
    }

    private IEnumerator ListenToPathCompletion()
    {
        while (true)
        {
            if (!_onPathCompletedActionInvoked && ReachedDestination())
            {
                _onPathCompletedActionInvoked = true;
                _onPathCompletedAction?.Invoke();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private bool ReachedDestination()
    {
        if (navMeshAgent.pathPending) return false;

        if (navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance) return false;

        if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f) return false;

        if (!rotationTransform.rotation.Equals(_targetRotation)) return false;
        
        return true;
    }

    private void RotateTowardsTargetOverTime()
    {
        Vector3 lookVector = _currentLookTarget.Vector3ToFlat() - rotationTransform.position.Vector3ToFlat();
        if (lookVector == Vector3.zero) return;
        _targetRotation = Quaternion.LookRotation(lookVector, Vector3.up);
        rotationTransform.rotation = Quaternion.RotateTowards(rotationTransform.rotation, _targetRotation,
            rotationSpeed * Time.deltaTime);
    }
    
    private void RotateTowardsTargetInstantly()
    {
        Vector3 lookVector = _currentLookTarget.Vector3ToFlat() - rotationTransform.position.Vector3ToFlat();
        if (lookVector == Vector3.zero) return;
        _targetRotation = Quaternion.LookRotation(lookVector, Vector3.up);
        rotationTransform.rotation = _targetRotation;
    }

    private void OnDestroy()
    {
        Destroy(navMeshAgent);
    }
}
