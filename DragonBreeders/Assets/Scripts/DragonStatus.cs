using UnityEngine;

public enum StatusType
{
    Default,
    Cold,           // 감기
    FoodPoisoning,  // 식중독
    HighFever,      // 고열
    Infection,      // 감염
    Scratches,      // 상처
    Bleeding,       // 출혈
    Fracture,       // 골절
    Dirty,          // 더러움
    Hungry,         // 배고픔
    Fatigue,        // 피로
    PassOut,        // 기절
}

public class DragonStatus
{
    public StatusType Type;

    public static StatusType CheckStatusByStats(DragonStats stats)
    {
        if (stats.fatigue >= 100) return StatusType.PassOut;

        if (stats.clean < 15 && Random.value < 0.25f) return StatusType.Infection;
        if (stats.fatigue > 80 && Random.value < 0.2f) return StatusType.Fatigue;
        if (stats.hunger < 30 && Random.value < 0.15f) return StatusType.Hungry;
        if (stats.clean < 40 && Random.value < 0.1f) return StatusType.Dirty;


        return StatusType.Default;
    }

}
