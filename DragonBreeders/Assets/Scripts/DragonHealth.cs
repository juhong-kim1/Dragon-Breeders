using System.Runtime.CompilerServices;
using UnityEngine;

public enum DragonGrowthState
{
    Infancy,
    GrowingUp,
    Maturity,
    Adult,
}

public class DragonHealth : LivingEntity
{
    public DragonGrowthState currentGrowth;
    private Vector3 targetScale;

    private float growSpeed = 5f;

    private float hungryTimer = 0f;
    private float hungryMaxTime = 10f;

    private float cleanTimer = 0f;
    private float cleanMaxTime = 15f;

    private void Start()
    {
        stamina = 100;
        vitality = 110;
        clean = 90;
        full = 100;
        intimacy = 0;
    }

    private void Update()
    {

        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                UpdateInfancy();
                break;
            case DragonGrowthState.GrowingUp:
                UpdateGrowingUp();
                break;
            case DragonGrowthState.Maturity:
                UpdateMaturity();
                break;
            case DragonGrowthState.Adult:
                UpdateAdult();
                break;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);

        OnHungry();
        OnClean();
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

    private void UpdateInfancy()
    {
        targetScale = new Vector3(0.2f, 0.2f, 0.2f);
        if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGrowth = DragonGrowthState.GrowingUp;
        }

        stamina = 100;
    }

    private void UpdateGrowingUp()
    {
        targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGrowth = DragonGrowthState.Maturity;
        }

        stamina = 150;

    }

    private void UpdateMaturity()
    {
        targetScale = new Vector3(0.6f, 0.6f, 0.6f);
        if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGrowth = DragonGrowthState.Adult;
        }

        stamina = 200;
    }


    private void UpdateAdult()
    {
        targetScale = new Vector3(1f, 1f, 1f);
        if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGrowth = DragonGrowthState.Infancy;
        }

        stamina = 250;
    }

    private void OnHungry()
    {
        hungryTimer += Time.deltaTime;

        if (hungryTimer > hungryMaxTime)
        {
            full -= 1;

            hungryTimer = 0f;
        }
    }

    private void OnClean()
    {
        cleanTimer += Time.deltaTime;

        if (cleanTimer > cleanMaxTime)
        {
            clean -= 2;

            cleanTimer = 0f;
        }
    }
}
