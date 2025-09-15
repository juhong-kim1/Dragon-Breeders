using UnityEngine;
using System.Collections.Generic;
using System;

[Flags]
public enum StatusType
{
    None = 0,
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
    [SerializeField] private StatusType currentStatuses = StatusType.None;
    private Dictionary<StatusType, float> statusTimers = new Dictionary<StatusType, float>();
    private StatusType previousStatuses = StatusType.None;

    public float maxTimer = 60f;

    public bool HasStatus(StatusType status)
    {
        return (currentStatuses & status) != 0;
    }

    public void AddStatus(StatusType status)
    {
        if (!HasStatus(status))
        {
            currentStatuses |= status;
            statusTimers[status] = 0f;
        }
    }

    public void RemoveStatus(StatusType status)
    {
        currentStatuses &= ~status;
        statusTimers.Remove(status);
    }

    public StatusType CheckStatusByStats(DragonStats stats)
    {
        StatusType newStatuses = StatusType.None;

        if (stats.fatigue >= stats.maxFatigue)
        { 
            newStatuses |= StatusType.PassOut;
        }
        else if (stats.fatigue > (stats.maxFatigue * 0.8f))
        {
            newStatuses |= StatusType.Fatigue;
        }
        if (stats.hunger < (stats.maxHunger * 0.2f))
        {
            newStatuses |= StatusType.Hungry;
        }
        if (stats.clean < (stats.maxClean * 0.2f))
        {
            newStatuses |= StatusType.Dirty;
        }

 
        StatusType addedStatuses = newStatuses & ~previousStatuses;
        if (addedStatuses != StatusType.None)
        {
            ApplyImmediateEffects(addedStatuses, stats);
        }

        currentStatuses = newStatuses;
        previousStatuses = newStatuses;

        return currentStatuses;
    }

    private void ApplyImmediateEffects(StatusType statuses, DragonStats stats)
    {
        if ((statuses & StatusType.PassOut) != 0)
            stats.ChangeStat(StatType.Intimacy, -50);

        if ((statuses & (StatusType.Fracture | StatusType.Hungry)) != 0)
            stats.maxFatigue -= 30;
    }

    public void UpdateTimersAndEffects(DragonStats stats)
    {
        for (int i = 0; i < 11; i++)
        {
            StatusType status = (StatusType)(1 << i);
            if (HasStatus(status))
            {
                if (!statusTimers.ContainsKey(status))
                    statusTimers[status] = 0f;

                statusTimers[status] += Time.deltaTime;
                EffectByStatus(status, stats);
            }
        }
    }

    public void EffectByStatus(StatusType status, DragonStats stats)
    {
        if (!statusTimers.ContainsKey(status)) return;

        float timer = statusTimers[status];

        switch (status)
        {
            case StatusType.Bleeding:
            case StatusType.Fatigue:
                if (timer >= maxTimer)
                {
                    stats.ChangeStat(StatType.Stamina, -2);
                    statusTimers[status] = 0f;
                }
                break;
        }
    }
}

