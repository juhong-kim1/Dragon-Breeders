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
    private float hungryMaxTime = 60f;

    private float cleanTimer = 0f;
    private float cleanMaxTime = 60f;

    private float intimacyTimer = 0f;
    private float intimacyMaxTime = 60f;

    public bool isPassOut = false;
    public bool hasTriggerPassOut = false;

    public StatusType currentStatuses = StatusType.None;

    private float rotationSpeed = 360f;
    public bool isFormChanging = false;

    private float rotationTime = 0f;
    private float rotationMaxTime = 2f;

    private float flySpeed = 5f;

    public ParticleSystem fireBreath;
    public ParticleSystem fire;

    private string infancyText = "유아기";
    private string growingUpText = "성장기";
    private string maturityText = "성숙기";
    private string adultText = "성인";
    public string currentGrowthText;

    public bool isLoadedFromSave = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //UpdateGrowthStats();
        if (status == null)
        {
            status = new DragonStatus();
        }

        ApplyTableData();

        if (!isLoadedFromSave)
        {
            currentGrowthText = infancyText;
        }
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

        if (!isLoadedFromSave)
        {
            stats.experienceMax = currentTableData.EVOEXP;
        }

        stats.dragonSpecies = currentTableData.StringSpecies;

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
        hungryTimer += Time.deltaTime;
        if (hungryTimer >= hungryMaxTime)
        {
            stats.ChangeStat(StatType.Hunger, -currentTableData.DEP_FOOD);

            if (stats.hunger > 0)
            {
                stats.ChangeStat(StatType.Stamina, currentTableData.DEP_FOOD);

                if (isPassOut && stats.stamina >= 1f)
                {
                    isPassOut = false;
                    status.RemoveStatus(StatusType.PassOut);
                    GetComponent<DragonBehavior>().PlayRecoverAnimation();
                    AlarmManager.Instance.ShowAlarm("드래곤이 의식을 되찾았어요!");
                    Debug.Log("자동 회복: 체력이 1 이상이 되어 깨어남");
                }
            }

            hungryTimer = 0f;
        }

        cleanTimer += Time.deltaTime;
        if (cleanTimer >= cleanMaxTime)
        {
            stats.ChangeStat(StatType.Clean, -currentTableData.DEP_HYG);
            cleanTimer = 0f;
        }

        intimacyTimer += Time.deltaTime;
        if (intimacyTimer >= intimacyMaxTime)
        {
            stats.ChangeStat(StatType.Intimacy, -currentTableData.DEP_FRN);
            intimacyTimer = 0f;
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
        stats.ChangeStat(StatType.Fatigue, -50f);
        GameManager.Instance.passOutCount++;
        AlarmManager.Instance.ShowAlarm("드래곤이 의식을 잃었어요!");
        animator.SetTrigger(isPassOutTrigger);
    }

    private void CheckPassOutStat()
    {
        if (stats.IsStatPassOut())
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

        float fatigueRecovery = stats.maxFatigue * data.REC_PERCENT / 100;
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
            stats.stamina = stats.maxStamina;

            isFormChanging = true;

            string Action = DragonBehavior.action[2];

            animator.SetTrigger(Action);
        }
        else 
        {
            GameManager.Instance.releaseButton.gameObject.SetActive(true);
        }

        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                currentGrowthText = infancyText;
                break;
            case DragonGrowthState.GrowingUp:
                currentGrowthText = growingUpText;
                break;
            case DragonGrowthState.Maturity:
                currentGrowthText = maturityText;
                break;
            case DragonGrowthState.Adult:
                currentGrowthText = adultText;
                break;
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

    public void GainExperience(float amount)
    {
        if (stats == null) return;

        stats.ChangeStat(StatType.Experience, amount);

        Debug.Log($"경험치 획득: {amount}, 현재 경험치: {stats.experience}/{stats.experienceMax}");

        if (stats.CanGrowUp())
        {
            GrowUp();
            stats.ConsumeGrowthExperience();
            Debug.Log("드래곤이 성장했습니다!");
        }

        if (currentGrowth == DragonGrowthState.Adult)
        {
            GameManager.Instance.releaseButton.gameObject.SetActive(true);
        }
    }

    public void ReleaseDragon()
    {
        transform.Translate(Vector3.up * flySpeed * Time.deltaTime, Space.World);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        string Action = DragonBehavior.play;

        animator.SetTrigger(Action);

        Destroy(gameObject, 5f);
    }
}
