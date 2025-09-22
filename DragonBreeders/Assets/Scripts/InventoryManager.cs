using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    public InventoryUI inventoryUI;

    private Dictionary<int, int> items = new Dictionary<int, int>();

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

    public void AddTestItems()
    {
        AddItem(5020201, 3);
        AddItem(5020204, 5); 
        AddItem(5040301, 2);
    }

    public void TestClearInventory()
    {
        ClearInventory();
    }
}