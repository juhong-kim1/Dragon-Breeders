using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    public GameObject battleTutorialPanel;
    public TextMeshProUGUI battleTutorialText;

    public GameManager gameManager;

    public GameObject mainWindow;
    public GameObject mapWindow;
    public GameObject eggVaultWindow;
    public GameObject nestWindow;
    public GameObject retryButton;

    public Button nextButton;
    public Button clearTutorialButton;
    public Button reTutorialButton;

    public GameObject eggCoverPanel;

    public bool isStatPanelOpen;
    public bool isFeedPanelOpen;
    public bool hasDragonEatFood;

    public GameObject statArrow;
    public GameObject feedArrow;
    public GameObject playArrow;

    public int currentStep = 0;
    public bool tutorialActive = false;
    public bool isTutorialClear = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Canvas tutorialCanvas = GetComponentInParent<Canvas>();
            if (tutorialCanvas != null)
            {
                DontDestroyOnLoad(tutorialCanvas.gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        clearTutorialButton.gameObject.SetActive(false);
        //isTutorialClear = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
        OnGameStart();

        nextButton.onClick.AddListener(OnClickNextButton);
        clearTutorialButton.onClick.AddListener(OnClickNextButton);
        reTutorialButton.onClick.AddListener(OnClickReTutorialButton);

        statArrow.SetActive(false);
        feedArrow.SetActive(false);
        playArrow.SetActive(false);
        eggCoverPanel.SetActive(false);
    }

    public void OnGameStart()
    {
        if (!tutorialActive && !isTutorialClear)
        {
            tutorialActive = true;
            currentStep = 1;
            tutorialPanel.SetActive(true);
            ShowStep(currentStep);
        }
    }

    //private void Update()
    //{
    //    if (!tutorialActive) return;

    //    switch (currentStep)
    //    {
    //        case 1:
    //            if (mainWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 2: 
    //            if (mapWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 3:
    //            if (eggVaultWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 4:
    //            if (nestWindow.activeSelf)
    //                NextStep();
    //            break;
    //    }
    //}

    public void OnWindowOpened(GameObject window)
    {
        if (!tutorialActive) return;

        switch (currentStep)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5: // 알 깨기
                //Debug.Log("step 5");
                //if (gameManager.dragonHealth != null)
                //{
                //    GameManager.Instance.dragonHealth.stats.experience += 100f;
                //    NextStep();
                //}
                break;
            case 6: // 메인윈도우로 돌아오기
                //Debug.Log("step 6");
                //if (window == mainWindow)
                //{
                //    NextStep();
                //}
                break;
            case 7:
                Debug.Log("step 7");
                break;
            case 8:
                Debug.Log("step 8");
                break;
            case 9:
                Debug.Log("step 9");
                break;
            case 10:
                Debug.Log("step 10");
                break;
        }
    }

    private void ShowStep(int step)
    {
        var text = DataTableManger.StringTable;

        switch (step)
        {
            case 1:
                tutorialText.text = text.Get("TUTORIAL1"); //어서 와, 신입 브리더!\n여기는 네가 드래곤을 키우게 될 둥지야.
                Debug.Log("step 1");
                break;
            case 2:
                tutorialText.text = text.Get("TUTORIAL2"); //드래곤을 데리러 가보자.\n먼저 알을 부화시켜야 돼. 지도를 눌러봐.
                Debug.Log("step 2");
                nextButton.gameObject.SetActive(false);
                break;
            case 3:
                tutorialText.text = text.Get("TUTORIAL3"); //알 보관소로 가자.
                Debug.Log("step 3");
                break;
            case 4:
                tutorialText.text = text.Get("TUTORIAL4"); //처음이니까 알을 준비해놨어.\n다음부터는 훈련을 하면 일정 확률로 알을\n받을 수 있어.
                Debug.Log("step 4");
                nextButton.gameObject.SetActive(true);
                eggCoverPanel.SetActive(true);
                break;
            case 5:
                tutorialText.text = text.Get("TUTORIAL5"); //알을 꾹 눌러 부화시켜봐
                Debug.Log("step 5");
                nextButton.gameObject.SetActive(false);
                eggCoverPanel.SetActive(false);
                break;
            case 6:
                tutorialText.text = text.Get("TUTORIAL6"); // 너의 첫번째 드래곤이야! 드래곤에게 이름을 지어줄래?
                Debug.Log("step 6");
                var dragonStat = GameManager.Instance.dragonHealth.stats;
                if(dragonStat.experience < 100f) GameManager.Instance.dragonHealth.stats.experience = 70f;
                if (dragonStat.hunger > 80f) GameManager.Instance.dragonHealth.stats.hunger = 80f;
                break;
            case 7:
                tutorialText.text = text.Get("TUTORIAL7"); //메뉴버튼 누르자
                Debug.Log("step 7");
                //nextButton.gameObject.SetActive(true);
                break;
            case 8:
                tutorialText.text = text.Get("TUTORIAL8"); //스탯버튼 누르자
                statArrow.SetActive(true);
                Debug.Log("step 8");
                //nextButton.gameObject.SetActive(false);
                break;
            case 9:
                tutorialText.text = text.Get("TUTORIAL9"); // 먹이버튼 눌러서 밥을 줘보자
                feedArrow.SetActive(true);
                statArrow.SetActive(false);
                Debug.Log("step 9");
                break;
            case 10:
                tutorialText.text = text.Get("TUTORIAL10");
                feedArrow.SetActive(false);
                break;
            case 11:
                tutorialText.text = text.Get("TUTORIAL11"); //아이템을 사용해서 드래곤과 놀자
                nextButton.gameObject.SetActive(true);
                break;
            case 12:
                tutorialText.text = text.Get("TUTORIAL12");
                playArrow.SetActive(true);
                nextButton.gameObject.SetActive(false);
                break;
            case 13:
                tutorialText.text = text.Get("TUTORIAL13"); // 상점으로 가봐
                nextButton.gameObject.SetActive(true);
                playArrow.SetActive(false);
                break;
            case 14:
                tutorialText.text = text.Get("TUTORIAL14"); //감이 좀 잡혔을까?
                break;
            case 15:
                tutorialText.text = text.Get("TUTORIAL15"); //마지막이야 지도로 가서 훈련을 해보자
                break;
            case 16:
                tutorialText.text = text.Get("TUTORIAL16");
                nextButton.gameObject.SetActive(false); //훈련지역은 5개로 나눠져있어, 맘에드는 지역을 골라봐
                break;
            case 17:
                tutorialText.text = text.Get("TUTORIAL17"); // 하급 훈련부터 해보자
                break;
            case 18:
                tutorialText.text = text.Get("TUTORIAL18");
                clearTutorialButton.gameObject.SetActive(true);// 드래곤이 공격할 차례야 스킬을 써봐
                break;
        }
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep > 18)
        {
            EndTutorial();
            return;
        }

        ShowStep(currentStep);
    }

    private void EndTutorial()
    {
        tutorialActive = false;
        isTutorialClear = true;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        tutorialPanel.SetActive(false);
        Debug.Log("튜토리얼 완료");
    }

    public void TutorialOff()
    {
        tutorialActive = false;
        isTutorialClear = true;
        tutorialPanel.SetActive(false);

        nextButton.gameObject.SetActive(true);
        clearTutorialButton.gameObject.SetActive(false);

        feedArrow.SetActive(false);
        playArrow.SetActive(false);
        statArrow.SetActive(false);
    }

    private void OnClickNextButton()
    {
        NextStep();
    }

    private void OnClickReTutorialButton()
    {
        if (tutorialActive) return;

        tutorialActive=true;
        isTutorialClear=false;

        PlayerPrefs.SetInt("TutorialCompleted", 0);
        PlayerPrefs.Save();

        tutorialPanel.SetActive(true);
        currentStep = 1;
        ShowStep(currentStep);
    }
}
