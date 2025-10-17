using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public IItem GetItem(int itemID)
    {
        var itemData = DataTableManger.ItemTable.Get(itemID);

        if (itemData != null)
        {
            return new Item
            {
                itemID = itemData.ITEM_ID,
                itemName = itemData.StringName,
                itemType = itemData.ITEM_TYPE,
                icon = itemData.SpriteIcon,
                description = itemData.StringDescription
            };
        }

        Debug.LogWarning($"아이템 ID {itemID}를 찾을 수 없습니다.");
        return null;
    }

    public bool HasItem(int itemID)
    {
        return DataTableManger.ItemTable.Get(itemID) != null;
    }

    public ItemTableData GetItemTableData(int itemID)
    {
        return DataTableManger.ItemTable.Get(itemID);
    }

    public List<IItem> GetItemsByType(int itemType)
    {
        var allItems = DataTableManger.ItemTable.GetAll();
        var result = new List<IItem>();

        foreach (var itemData in allItems)
        {
            if (itemData.ITEM_TYPE == itemType)
            {
                result.Add(GetItem(itemData.ITEM_ID));
            }
        }

        return result;
    }

    public List<IItem> GetAllItems()
    {
        var allItems = DataTableManger.ItemTable.GetAll();
        var result = new List<IItem>();

        foreach (var itemData in allItems)
        {
            result.Add(GetItem(itemData.ITEM_ID));
        }

        return result;
    }
}
