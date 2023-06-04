using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sounds/SoundsPresetRandom", fileName = "RndSound_")]
public class SoundPresetRandom : SoundPresetBase
{
    public override AudioClip GetClip => clip.GetRandomElement();

    [SerializeField] private List<AudioClip> clip;
}
