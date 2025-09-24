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
        foreach (var id in DataTableIds.StringTableIds)
        {
            var table = new StringTable();
            table.Load(id);
            tables.Add(id, table);
        }
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

        var shopTable = new ShopTable();
        shopTable.Load(DataTableIds.Shop);
        tables.Add(DataTableIds.Shop, shopTable);

        var monsterTable = new MonsterTable();
        monsterTable.Load(DataTableIds.Monster);
        tables.Add(DataTableIds.Monster, monsterTable);

        var skillTable = new SkillTable();
        skillTable.Load(DataTableIds.Skill);
        tables.Add(DataTableIds.Skill, skillTable);

        var dropTable = new DropTable();
        dropTable.Load(DataTableIds.Drop);
        tables.Add(DataTableIds.Drop, dropTable);

    }

    public static StringTable StringTable
    {
        get
        {
            return Get<StringTable>(DataTableIds.String);
        }
    }

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

    public static ShopTable ShopTable
    {
        get
        {
            return Get<ShopTable>(DataTableIds.Shop);
        }

    }

    public static MonsterTable MonsterTable
    {
        get
        {
            return Get<MonsterTable>(DataTableIds.Monster);
        }
    }

    public static SkillTable SkillTable
    {
        get
        {
            return Get<SkillTable>(DataTableIds.Skill);
        }
    }

    public static DropTable DropTable
    {
        get
        {
            return Get<DropTable>(DataTableIds.Drop);
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
