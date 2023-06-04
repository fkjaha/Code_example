using UnityEngine;

public class AIStatsVisualizerSounds : MonoBehaviour
{
    [SerializeField] private AIStatsVisualizer aiStatsVisualizer;
    [SerializeField] private SoundPresetBase switchSound;

    private void OnEnable()
    {
        aiStatsVisualizer.OnVisualizationTargetChanged += PlaySwitchSound;
    }
    
    private void OnDisable()
    {
        aiStatsVisualizer.OnVisualizationTargetChanged -= PlaySwitchSound;
    }

    private void PlaySwitchSound()
    {
        SoundsPlayer.Instance.PlaySoundOnly(switchSound, transform.position);
    }
}
