using UnityEngine;

public class AIItemHolderSlot : ItemHolderSlot
{
    [SerializeField] private BaseItem startWeapon;
    
    private void Start()
    {
        SetItem(startWeapon);
    }
}
