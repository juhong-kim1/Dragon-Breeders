using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    public InventoryUI inventoryUI;
    public InventoryUI foodInventoryUI;
    public InventoryUI playInventoryUI;

    private Dictionary<int, int> items = new Dictionary<int, int>();

    private void Start()
    {
        AddDefaultItems();
    }

    public void AddItem(int itemID, int amount = 1)
    {
        if (items.ContainsKey(itemID))
        {
            items[itemID] += amount;
        }
        else
        {
            items[itemID] = amount;
        }

        Debug.Log($"아이템 추가: ID={itemID}, 수량={amount}, 총={items[itemID]}개");

        if (inventoryUI != null)
        {
            inventoryUI.RefreshDisplay(GetAllItems());
        }
    }

    public void RemoveItem(int itemID, int amount = 1)
    {
        if (items.ContainsKey(5040001) || items.ContainsKey(5050001) || items.ContainsKey(5090001)) return;

        if (items.ContainsKey(itemID))
        {
            items[itemID] -= amount;

            if (items[itemID] <= 0)
            {
                items.Remove(itemID);
            }

            Debug.Log($"아이템 제거: ID={itemID}, 수량={amount}");

            if (inventoryUI != null)
            {
                inventoryUI.RefreshDisplay(GetAllItems());
            }
        }
    }

    public void RefreshAllUIs()
    {
        if (inventoryUI != null)
        {
            inventoryUI.RefreshDisplay(GetAllItems());
        }

        if (foodInventoryUI != null && foodInventoryUI.gameObject.activeInHierarchy)
        {
            foodInventoryUI.RefreshDisplay(GetAllItems(), 2);
        }
    }

    public void RefreshFoodUI()
    {
        if (foodInventoryUI != null)
        {
            foodInventoryUI.RefreshDisplay(GetAllItems(), 2);
        }
    }

    public void RefreshPlayUI()
    {
        if (playInventoryUI != null)
        {
            playInventoryUI.RefreshDisplay(GetAllItems(), 6);
        }
    }

    public int GetItemAmount(int itemID)
    {
        return items.ContainsKey(itemID) ? items[itemID] : 0;
    }

    public bool HasItem(int itemID, int amount = 1)
    {
        return GetItemAmount(itemID) >= amount;
    }

    public Dictionary<int, int> GetAllItems()
    {
        return new Dictionary<int, int>(items);
    }

    public IItem GetItemData(int itemID)
    {
        return itemDatabase?.GetItem(itemID);
    }

    public void ClearInventory()
    {
        items.Clear();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshDisplay(GetAllItems());
        }
    }

    public void TestClearInventory()
    {
        ClearInventory();
    }

    public void AddDefaultItems()
    {
        AddItem(5020201, 5);
        AddItem(5040001, 99);
        AddItem(5060001, 99);
        AddItem(5050001, 99);
    }
}