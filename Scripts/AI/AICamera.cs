using UnityEngine;

public class AICamera : MonoBehaviour
{
    [SerializeField] private AISightProcessor sightProcessor;
    [SerializeField] private AIVoice aiVoice;

    private void Start()
    {
        sightProcessor.OnAdditionalTargetInSight += position => aiVoice.AlertNearby(position);
    }

    private void Update()
    {
        if(sightProcessor.GetMainTargetInSightState)
            TryToAlert();
    }

    private void TryToAlert()
    {
        aiVoice.AlertNearby(AISightTargets.Instance.GetMainTarget.position);
    }
}
