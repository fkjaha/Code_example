using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo_", menuName = "Scriptable Objects/New Item Info")]
public class ItemGeneralInfo : ScriptableObject
{
    public string GetItemName => itemName;
    public Sprite GetItemSprite => itemSprite;
    public PhysicalItem GetPhysicalItemPrefab => physicalItemPrefab;
    public float GetMaxUseRange => maxUseRange;
    public bool GetIsConsumable => isConsumable;
    public int GetUseCountToConsume => useCountToConsume;
    
    [Header("Main variables")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;
    [Space(20f)]
    [SerializeField] private PhysicalItem physicalItemPrefab;
    [SerializeField] private float maxUseRange;

    [Header("Consumable")] 
    [SerializeField] private bool isConsumable;
    [Tooltip("Matters if field above is true!")]
    [SerializeField] private int useCountToConsume;
}
