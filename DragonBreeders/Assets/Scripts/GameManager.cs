using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TrainingPlace
{ 
    None = 0,
    Desert,
    Marine,
    Forest,
    GrassField,
    SnowField,
}

public enum Difficulty
{ 
    Low,
    Medium,
    High,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool returningToTraining = false;

    public DragonHealth dragonHealth;
    public PlayerManager playerManager;
    public InventoryManager inventoryManager;
    public EggVault vault;

    public GameObject[] dragonPrefabs;
    public Sprite[] icon;

    public TextMeshProUGUI growthStateText;
    public TextMeshProUGUI currentStatusText;

    public TextMeshProUGUI currentStaminaValue;
    public TextMeshProUGUI maxStaminaValue;

    public TextMeshProUGUI currentFatigueValue;
    public TextMeshProUGUI maxFatigueValue;

    public TextMeshProUGUI currentHungryValue;
    public TextMeshProUGUI maxHungryValue;

    public TextMeshProUGUI currentIntimacyValue;
    public TextMeshProUGUI maxIntimacyValue;

    public TextMeshProUGUI currentCleanValue;
    public TextMeshProUGUI maxCleanValue;

    public TextMeshProUGUI currentExperienceValue;
    public TextMeshProUGUI maxExperienceValue;

    public GameObject alarmPanel;
    public TextMeshProUGUI alarmText;

    public Slider staminaSlider;
    public Slider fatigueSlider;
    public Slider hungrySlider;
    public Slider intimacySlider;
    public Slider cleanSlider;
    public Slider experienceSlider;

    public Slider experienceSliderToMain;

    public TrainingPlace TrainingPlace;
    public Difficulty Difficulty;

    public TextMeshProUGUI[] mapStatTexts;

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

    public Image playProgressBar;
    public Image feedProgressBar;
    public Image bathProgressBar;
    public Image exploreProgressBar;
    public Image restProgressBar;

    public TextMeshProUGUI fpsText;

    private int frameCount = 0;
    private float fpsTimer = 0f;

    public OtherWindowUI[] otherWindows;

    public TextMeshProUGUI famePointText;
    public TextMeshProUGUI coinText;

    public Button releaseButton;

    public ParticleSystem levelUpParticle;
    private bool isPlaying = false;
    private bool isDragonReleased = false;

    public TextMeshProUGUI dragonFeedback;

    public Camera mainCamera;
    public Canvas mainCanvas;
    public Light directionalLight;
    public Light light1;
    public Light light2;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        releaseButton.gameObject.SetActive(false);
        alarmPanel.gameObject.SetActive(false);

