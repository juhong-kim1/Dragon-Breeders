using System.Collections.Generic;
using UnityEngine;

public class NurtureTableData
{
    public int ID { get; set; }
    public string NURTURE_NAME { get; set; }
    public int DEPLETE_HYG { get; set; }
    public int RECEIVE_FTG { get; set; }
    public int REC_TYPE { get; set; } 
    public int REC_PERCENT { get; set; }
    public int EXPGROWTH { get; set; }
    public int RATE_DISEASE { get; set; }
    public int RATE_INJURY { get; set; }
    public int TIME { get; set; }
    public int ACTIVATION_TYPE { get; set; }
    public int UNLOCK_TYPE { get; set; }
    public int ORDER { get; set; }

    public override string ToString()
    {
        return $"{ID} / {NURTURE_NAME}";
    }
}

public class NurtureTable : DataTable
{
    private readonly Dictionary<int, NurtureTableData> table = new Dictionary<int, NurtureTableData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<NurtureTableData>(textAsset.text);
        foreach (var nurture in list)
        {
            if (!table.ContainsKey(nurture.ID))
            {
                table.Add(nurture.ID, nurture);
            }
            else
            {
                Debug.LogError("너츄얼 아이디 중복!");
            }
        }
    }

    public NurtureTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
