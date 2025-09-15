using UnityEngine;

public enum StatusType
{
    Default,
    Cold,           // ����
    FoodPoisoning,  // ���ߵ�
    HighFever,      // ��
    Infection,      // ����
    Scratches,      // ��ó
    Bleeding,       // ����
    Fracture,       // ����
    Dirty,          // ������
    Hungry,         // �����
    Fatigue,        // �Ƿ�
    PassOut,        // ����
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
