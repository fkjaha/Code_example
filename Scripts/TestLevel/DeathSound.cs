using System.Collections.Generic;
using UnityEngine;

public class DeathSound : MonoBehaviour
{
    [SerializeField] private HPContainer hpContainer;
    [SerializeField] private List<AudioClip> deathClips;

    private void Start()
    {
        hpContainer.OnDeath += PlayDeathSound;
    }

    private void PlayDeathSound()
    {
        SoundsPlayer.Instance.PlaySoundOnly(deathClips.GetRandomElement(), hpContainer.transform.position);
    }
}
