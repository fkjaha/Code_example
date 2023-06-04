using System.Collections;
using UnityEngine;

public class Steps : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SoundPresetBase soundPreset;
    [SerializeField] private float timeBetweenSteps;
    [SerializeField] private float runMultiplier;

    private void Start()
    {
        StartCoroutine(StepsPlayer());
    }

    private IEnumerator StepsPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds((playerController.IsRunning ? runMultiplier : 1) * timeBetweenSteps);
            if (playerController.IsMoving)
            {
                SoundsPlayer.Instance.PlaySoundOnly(soundPreset, playerController.GetPosition);
            }
        }
    }
}
