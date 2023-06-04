using System.Collections;
using UnityEngine;

public class AIVoice : MonoBehaviour
{
    [SerializeField] private Ear selfEar;
    [SerializeField] private float alertRadius;
    [SerializeField] private float alertCooldown;
    [SerializeField] private float maxHeightSoundDelta;

    private bool _cooldownActive;

    public void AlertNearby(Vector3 alertPosition, AIMindBase client = null)
    {
        if(_cooldownActive) return;

        SoundsPlayer.Instance.PlayAudibleSound(true, alertPosition, alertRadius, maxHeightSoundDelta, selfEar);

        StartCoroutine(AlertCooldown());
    }

    private IEnumerator AlertCooldown()
    {
        _cooldownActive = true;
        yield return new WaitForSeconds(alertCooldown);
        _cooldownActive = false;
    }
}
