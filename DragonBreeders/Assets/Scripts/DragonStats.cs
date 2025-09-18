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
    public int stamina = 100;
    public int fatigue = 0;
    public int hunger = 100;
    public int intimacy = 0;
    public int clean = 100;

    [Header("MaxValues")]
    public int maxStamina = 250;
    public int maxFatigue = 100;
    public int maxHunger = 100;
    public int maxIntimacy = 100;
    public int maxClean = 100;

    [Header("GrowthSystem")]
    public float experience = 0;
    public float experienceMax = 100;

    public void ChangeStat(StatType statType, int amount)
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

    public int CalculateExperience()
    {
        return (stamina + hunger + clean + intimacy);
    }

    public bool CanGrowUp()
    {
        return experience >= experienceMax;
    }

    public void ConsumeGrowthExperience()
    {
        experience -= experienceMax;
        if (experience < 0) experience = 0;
    }
}
