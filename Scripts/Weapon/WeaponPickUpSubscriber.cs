using UnityEngine;

public class WeaponPickUpSubscriber : MonoBehaviour
{
    [SerializeField] private string pickUpGameInputName;
    [SerializeField] private PlayerInventory playerInventory;
    

    private void Start()
    {
        SceneInputDetector.Instance.AddSceneInputListener(pickUpGameInputName, (BaseItem weapon) =>
        {
            playerInventory.TryToPickUpWeapon(weapon);
        });
    }
}
