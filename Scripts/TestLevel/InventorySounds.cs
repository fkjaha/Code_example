using UnityEngine;

public class InventorySounds : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private SoundPresetBase pickUpSound;
    [SerializeField] private SoundPresetBase slotSwitchSound;

    private void OnEnable()
    {
        inventory.OnItemPickedUp += PlayPickUpSound;
        inventory.OnSlotSwitched += PlaySlotSwitchSound;
    }
    
    private void OnDisable()
    {
        inventory.OnItemPickedUp -= PlayPickUpSound;
        inventory.OnSlotSwitched -= PlaySlotSwitchSound;
    }

    private void PlayPickUpSound()
    {
        SoundsPlayer.Instance.PlaySoundOnly(pickUpSound, transform.position);
    }
    
    private void PlaySlotSwitchSound()
    {
        SoundsPlayer.Instance.PlaySoundOnly(slotSwitchSound, transform.position);
    }
}
