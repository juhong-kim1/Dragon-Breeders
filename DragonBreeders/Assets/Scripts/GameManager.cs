using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;
    public EggVault vault;

    public GameObject[] dragonPrefabs;
    public Sprite[] icon;


    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI figureText;
    public TextMeshProUGUI hungryText;
    public TextMeshProUGUI intimacyText;
    public TextMeshProUGUI cleanText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI growthStateText;
    public TextMeshProUGUI currentStatusText;

    public TextMeshProUGUI[] mapStatTexts;
    public TextMeshProUGUI[] homeStatTexts;

    private float exploreTimer = 0f;
    private float restTimer = 0f;
    private float feedTimer = 0f;
    private float bathTimer = 0f;
    private float playTimer = 0f;

    private bool canExplore = true;
    private bool canRest = true;
    private bool canFeed = true;
    private bool canBath = true;
    private bool canPlay = true;

    public void Update()
    {
        UpdateStatText();

        if (!canExplore)
        {
            exploreTimer += Time.deltaTime;
            var exploreData = DataTableManger.NurtureTable.Get(50000);

            if (exploreData != null && exploreTimer >= exploreData.TIME)
            {
                canExplore = true;
                exploreTimer = 0f;
            }
        }

        if (!canRest)
        {
            restTimer += Time.deltaTime;
            var restData = DataTableManger.NurtureTable.Get(50200);

            if (restData != null && restTimer >= restData.TIME)
            {
                canRest = true;
                restTimer = 0f;
            }
        }

        if (!canFeed)
        {
            feedTimer += Time.deltaTime;
            var feedData = DataTableManger.NurtureTable.Get(50300);

            if (feedData != null && feedTimer >= feedData.TIME)
            {
                canFeed = true;
                feedTimer = 0f;
            }
        }


        if (!canBath)
        {
            bathTimer += Time.deltaTime;
            var bathData = DataTableManger.NurtureTable.Get(50400);

            if (bathData != null && bathTimer >= bathData.TIME)
            {
                canBath = true;
                bathTimer = 0f;
            }
        }


        if (!canPlay)
        {
            playTimer += Time.deltaTime;
            var playData = DataTableManger.NurtureTable.Get(50500);

            if (playData != null && playTimer >= playData.TIME)
            {
                canPlay = true;
                playTimer = 0f;
            }
        }


        if (dragonHealth.currentGrowth == DragonGrowthState.Adult)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Destroy(dragonHealth.gameObject);

                EggSlot.isDragonActive = false;
            
            }
        
        }


    }

    public void UpdateStatText()
    {
        if (dragonHealth == null) return;

        var stats = dragonHealth.stats;

        if (staminaText) staminaText.text = $"Stamina: {stats.stamina:F0}/{stats.maxStamina:F0}";
        if (hungryText) hungryText.text = $"Hunger: {stats.hunger:F0}";
        if (intimacyText) intimacyText.text = $"Intimacy: {stats.intimacy:F0}";
        if (cleanText) cleanText.text = $"Clean: {stats.clean:F0}";
        if (figureText) figureText.text = $"Fatigue: {stats.fatigue:F0}";
        if (experienceText) experienceText.text = $"Exp: {stats.experience:F0}/{stats.experienceMax:F0}";
        if (growthStateText) growthStateText.text = $"Stage: {dragonHealth.currentGrowth}";
        if (currentStatusText) currentStatusText.text = $"Status: {dragonHealth.currentStatuses}";

        UpdateMapUI(stats);
        UpdateHomeUI(stats);
    }

    private void UpdateMapUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 8)
        {
            mapStatTexts[0].text = $"Stamina: {stats.stamina:F0}";
            mapStatTexts[1].text = $"Hunger: {stats.hunger:F0}";
            mapStatTexts[2].text = $"Intimacy: {stats.intimacy:F0}";
            mapStatTexts[3].text = $"Clean: {stats.clean:F0}";
            mapStatTexts[4].text = $"Fatigue: {stats.fatigue:F0}";
            mapStatTexts[5].text = $"Exp: {stats.experience:F0}/{stats.experienceMax:F0}";
            mapStatTexts[6].text = $"Stage: {dragonHealth.currentGrowth}";
            mapStatTexts[7].text = $"Status: {dragonHealth.currentStatuses}";
        }
    }
    private void UpdateHomeUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 8)
        {
            homeStatTexts[0].text = $"Stamina: {stats.stamina:F0}";
            homeStatTexts[1].text = $"Hunger: {stats.hunger:F0}";
            homeStatTexts[2].text = $"Intimacy: {stats.intimacy:F0}";
            homeStatTexts[3].text = $"Clean: {stats.clean:F0}";
            homeStatTexts[4].text = $"Fatigue: {stats.fatigue:F0}";
            homeStatTexts[5].text = $"Exp: {stats.experience:F0}/{stats.experienceMax:F0}";
            homeStatTexts[6].text = $"Stage: {dragonHealth.currentGrowth}";
            homeStatTexts[7].text = $"Status: {dragonHealth.currentStatuses}";
        }
    }


    public void OnClickFeed()
    {
        if (dragonHealth.isPassOut) return;
        if (!canFeed) { Debug.Log("먹이주기 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(50300);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("먹이주기 조건 불충족: 배고픔이 100%");
            return;
        }

        int hungerRecovery = dragonHealth.stats.maxHunger * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Hunger, hungerRecovery);

        canFeed = false;
        feedTimer = 0f;


    }

    public void OnClickBath()
    {
        if (dragonHealth.isPassOut) return;
        if (!canBath) { Debug.Log("목욕 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(50400);
        if (data == null) return;

        int cleanRecovery = dragonHealth.stats.maxClean * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Clean, cleanRecovery);

        canBath = false;
        bathTimer = 0f;
    }

    public void OnClickPlay()
    {
        if (dragonHealth.isPassOut) return;
        if(!canPlay) { Debug.Log("놀아주기 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(50500);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("놀아주기 조건 불충족: 피로도가 75% 이상");
            return;
        }


        int intimacyRecovery = dragonHealth.stats.maxIntimacy * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Intimacy, intimacyRecovery);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        canPlay = false;
        playTimer = 0f;
    }

    public void OnClickRest()
    {
        if (!canRest) { Debug.Log("휴식 쿨 진행 중"); return; }

        if (dragonHealth.isPassOut)
        {
            dragonHealth.Recover();

            canRest = false;
            restTimer = 0f;
        }
        else
        {
            var data = DataTableManger.NurtureTable.Get(50200);
            if (data == null) return;

            if (!CanExecuteNurture(data))
            {
                Debug.Log("휴식 조건 불충족: 피로도가 80% 미만");
                return;
            }

            int fatigueRecovery = dragonHealth.stats.maxFatigue * data.REC_PERCENT / 100;
            dragonHealth.stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);

            canRest = false;
            restTimer = 0f;
        }


    }

    public void OnClickTrain()
    {
        if (dragonHealth.isPassOut) { Debug.Log("기절 중, 훈련 불가"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) { Debug.Log("유아기에선 훈련 불가"); return; }

        var data = DataTableManger.NurtureTable.Get(50014);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("훈련 조건 불충족: 피로도가 65% 이상");
            return;
        }

        dragonHealth.stats.ChangeStat(StatType.Experience, data.EXPGROWTH);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        if (Random.Range(0f, 100f) < data.RATE_INJURY)
        {
            StatusType[] injuries = { StatusType.Scratches, StatusType.Bleeding, StatusType.Fracture };
            StatusType randomInjury = injuries[Random.Range(0, injuries.Length)];
            dragonHealth.status.AddStatus(randomInjury);
        }
    }

    public void OnClickExplore()
    {
        if (dragonHealth.isPassOut) { Debug.Log("기절 중, 탐험 불가"); return; }
        if (!canExplore) { Debug.Log("탐험 쿨 진행 중"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) {Debug.Log("유아기에선 탐험 불가"); return; }


        var data = DataTableManger.NurtureTable.Get(50000);
        if (data == null) return;

        if(!CanExecuteNurture(data))
        {
            Debug.Log("탐험 조건 불충족: 피로도가 65% 이상");
            return;
        }

        Debug.Log("탐험 시작");

        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        if (Random.Range(0f, 100f) < data.RATE_DISEASE)
        {
            StatusType[] diseases = { StatusType.Cold, StatusType.FoodPoisoning, StatusType.HighFever, StatusType.Infection };
            StatusType randomDisease = diseases[Random.Range(0, diseases.Length)];
            dragonHealth.status.AddStatus(randomDisease);
        }

        var itemData = DataTableManger.ItemTable;

        if (Random.Range(0f, 100f) > 50f) //itemData.Get(5010001).DROP_RATE
        {
            int random = Random.Range(0, 4);
            int randomTypeDragon = Random.Range(0, 4);

            Debug.Log("랜덤 알 생성");

            switch (random)
            {
                case 0:
                    Egg egg1 = new Egg
                    {
                        eggName = "Grass Egg",
                        icon = icon[random],
                        dragonPrefab = dragonPrefabs[randomTypeDragon]
                    };
                    vault.AddEgg(egg1);
                    break;
                case 1:
                    Egg egg2 = new Egg
                    {
                        eggName = "FIre Egg",
                        icon = icon[random],
                        dragonPrefab = dragonPrefabs[randomTypeDragon + 4]
                    };
                    vault.AddEgg(egg2);
                    break;
                case 2:
                    Egg egg3 = new Egg
                    {
                        eggName = "Water Egg",
                        icon = icon[random],
                        dragonPrefab = dragonPrefabs[randomTypeDragon + 8]
                    };
                    vault.AddEgg(egg3);
                    break;
                case 3:
                    Egg egg4 = new Egg
                    {
                        eggName = "Wind Egg",
                        icon = icon[random],
                        dragonPrefab = dragonPrefabs[randomTypeDragon + 12]
                    };
                    vault.AddEgg(egg4);
                    break;

            }



        }


        canExplore = false;
        exploreTimer = 0f;
    }

    bool CanExecuteNurture(NurtureTableData data)
    {
        switch (data.ACTIVATION_TYPE)
        {
            case 0:
                return true;

            case 1: // 배부름 수치가 100% 보다 적을 때
                return dragonHealth.stats.hunger < dragonHealth.stats.maxHunger;

            case 2: // 청결 수치가 100% 보다 적을 때
                return dragonHealth.stats.clean < dragonHealth.stats.maxClean;

            case 3: // 피로도 수치가 80% 이상일 때
                return dragonHealth.stats.fatigue >= (dragonHealth.stats.maxFatigue * 0.8f);

            case 4: // 피로도 수치가 75% 이하일 때
                return dragonHealth.stats.fatigue <= (dragonHealth.stats.maxFatigue * 0.75f);

            case 5: // 피로도 수치가 65% 이하일 때
                return dragonHealth.stats.fatigue <= (dragonHealth.stats.maxFatigue * 0.65f);

            default:
                return true;
        }
    }

}