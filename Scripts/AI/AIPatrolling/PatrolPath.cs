using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<PatrolPoint> GetPatrolPoints => patrolPoints;

    [SerializeField] private List<PatrolPoint> patrolPoints;
    [SerializeField] private Color pathColor;

    private void OnDrawGizmosSelected()
    {

        if (patrolPoints == null) return;
        
        if(patrolPoints.Count <= 0) return;
        
        Gizmos.color = pathColor;

        Gizmos.DrawWireCube(patrolPoints[0].transform.position, Vector3.one * .5f);

        if(patrolPoints.Count < 2) return;
        
        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(patrolPoints[i].transform.position, patrolPoints[i + 1].transform.position);
        }

        Gizmos.DrawLine(patrolPoints[^1].transform.position, patrolPoints[0].transform.position);
    }
}
