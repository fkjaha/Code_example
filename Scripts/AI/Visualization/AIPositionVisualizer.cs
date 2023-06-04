using System;
using UnityEngine;

public class AIPositionVisualizer : AIStatVisualizer
{
    [SerializeField] private Transform positionFollower;

    private void Update()
    {
        if (currentTarget == null) return;

        positionFollower.position = _targetTransform.position;
    }
}
