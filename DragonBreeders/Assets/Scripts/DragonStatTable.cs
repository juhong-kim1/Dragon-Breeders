using System.Collections.Generic;
using UnityEngine;

public class DragonStatTableData
{
    public int ID { get; set; }
    public string DRAGON_NAME { get; set; }
    public int SPECIES_TYPE { get; set; }
    public int ELEMENT_TYPE { get; set; }
    public int GROWTH_TYPE { get; set; }
    public int MAXHP { get; set; }
    public int MAXFTG { get; set; }
    public int MAXHYG { get; set; }
    public int MAXFOOD { get; set; }
    public int MAXFRN { get; set; }
    public int EXPED_SPECIALTY_ID { get; set; }
    public float SCALE_SIZE { get; set; }
    public int EXP_REQ { get; set; }
    public float DEPRATE_FOOD { get; set; }
    public float DEPRATE_HYG { get; set; }
    public float DEPRATE_FRN { get; set; }
    public string DRAGON_IMAGE { get; set; }
    public int ORDER { get; set; }

    public override string ToString()
    {
        return $"{ID} / {DRAGON_NAME} / 성장:{GROWTH_TYPE} / HP:{MAXHP}";
    }
}

public class DragonStatTable : DataTable
{
    private readonly Dictionary<int, DragonStatTableData> table = new Dictionary<int, DragonStatTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);

        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<DragonStatTableData>(textAsset.text);
        foreach (var dragon in list)
        {
            if (!table.ContainsKey(dragon.ID))
            {
                table.Add(dragon.ID, dragon);
            }
            else
            {
                Debug.LogError("드래곤 아이디 중복!");
            }
        }
    }

    public DragonStatTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public DragonStatTableData GetByTypes(int speciesType, int elementType, int growthType)
    {
        foreach (var dragon in table.Values)
        {
            if (dragon.SPECIES_TYPE == speciesType && dragon.ELEMENT_TYPE == elementType && dragon.GROWTH_TYPE == growthType)
            {
                return dragon;
            }
        }
        return null;
    }

    public List<DragonStatTableData> GetAllDragons()
    {
        return new List<DragonStatTableData>(table.Values);
    }

    public List<DragonStatTableData> GetDragonsBySpeciesAndElement(int speciesType, int elementType)
    {
        var result = new List<DragonStatTableData>();
        foreach (var dragon in table.Values)
        {
            if (dragon.SPECIES_TYPE == speciesType && dragon.ELEMENT_TYPE == elementType)
            {
                result.Add(dragon);
            }
        }
        return result;
    }
}
