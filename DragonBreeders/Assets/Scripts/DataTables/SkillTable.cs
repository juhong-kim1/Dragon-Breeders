using System.Collections.Generic;
using UnityEngine;

public class SkillTableData
{
    public int SKILL_ID { get; set; }
    public string SKILL_NAME { get; set; }
    public int SKILL_TYPE { get; set; }
    public int SKILL_CD { get; set; }
    public int SKILL_POWER { get; set; }
    public string SKILL_DESCRIPTION { get; set; }
    public string SKILL_EFFECT { get; set; }
    public string SKILL_HIT { get; set; }
    public int ORDER { get; set; }

    public override string ToString()
    {
        return $"{SKILL_ID} / {SKILL_NAME} / Power:{SKILL_POWER} / CD:{SKILL_CD}";
    }
}

public class SkillTable : DataTable
{
    private readonly Dictionary<int, SkillTableData> table = new Dictionary<int, SkillTableData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            return;
        }

        var list = LoadCSV<SkillTableData>(textAsset.text);
        foreach (var skill in list)
        {
            if (!table.ContainsKey(skill.SKILL_ID))
            {
                table.Add(skill.SKILL_ID, skill);
            }
            else
            {
                Debug.LogError("스킬 아이디 중복!");
            }
        }
    }

    public SkillTableData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public List<SkillTableData> GetAllSkills()
    {
        return new List<SkillTableData>(table.Values);
    }

    public List<SkillTableData> GetSkillsByType(int skillType)
    {
        var result = new List<SkillTableData>();
        foreach (var skill in table.Values)
        {
            if (skill.SKILL_TYPE == skillType)
            {
                result.Add(skill);
            }
        }
        return result;
    }

    public List<SkillTableData> GetDragonSkills()
    {
        return GetSkillsByType(1);
    }

    public List<SkillTableData> GetMonsterSkills()
    {
        return GetSkillsByType(2);
    }
}
