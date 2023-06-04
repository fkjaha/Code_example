using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVisualizer : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    
    [SerializeField] private Image currentItemImage;
    [SerializeField] private Text currentItemNameText;

    private void Start()
    {
        inventory.OnInventoryUpdated += UpdateInventoryVisualization;
        UpdateInventoryVisualization();
    }

    private void UpdateInventoryVisualization()
    {
        UpdateItemInHand();
        UpdateInventoryUI();
    }
    
    private void UpdateItemInHand()
    {
        for (int i = 0; i < Inventory.ThisInventory.GetItems.Count; i++)
        {
            Inventory.ThisInventory.GetItems[i].gameObject.SetActive(i == Inventory.ThisInventory.GetCurrentSelectionIndex);
        }
        
        // "..." Play Update Hand Animation
    }
    
    private void UpdateInventoryUI()
    {
        UpdateCurrentItemImage();
        UpdateCurrentItemName();
    }

    private void UpdateCurrentItemImage()
    {
        // currentItemImage.sprite = Inventory.ThisInventory.GetCurrentItem().GetSprite;
    }

    private void UpdateCurrentItemName()
    {
        // currentItemNameText.text = Inventory.ThisInventory.GetCurrentItem().GetItemName;
    }
}
