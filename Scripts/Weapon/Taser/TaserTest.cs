using UnityEngine;

public class TaserTest : ExecutionWeapon<ITaserExecutable>
{
    [SerializeField] private ParticleSystem _taserParticleSystem;
    
    private protected override void UseItem(ITaserExecutable target, Vector3 useWorldPosition = default)
    {
        ParticleSystem testSystem = Instantiate(_taserParticleSystem, useWorldPosition, Quaternion.identity);
        testSystem.Play(true);
        target.GetExecuted(deathType, GetExecutionTime());
    }
}
