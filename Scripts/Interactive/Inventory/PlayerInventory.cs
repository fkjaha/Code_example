using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public event UnityAction<BaseItem, Transform, Vector3> OnItemUsed;
    public event UnityAction OnActiveSlotUpdated;
    public event UnityAction OnItemPickedUp;
    public event UnityAction OnSlotSwitched;

    public BaseItem GetActiveWeapon => slots[_currentSlotIndex].GetItem;

    [SerializeField] private Transform playerTransformToAvoidSelfItemUse;
    [SerializeField] private List<PlayerItemHolderSlot> slots;
    [SerializeField] private string useItemGameInputName;

    private int _currentSlotIndex;

    private void Start()
    {
        SceneInputDetector.Instance.AddSceneInputListener<Transform>(useItemGameInputName, UseCurrentItem);
        for (int i = 0; i < slots.Count; i++)
        {
            int index = i;
            InputDetector.Instance.AddInputListener((index+1).ToString(), () => SwitchSlot(index));
        }

        for (var i = 0; i < slots.Count; i++)
        {
            int slotIndex = i;
            slots[i].OnSlotUpdated += () => OnSlotUpdated(slotIndex);
        }

        UpdateSlots();
    }
    
    public bool TryGetCurrentItemInteractByObjectTag(out string interactTag)
    {
        interactTag = "";
        if (slots.Count != 0) return false;
        
        bool result = slots[_currentSlotIndex].GetItem.TryGetComponent(out InteractByObject interactByObject);

        if (!result) return false;
        
        interactTag = interactByObject.GetInteractionTag;
        
        return true;
    }

    private void UseCurrentItem(Transform useTarget, Vector3 inputWorldPosition)
    {
        ItemHolderSlot itemHolderSlot = slots[_currentSlotIndex];
        
        if(!itemHolderSlot.SlotFilled) return;
        
        if(!itemHolderSlot.GetItem.GetCanBeUsedOnSelf && playerTransformToAvoidSelfItemUse == useTarget) return;

        if(itemHolderSlot.TryToUse(useTarget.gameObject, inputWorldPosition))
            OnItemUsed?.Invoke(itemHolderSlot.GetItem, useTarget, inputWorldPosition);
    }

    public void TryToPickUpWeapon(BaseItem item)
    {
        if (!slots[_currentSlotIndex].SlotFilled)
        {
            PickUpItem(slots[_currentSlotIndex], item);
            return;
        }

        foreach (PlayerItemHolderSlot itemHolderSlot in slots)
        {
            if (!itemHolderSlot.SlotFilled)
            {
                PickUpItem(itemHolderSlot, item);
                return;
            }
        }
        
        PickUpItem(slots[_currentSlotIndex], item);
    }

    private void PickUpItem(ItemHolderSlot slot, BaseItem item)
    {
        slot.SetItem(item);
        OnItemPickedUp?.Invoke();
    }

    private void SwitchSlot(int targetIndex)
    {
        if (targetIndex < 0) targetIndex = slots.Count - 1;
        else if (targetIndex >= slots.Count) targetIndex = 0;

        _currentSlotIndex = targetIndex;
        UpdateSlots();
        OnSlotSwitched?.Invoke();
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetActive(i == _currentSlotIndex);
        }
    }

    private void OnSlotUpdated(int slotIndex)
    {
        if (_currentSlotIndex == slotIndex)
        {
            OnActiveSlotUpdated?.Invoke();
        }
    }
}
