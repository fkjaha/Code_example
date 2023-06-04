using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory ThisInventory;

    public event UnityAction OnInventoryUpdated;
    
    public List<BaseItem> GetItems => items;
    public int GetCurrentSelectionIndex => _currentSelectionIndex;
    
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Transform itemsTransform;
    [SerializeField] private BaseItem handAsItem;
    [SerializeField] private List<BaseItem> items = new();

    private int _currentSelectionIndex;
    
    private void Awake()
    {
        ThisInventory = this;
        items.Add(handAsItem);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            ChangeInventorySelectionIndex(Input.mouseScrollDelta.y > 0 ? -1 : 1);
        }
    }

    public bool TryGetCurrentItemInteractByObjectTag(out string interactTag)
    {
        interactTag = "";
        if (items.Count != 0) return false;
        
        bool result = items[_currentSelectionIndex].TryGetComponent(out InteractByObject interactByObject);

        if (!result) return false;
        
        interactTag = interactByObject.GetInteractionTag;
        
        return true;
    }

    public BaseItem GetCurrentItem()
    {
        return items[_currentSelectionIndex];
    }
    
    public void OnCurrentItemUsed()
    {
        items[_currentSelectionIndex].OnUsed();
    }

    public void TryUseCurrentItem(GameObject useTarget)
    {
        if(items[_currentSelectionIndex] != null)
            items[_currentSelectionIndex].TryUse(useTarget, new Vector3());
    }
    
    public void AddItemToInventory(BaseItem item)
    {
        item.gameObject.SetActive(false);
        item.InteractiveObject.DisableInteractions();
        
        Transform itemTransform = item.transform;
        itemTransform.parent = itemsParent;
        itemTransform.position = itemsTransform.position;
        itemTransform.localRotation = itemsTransform.localRotation;

        foreach (Collider itemCollider in itemTransform.GetComponents<Collider>())
        {
            itemCollider.enabled = false;
        }
        
        items.Add(item);
    }

    public void GetRidOfItem()
    {
        GetRidOfItem(items[_currentSelectionIndex]);
    }
    
    public void GetRidOfItem(int itemIndex)
    {
        GetRidOfItem(items[itemIndex]);
    }
    
    public void GetRidOfItem(BaseItem item)
    {
        BaseItem itemToDestroy = items[_currentSelectionIndex];
        items.Remove(itemToDestroy);
        Destroy(itemToDestroy.gameObject);
        ChangeInventorySelectionIndex(0);
    }

    private void ChangeInventorySelectionIndex(int indexDelta)
    {
        _currentSelectionIndex = GetHandledSelectionIndex(_currentSelectionIndex + indexDelta);
        OnInventoryUpdated?.Invoke();
    }

    private int GetHandledSelectionIndex(int indexToHandle)
    {
        if (indexToHandle >= items.Count) 
            return 0;
        
        if (indexToHandle < 0) 
            return items.Count - 1;
        
        return indexToHandle;
    }
}
