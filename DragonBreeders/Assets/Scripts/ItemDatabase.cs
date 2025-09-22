using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public IItem GetItem(int itemID)
    {
        var itemData = DataTableManger.ItemTable.Get(itemID);

        if (itemData != null)
        {
            Debug.Log($"아이템 데이터 찾음: ID={itemData.ITEM_ID}");
            Debug.Log($"아이템 이름 키: {itemData.ITEM_NAME}");
            Debug.Log($"다국어 이름: {itemData.StringName}");
            Debug.Log($"아이콘 이름: {itemData.ITEM_IMAGE}");
            Debug.Log($"아이콘 스프라이트: {itemData.SpriteIcon}");

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
    public System.Collections.Generic.List<IItem> GetItemsByType(int itemType)
    {
        var allItems = DataTableManger.ItemTable.GetAll();
        var result = new System.Collections.Generic.List<IItem>();

        foreach (var itemData in allItems)
        {
            if (itemData.ITEM_TYPE == itemType)
            {
                result.Add(GetItem(itemData.ITEM_ID));
            }
        }

        return result;
    }

    public System.Collections.Generic.List<IItem> GetAllItems()
    {
        var allItems = DataTableManger.ItemTable.GetAll();
        var result = new System.Collections.Generic.List<IItem>();

        foreach (var itemData in allItems)
        {
            result.Add(GetItem(itemData.ITEM_ID));
        }

        return result;
    }
}
