using System.Collections.Generic;
using UnityEngine;

public class MonsterTableData
{
        public int MON_ID { get; set; }
        public string MON_NAME { get; set; }
        public int MON_TYPE { get; set; }
        public int MONHP { get; set; }
        public int MONATT { get; set; }
        public int MONDEF { get; set; }
        public string SPAWN_BIOME { get; set; }
        public int MONSKILL1_ID { get; set; }
        public string MONSKILL1_ANIM { get; set; }
        public int MONSKILL2_ID { get; set; }
        public string MONSKILL2_ANIM { get; set; }
        public int ORDER { get; set; }

    public string StringMonName => DataTableManger.StringTable.Get(MON_NAME);

    public override string ToString()
    {
        return $"{MON_ID} / {MON_NAME} / HP:{MONHP} / ATT:{MONATT} / DEF:{MONDEF}";
    }
}

public class MonsterTable : DataTable
{
    private readonly Dictionary<int, MonsterTableData> table = new Dictionary<int, MonsterTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<MonsterTableData>(textAsset.text);
        foreach (var monster in list)
        {
            if (!table.ContainsKey(monster.MON_ID))
            {
                table.Add(monster.MON_ID, monster);
            }
            else
            {
                Debug.LogError("몬스터 아이디 중복!");
            }
        }
    }

    public MonsterTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public List<MonsterTableData> GetAllMonsters()
    {
        return new List<MonsterTableData>(table.Values);
    }

    public List<MonsterTableData> GetMonstersByType(int monsterType)
    {
        var result = new List<MonsterTableData>();
        foreach (var monster in table.Values)
        {
            if (monster.MON_TYPE == monsterType)
            {
                result.Add(monster);
            }
        }
        return result;
    }

    //public List<MonsterTableData> GetMonstersByBiome(int biome)
    //{
    //    var result = new List<MonsterTableData>();
    //    foreach (var monster in table.Values)
    //    {
    //        if (monster.SpawnBiomes.Contains(biome))
    //        {
    //            result.Add(monster);
    //        }
    //    }
    //    return result;
    //}
}