using UnityEngine;
using UnityEngine.Events;

public class LockedDoor : Door
{
    public event UnityAction OnUnlocked;

    [SerializeField] private string lockedStateInteractionTag;
    [SerializeField] private string unlockedStateInteractionTag;
    [SerializeField] private bool locked;
    [SerializeField] private GameObject lockedColliderObject;
    [SerializeField] private EventTask task;
    [Space(10f)] 
    [SerializeField] private SoundPresetBase unlockSound;

    private void Start()
    {
        task.OnComplete += Unlock;
        OnUnlocked += () => SoundsPlayer.Instance.PlaySoundOnly(unlockSound, transform.position);

        UpdateDependencies();
    }

    private void Unlock()
    {
        locked = false;
        UpdateDependencies();
        OnUnlocked?.Invoke();
    }

    private void UpdateDependencies()
    {
        lockedColliderObject.SetActive(locked);
        interactionTag = locked ? lockedStateInteractionTag : unlockedStateInteractionTag;
        requireHoldingBeforeInteract = locked;
    }

    public override void Interact()
    {
        if(!locked)
            base.Interact();
        else task.Interact();
    }

    public override void Interact(GameObject client)
    {
        if(!locked)
            base.Interact(client);
        else task.Interact(client);
    }
}
