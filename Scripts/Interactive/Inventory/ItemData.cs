using UnityEngine;

public class ItemData : MonoBehaviour
{
    public ItemGeneralInfo GetStoredItemGeneralInfo => itemGeneralInfo;
    public int GetStoredItemConsumesLeftCount => _consumesLeftCount;
    
    [SerializeField] private ItemGeneralInfo itemGeneralInfo;

    private int _consumesLeftCount;

    public void StoreItem(ItemGeneralInfo targetItemGeneralInfo, int targetConsumesLeftCount = 0)
    {
        itemGeneralInfo = targetItemGeneralInfo;
        _consumesLeftCount = targetConsumesLeftCount;
    }
}
