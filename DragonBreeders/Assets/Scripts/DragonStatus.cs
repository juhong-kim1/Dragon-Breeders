using UnityEngine;
using System.Collections.Generic;
using System;

[Flags]
public enum StatusType
{
    None = 0,
    Disease = 1,        // 질병
    Injury = 2,         // 부상
    Dirty = 4,          // 더러움
    Hungry = 8,         // 배고픔
    Fatigue = 16,       // 피로
    PassOut = 32,       // 기절
}

[System.Serializable]
public class DragonStatus
{
    [SerializeField] private StatusType currentStatuses = StatusType.None;
    private Dictionary<StatusType, float> statusTimers = new Dictionary<StatusType, float>();
    private StatusType previousStatuses = StatusType.None;

    [Header("질병/부상 확률 설정")]
    public float diseaseChance = 10f;
    public float injuryChance = 10f;
    public float dirtyDiseaseMultiplier = 2f;

    [Header("디버프 효과")]
    public float attackDebuff = 0.7f;
    public float defenseDebuff = 0.7f;
    public float passOutIntimacyLoss = 50f;

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

            string statusName = GetStatusName(status);
            AlarmManager.Instance?.ShowAlarm($"상태이상: {statusName}");
            Debug.Log($"[DragonStatus] {statusName} 상태 추가됨");
        }
    }

    public void RemoveStatus(StatusType status)
    {
        if (HasStatus(status))
        {
            currentStatuses &= ~status;
            statusTimers.Remove(status);

            string statusName = GetStatusName(status);
            Debug.Log($"[DragonStatus] {statusName} 상태 제거됨");
        }
    }

    public StatusType CheckStatusByStats(DragonStats stats)
    {
        StatusType newStatuses = currentStatuses & (StatusType.Disease | StatusType.Injury);

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

        StatusType removedStatuses = previousStatuses & ~newStatuses;
        if (removedStatuses != StatusType.None)
        {
            if ((removedStatuses & StatusType.Hungry) != 0)
                AlarmManager.Instance?.ShowAlarm("배고픔 상태 해제!");
            if ((removedStatuses & StatusType.Dirty) != 0)
                AlarmManager.Instance?.ShowAlarm("더러움 상태 해제!");
            if ((removedStatuses & StatusType.Fatigue) != 0)
                AlarmManager.Instance?.ShowAlarm("피로 상태 해제!");
        }

        currentStatuses = newStatuses;
        previousStatuses = newStatuses;

        return currentStatuses;
    }

    private void ApplyImmediateEffects(StatusType statuses, DragonStats stats)
    {
        if ((statuses & StatusType.PassOut) != 0)
        {
            stats.ChangeStat(StatType.Intimacy, -passOutIntimacyLoss);
        }
    }

    public void UpdateTimersAndEffects(DragonStats stats)
    {
        foreach (var status in new List<StatusType>(statusTimers.Keys))
        {
            if (HasStatus(status))
            {
                statusTimers[status] += Time.deltaTime;
            }
        }
    }

    public void TryApplyTrainingDebuff()
    {
        float currentDiseaseChance = diseaseChance;

        if (HasStatus(StatusType.Dirty))
        {
            currentDiseaseChance *= dirtyDiseaseMultiplier;
        }

        float diseaseRoll = UnityEngine.Random.Range(0f, 100f);
        if (diseaseRoll < currentDiseaseChance)
        {
            AddStatus(StatusType.Disease);
        }

        float injuryRoll = UnityEngine.Random.Range(0f, 100f);
        if (injuryRoll < injuryChance)
        {
            AddStatus(StatusType.Injury);
        }
    }

    public float GetAttackMultiplier()
    {
        if (HasStatus(StatusType.Disease))
        {
            return attackDebuff;
        }
        return 1f;
    }

    public float GetDefenseMultiplier()
    {
        if (HasStatus(StatusType.Injury))
        {
            return defenseDebuff;
        }
        return 1f;
    }

    public List<StatusType> GetActiveStatuses()
    {
        List<StatusType> activeStatuses = new List<StatusType>();

        if (HasStatus(StatusType.Disease)) activeStatuses.Add(StatusType.Disease);
        if (HasStatus(StatusType.Injury)) activeStatuses.Add(StatusType.Injury);
        if (HasStatus(StatusType.Dirty)) activeStatuses.Add(StatusType.Dirty);
        if (HasStatus(StatusType.Hungry)) activeStatuses.Add(StatusType.Hungry);
        if (HasStatus(StatusType.Fatigue)) activeStatuses.Add(StatusType.Fatigue);
        if (HasStatus(StatusType.PassOut)) activeStatuses.Add(StatusType.PassOut);

        return activeStatuses;
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

    private string GetStatusName(StatusType status)
    {
        switch (status)
        {
            case StatusType.Disease: return "질병";
            case StatusType.Injury: return "부상";
            case StatusType.Dirty: return "더러움";
            case StatusType.Hungry: return "배고픔";
            case StatusType.Fatigue: return "피로";
            case StatusType.PassOut: return "기절";
            default: return "알 수 없음";
        }
    }
}