using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTableData
{
    public int DROP_ID { get; set; }
    public int ITEM_ID { get; set; }
    public string DROP_BIOME { get; set; }
    public int DROP_RATE { get; set; }
    public int MINDROP { get; set; }
    public int MAXDROP { get; set; }
    public int ORDER { get; set; }

    public List<int> GetBiomeList()
    {
        if (string.IsNullOrEmpty(DROP_BIOME))
            return new List<int>();

        return DROP_BIOME.Split(',')
                         .Select(x => int.Parse(x.Trim()))
                         .ToList();
    }

    public bool CanDropInBiome(int biome)
    {
        return GetBiomeList().Contains(biome);
    }

    public override string ToString()
    {
        return $"DROP_ID: {DROP_ID}, ITEM_ID: {ITEM_ID}, DROP_BIOME: {DROP_BIOME}, DROP_RATE: {DROP_RATE}%";
    }
}

public class DropTable : DataTable
{
    private readonly Dictionary<int, DropTableData> table = new Dictionary<int, DropTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"DropTable 파일을 찾을 수 없습니다: {filename}");
            return;
        }

        var list = LoadCSV<DropTableData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.DROP_ID))
            {
                table.Add(item.DROP_ID, item);
            }
            else
            {
                Debug.LogError($"드롭 아이디 중복! DROP_ID: {item.DROP_ID}");
            }
        }

        Debug.Log($"DropTable 로드 완료: {table.Count}개 아이템");
    }

    public DropTableData Get(int dropId)
    {
        if (!table.ContainsKey(dropId))
        {
            return null;
        }
        return table[dropId];
    }

    public List<DropTableData> GetAll()
    {
        return new List<DropTableData>(table.Values);
    }

    public List<DropTableData> GetDropsByBiome(int biome)
    {
        return table.Values.Where(drop => drop.CanDropInBiome(biome) && !IsCoin(drop.ITEM_ID)).ToList();
    }

    private bool IsCoin(int itemId)
    {
        return itemId == 5070001;
    }
}
