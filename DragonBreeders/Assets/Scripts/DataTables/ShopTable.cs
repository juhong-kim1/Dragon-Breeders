using System.Collections.Generic;
using UnityEngine;

public class ShopTableData
{
    public int SHOP_ID { get; set; }
    public int ITEM_ID_SHOP { get; set; }
    public int SHOP_TYPE { get; set; }
    public int PRICE { get; set; }
    public int UNLOCK_TYPE { get; set; }
    public int ORDER { get; set; }

    public override string ToString() => $"{SHOP_ID} / {ITEM_ID_SHOP} / {PRICE}";
}

public class ShopTable : DataTable
{
    private readonly Dictionary<int, ShopTableData> table = new Dictionary<int, ShopTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<ShopTableData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.ITEM_ID_SHOP))
            {
                table.Add(item.ITEM_ID_SHOP, item);
            }
            else
            {
                Debug.LogError("샵 아이템 아이디 중복!");
            }
        }
    }

    public ShopTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public List<ShopTableData> GetAll()
    {
        return new List<ShopTableData>(table.Values);
    }
    public List<ShopTableData> GetShopItems()
    {
        List<ShopTableData> shopItems = new List<ShopTableData>();

        foreach (var item in table.Values)
        {
            if (item.SHOP_TYPE == 1)
            {
                shopItems.Add(item);
            }
        }

        return shopItems;
    }
}
