using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;
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

    public int famePoint = 0;
    public TextMeshProUGUI famePointText;

    public Button releaseButton;

    public ParticleSystem levelUpParticle;
    private bool isPlaying = false;

    private void Start()
    {
        releaseButton.gameObject.SetActive(false);
        alarmPanel.gameObject.SetActive(false);

    }

    public void Update()
    {
        CheckFPS();

        UpdateStatText();
        UpdateReleaseButton();

        if (!canExplore)
        {
            exploreTimer += Time.deltaTime;
            var exploreData = DataTableManger.NurtureTable.Get(50000);
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

            var restData = DataTableManger.NurtureTable.Get(50200);
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
            var feedData = DataTableManger.NurtureTable.Get(50300);
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
            var bathData = DataTableManger.NurtureTable.Get(50400);
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
            var playData = DataTableManger.NurtureTable.Get(50500);
            playProgressBar.fillAmount = Mathf.Clamp01(playTimer / playData.TIME);
            if (playData != null && playTimer >= playData.TIME)
            {
                canPlay = true;
                playTimer = 0f;
                playProgressBar.fillAmount = 0f;
            }
        }

        if (dragonHealth != null && dragonHealth.isFormChanging && !isPlaying)
        { 
            levelUpParticle.Play();

            isPlaying = true;
        }

        if (dragonHealth != null && !dragonHealth.isFormChanging)
        {
            isPlaying = false;
        }

    }

    public void UpdateStatText()
    {
        if (dragonHealth == null) return;

        var stats = dragonHealth.stats;


        growthStateText.text = $"{dragonHealth.currentGrowth}";

        currentStaminaValue.text = $"{stats.stamina}";
        currentFatigueValue.text = $"{stats.fatigue}";
        currentHungryValue.text = $"{stats.hunger}";
        currentIntimacyValue.text = $"{stats.intimacy}";
        currentCleanValue.text = $"{stats.clean}";
        currentExperienceValue.text = $"{stats.experience}";



        maxStaminaValue.text = $"{stats.maxStamina}";
        maxFatigueValue.text = $"{stats.maxFatigue}";
        maxHungryValue.text = $"{stats.maxHunger}";
        maxIntimacyValue.text = $"{stats.maxIntimacy}";
        maxCleanValue.text = $"{stats.maxClean}";
        maxExperienceValue.text = $"{stats.experienceMax}";

        fatigueSlider.value = Mathf.Clamp01((float)stats.fatigue / stats.maxFatigue);
        staminaSlider.value = Mathf.Clamp01((float)stats.stamina / stats.maxStamina);
        hungrySlider.value = Mathf.Clamp01((float)stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01((float)stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01((float)stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);

        famePointText.text = $"�� : {famePoint}";

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

        currentStaminaValue.text = $"{stats.stamina}";
        currentFatigueValue.text = $"{stats.fatigue}";
        currentHungryValue.text = $"{stats.hunger}";
        currentIntimacyValue.text = $"{stats.intimacy}";
        currentCleanValue.text = $"{stats.clean}";
        currentExperienceValue.text = $"{stats.experience}";

        maxStaminaValue.text = $"{stats.maxStamina}";
        maxFatigueValue.text = $"{stats.maxFatigue}";
        maxHungryValue.text = $"{stats.maxHunger}";
        maxIntimacyValue.text = $"{stats.maxIntimacy}";
        maxCleanValue.text = $"{stats.maxClean}";
        maxExperienceValue.text = $"{stats.experienceMax}";

        staminaSlider.value = Mathf.Clamp01((float)stats.stamina / stats.maxStamina);
        fatigueSlider.value = Mathf.Clamp01((float)stats.fatigue / stats.maxFatigue);
        hungrySlider.value = Mathf.Clamp01((float)stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01((float)stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01((float)stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);
    }


    public void OnClickFeed()
    {
        if (dragonHealth.isPassOut) return;
        if (!canFeed) { Debug.Log("�����ֱ� �� ���� ��"); return; }

        var data = DataTableManger.NurtureTable.Get(50300);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("�����ֱ� ���� ������: ������� 100%");
            alarmText.text = "�����ֱ� ���� ������: ��θ��� 100%";
            return;
        }

        levelUpParticle.Play();
        int hungerRecovery = dragonHealth.stats.maxHunger * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Hunger, hungerRecovery);

        alarmText.text = "�����ֱ� �Ϸ�, ��θ��� 25% ����";

        canFeed = false;
        feedTimer = 0f;


    }

    public void OnClickBath()
    {
        if (dragonHealth.isPassOut) return;
        if (!canBath) { Debug.Log("��� �� ���� ��"); return; }

        var data = DataTableManger.NurtureTable.Get(50400);
        if (data == null) return;

        int cleanRecovery = dragonHealth.stats.maxClean * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Clean, cleanRecovery);

        alarmText.text = "��� �Ϸ�, û�ᵵ 30% ȸ��";

        canBath = false;
        bathTimer = 0f;
    }

    public void OnClickPlay()
    {
        if (dragonHealth.isPassOut) return;
        if(!canPlay) { Debug.Log("����ֱ� �� ���� ��"); return; }

        var data = DataTableManger.NurtureTable.Get(50500);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("����ֱ� ���� ������: �Ƿε��� 75% �̻�");
            alarmText.text = "����ֱ� ���� ������: �Ƿε��� 75% �̻�";
            return;
        }

        dragonHealth.GetComponent<DragonBehavior>().PlayPlayAnimation();

        int intimacyRecovery = dragonHealth.stats.maxIntimacy * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Intimacy, intimacyRecovery);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        alarmText.text = "����ֱ� �Ϸ�, ģ�е� 10% ����";

        canPlay = false;
        playTimer = 0f;
    }

    public void OnClickRest()
    {
        if (!canRest) { Debug.Log("�޽� �� ���� ��"); return; }

        if (dragonHealth.isPassOut)
        {
            dragonHealth.Recover();
            dragonHealth.GainExperienceFromStats();

            canRest = false;
            restTimer = 0f;
        }
        else
        {
            var data = DataTableManger.NurtureTable.Get(50200);
            if (data == null) return;

            if (!CanExecuteNurture(data))
            {
                Debug.Log("�޽� ���� ������: �Ƿε��� 80% �̸�");
                alarmText.text = "�޽� ���� ������: �Ƿε��� 80% �̸�";
                return;
            }

            int fatigueRecovery = dragonHealth.stats.maxFatigue * data.REC_PERCENT / 100;
            dragonHealth.stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);
            dragonHealth.GainExperienceFromStats();

            alarmText.text = "�Ƿε� 100% ȸ�� ����";

            dragonHealth.GetComponent<DragonBehavior>().PlayRestAnimation();

            canRest = false;
            restTimer = 0f;
        }


    }

    public void OnClickTrain()
    {
        if (dragonHealth.isPassOut) { Debug.Log("���� ��, �Ʒ� �Ұ�"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) { Debug.Log("���Ʊ⿡�� �Ʒ� �Ұ�"); alarmText.text = "���Ʊ⿡�� �Ʒ� �Ұ�"; return; }

        var data = DataTableManger.NurtureTable.Get(50014);
        if (data == null) return;

        if (!CanExecuteNurture(data))
        {
            Debug.Log("�Ʒ� ���� ������: �Ƿε��� 65% �̻�");
            alarmText.text = "�Ʒ� ���� ������: �Ƿε��� 65% �̻�";
            return;
        }

        dragonHealth.stats.ChangeStat(StatType.Experience, data.EXPGROWTH);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);


        alarmText.text = "�ƷÿϷ�, ����ġ 14 ȹ��";

        if (Random.Range(0f, 100f) < data.RATE_INJURY)
        {
            StatusType[] injuries = { StatusType.Scratches, StatusType.Bleeding, StatusType.Fracture };
            StatusType randomInjury = injuries[Random.Range(0, injuries.Length)];
            dragonHealth.status.AddStatus(randomInjury);
        }
    }

    public void OnClickExplore()
    {
        if (dragonHealth.isPassOut) { Debug.Log("���� ��, Ž�� �Ұ�"); return; }
        if (!canExplore) { Debug.Log("Ž�� �� ���� ��"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) {Debug.Log("���Ʊ⿡�� Ž�� �Ұ�"); alarmText.text = "���Ʊ⿡�� Ž�� �Ұ�"; return; }


        var data = DataTableManger.NurtureTable.Get(50000);
        if (data == null) return;

        if(!CanExecuteNurture(data))
        {
            Debug.Log("Ž�� ���� ������: �Ƿε��� 65% �̻�");
            alarmText.text = "Ž�� ���� ������: �Ƿε��� 65% �̻�";
            return;
        }

        Debug.Log("Ž�� ����");

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

            Debug.Log("���� �� ����");
            alarmText.text = "Ž�� ����! ���� ���� ������ϴ�.";

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

            case 1: // ��θ� ��ġ�� 100% ���� ���� ��
                return dragonHealth.stats.hunger < dragonHealth.stats.maxHunger;

            case 2: // û�� ��ġ�� 100% ���� ���� ��
                return dragonHealth.stats.clean < dragonHealth.stats.maxClean;

            case 3: // �Ƿε� ��ġ�� 80% �̻��� ��
                return dragonHealth.stats.fatigue >= (dragonHealth.stats.maxFatigue * 0.8f);

            case 4: // �Ƿε� ��ġ�� 75% ������ ��
                return dragonHealth.stats.fatigue <= (dragonHealth.stats.maxFatigue * 0.75f);

            case 5: // �Ƿε� ��ġ�� 65% ������ ��
                return dragonHealth.stats.fatigue <= (dragonHealth.stats.maxFatigue * 0.65f);

            default:
                return true;
        }
    }

    public void OnClickEggCheat()
    {
        int random = Random.Range(0, 4);
        int randomTypeDragon = Random.Range(0, 4);

        Debug.Log("���� �� ����");

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
            famePoint += 100;
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

    public void ExperienceCheatButton()
    {
        dragonHealth.stats.ChangeStat(StatType.Experience, 100);
        dragonHealth.GainExperienceFromStats();
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

        Destroy(dragonHealth.gameObject);
        famePoint += 100;
        EggSlot.isDragonActive = false;

        releaseButton.gameObject.SetActive(false);
        Debug.Log("�巡���� ����߽��ϴ�!");
    }
}