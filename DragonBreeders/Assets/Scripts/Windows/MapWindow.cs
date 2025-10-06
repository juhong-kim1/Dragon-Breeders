using NUnit.Framework.Constraints;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapWindow : GenericWindow
{
    public Camera mainCamera;
    public Canvas mainCanvas;

    public Button backButton;
    public Button menuButton;
    public Button statButton;
    public Button helpButton;
    public Button trainingButton;
    public Button inventoryButton;
    public Button indexButton;

    public Button[] locationButtons = new Button[5];
    public Button[] difficultyButtons = new Button[3];
    public Button[] xButtons = new Button[3];

    private int locationIndex;

    public GameObject eggVaultWindowObject;
    public GameObject mainWindowObject;
    public TutorialManager tutorialManager;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject trainingLocationPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject trainingStartPanel;
    [SerializeField] private Image difficultyPanelImage;
    [SerializeField] private Image trainingStartPanelImage;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject indexPanel;

    public IndexUI indexUI;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI difficultyText;

    private readonly Color32[] locationColors = {
    new Color32(209,241,88,255),
    new Color32(30,140,254,255),
    new Color32(58,203,92,255),
    new Color32(118,223,79,255),
    new Color32(230,220,230,255)
};

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        statButton.onClick.AddListener(ToggleStat);
        helpButton.onClick.AddListener(ToggleHelp);
        trainingButton.onClick.AddListener(ToggleTraining);
        inventoryButton.onClick.AddListener(ToggleInventory);
        indexButton.onClick.AddListener(ToggleIndex);

        for (int i = 0; i < locationButtons.Length; ++i)
        {
            int index = i;

            locationButtons[i].onClick.AddListener(() => ToggleLocation(index));
        }

        for (int i = 0; i < difficultyButtons.Length; ++i)
        {
            int index = i;

            difficultyButtons[i].onClick.AddListener(() => ToggleDifficulty(index));
        }

        for (int i = 0; i < xButtons.Length; ++i)
        {
            xButtons[i].onClick.AddListener(() => ToggleXButton());
        }

        menuPanel.SetActive(false);
        statPanel.SetActive(false);
        helpPanel.SetActive(false);
        trainingLocationPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        trainingStartPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        indexPanel.SetActive(false);

    }

    public void OnClickStart()
    {
        manager.Open(Windows.Start);
        GameManager.Instance.alarmPanel.gameObject.SetActive(false);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        SoundManager.Instance.PlayBGM(SoundManager.Instance.StartBGM);
    }

    public void OnClickBack()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        manager.Open(Windows.Game);
    }

    public void OnClickEgg()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        manager.Open(Windows.EggVault);

        if (TutorialManager.Instance.currentStep == 3 && TutorialManager.Instance.tutorialActive)
            TutorialManager.Instance.NextStep();
    }

    public void OnClickTrainingStart()
    {
        GameManager.Instance.MoveBattleScene();

        SoundManager.Instance.PlayBGM(SoundManager.Instance.BattleBGM);

        if(TutorialManager.Instance.currentStep == 21)
        TutorialManager.Instance.NextStep();
    }

    public void OnClickHospital()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        manager.Open(Windows.Hospital);
    }

    private void ToggleMenu()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    private void ToggleStat()
    {
        statPanel.SetActive(!statPanel.activeSelf);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

    }

    private void ToggleHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

    }

    public void OnClickShop()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        manager.Open(Windows.Shop);
    }

    private void ToggleTraining()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        if (GameManager.Instance.dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("혼자선 싸울 수 없어요!");
            return;
        }

        trainingLocationPanel.SetActive(!trainingLocationPanel.activeSelf);
    }

    private void ToggleLocation(int index)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickTraining);

        GameManager.Instance.TrainingPlace = (TrainingPlace)index + 1;
        locationIndex = index;
        difficultyPanel.SetActive(true);
        difficultyPanelImage.color = locationColors[index];

        if(TutorialManager.Instance.currentStep == 20)
        TutorialManager.Instance.NextStep();
    }

    private void ToggleDifficulty(int index)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickTraining);


        GameManager.Instance.Difficulty = (Difficulty)index + 1;

        trainingStartPanel.SetActive(true);
        trainingStartPanelImage.color = locationColors[locationIndex];

        TrainingTextSet();
    }

    private void ToggleXButton()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        trainingStartPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        trainingLocationPanel.SetActive(false);
    }
    public void CloseStat()
    {
        statPanel.SetActive(false);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);
    }

    private void ToggleInventory()
    {
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        if (InventoryPanel.activeSelf)
        {
            if (GameManager.Instance?.inventoryManager != null)
            {
                if (GameManager.Instance.inventoryManager.inventoryMapUI != null)
                {
                    GameManager.Instance.inventoryManager.inventoryMapUI.RefreshDisplay(
                        GameManager.Instance.inventoryManager.GetAllItems()
                    );
                    Debug.Log("[MapWindow] 맵 인벤토리 새로고침 완료");
                }
            }
        }
    }

    public void CloseInventory()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (InventoryPanel.activeSelf)
            InventoryPanel.SetActive(false);
    }

    public void ToggleIndex()
    {
        indexPanel.SetActive(!indexPanel.activeSelf);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickSource);

        if (indexPanel.activeSelf && indexUI != null && GameManager.Instance.dragonIndex != null)
        {
            indexUI.ShowIndex(GameManager.Instance.dragonIndex);
        }
    }

    public void CloseIndex()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (indexPanel.activeSelf)
            indexPanel.SetActive(false);
    }

    private void TrainingTextSet()
    {
        switch (GameManager.Instance.TrainingPlace)
        {
            case TrainingPlace.Desert:
                locationText.text = "사막";
                break;
            case TrainingPlace.Marine:
                locationText.text = "해양";
                break;
            case TrainingPlace.Forest:
                locationText.text = "숲";
                break;
            case TrainingPlace.GrassField:
                locationText.text = "초원";
                break;
            case TrainingPlace.SnowField:
                locationText.text = "설원";
                break;
        }

        switch (GameManager.Instance.Difficulty)
        {
            case Difficulty.Low:
                difficultyText.text = "하";
                break;
            case Difficulty.Medium:
                difficultyText.text = "중";
                break;
            case Difficulty.High:
                difficultyText.text = "상";
                break;
        }

    }
}