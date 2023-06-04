using System;
using UnityEngine;

public abstract class SoundPresetBase : ScriptableObject
{
    public abstract AudioClip GetClip { get; }
    public SoundPresetSettings GetSettings => soundPresetSettings;
    
    [SerializeField] private SoundPresetSettings soundPresetSettings;
}

[Serializable]
public class SoundPresetSettings
{
    public float GetVolume => volume;
    public float GetSpatialBlend => spatialBlend;
    public float GetPitch => pitch;
    
    [Range(0,1)]
    [SerializeField] private float volume;
    [Range(0,1)]
    [SerializeField] private float spatialBlend;
    [Range(-3,3)]
    [SerializeField] private float pitch;

    public SoundPresetSettings(float volumeTarget = 1, float spatialBlendTarget = 0, float pitchTarget = 1)
    {
        volume = volumeTarget;
        spatialBlend = spatialBlendTarget;
        pitch = pitchTarget;
    }
    
    public SoundPresetSettings()
    {
        volume = 1;
        spatialBlend = 0;
        pitch = 1;
    }
}
