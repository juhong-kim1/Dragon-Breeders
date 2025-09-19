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
    public DragonStatus status;
    private Animator animator;
    public DragonGrowthState currentGrowth;
    private Vector3 targetScale;

    public int currentSpeciesType = 1;
    public int currentElementType = 1;

    public DragonStatTableData currentTableData;

    private float growSpeed = 1.2f;

    private float hungryTimer = 0f;
    private float hungryMaxTime = 10f; //기존 3

    private float cleanTimer = 0f;
    private float cleanMaxTime = 20f; //기존 15

    public bool isPassOut = false;
    public bool hasTriggerPassOut = false;

    public StatusType currentStatuses = StatusType.None;

    private float rotationSpeed = 360f;
    public bool isFormChanging = false;

    private float rotationTime = 0f;
    private float rotationMaxTime = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //UpdateGrowthStats();
        if (status == null)
        {
            status = new DragonStatus();
        }

        ApplyTableData();
    }

    private void Update()
    {
        UpdateGrowth();
        UpdateStats();
        CheckDragonStatus();
        CheckPassOutStat();
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
        stats.maxHunger = currentTableData.MAXFOOD;
        stats.maxClean = currentTableData.MAXHYG;
        stats.maxIntimacy = currentTableData.MAXFRN;
        stats.experienceMax = currentTableData.EXP_REQ;

        if (stats.stamina <= 0) stats.stamina = stats.maxStamina;
        if (stats.hunger <= 0) stats.hunger = stats.maxHunger;
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

        if (isFormChanging == true && rotationTime < rotationMaxTime)
        {
            transform.localRotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
            rotationTime += Time.deltaTime;
        }
        else
        { 
            isFormChanging = false;
            rotationTime = 0f;
        }
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
        if (isPassOut)
        {
            return;
        }

        Debug.Log("온패스아웃 호출");

        isPassOut = true;


        hasTriggerPassOut = true;
        status.AddStatus(StatusType.PassOut);
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
        status.RemoveStatus(StatusType.PassOut);

        var data = DataTableManger.NurtureTable.Get(50200);
        if (data == null) return;

        int fatigueRecovery = stats.maxFatigue * data.REC_PERCENT / 100;
       stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);
        animator.Rebind();
    }

    public void GrowUp()
    {
        if (currentGrowth < DragonGrowthState.Adult)
        {
            currentGrowth++;
            ApplyTableData();
            status.RemoveStatus(StatusType.Hungry);
            status.RemoveStatus(StatusType.Fatigue);
            isFormChanging = true;
        }
    }

    private void CheckDragonStatus()
    {
        if (status == null) return;

        currentStatuses = status.CheckStatusByStats(stats);

        status.UpdateTimersAndEffects(stats);

        //SyncPassOutState();
    }

    //private void SyncPassOutState()
    //{
    //    bool shouldPassOut = status.HasStatus(StatusType.PassOut);

    //    if (shouldPassOut != isPassOut)
    //    {
    //        if (shouldPassOut && !isPassOut)
    //        {
    //            OnPassOut();
    //        }
    //        else if (!shouldPassOut && isPassOut)
    //        {
    //            isPassOut = false;
    //            animator.Rebind();
    //        }
    //    }
    //}

    public void GainExperienceFromStats()
    {
        if (stats == null) return;

        int gainedExp = stats.CalculateExperience();

        stats.ChangeStat(StatType.Experience, gainedExp);

        Debug.Log($"경험치 획득: {gainedExp}, 현재 경험치: {stats.experience}/{stats.experienceMax}");

        if (stats.CanGrowUp())
        {
            GrowUp();
            stats.ConsumeGrowthExperience();
            Debug.Log("드래곤이 성장했습니다!");
        }
    }

}
