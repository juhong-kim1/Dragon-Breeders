using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ShopSlot[] shopSlots = new ShopSlot[6];
    private List<ShopTableData> allItems;

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        allItems = DataTableManger.ShopTable.GetAll();

        List<ShopTableData> randomItems = GetRandomItems(6);

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i >= randomItems.Count)
            {
                shopSlots[i].gameObject.SetActive(false);
                continue;
            }

            var shopData = randomItems[i];
            var runtimeItem = CreateRuntimeItem(shopData.ITEM_ID_SHOP, shopData.PRICE);
            shopSlots[i].SetItem(runtimeItem, shopData.PRICE);
        }
    }

    private List<ShopTableData> GetRandomItems(int count)
    {
        List<ShopTableData> copy = new List<ShopTableData>(allItems);
        List<ShopTableData> result = new List<ShopTableData>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }

    private IItem CreateRuntimeItem(int itemId, int price)
    {
        var itemData = DataTableManger.ItemTable.Get(itemId);
        if (itemData == null)
        {
            Debug.LogError($"ItemTable¿¡ ID {itemId} ¾øÀ½");
            return null;
        }

        Item runtimeItem = new Item
        {
            itemID = itemData.ITEM_ID,
            itemName = itemData.StringName,
            icon = itemData.SpriteIcon,
            description = itemData.StringDescription,
            itemType = itemData.ITEM_TYPE,
            price = price,
        };

        return runtimeItem;
    }
}
