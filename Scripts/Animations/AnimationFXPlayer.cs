using UnityEngine;

public class AnimationFXPlayer : MonoBehaviour
{
    [SerializeField] private Transform fxSpawnPoint;
    
    public void PlaySelectedFX(ParticleSystem particleSystemPrefab)
    {
        ParticleSystem systemInstance = Instantiate(particleSystemPrefab, fxSpawnPoint.position, fxSpawnPoint.rotation);
        ParticleSystem.MainModule main = systemInstance.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        systemInstance.Play(true);
    }
    
    public void PlaySoundFX(SoundPresetBase soundPresetBase)
    {
        if(SoundsPlayer.Instance == null) return;
        SoundsPlayer.Instance.PlaySoundOnly(soundPresetBase, fxSpawnPoint.position);
    }
}
