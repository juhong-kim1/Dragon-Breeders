using UnityEngine;

public enum StatType
{ 
    Stamina,
    Fatigue,
    Hunger,
    Intimacy,
    Clean,
    Experience,
}

[System.Serializable]
public class DragonStats
{
    [Header("CoreStats")]
    public float stamina = 100f;
    public float fatigue = 0f;
    public float hunger = 100f;
    public float intimacy = 0f;
    public float clean = 100f;

    [Header("MaxValues")]
    public float maxStamina = 250f;
    public float maxFatigue = 100f;
    public float maxHunger = 100f;
    public float maxIntimacy = 100f;
    public float maxClean = 100f;

    [Header("GrowthSystem")]
    public float experience = 1;
    public float experienceMax = 100;

    public void ChangeStat(StatType statType, float amount)
    {
        switch (statType)
        {
            case StatType.Stamina:
                stamina = Mathf.Clamp(stamina + amount, 0, maxStamina);
                break;
            case StatType.Fatigue:
                fatigue = Mathf.Clamp(fatigue + amount, 0, maxFatigue);
                break;
            case StatType.Hunger:
                hunger = Mathf.Clamp(hunger + amount, 0, maxHunger);
                break;
            case StatType.Intimacy:
                intimacy = Mathf.Clamp(intimacy + amount, 0, maxIntimacy);
                break;
            case StatType.Clean:
                clean = Mathf.Clamp(clean + amount, 0, maxClean);
                break;
            case StatType.Experience:
                experience = Mathf.Clamp(experience + amount, 0, float.MaxValue);
                break;
        }
    }

    public bool IsStatPassOut(StatType statType)
    {
        switch (statType)
        {
            case StatType.Fatigue:
                return fatigue >= maxFatigue;
        }
        return false;
    }

    public bool CanGrowUp()
    {
        return experience >= experienceMax;
    }

    public void ConsumeGrowthExperience()
    {
        experienceMax += GameManager.Instance.dragonHealth.currentTableData.EVOEXP;
    }
}
