using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ItemTableData
{
    public int ITEM_ID { get; set; }
    public string ITEM_NAME { get; set; }
    public int ITEM_TYPE { get; set; }
    public int EFFECT1_TYPE { get; set; }
    public int EFFECT1_VALUE { get; set; }
    public int EFFECT2_TYPE { get; set; }
    public int EFFECT2_VALUE { get; set; }
    public int DROPPED { get; set; }
    public int COOKABLE { get; set; }
    public int INVENTORY { get; set; }
    public int USEMETHOD { get; set; }
    public int CONSUMABLE { get; set; }
    public int CONSUMPTION { get; set; }
    public int MAXSTACK { get; set; }
    public string ITEM_DESCRIPTION { get; set; }
    public string ITEM_IMAGE { get; set; }
    public int ORDER { get; set; }

    public override string ToString()
    {
        return $"{ITEM_ID} / {ITEM_NAME}";
    }

    public string StringName => DataTableManger.StringTable.Get(ITEM_NAME);
    public string StringDescription => DataTableManger.StringTable.Get(ITEM_DESCRIPTION);
    public Sprite SpriteIcon => Resources.Load<Sprite>($"ItemImages/{ITEM_IMAGE}");
}




public class ItemTable : DataTable
{
    private readonly Dictionary<int, ItemTableData> table = new Dictionary<int, ItemTableData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<ItemTableData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.ITEM_ID))
            {
                table.Add(item.ITEM_ID, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public ItemTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public List<ItemTableData> GetAll()
    {
        return new List<ItemTableData>(table.Values);
    }
}