        coinText.text = playerManager.coin.ToString();
    }

    public void Update()
    {
        CheckFPS();

        if (dragonHealth == null)
        {
            isDragonReleased = false;
        }

        if (isDragonReleased && dragonHealth != null)
        {
            dragonHealth.ReleaseDragon();
        }

        UpdateStatText();
        UpdateReleaseButton();

        if (!canExplore)
        {
            exploreTimer += Time.deltaTime;
            var exploreData = DataTableManger.NurtureTable.Get(4000502);
            exploreProgressBar.fillAmount = Mathf.Clamp01(exploreTimer / exploreData.TIME);

            if (exploreData != null && exploreTimer >= exploreData.TIME)
            {
                canExplore = true;
                exploreTimer = 0f;
                exploreProgressBar.fillAmount = 0f;
            }
        }

        if (!canRest)
        {
            restTimer += Time.deltaTime;

            var restData = DataTableManger.NurtureTable.Get(4020301);
            restProgressBar.fillAmount = Mathf.Clamp01(restTimer / restData.TIME);

            if (restData != null && restTimer >= restData.TIME)
            {
                canRest = true;
                restTimer = 0f;
                restProgressBar.fillAmount = 0f;
            }
        }

        if (!canFeed)
        {
            feedTimer += Time.deltaTime;
            var feedData = DataTableManger.NurtureTable.Get(4030101);
            feedProgressBar.fillAmount = Mathf.Clamp01(feedTimer / feedData.TIME);


            if (feedData != null)
            {
                if (feedTimer >= feedData.TIME)
                {
                    canFeed = true;
                    feedTimer = 0f;
                    feedProgressBar.fillAmount = 0f;
                }
            }
        }


        if (!canBath)
        {
            bathTimer += Time.deltaTime;
            var bathData = DataTableManger.NurtureTable.Get(4040201);
            bathProgressBar.fillAmount = Mathf.Clamp01(bathTimer / bathData.TIME);
            if (bathData != null && bathTimer >= bathData.TIME)
            {
                canBath = true;
                bathTimer = 0f;
                bathProgressBar.fillAmount = 0f;
            }
        }


        if (!canPlay)
        {
            playTimer += Time.deltaTime;
            var playData = DataTableManger.NurtureTable.Get(4050401);
            playProgressBar.fillAmount = Mathf.Clamp01(playTimer / playData.TIME);
            if (playData != null && playTimer >= playData.TIME)
            {
                canPlay = true;
                playTimer = 0f;
                playProgressBar.fillAmount = 0f;
            }
        }


        GrowUpEffect();
    }

    public void UpdateStatText()
    {
        if (dragonHealth == null) return;

        var stats = dragonHealth.stats;


        growthStateText.text = $"{dragonHealth.currentGrowth}";

        currentStaminaValue.text = $"{(int)stats.stamina}";
        currentFatigueValue.text = $"{(int)stats.fatigue}";
        currentHungryValue.text = $"{(int)stats.hunger}";
        currentIntimacyValue.text = $"{(int)stats.intimacy}";
        currentCleanValue.text = $"{(int)stats.clean}";
        currentExperienceValue.text = $"{(int)stats.experience}";



        maxStaminaValue.text = $"{(int)stats.maxStamina}";
        maxFatigueValue.text = $"{(int)stats.maxFatigue}";
        maxHungryValue.text = $"{(int)stats.maxHunger}";
        maxIntimacyValue.text = $"{(int)stats.maxIntimacy}";
        maxCleanValue.text = $"{(int)stats.maxClean}";
        maxExperienceValue.text = $"{(int)stats.experienceMax}";

        fatigueSlider.value = Mathf.Clamp01(stats.fatigue / stats.maxFatigue);
        staminaSlider.value = Mathf.Clamp01(stats.stamina / stats.maxStamina);
        hungrySlider.value = Mathf.Clamp01(stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01(stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01(stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);
        experienceSliderToMain.value = Mathf.Clamp01(stats.experience / stats.experienceMax);

        UpdateMapUI(stats);

        UpdateMainUI(dragonHealth);
        foreach (var window in otherWindows)
        {
            if (window != null)
                window.UpdateStats(dragonHealth);
        }
    }

    private void UpdateMapUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 8)
        {

        }
    }

    private void UpdateMainUI(DragonHealth dragon)
    {
        var stats = dragon.stats;

        growthStateText.text = $"{dragon.currentGrowth}";

        currentStaminaValue.text = $"{(int)stats.stamina}";
        currentFatigueValue.text = $"{(int)stats.fatigue}";
        currentHungryValue.text = $"{(int)stats.hunger}";
        currentIntimacyValue.text = $"{(int)stats.intimacy}";
        currentCleanValue.text = $"{(int)stats.clean}";
        currentExperienceValue.text = $"{(int)stats.experience}";

        maxStaminaValue.text = $"{(int)stats.maxStamina}";
        maxFatigueValue.text = $"{(int)stats.maxFatigue}";
        maxHungryValue.text = $"{(int)stats.maxHunger}";
        maxIntimacyValue.text = $"{(int)stats.maxIntimacy}";
        maxCleanValue.text = $"{(int)stats.maxClean}";
        maxExperienceValue.text = $"{(int)stats.experienceMax}";

        staminaSlider.value = Mathf.Clamp01(stats.stamina / stats.maxStamina);
        fatigueSlider.value = Mathf.Clamp01(stats.fatigue / stats.maxFatigue);
        hungrySlider.value = Mathf.Clamp01(stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01(stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01(stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);
    }


    public void OnClickFeed()
    {
        if (dragonHealth.isPassOut) return;
        if (!canFeed) { Debug.Log("먹이주기 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(4030101);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("먹이주기 조건 불충족: 배고픔이 100%");
            alarmText.text = "먹이주기 조건 불충족: 배부름이 100%";
            return;
        }

        levelUpParticle.Play();
        int hungerRecovery = (int)(dragonHealth.stats.maxHunger * data.REC_PERCENT / 100);
        dragonHealth.stats.ChangeStat(StatType.Hunger, hungerRecovery);

        // 경험치 추가
        dragonHealth.GainExperience(data.EXPGROWTH);

        alarmText.text = "먹이주기 완료, 배부름이 25% 증가";

        canFeed = false;
        feedTimer = 0f;


    }

    public void OnClickBath()
    {
        if (dragonHealth.isPassOut) return;
        if (!canBath) { Debug.Log("목욕 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(4040201);
        if (data == null) return;

        int cleanRecovery = (int)(dragonHealth.stats.maxClean * data.REC_PERCENT / 100);
        dragonHealth.stats.ChangeStat(StatType.Clean, cleanRecovery);

        // 경험치 추가
        dragonHealth.GainExperience(data.EXPGROWTH);

        alarmText.text = "목욕 완료, 청결도 30% 회복";

        canBath = false;
        bathTimer = 0f;
    }

    public void OnClickPlay()
    {
        if (dragonHealth.isPassOut) return;
        if (!canPlay) { Debug.Log("놀아주기 쿨 진행 중"); return; }

        var data = DataTableManger.NurtureTable.Get(4050401);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("놀아주기 조건 불충족: 피로도가 75% 이상");
            alarmText.text = "놀아주기 조건 불충족: 피로도가 75% 이상";
            return;
        }

        dragonHealth.GetComponent<DragonBehavior>().PlayPlayAnimation();

        float intimacyRecovery = dragonHealth.stats.maxIntimacy * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Intimacy, intimacyRecovery);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        // 경험치 추가
        dragonHealth.GainExperience(data.EXPGROWTH);

        alarmText.text = "놀아주기 완료, 친밀도 10% 증가";

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
            var data = DataTableManger.NurtureTable.Get(4020301);
            if (data == null) return;

            if (!CanExecuteNurture(data))
            {
                Debug.Log("휴식 조건 불충족: 피로도가 80% 미만");
                alarmText.text = "휴식 조건 불충족: 피로도가 80% 미만";
                return;
            }

            float fatigueRecovery = dragonHealth.stats.maxFatigue * data.REC_PERCENT / 100;
            dragonHealth.stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);

            // 경험치 추가
            dragonHealth.GainExperience(data.EXPGROWTH);

            alarmText.text = "피로도 100% 회복 성공";

            dragonHealth.GetComponent<DragonBehavior>().PlayRestAnimation();

            canRest = false;
            restTimer = 0f;
        }


    }

    public void OnClickTrain()
    {
        //if (dragonHealth.isPassOut) { Debug.Log("기절 중, 훈련 불가"); return; }
        //if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) { Debug.Log("유아기에선 훈련 불가"); alarmText.text = "유아기에선 훈련 불가"; return; }

        //var data = DataTableManger.NurtureTable.Get(4000501);
        //if (data == null) return;

        //if (!CanExecuteNurture(data))
        //{
        //    Debug.Log("훈련 조건 불충족: 피로도가 65% 이상");
        //    alarmText.text = "훈련 조건 불충족: 피로도가 65% 이상";
        //    return;
        //}

        //dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        //dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        //// 경험치 추가
        //dragonHealth.GainExperience(data.EXPGROWTH);

        //alarmText.text = $"훈련 완료, 경험치 {data.EXPGROWTH} 획득";

        //if (Random.Range(0f, 100f) < data.RATE_INJURY)
        //{
        //    StatusType[] injuries = { StatusType.Scratches, StatusType.Bleeding, StatusType.Fracture };
        //    StatusType randomInjury = injuries[Random.Range(0, injuries.Length)];
        //    dragonHealth.status.AddStatus(randomInjury);
        //}
    }

    //public void OnClickExplore()
    //{
    //    if (dragonHealth.isPassOut) { Debug.Log("기절 중, 탐험 불가"); return; }
    //    if (!canExplore) { Debug.Log("탐험 쿨 진행 중"); return; }
    //    if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) { Debug.Log("유아기에선 탐험 불가"); alarmText.text = "유아기에선 탐험 불가"; return; }


    //    var data = DataTableManger.NurtureTable.Get(4000502);
    //    if (data == null) return;

    //    if (!CanExecuteNurture(data))
    //    {
    //        Debug.Log("탐험 조건 불충족: 피로도가 65% 이상");
    //        alarmText.text = "탐험 조건 불충족: 피로도가 65% 이상";
    //        return;
    //    }

    //    Debug.Log("탐험 시작");

    //    dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
    //    dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

    //    // 경험치 추가
    //    dragonHealth.GainExperience(data.EXPGROWTH);

    //    if (Random.Range(0f, 100f) < data.RATE_DISEASE)
    //    {
    //        StatusType[] diseases = { StatusType.Cold, StatusType.FoodPoisoning, StatusType.HighFever, StatusType.Infection };
    //        StatusType randomDisease = diseases[Random.Range(0, diseases.Length)];
    //        dragonHealth.status.AddStatus(randomDisease);
    //    }

    //    var itemData = DataTableManger.ItemTable;

    //    if (Random.Range(0f, 100f) > 50f) //itemData.Get(5010001).DROP_RATE
    //    {
    //        int random = Random.Range(0, 4);
    //        int randomTypeDragon = Random.Range(0, 4);

    //        Debug.Log("랜덤 알 생성");
    //        alarmText.text = "탐험 성공! 랜덤 알을 얻었습니다.";

    //        switch (random)
    //        {
    //            case 0:
    //                Egg egg1 = new Egg
    //                {
    //                    eggName = "Grass Egg",
    //                    icon = icon[random],
    //                    dragonPrefab = dragonPrefabs[randomTypeDragon]
    //                };
    //                vault.AddEgg(egg1);
    //                break;
    //            case 1:
    //                Egg egg2 = new Egg
    //                {
    //                    eggName = "FIre Egg",
    //                    icon = icon[random],
    //                    dragonPrefab = dragonPrefabs[randomTypeDragon + 4]
    //                };
    //                vault.AddEgg(egg2);
    //                break;
    //            case 2:
    //                Egg egg3 = new Egg
    //                {
    //                    eggName = "Water Egg",
    //                    icon = icon[random],
    //                    dragonPrefab = dragonPrefabs[randomTypeDragon + 8]
    //                };
    //                vault.AddEgg(egg3);
    //                break;
    //            case 3:
    //                Egg egg4 = new Egg
    //                {
    //                    eggName = "Wind Egg",
    //                    icon = icon[random],
    //                    dragonPrefab = dragonPrefabs[randomTypeDragon + 12]
    //                };
    //                vault.AddEgg(egg4);
    //                break;
    //        }
    //    }

    //    if (Random.Range(0f, 100f) > 30f)
    //    {
    //        var allItems = DataTableManger.ItemTable.GetAll();
    //        if (allItems != null && allItems.Count > 0)
    //        {
    //            var randomItemData = allItems[Random.Range(0, allItems.Count)];


    //            Item newItem = new Item
    //            {
    //                itemID = randomItemData.ITEM_ID,
    //                itemName = DataTableManger.StringTable.Get(randomItemData.ITEM_NAME),
    //                itemType = randomItemData.ITEM_TYPE,
    //                icon = Resources.Load<Sprite>($"ItemImages/{randomItemData.ITEM_IMAGE}"),
    //                description = DataTableManger.StringTable.Get(randomItemData.ITEM_DESCRIPTION)
    //            };

    //            Debug.Log($"아이템 획득: {newItem.GetName()}");
    //            alarmText.text = $"탐험 성공! {newItem.GetName()} 을(를) 획득했습니다.";
    //        }
    //    }

    //    canExplore = false;
    //    exploreTimer = 0f;
    //}

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

    public void OnClickEggCheat()
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

    public void GrowCheatButton()
    {
        if (dragonHealth == null)
            return;

        if (dragonHealth.currentGrowth == DragonGrowthState.Adult)
        {
            Destroy(dragonHealth.gameObject);
            playerManager.famePoint += 100;
            EggSlot.isDragonActive = false;

        }

        dragonHealth.GrowUp();
    }

    public void CoolTimeResetButton()
    {
        canBath = true;
        canExplore = true;
        canFeed = true;
        canPlay = true;
        canRest = true;

        exploreTimer = 0f;
        restTimer = 0f;
        feedTimer = 0f;
        bathTimer = 0f;
        playTimer = 0f;

        playProgressBar.fillAmount = 0f;
        bathProgressBar.fillAmount = 0f;
        feedProgressBar.fillAmount = 0f;
        exploreProgressBar.fillAmount = 0f;
        restProgressBar.fillAmount = 0f;
    }

    public void AddItemCheatButton()
    {
        var allItems = DataTableManger.ItemTable.GetAll();

        if (allItems.Count > 0)
        {
            // 랜덤으로 하나 선택
            int randomIndex = Random.Range(0, allItems.Count);
            var randomItem = allItems[randomIndex];

            inventoryManager.AddItem(randomItem.ITEM_ID, 1);
            Debug.Log($"랜덤 아이템 추가: {randomItem.ITEM_NAME} (ID: {randomItem.ITEM_ID})");
        }
        else
        {
            Debug.LogError("아이템 테이블이 비어있습니다!");
        }
    }

    public void ExperienceCheatButton()
    {
        dragonHealth.GainExperience(100);
    }

    private void CheckFPS()
    {
        frameCount++;
        fpsTimer += Time.unscaledDeltaTime;

        if (fpsTimer >= 1f)
        {
            float fps = frameCount / fpsTimer;
            fpsText.text = $"{Mathf.Ceil(fps)} FPS";

            frameCount = 0;
            fpsTimer = 0f;
        }
    }

    private void UpdateReleaseButton()
    {
        if (releaseButton == null) return;

        if (dragonHealth == null)
        {
            releaseButton.gameObject.SetActive(false);
            return;
        }

        releaseButton.gameObject.SetActive(dragonHealth.currentGrowth == DragonGrowthState.Adult);
    }

    public void OnClickReleaseDragon()
    {
        if (dragonHealth == null) return;

        //Destroy(dragonHealth.gameObject);
        EggSlot.isDragonActive = false;

        isDragonReleased = true;

        releaseButton.gameObject.SetActive(false);
        Debug.Log("드래곤을 방생했습니다!");
    }

    private void GrowUpEffect()
    {

        if (dragonHealth != null && dragonHealth.isFormChanging && !isPlaying)
        {
            levelUpParticle.Play();

            levelUpParticle.transform.localScale = Vector3.one * (float)dragonHealth.currentGrowth;

            isPlaying = true;
        }

        if (dragonHealth != null && !dragonHealth.isFormChanging)
        {
            isPlaying = false;
        }
    }

    public void MoveBattleScene()
    {
        MoveSceneOnOff();

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);

        dragonHealth.transform.localPosition = new Vector3(-0.6f, 0f, 13f);
        dragonHealth.transform.localRotation = Quaternion.Euler(0f, 210f, 0f);

    }

    public void MoveSceneOnOff()
    {
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
        mainCanvas.gameObject.SetActive(!mainCanvas.gameObject.activeSelf);
        directionalLight.gameObject.SetActive(!directionalLight.gameObject.activeSelf);
        light1.gameObject.SetActive(!light1.gameObject.activeSelf);
        light2.gameObject.SetActive(!light2.gameObject.activeSelf);
    }
}