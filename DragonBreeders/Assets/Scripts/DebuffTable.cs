using System.Collections.Generic;
using UnityEngine;

public class DebuffTableData
{
    public int ID { get; set; }
    public string DEBUFF_NAME { get; set; }
    public int DEBUFF_TYPE { get; set; }
    public int EFFECT_TYPE { get; set; }
    public float VALUE { get; set; }
    public int TRIGGER_TYPE { get; set; }
    public float TRIGGER_RATE { get; set; }
    public int CURE_TYPE { get; set; }
    public int ORDER { get; set; }

    public override string ToString()
    {
        return $"{ID} / {DEBUFF_NAME}";
    }
}


public class DebuffTable : DataTable
{
    private readonly Dictionary<int, DebuffTableData> table = new Dictionary<int, DebuffTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<DebuffTableData>(textAsset.text);
        foreach (var debuff in list)
        {
            if (!table.ContainsKey(debuff.ID))
            {
                table.Add(debuff.ID, debuff);
            }
            else
            {
                Debug.LogError("디버프 아이디 중복!");
            }
        }
    }

    public DebuffTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
