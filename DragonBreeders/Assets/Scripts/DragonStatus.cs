using UnityEngine;
using System.Collections.Generic;

[System.Flags]
public enum StatusType
{
    Default = 0,
    Cold = 1,           // ����
    FoodPoisoning = 2,  // ���ߵ�
    HighFever = 4,      // ��
    Infection = 8,      // ����
    Scratches = 16,     // ��ó
    Bleeding = 32,      // ����
    Fracture = 64,      // ����
    Dirty = 128,        // ������
    Hungry = 256,       // �����
    Fatigue = 512,      // �Ƿ�
    PassOut = 1024,     // ����
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
                //Ž��� ����Ȯ�� ����
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
