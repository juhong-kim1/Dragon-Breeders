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

    private static readonly Dictionary<StatusType, int> StatusToTableID = new Dictionary<StatusType, int>
    {
        { StatusType.Cold, 20101 },
        { StatusType.FoodPoisoning, 20102 },
        { StatusType.HighFever, 20103 },
        { StatusType.Infection, 20104 },
        { StatusType.Scratches, 20205 },
        { StatusType.Bleeding, 20201 },
        { StatusType.Fracture, 20204 },
        { StatusType.Dirty, 20305 },
        { StatusType.Hungry, 20304 },
        { StatusType.Fatigue, 20303 },
        { StatusType.PassOut, 20306 }
    };

    public DebuffTableData GetDebuffData(StatusType status)
    {
        if (StatusToTableID.TryGetValue(status, out int id))
        {
            return DataTableManger.DebuffTable.Get(id);
        }
        return null;
    }

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

        if (stats.stamina <= 0)
            newStatuses |= StatusType.PassOut;

        else if (stats.fatigue >= (stats.maxFatigue * 0.8f))
            newStatuses |= StatusType.Fatigue;

        if (stats.hunger <= (stats.maxHunger * 0.2f))
            newStatuses |= StatusType.Hungry;

        if (stats.clean <= (stats.maxClean * 0.2f))
            newStatuses |= StatusType.Dirty;

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
        for (int i = 0; i < 11; i++)
        {
            StatusType status = (StatusType)(1 << i);
            if ((statuses & status) != 0)
            {
                var debuffData = GetDebuffData(status);
                if (debuffData != null && debuffData.TRIGGER_TYPE == 1)
                {
                    ApplyDebuffEffect(debuffData, stats);
                }
            }
        }
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

        var debuffData = GetDebuffData(status);
        if (debuffData == null || debuffData.TRIGGER_TYPE != 2) return;

        float timer = statusTimers[status];
        float interval = 60f / debuffData.TRIGGER_RATE;

        if (timer >= interval)
        {
            ApplyDebuffEffect(debuffData, stats);
            statusTimers[status] = 0f;
        }
    }

    private void ApplyDebuffEffect(DebuffTableData debuffData, DragonStats stats)
    {
        switch (debuffData.EFFECT_TYPE)
        {
            case 1:
                int maxFatigueReduction = Mathf.RoundToInt(stats.maxFatigue * debuffData.VALUE / 100f);
                stats.maxFatigue -= maxFatigueReduction;
                break;
            case 2:
                float hungerDrain = stats.maxHunger * debuffData.VALUE / 100f;
                stats.ChangeStat(StatType.Hunger, -Mathf.RoundToInt(hungerDrain));
                break;
            case 3:
                float fatigueAmount = stats.maxFatigue * debuffData.VALUE / 100f;
                stats.ChangeStat(StatType.Fatigue, Mathf.RoundToInt(fatigueAmount));
                break;
            case 4:
                float staminaDrain = stats.maxStamina * debuffData.VALUE / 100f;
                stats.ChangeStat(StatType.Stamina, -Mathf.RoundToInt(staminaDrain));
                break;
            case 5: // 탐험 시 질병 확률 증가
                break;
            case 6:
                stats.ChangeStat(StatType.Intimacy, -Mathf.RoundToInt(debuffData.VALUE));
                break;
        }
    }

    public void TryApplyRandomDebuff(StatusType status)
    {
        var debuffData = GetDebuffData(status);
        if (debuffData != null &&
            (debuffData.TRIGGER_TYPE == 1 || debuffData.TRIGGER_TYPE == 2))
        {
            float random = UnityEngine.Random.Range(0f, 100f);
            if (random < debuffData.TRIGGER_RATE)
            {
                AddStatus(status);
            }
        }
    }

    public int GetStatusCount()
    {
        int count = 0;
        StatusType temp = currentStatuses;

        while (temp != 0)
        {
            temp &= temp - 1;
            count++;
        }
        return count;
    }

    public float GetDiseaseRiskMultiplier()
    {
        float multiplier = 1f;

        for (int i = 0; i < 11; i++)
        {
            StatusType status = (StatusType)(1 << i);
            if (HasStatus(status))
            {
                var debuffData = GetDebuffData(status);
                if (debuffData != null && debuffData.EFFECT_TYPE == 5)
                {
                    multiplier += debuffData.VALUE / 100f;
                }
            }
        }

        return multiplier;
    }

    public bool CanCureWith(StatusType status, int cureType)
    {
        var debuffData = GetDebuffData(status);
        return debuffData != null && debuffData.CURE_TYPE == cureType;
    }

    public List<StatusType> GetCurableStatuses(int cureType)
    {
        var result = new List<StatusType>();

        for (int i = 0; i < 11; i++)
        {
            StatusType status = (StatusType)(1 << i);
            if (HasStatus(status) && CanCureWith(status, cureType))
            {
                result.Add(status);
            }
        }

        return result;
    }
}

