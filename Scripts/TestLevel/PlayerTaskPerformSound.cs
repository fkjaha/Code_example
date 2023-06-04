using UnityEngine;


// REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE
public class PlayerTaskPerformSound : MonoBehaviour
{
    [SerializeField] private EventTask connectedTask;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip clip2;

    private AudioClip _clip;

    private void Start()
    {
        _clip = clip;
        PlayerTaskPerformer.Instance.OnPerformingStarted += task =>
        {
            if (connectedTask == task)
            {
                SoundsPlayer.Instance.PlaySoundOnly(_clip, connectedTask.transform.position);
                _clip = clip2;
            }
        };
    }
}
// REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE REWRITE

