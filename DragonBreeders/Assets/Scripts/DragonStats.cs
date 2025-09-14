using UnityEngine;

public enum StatType
{ 
    Stamina,
    Fatigue,
    Hunger,
    Intimacy,
    Clean,
}


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
    public int maxhunger = 100;
    public int maxIntimacy = 100;
    public int maxClean = 100;

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
                hunger = Mathf.Clamp(hunger + amount, 0, maxhunger);
                break;
            case StatType.Intimacy:
                intimacy = Mathf.Clamp(intimacy + amount, 0, maxIntimacy);
                break;
            case StatType.Clean:
                clean = Mathf.Clamp(clean + amount, 0, maxClean);
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

}
