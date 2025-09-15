using UnityEngine;
using System.Collections.Generic;

[System.Flags]
public enum StatusType
{
    Default = 0,
    Cold = 1,           // 감기
    FoodPoisoning = 2,  // 식중독
    HighFever = 4,      // 고열
    Infection = 8,      // 감염
    Scratches = 16,     // 상처
    Bleeding = 32,      // 출혈
    Fracture = 64,      // 골절
    Dirty = 128,        // 더러움
    Hungry = 256,       // 배고픔
    Fatigue = 512,      // 피로
    PassOut = 1024,     // 기절
}
[System.Serializable]
public class DragonStatus
{
    public StatusType Type;

    public float timer = 0f;
    public float maxTime = 60f;

    public StatusType CheckStatusByStats(DragonStats stats)
    {
        if (stats.fatigue >= stats.maxFatigue) return StatusType.PassOut;

        if (stats.fatigue > (stats.maxFatigue * 0.8)) return StatusType.Fatigue;
        if (stats.hunger < (stats.maxHunger * 0.2)) return StatusType.Hungry;
        if (stats.clean < (stats.maxClean * 0.2)) return StatusType.Dirty;

        return StatusType.Default;
    }

    public void EffectImmediateByStatus(StatusType status, DragonStats stats)
    {
        switch (status)
        {
            case StatusType.PassOut:
                stats.ChangeStat(StatType.Intimacy, -50);
                break;
            case StatusType.Fracture:
            case StatusType.Hungry:
                stats.maxFatigue -= 30;
                break;
            case StatusType.Scratches:
            case StatusType.Dirty:
                //탐험시 질병확률 증가
                break;
            
        } 
    }

    public void EffectByStatus(StatusType status, DragonStats stats)
    {
        switch (status)
        {
            case StatusType.Bleeding:
            case StatusType.Fatigue:
                if(timer>=maxTime) stats.ChangeStat(StatType.Stamina, -2);
                break;
        }



    }

}
