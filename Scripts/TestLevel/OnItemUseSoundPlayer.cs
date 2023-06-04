using UnityEngine;

public class OnItemUseSoundPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory.OnItemUsed += PlayItemUseSound;
    }

    private void PlayItemUseSound(BaseItem baseItem, Transform target, Vector3 inputPosition)
    {
        SoundsPlayer.Instance.PlayAudibleSound(false, baseItem.transform.position, baseItem.GetUseSoundForce);
    }
}
