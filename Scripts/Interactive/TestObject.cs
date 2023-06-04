using UnityEngine;

public class TestObject : InteractiveObject
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float radius;
    
    public override void Interact()
    {
        SoundsPlayer.Instance.PlaySoundAndAudibleSound(transform.position, clip, radius);
        // SoundsPlayer.Instance.PlayAlertOnly(transform.position, radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
