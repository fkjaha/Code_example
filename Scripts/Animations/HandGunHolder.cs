using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandGunHolder : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private AISightProcessor aiSightProcessor;
    [SerializeField] private MultiAimConstraint gunHolderConstraint;

    [Space(20f)] 
    [SerializeField] private Transform secondaryHandTarget;

    private void OnDestroy()
    {
        rig.weight = 0;
    }

    private void Update()
    {
        gunHolderConstraint.weight = aiSightProcessor.GetMainTargetInSightState ? 1 : 0;
    }
}
