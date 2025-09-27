using System.Collections.Generic;
using UnityEngine;

public class DragonStatTableData
{
    public int DRAGON_ID { get; set; }
    public string DRAGON_NAME { get; set; }
    public int SPECIES_TYPE { get; set; }
    public int ELEMENT_TYPE { get; set; }
    public int GROWTH_TYPE { get; set; }
    public float MAXHP { get; set; }
    public float MAXFTG { get; set; }
    public float MAXHYG { get; set; }
    public float MAXFOOD { get; set; }
    public float MAXFRN { get; set; }
    public float SCALE_SIZE { get; set; }
    public int EVOEXP { get; set; }
    public float DEP_FOOD { get; set; }
    public float DEP_HYG { get; set; }
    public float DEP_FRN { get; set; }
    public float ATT_MULT { get; set; }
    public float DEF_MULT { get; set; }
    public float GROWTH_MULT { get; set; }
    public int SKILL1_ID { get; set; }
    public string SKILL1_ANIM { get; set; }
    public int SKILL2_ID { get; set; }
    public string SKILL2_ANIM { get; set; }
    public int ORDER_1 { get; set; }

    public string StringSpecies => DataTableManger.StringTable.Get(DRAGON_NAME);

    public override string ToString()
    {
        return $"{DRAGON_ID} / {DRAGON_NAME} / 성장:{GROWTH_TYPE} / HP:{MAXHP}";
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
            if (!table.ContainsKey(dragon.DRAGON_ID))
            {
                table.Add(dragon.DRAGON_ID, dragon);
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