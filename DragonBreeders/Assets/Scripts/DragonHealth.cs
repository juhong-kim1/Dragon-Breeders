using UnityEngine;

public enum DragonGrowthState
{
    Infancy,
    GrowingUp,
    Maturity,
    Adult,
}

public class DragonHealth : MonoBehaviour
{
    public static readonly string isPassOutTrigger = "IsPassOut";

    public DragonStats stats;
    private Animator animator;
    public DragonGrowthState currentGrowth;
    private Vector3 targetScale;

    public int currentSpeciesType = 1;
    public int currentElementType = 1;

    private DragonStatTableData currentTableData;

    private float growSpeed = 5f;

    private float hungryTimer = 0f;
    private float hungryMaxTime = 3f;

    private float cleanTimer = 0f;
    private float cleanMaxTime = 15f;

    public bool isPassOut = false;
    public bool hasTriggerPassOut = false;

    public StatusType currentStatus = StatusType.Default;

    private float statusCheckTimer = 0f;
    private float statusCheckMaxTime = 180f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //UpdateGrowthStats();
        ApplyTableData();
    }

    private void Update()
    {
        UpdateGrowth();
        UpdateStats();
        CheckPassOutStat();
        CheckDragonStatus();
    }

    private void ApplyTableData()
    {
        int growthType = GetGrowthTypeFromState(currentGrowth);
        currentTableData = DataTableManger.DragonStatTable.GetByTypes(currentSpeciesType, currentElementType, growthType);

        if (currentTableData == null)
        {

            Debug.LogError($"테이블 데이터를 찾을 수 없습니다: Species:{currentSpeciesType}, Element:{currentElementType}, Growth:{growthType}");

            targetScale = Vector3.one * 0.2f;
            return;
        }

        if (stats == null)
        {
            stats = new DragonStats();
        }

        stats.maxStamina = currentTableData.MAXHP;
        stats.maxFatigue = currentTableData.MAXFTG;
        stats.maxhunger = currentTableData.MAXFOOD;
        stats.maxClean = currentTableData.MAXHYG;
        stats.maxIntimacy = currentTableData.MAXFRN;
        stats.experienceMax = currentTableData.EXP_REQ;

        if (stats.stamina <= 0) stats.stamina = stats.maxStamina;
        if (stats.hunger <= 0) stats.hunger = stats.maxhunger;
        if (stats.clean <= 0) stats.clean = stats.maxClean;

        if (currentGrowth == DragonGrowthState.Adult)
        {
            targetScale = Vector3.one;
        }
        else
        {
            targetScale = Vector3.one * currentTableData.SCALE_SIZE;
        }

        hungryMaxTime = currentTableData.DEPRATE_FOOD * 4f;
        cleanMaxTime = currentTableData.DEPRATE_HYG * 15f;
    }

    private int GetGrowthTypeFromState(DragonGrowthState state)
    {
        switch (state)
        {
            case DragonGrowthState.Infancy: return 1;
            case DragonGrowthState.GrowingUp: return 2;
            case DragonGrowthState.Maturity: return 3;
            case DragonGrowthState.Adult: return 3;
            default: return 1;
        }
    }

    private void UpdateGrowth()
    {
        //switch (currentGrowth)
        //{
        //    case DragonGrowthState.Infancy:
        //        targetScale = Vector3.one * 0.2f;
        //        break;
        //    case DragonGrowthState.GrowingUp:
        //        targetScale = Vector3.one * 0.4f;
        //        break;
        //    case DragonGrowthState.Maturity:
        //        targetScale = Vector3.one * 0.6f;
        //        break;
        //    case DragonGrowthState.Adult:
        //        targetScale = Vector3.one * 1f;
        //        break;
        //}

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);

    }

    private void UpdateStats()
    {
        if (isPassOut)
            return;

        hungryTimer += Time.deltaTime;
        if (hungryTimer >= hungryMaxTime)
        {
            stats.ChangeStat(StatType.Hunger, -10);

            if (stats.hunger <= 0)
            {
                stats.ChangeStat(StatType.Fatigue, 15);
            }

            hungryTimer = 0f;
        }

        cleanTimer += Time.deltaTime;
        if (cleanTimer >= cleanMaxTime)
        {
            stats.ChangeStat(StatType.Clean, -2);
            cleanTimer = 0f;
        }
    }

    private void OnPassOut()
    {
        isPassOut = true;
        hasTriggerPassOut = false;
        currentStatus = StatusType.PassOut;
        animator.SetTrigger(isPassOutTrigger);
    }

    private void CheckPassOutStat()
    {
        if (stats.IsStatPassOut(StatType.Fatigue))
        {
            if (!isPassOut)
            {
                OnPassOut();
            }
        }

    }

    public void Recover()
    {
        isPassOut = false;
        hasTriggerPassOut = true;
        stats.ChangeStat(StatType.Fatigue, -30);
        animator.Rebind();
    }

    //private void UpdateGrowthStats()
    //{
    //    switch (currentGrowth)
    //    {
    //        case DragonGrowthState.Infancy:
    //            stats.maxStamina = 100;
    //            stats.experienceMax = 100f;
    //            break;
    //        case DragonGrowthState.GrowingUp:
    //            stats.maxStamina = 150;
    //            stats.experienceMax = 100f;
    //            break;
    //        case DragonGrowthState.Maturity:
    //            stats.maxStamina = 200;
    //            stats.experienceMax = 100f;
    //            break;
    //        case DragonGrowthState.Adult:
    //            stats.maxStamina = 250;
    //            stats.experienceMax = 100f;
    //            break;
    //    }

    //    stats.stamina = stats.maxStamina;
    //}

    public void GrowUp()
    {
        if (currentGrowth < DragonGrowthState.Adult)
        {
            currentGrowth++;
            ApplyTableData();
            //UpdateGrowthStats();
            currentStatus = StatusType.Default;
        }
    }

    public void StartResting()
    {
        if (stats.fatigue < 60)
        {
            return;
        }

        if (isPassOut) return;

        int expGained = stats.CalculateExperience();
        stats.ChangeStat(StatType.Experience, expGained);

        stats.ChangeStat(StatType.Fatigue, -30);

        CheckGrowth();
    }

    private void CheckGrowth()
    {
        if (stats.CanGrowUp() && currentGrowth < DragonGrowthState.Adult)
        {
            stats.ConsumeGrowthExperience();
            currentGrowth++;
            ApplyTableData();
            // UpdateGrowthStats();
        }
    }

    private void CheckDragonStatus()
    {
        if (currentStatus == StatusType.Default)
        {
            statusCheckTimer += Time.deltaTime;

            if (statusCheckTimer >= statusCheckMaxTime)
            {
                StatusType newStatus = DragonStatus.CheckStatusByStats(stats);

                if (newStatus != StatusType.Default)
                {
                    currentStatus = newStatus;
                }

                statusCheckTimer = 0;
            }
        }
    }
}
