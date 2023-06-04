using UnityEngine;
using UnityEngine.AI;

public class AIMindCivilian : AIMindBase
{
    [Space(20f)]
    [SerializeField] private float runAwayRadius;

    private bool _runningAway;

    private protected override void GetAlerted(Vector3 position)
    {
        StopAllActivities();
        RunAwayFromPosition(_target.position);
    }

    private protected override void GetSound(Vector3 position)
    {
        StopAllActivities();
        
        if (IsConcerned)
            RunAwayFromPosition(_target.position);
        else
            SuspectPosition(position);
    }

    private protected override void IsInMemory()
    {
        
    }

    private protected override void InSightAndConcerned()
    {
        AlertAboutTarget();
        StopAllActivities();
        RunAwayFromPosition(_target.position);
    }

    private void RunAwayFromPosition(Vector3 position)
    {
        Debug.Log("Running away: " + _runningAway);
        if(_runningAway) return;

        _performingResearch = true;
        _runningAway = true;

        position = GetRandomRunPosition(position);

        walkController.GoTo(position, () =>
        {
            LookAround(() => _performingResearch = false);
            _runningAway = false;
        });
    }

    private protected override void NotInMemory()
    {
        if(!_runningAway)
            base.NotInMemory();
    }

    private protected override void InSightAndSuspected()
    {
        StopAllActivities();
        SuspectPosition(_target.position);
    }

    private Vector3 GetRandomRunPosition(Vector3 position)
    {
        Vector3 randomPosition = Random.insideUnitCircle.normalized.Vector2ToVector3XZ() * runAwayRadius;
        randomPosition += position;
        NavMesh.SamplePosition(randomPosition, out NavMeshHit navMeshHit, runAwayRadius, 1);
        return navMeshHit.position;
    }
}
