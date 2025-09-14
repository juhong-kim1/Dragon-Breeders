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

    private float growSpeed = 5f;

    private float hungryTimer = 0f;
    private float hungryMaxTime = 3f;

    private float cleanTimer = 0f;
    private float cleanMaxTime = 15f;

    public bool isPassOut = false;
    public bool hasTriggerPassOut = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateGrowthStats();
    }

    private void Update()
    {
        UpdateGrowth();
        UpdateStats();
        CheckPassOutStat();
    }

    private void UpdateGrowth()
    {
        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                targetScale = Vector3.one * 0.2f;
                break;
            case DragonGrowthState.GrowingUp:
                targetScale = Vector3.one * 0.4f;
                break;
            case DragonGrowthState.Maturity:
                targetScale = Vector3.one * 0.6f;
                break;
            case DragonGrowthState.Adult:
                targetScale = Vector3.one * 1f;
                break;
        }

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

    private void UpdateGrowthStats()
    {
        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                stats.maxStamina = 100;
                stats.experienceMax = 100f;
                break;
            case DragonGrowthState.GrowingUp:
                stats.maxStamina = 150;
                stats.experienceMax = 100f;
                break;
            case DragonGrowthState.Maturity:
                stats.maxStamina = 200;
                stats.experienceMax = 100f;
                break;
            case DragonGrowthState.Adult:
                stats.maxStamina = 250;
                stats.experienceMax = 100f;
                break;
        }

        stats.stamina = stats.maxStamina;
    }

    public void GrowUp()
    {
        if (currentGrowth < DragonGrowthState.Adult)
        {
            currentGrowth++;
            UpdateGrowthStats();
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
            UpdateGrowthStats();
        }
    }
}
