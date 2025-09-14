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
    public Animator animator;
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

    private bool Touch3()
    {
        if (Input.touchCount == 3)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Touch touch2 = Input.GetTouch(2);

            if (touch0.phase == TouchPhase.Began && touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
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
                break;
            case DragonGrowthState.GrowingUp:
                stats.maxStamina = 150;
                break;
            case DragonGrowthState.Maturity:
                stats.maxStamina = 200;
                break;
            case DragonGrowthState.Adult:
                stats.maxStamina = 250;
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
    
    }
}
