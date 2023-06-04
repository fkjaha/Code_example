using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AIPatrolMind : MonoBehaviour
{
    public PatrolPoint GetCurrentTargetPoint => _currentTargetPatrolPoint;
    public int GetPointsCount => patrolPath.GetPatrolPoints.Count;
    public bool IsPatrolling => _isPatrolling;

    [SerializeField] private AIMindBase aiMind;
    [SerializeField] private AIWalkController walkController;
    [SerializeField] private PatrolPath patrolPath;
    
    private bool _isPatrolling;
    private int _currentTargetPointIndex;
    private PatrolPoint _currentTargetPatrolPoint;
    private IEnumerator _patrollingCoroutine;
    private IEnumerator _patrollingActionsCoroutine;

    private void Start()
    {
        ResetPatrolProgress();
    }

    public void RequestCurrentPatrolPointGoNext()
    {
        ChangeCurrentTargetPointToPointAtIndex(_currentTargetPointIndex + 1);
    }

    private void ResetPatrolProgress()
    {
        ChangeCurrentTargetPointToPointAtIndex(0);
    }

    private void ChangeCurrentTargetPointToPointAtIndex(int newPointIndex)
    {
        _currentTargetPointIndex = newPointIndex % patrolPath.GetPatrolPoints.Count;
        _currentTargetPatrolPoint = patrolPath.GetPatrolPoints[_currentTargetPointIndex];
    }
    
    public void GoPatrolling()
    {
        _isPatrolling = true;
        _patrollingCoroutine = Patrolling();
        StartCoroutine(_patrollingCoroutine);
    }
    
    private IEnumerator Patrolling()
    {
        bool stageCompleted = false;
        bool endlessLoopRequired = GetPointsCount > 1;
        while (true)
        {
            PatrolPoint point = GetCurrentTargetPoint;
            stageCompleted = false;
            walkController.GoTo(point.transform.position, () =>
            {
                RequestCurrentPatrolPointGoNext();
                stageCompleted = true;
            });

            while (!stageCompleted)
            {
                yield return new WaitForEndOfFrame();
            }

            if (point.LoopActions)
            {
                while (true)
                {
                    yield return StartCoroutine(PathActionsCoroutine(point));
                }
            }
            
            yield return StartCoroutine(PathActionsCoroutine(point));

            if (point.GetRepeatCount > 0)
            {
                for (int i = 0; i < point.GetRepeatCount; i++)
                {
                    yield return StartCoroutine(PathActionsCoroutine(point));
                }
            }
            
            if(!endlessLoopRequired) break;
        }
    }

    private IEnumerator PathActionsCoroutine(PatrolPoint point)
    {
        if (point.GetPathActions == null) yield break;
        
        foreach (PathAction pathAction in point.GetPathActions)
        {
            if (pathAction != null && pathAction.ActionGotAChance)
            {
                yield return StartCoroutine(pathAction.StartActions(aiMind.GetActionsEvents()));
            }
        }
    }

    public void StopPatrolling()
    {
        if(!_isPatrolling) return;
        
        if(_patrollingCoroutine != null)
            StopCoroutine(_patrollingCoroutine);
        _isPatrolling = false;
    }
}
