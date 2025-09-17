using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataTableManger
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManger()
    {
        Init();
    }

    private static void Init()
    {
#if UNITY_EDITOR
        //foreach (var id in DataTableIds.StringTableIds)
        //{
        //    var table = new StringTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}
#else
        var stringTable = new StringTable();
        stringTable.Load(DataTableIds.String);
        tables.Add(DataTableIds.String, stringTable);
#endif

        var dragonStatTable = new DragonStatTable();
        dragonStatTable.Load(DataTableIds.DragonStat);
        tables.Add(DataTableIds.DragonStat, dragonStatTable);


        var debuffTable = new DebuffTable();
        debuffTable.Load(DataTableIds.Debuff);
        tables.Add(DataTableIds.Debuff, debuffTable);

        var nurtualTable = new NurtureTable();
        nurtualTable.Load(DataTableIds.Nurture);
        tables.Add(DataTableIds.Nurture, nurtualTable);

        var itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

    }

    //public static StringTable StringTable
    //{
    //    get
    //    {
    //        return Get<StringTable>(DataTableIds.String);
    //    }
    //}

    public static DragonStatTable DragonStatTable
    {
        get
        {
            return Get<DragonStatTable>(DataTableIds.DragonStat);
        }
    }

    public static DebuffTable DebuffTable
    {
        get
        {
            return Get<DebuffTable>(DataTableIds.Debuff);
        }
    }

    public static NurtureTable NurtureTable
    {
        get
        {
            return Get<NurtureTable>(DataTableIds.Nurture);
        }
    }

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(DataTableIds.Item);
        }
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}
