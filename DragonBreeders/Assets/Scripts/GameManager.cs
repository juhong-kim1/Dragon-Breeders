using System.Collections.Generic;
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
    None = 0,
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
    public Sprite[] dragonImages;

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
    public bool canBath = true;
    public bool canPlay = true;

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
    private bool isGrowthEffectPlaying = false;
    private bool isDragonReleased = false;

    public TextMeshProUGUI dragonFeedback;

    public Camera mainCamera;
    public Canvas mainCanvas;
    public Light directionalLight;
    public Light light1;
    public Light light2;

    public bool isFighting = false;

    public TextMeshProUGUI feedItemDiscription;
    public TextMeshProUGUI playItemDiscription;
    public TextMeshProUGUI soapItemDiscription;
    public TextMeshProUGUI brushItemDiscription;
    public Image feedItemImage;
    public Image playItemImage;
    public Image soapItemImage;
    public Image brushItemImage;

    public bool isFeeding = false;
    public bool isPlaying = false;
    public bool isSoaping = false;
    public bool isBrushing = false;

    public bool isReadyShower = false;

    public bool isShowering = false;
    public Image showerItemImage;

    public bool hasSoaped = false;
    public bool hasBrushed = false;
    private float accumulatedClean = 0f;

    public ParticleSystem bathParticle;

    public WindowManager windowManager;
    public MainWindow mainWindow;

    public DragonIndex dragonIndex;

    private int releaseToCoin = 3000;

    public int trainingWinCount = 0;
    public int trainingLoseCount = 0;
    public int playCount = 0;
    public int bathCount = 0;
    public int feedCount = 0;
    public int restCount = 0;
    public int passOutCount = 0;

    public Shop shop;

    public Sprite[] statusSprites;
    public StatusUI statusUI;

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
        LoadGame();

        releaseButton.gameObject.SetActive(false);
        alarmPanel.gameObject.SetActive(false);

        coinText.text = playerManager.coin.ToString();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
            Debug.Log("S키로 수동 저장");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
            Debug.Log("L키로 수동 로드");
        }

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

        if (dragonHealth != null && statusUI != null)
        {
            statusUI.UpdateStatusIcons(dragonHealth.status);
        }
    }

    public void UpdateStatText()
    {
        if (dragonHealth == null) return;

        var stats = dragonHealth.stats;


        growthStateText.text = $"{dragonHealth.currentGrowthText}";

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

        if (mainWindow != null && mainWindow.dragonNameText != null && dragonHealth != null)
        {
            mainWindow.dragonNameText.text = dragonHealth.stats.dragonName;
        }

        UpdateMainUI(dragonHealth);
        foreach (var window in otherWindows)
        {
            if (window != null)
                window.UpdateStats(dragonHealth);
        }
    }

    private void UpdateMainUI(DragonHealth dragon)
    {
        var stats = dragon.stats;

        growthStateText.text = $"{dragon.currentGrowthText}";

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


    public void GetFeed()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("밥 먹일 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canFeed)
        {
            AlarmManager.Instance.ShowAlarm("방금 밥먹었어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        isFeeding = true;

        inventoryManager.RefreshFoodUI();

        Debug.Log("음식 아이템만 표시됨");


    }

    public void OnClickBath()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("목욕시킬 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.isPassOut)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 기절했어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canBath)
        {
            AlarmManager.Instance.ShowAlarm("아직 목욕할 수 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }


    }

    public void GetSoap()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("목욕시킬 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.isPassOut)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 기절했어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canBath)
        {
            AlarmManager.Instance.ShowAlarm("아직 목욕할 수 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        isSoaping = true;
        inventoryManager.RefreshSoapUI();
        Debug.Log("비누 아이템만 표시됨");
    }

    public void GetBrush()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("목욕시킬 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.isPassOut)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 기절했어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canBath)
        {
            AlarmManager.Instance.ShowAlarm("아직 목욕할 수 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        isBrushing = true;
        inventoryManager.RefreshBrushUI();
        Debug.Log("브러쉬 아이템만 표시됨");
    }

    public void GetShower()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("샤워시킬 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.isPassOut)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 기절했어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canBath)
        {
            AlarmManager.Instance.ShowAlarm("아직 목욕할 수 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!hasSoaped)
        {
            AlarmManager.Instance.ShowAlarm("먼저 비누를 사용해주세요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!hasBrushed)
        {
            AlarmManager.Instance.ShowAlarm("먼저 브러쉬를 사용해주세요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        isShowering = true;
        Debug.Log("샤워 모드 활성화");
    }

    public void GetPlay()
    {
        if (dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이랑 같이 노는게 좋지 않을까요?!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.isPassOut)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 KO 상태네요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (dragonHealth.stats.fatigue/dragonHealth.stats.maxFatigue >= 0.75f)
        {
            AlarmManager.Instance.ShowAlarm("드래곤이 과로했습니다..");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canPlay)
        {
            AlarmManager.Instance.ShowAlarm("아직 놀 수 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }


        isPlaying = true;

        inventoryManager.RefreshPlayUI();

        Debug.Log("음식 아이템만 표시됨");
    }

    public void OnClickRest()
    {
        if (!canRest)
        {
            Debug.Log("휴식 쿨 진행 중");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        //if (dragonHealth.isPassOut)
        //{
        //    dragonHealth.Recover();

        //    canRest = false;
        //    restTimer = 0f;
        //}
        else
        {
            var data = DataTableManger.NurtureTable.Get(4020301);
            if (data == null) return;

            if (!CanExecuteNurture(data))
            {
                Debug.Log("휴식 조건 불충족: 피로도가 80% 미만");

                AlarmManager.Instance.ShowAlarm("휴식하기엔 아직 팔팔합니다!");
                SoundManager.Instance.PlayErrorSound();
                return;
            }

            SoundManager.Instance.PlaySFX(SoundManager.Instance.restAudioClip);

            float fatigueRecovery = dragonHealth.stats.maxFatigue * data.REC_PERCENT / 100;
            dragonHealth.stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);

            AlarmManager.Instance.ShowAlarm("개운하다~");

            restCount++;

            dragonHealth.GetComponent<DragonBehavior>().PlayRestAnimation();

            canRest = false;
            restTimer = 0f;
        }


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

    public void OnClickEggCheat()
    {
        int randomElement = Random.Range(0, 4);
        int randomSpecies = Random.Range(0, 4);

        Debug.Log("랜덤 알 생성");

        int prefabIndex = (randomSpecies * 4) + randomElement;

        string[] eggNames = { "Grass Egg", "Fire Egg", "Water Egg", "Wind Egg" };

        Egg newEgg = new Egg
        {
            eggName = eggNames[randomElement],
            icon = icon[randomElement],
            dragonPrefab = dragonPrefabs[prefabIndex],
            speciesType = randomSpecies + 1,
            elementType = randomElement + 1 
        };

        vault.AddEgg(newEgg);
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
            int randomIndex = Random.Range(0, allItems.Count);
            var randomItem = allItems[randomIndex];

            inventoryManager.AddItem(randomItem.ITEM_ID, 1);

            inventoryManager.RefreshPlayUI();
            inventoryManager.RefreshFoodUI();

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

    public void OnClickReleaseDragon()
    {
        if (dragonHealth == null) return;

        if (dragonIndex != null)
        {
            dragonIndex.RegisterDragon(dragonHealth);
        }

        EggSlot.isDragonActive = false;

        isDragonReleased = true;

        releaseButton.gameObject.SetActive(false);

        playerManager.coin += releaseToCoin;

        playerManager.UpdateCoinUI();

        AlarmManager.Instance.ShowAlarm("잘가 드래곤~");
        Debug.Log("드래곤을 방생했습니다!");

        mainWindow.dragonNameText.text = string.Empty;

        playCount = 0;
        feedCount = 0;
        bathCount = 0;
        trainingLoseCount = 0;
        trainingWinCount = 0;
        passOutCount = 0;
        restCount = 0;

        vault.AddRandomEggIfEmpty();
    }

    private void GrowUpEffect()
    {

        if (dragonHealth != null && dragonHealth.isFormChanging && !isGrowthEffectPlaying)
        {
            levelUpParticle.Play();

            levelUpParticle.transform.localScale = Vector3.one * (float)dragonHealth.currentGrowth;

            isGrowthEffectPlaying = true;
        }

        if (dragonHealth != null && !dragonHealth.isFormChanging)
        {
            isGrowthEffectPlaying = false;
        }
    }

    public void MoveBattleScene()
    {
        MoveSceneOnOff();

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);

        isFighting = true;

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
    public void FullStaminaCheat()
    {
        float staminaToAdd = dragonHealth.stats.maxStamina - dragonHealth.stats.stamina;
        dragonHealth.stats.ChangeStat(StatType.Stamina, staminaToAdd);
        dragonHealth.GetComponent<DragonBehavior>().PlayRecoverAnimation();

        dragonHealth.isPassOut = false;

        dragonHealth.status.RemoveStatus(StatusType.PassOut);
    }

    public void UseFoodItem(int itemId, int amount = 1)
    {
        if (!inventoryManager.HasItem(itemId, amount))
        {
            AlarmManager.Instance.ShowAlarm("아이템이 부족합니다!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        ItemTableData itemData = DataTableManger.ItemTable.Get(itemId);

        if (dragonHealth.stats.hunger >= dragonHealth.stats.maxHunger)
        {
            AlarmManager.Instance.ShowAlarm("배가 터져 죽을것같아요");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        inventoryManager.RemoveItem(itemId, amount);
        inventoryManager.RefreshFoodUI();

        NurtureTableData nurtureData = DataTableManger.NurtureTable.Get(4030101);

        int hungerRecovery = itemData.EFFECT1_VALUE;

        dragonHealth.stats.ChangeStat(StatType.Hunger, ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxHunger) + hungerRecovery);

        float experience = ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxHunger) + hungerRecovery;

        dragonHealth.GainExperience(experience);

        string itemName = itemData.StringName;
        AlarmManager.Instance.ShowAlarm($"{itemName} 사용! 배부름 +{((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxHunger) + hungerRecovery}");

        SoundManager.Instance.PlaySFX(SoundManager.Instance.eatAudioClip);

        canFeed = false;
        feedTimer = 0f;

        if (TutorialManager.Instance.currentStep == 11)
            TutorialManager.Instance.NextStep();

        if (feedItemImage != null)
        {
            feedItemImage.enabled = false;
        }

        feedCount++;
        Debug.Log($"{itemName} 사용 완료 - 배부름 {hungerRecovery} 회복");
    }

    public void UsePlayItem(int itemId, int amount = 1)
    {
        if (!inventoryManager.HasItem(itemId, amount))
        {
            AlarmManager.Instance.ShowAlarm("아이템이 부족합니다!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        ItemTableData itemData = DataTableManger.ItemTable.Get(itemId);

        if (dragonHealth.stats.intimacy >= dragonHealth.stats.maxIntimacy)
        {
            AlarmManager.Instance.ShowAlarm("이미 너무 친합니다!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!canRest)
        {
            AlarmManager.Instance.ShowAlarm("방금 놀았어요!");
            return;
        }

        inventoryManager.RemoveItem(itemId, amount);
        inventoryManager.RefreshPlayUI();

        NurtureTableData nurtureData = DataTableManger.NurtureTable.Get(4050401);

        int increaseIntimacy = itemData.EFFECT1_VALUE;
        dragonHealth.stats.ChangeStat(StatType.Intimacy, ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxIntimacy) + increaseIntimacy);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, nurtureData.RECEIVE_FTG);

        float experience = ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxIntimacy) + increaseIntimacy;

        dragonHealth.GainExperience(experience);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.successAudioClip);

        if (TutorialManager.Instance.currentStep == 14)
            TutorialManager.Instance.NextStep();

        string itemName = itemData.StringName;
        AlarmManager.Instance.ShowAlarm($"{itemName} 사용! 친밀도 +{((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxIntimacy) + increaseIntimacy}");

        canPlay = false;
        playTimer = 0f;

        if (playItemImage != null)
        {
            playItemImage.enabled = false;
        }

        playCount++;
        Debug.Log($"{itemName} 사용 완료 - 친밀도 {increaseIntimacy} 증가");
    }

    public void UseSoapItem(int itemId, int amount = 1)
    {
        if (!inventoryManager.HasItem(itemId, amount))
        {
            AlarmManager.Instance.ShowAlarm("아이템이 부족합니다!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        ItemTableData itemData = DataTableManger.ItemTable.Get(itemId);

        inventoryManager.RemoveItem(itemId, amount);
        inventoryManager.RefreshSoapUI();

        NurtureTableData nurtureData = DataTableManger.NurtureTable.Get(4040201);

        int cleanRecovery = itemData.EFFECT1_VALUE;
        float cleanAmount = ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxClean) + cleanRecovery;

        accumulatedClean += cleanAmount;
        hasSoaped = true;

        string itemName = itemData.StringName;
        AlarmManager.Instance.ShowAlarm($"{itemName} 사용! 이제 브러쉬를 사용하세요");

        if (soapItemImage != null)
        {
            soapItemImage.enabled = false;
        }

        Debug.Log($"{itemName} 사용 완료 - 청결도 {cleanAmount} 누적");
    }
    public void UseBrushItem(int itemId, int amount = 1)
    {
        if (!hasSoaped)
        {
            AlarmManager.Instance.ShowAlarm("먼저 비누를 사용해주세요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!inventoryManager.HasItem(itemId, amount))
        {
            AlarmManager.Instance.ShowAlarm("아이템이 부족합니다!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        ItemTableData itemData = DataTableManger.ItemTable.Get(itemId);

        inventoryManager.RemoveItem(itemId, amount);
        inventoryManager.RefreshBrushUI();

        NurtureTableData nurtureData = DataTableManger.NurtureTable.Get(4040201);

        int cleanRecovery = itemData.EFFECT1_VALUE;
        float cleanAmount = ((nurtureData.REC_PERCENT / 100f) * dragonHealth.stats.maxClean) + cleanRecovery;

        accumulatedClean += cleanAmount;
        hasBrushed = true;


        string itemName = itemData.StringName;
        AlarmManager.Instance.ShowAlarm($"{itemName} 사용! 이제 샤워를 하세요");

        if (brushItemImage != null)
        {
            brushItemImage.enabled = false;
        }

        Debug.Log($"{itemName} 사용 완료 - 청결도 {cleanAmount} 누적");
    }
    public void CompleteShower()
    {
        dragonHealth.stats.ChangeStat(StatType.Clean, accumulatedClean);
        dragonHealth.stats.ChangeStat(StatType.Experience, accumulatedClean);

        dragonHealth.GainExperience(accumulatedClean);

        AlarmManager.Instance.ShowAlarm($"목욕 완료! 청결도 +{accumulatedClean}");
        SoundManager.Instance.PlaySFX(SoundManager.Instance.successAudioClip);

        hasSoaped = false;
        hasBrushed = false;
        accumulatedClean = 0f;
        isShowering = false;

        canBath = false;
        bathTimer = 0f;

        if (showerItemImage != null)
        {
            showerItemImage.enabled = false;
        }

        bathCount++;

        Debug.Log("목욕 시퀀스 완료");
    }

    public void SaveGame()
    {
        var saveData = SaveLoadManager.Data;

        saveData.InventoryItems = inventoryManager.GetAllItems();
        saveData.Coin = playerManager.coin;
        saveData.DragonIndex = dragonIndex.GetAllEntries();

        if (dragonHealth != null)
        {
            saveData.CurrentDragon = new SaveDragonData(dragonHealth, this);
        }

        saveData.EggVault = new List<SaveEggData>();
        foreach (var slot in vault.slots)
        {
            if (slot != null && !slot.IsEmpty())
            {
                saveData.EggVault.Add(new SaveEggData(slot.egg));
            }
        }

        if (shop != null)
        {
            shop.SaveShopData(saveData.ShopItems);
        }

        if (TutorialManager.Instance != null)
        {
            saveData.TutorialStep = TutorialManager.Instance.currentStep;
            saveData.TutorialCompleted = TutorialManager.Instance.isTutorialClear;
        }

        saveData.LastSaveTime = System.DateTime.Now.ToBinary().ToString();

        bool success = SaveLoadManager.Save();

        if (success)
        {
            Debug.Log("게임 저장 완료");
        }
    }

    public void LoadGame()
    {
        bool success = SaveLoadManager.Load();

        if (!success)
        {
            return;
        }

        var saveData = SaveLoadManager.Data;

        if (saveData.InventoryItems != null)
        {
            inventoryManager.LoadItems(saveData.InventoryItems);
        }

        playerManager.coin = saveData.Coin;
        playerManager.UpdateCoinUI();

        if (saveData.DragonIndex != null)
        {
            dragonIndex.LoadEntries(saveData.DragonIndex);
        }

        if (saveData.CurrentDragon != null)
        {
            dragonHealth = saveData.CurrentDragon.CreateDragon(this);

            if (dragonHealth != null)
            {
                var behavior = dragonHealth.GetComponent<DragonBehavior>();
                if (behavior != null)
                {
                    behavior.SetTouchUI(dragonFeedback);
                }

                if (dragonHealth.isPassOut && dragonHealth.stats.stamina >= 1f)
                {
                    dragonHealth.isPassOut = false;
                    dragonHealth.status.RemoveStatus(StatusType.PassOut);
                    Debug.Log("[LoadGame] 체력이 회복되어 기절 상태 해제");
                }
                else if (dragonHealth.stats.IsStatPassOut() || dragonHealth.isPassOut)
                {
                    StartCoroutine(PlayPassOutAfterInit(dragonHealth));
                }
            }
        }

        if (saveData.EggVault != null)
        {
            LoadEggVault(saveData.EggVault);
        }

        if (shop != null)
        {
            shop.LoadShopData(saveData.ShopItems);
        }

        if (!string.IsNullOrEmpty(saveData.LastSaveTime) && dragonHealth != null)
        {
            //CalculateOfflineProgress(saveData.LastSaveTime);
        }

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.currentStep = saveData.TutorialStep;
            TutorialManager.Instance.isTutorialClear = saveData.TutorialCompleted;

            if (!saveData.TutorialCompleted && saveData.TutorialStep > 0)
            {
                TutorialManager.Instance.tutorialActive = true;
                TutorialManager.Instance.tutorialPanel.SetActive(true);
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGame();
        }
    }

    private void LoadEggVault(List<SaveEggData> eggDataList)
    {
        foreach (var slot in vault.slots)
        {
            if (slot != null)
            {
                slot.ClearEgg();
            }
        }

        for (int i = 0; i < eggDataList.Count && i < vault.slots.Length; i++)
        {
            var eggData = eggDataList[i];
            var egg = eggData.CreateEgg(this);

            if (egg != null && vault.slots[i] != null)
            {
                vault.slots[i].SetEgg(egg);
            }
        }

        Debug.Log($"{eggDataList.Count}개의 알을 로드했습니다.");
    }

    private void CalculateOfflineProgress(string lastSaveTimeString)
    {
        try
        {
            if (string.IsNullOrEmpty(lastSaveTimeString))
            {
                return;
            }

            long lastSaveTimeBinary = System.Convert.ToInt64(lastSaveTimeString);
            System.DateTime lastSaveTime = System.DateTime.FromBinary(lastSaveTimeBinary);
            System.DateTime currentTime = System.DateTime.Now;


            double offlineMinutes = (currentTime - lastSaveTime).TotalMinutes;

            if (offlineMinutes <= 0)
            {
                return;
            }

            if (dragonHealth == null)
            {
                return;
            }

            if (dragonHealth.currentTableData == null)
            {
                return;
            }

            int decreaseCycles = (int)(offlineMinutes);

            if (decreaseCycles > 0)
            {
                var tableData = dragonHealth.currentTableData;

                float hungerDecrease = tableData.DEP_FOOD * decreaseCycles;
                float cleanDecrease = tableData.DEP_HYG * decreaseCycles;
                float intimacyDecrease = tableData.DEP_FRN * decreaseCycles;


                dragonHealth.stats.ChangeStat(StatType.Hunger, -hungerDecrease);
                dragonHealth.stats.ChangeStat(StatType.Clean, -cleanDecrease);
                dragonHealth.stats.ChangeStat(StatType.Intimacy, -intimacyDecrease);

                if (dragonHealth.stats.hunger > 0)
                {
                    float staminaIncrease = tableData.DEP_FOOD * decreaseCycles;
                    Debug.Log($"배고픔이 0보다 크므로 체력 회복: +{staminaIncrease}");
                    dragonHealth.stats.ChangeStat(StatType.Stamina, staminaIncrease);
                    Debug.Log($"체력: {dragonHealth.stats.stamina}/{dragonHealth.stats.maxStamina}");
                }
                else
                {

                }
            }
            else
            {
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"CalculateOfflineProgress 오류 발생!");
            Debug.LogError($"오류 메시지: {e.Message}");
            Debug.LogError($"스택 트레이스: {e.StackTrace}");
        }
    }

    private System.Collections.IEnumerator PlayPassOutAfterInit(DragonHealth dragon)
    {
        yield return null;

        if (dragon != null)
        {
            dragon.PlayPassOutAnimation();
            Debug.Log("[LoadGame] 기절 애니메이션 재생 완료");
        }
    }
}