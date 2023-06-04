using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    public static SoundsPlayer Instance;

    [SerializeField] private float defaultSoundsMaxHeightDelta;
    [SerializeField] private NearbyDetector soundsNearbyDetector;
    [SerializeField] private Pool<AudioSource> sourcesPool;

    private SoundPresetSettings _defaultPresetSettings = new();

    private void Awake()
    {
        Instance = this;
    }
    
    public void PlaySoundAndAudibleSound(Vector3 position, AudioClip clip = null, float alertRadius = 0, float maxHeightDelta = 0)
    {
        PlaySoundAndAudibleSound(position, clip, null, alertRadius, maxHeightDelta);
    }

    public void PlaySoundAndAudibleSound(Vector3 position, SoundPresetBase soundPreset = null, float alertRadius = 0, float maxHeightDelta = 0)
    {
        if (soundPreset != null)
        {
            PlaySoundAndAudibleSound(position, soundPreset.GetClip, soundPreset.GetSettings, alertRadius, maxHeightDelta);
        }
        else
        {
            PlaySoundAndAudibleSound(position, null, null, alertRadius, maxHeightDelta);
        }
    }
    
    public void PlaySoundAndAudibleSound(Vector3 position, AudioClip clip = null,
        SoundPresetSettings soundPresetSettings = null, float alertRadius = 0, float maxHeightDelta = 0)
    {
        if(clip != null)
            PlaySoundOnly(clip, position, soundPresetSettings);
        
        if(alertRadius <= 0) return;
        PlayAudibleSound(false, position, alertRadius, maxHeightDelta);
    }

    public void PlaySoundOnly(AudioClip clip, Vector3 position, SoundPresetSettings soundPresetSettings = null)
    {
        AudioSource source = sourcesPool.GetObject();

        soundPresetSettings ??= _defaultPresetSettings;
        source.volume = soundPresetSettings.GetVolume;
        source.pitch = soundPresetSettings.GetPitch;
        source.spatialBlend = soundPresetSettings.GetSpatialBlend;
        
        source.clip = clip;
        Transform sourceTransform = source.transform;
        sourceTransform.position = position;

        source.Play();
    }
    
    public void PlaySoundOnly(SoundPresetBase soundPreset, Vector3 position)
    {
        if(soundPreset == null) return;
        PlaySoundOnly(soundPreset.GetClip, position, soundPreset.GetSettings);
    }

    public void PlayAudibleSound(bool isAlert, Vector3 position, float alertRadius = 0, float maxHeightDelta = 0,
        Ear excludeEar = null)
    {
        List<Ear> ears = soundsNearbyDetector.GetNearbyInRadius<Ear>(position, alertRadius,
            maxHeightDelta <= 0 ? defaultSoundsMaxHeightDelta : maxHeightDelta);
        ears.Remove(excludeEar);
        foreach (Ear ear in ears)
        {
            ear.GetSound(position, isAlert);
        }
    }
}
