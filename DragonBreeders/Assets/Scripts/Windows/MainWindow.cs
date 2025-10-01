using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : GenericWindow
{
    public Button mapButton;
    public Button menuButton;
    public Button statButton;
    public Button helpButton;
    public Button feedButton;
    public Button playButton;
    public Button inventoryButton;
    public Button bathButton;
    public Button closeBathButton;
    public Button soapButton;
    public Button brushButton;
    public Button showerButton;
    public Button confirmNameButton;
    public Button indexButton;

    public TMP_InputField nameInputField;

    public GameObject mapWindowObject;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject feedPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject downUi;
    [SerializeField] private GameObject bathUi;
    [SerializeField] private GameObject soapPanel;
    [SerializeField] private GameObject brushPanel;
    [SerializeField] private GameObject indexPanel;

    public GameObject dragonNameInputPanel;
    public TextMeshProUGUI dragonNameText;

    [SerializeField] private GameObject showerImageObject;

    [SerializeField] private InventoryManager inventoryManager;
    public TutorialManager tutorialManager;
    public GameManager gameManager;

    public IndexUI indexUI;

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        statButton.onClick.AddListener(ToggleStat);
        helpButton.onClick.AddListener(ToggleHelp);
        inventoryButton.onClick.AddListener(ToggleInventory);
        feedButton.onClick.AddListener(ToggleFeed);
        playButton.onClick.AddListener(TogglePlay);
        bathButton.onClick.AddListener(ToggleBath);
        closeBathButton.onClick.AddListener(CloseBath);
        soapButton.onClick.AddListener(ToggleSoap);
        brushButton.onClick.AddListener(ToggleBrush);
        showerButton.onClick.AddListener(ToggleShower);
        confirmNameButton.onClick.AddListener(ConfirmDragonName);
        indexButton.onClick.AddListener(ToggleIndex);

        AllMainPanelsOut();
    }

    public void OnClickStart()
    {
        manager.Open(Windows.Start);
        gameManager.alarmPanel.gameObject.SetActive(false);

        SoundManager.Instance.PlayBGM(SoundManager.Instance.StartBGM);

    }

    public void OnClickMap()
    {
        manager.Open(Windows.Map);

        AllMainPanelsOut();

        if (TutorialManager.Instance.currentStep == 2 && TutorialManager.Instance.tutorialActive)
            TutorialManager.Instance.NextStep();
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);

        if (TutorialManager.Instance.currentStep == 8 && TutorialManager.Instance.tutorialActive)
            TutorialManager.Instance.NextStep();
    }

    private void ToggleStat()
    {
        statPanel.SetActive(!statPanel.activeSelf);

        if (TutorialManager.Instance.currentStep == 9 && TutorialManager.Instance.tutorialActive)
            TutorialManager.Instance.NextStep();
    }

    private void ToggleHelp()
    {
        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    private void ToggleInventory()
    { 
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);
    }

    public void CloseInventory()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (InventoryPanel.activeSelf)
            InventoryPanel.SetActive(false);
    }

    private void ToggleFeed()
    {
        CloseStat();

        if(TutorialManager.Instance.currentStep == 10 && TutorialManager.Instance.tutorialActive)
        TutorialManager.Instance.NextStep();

        if (GameManager.Instance.dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("드래곤 데려오세요!");
            return;
        }

        feedPanel.SetActive(!feedPanel.activeSelf);

        ClosePlay();

        if (feedPanel.activeSelf)
        {
            GameManager.Instance.GetFeed();
        }
    }

    public void CloseFeed()
    {
        CloseStat();

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (feedPanel.activeSelf)
            feedPanel.SetActive(false);

        GameManager.Instance.isFeeding = false;
        GameManager.Instance.feedItemImage.enabled = false;
    }

    public void TogglePlay()
    {
        if (GameManager.Instance.dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("드래곤 데려오세요!");
            return;
        }

        var dragon = GameManager.Instance.dragonHealth.stats;

        if (dragon.fatigue / dragon.maxFatigue >= 0.75f)
        {
            AlarmManager.Instance.ShowAlarm("드래곤의 과로했어요!");
            return;
        }

        playPanel.SetActive(!playPanel.activeSelf);
        CloseFeed();

        if (playPanel.activeSelf)
        { 
            GameManager.Instance.GetPlay();
        }
    }

    public void ClosePlay()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (playPanel.activeSelf)
            playPanel.SetActive(false);

        GameManager.Instance.isPlaying = false;
        GameManager.Instance.playItemImage.enabled = false;
    }

    public void ToggleBath()
    {
        if (GameManager.Instance.dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("드래곤 데려오세요!");
            return;
        }

        if (GameManager.Instance.dragonHealth.stats.FullFatigue())
        {
            AlarmManager.Instance.ShowAlarm("드래곤의 과로했어요!");
            return;
        }

        downUi.SetActive(false);
        bathUi.SetActive(true);
        playPanel.SetActive(false);
        feedPanel.SetActive(false);
    }

    public void CloseBath()
    {
        downUi.SetActive(true);
        bathUi.SetActive(false);

        soapPanel.SetActive(false);
        brushPanel.SetActive(false);
        showerImageObject.SetActive(false);
    }
    public void ToggleSoap()
    {
        soapPanel.SetActive(!soapPanel.activeSelf);
        CloseBrush();
        showerImageObject.SetActive(false);

        if (soapPanel.activeSelf)
        {
            GameManager.Instance.GetSoap();
        }
    }

    public void ToggleBrush()
    {
        brushPanel.SetActive(!brushPanel.activeSelf);
        CloseSoap();
        showerImageObject.SetActive(false);

        if (brushPanel.activeSelf)
        {
            GameManager.Instance.GetBrush();
        }
    }

    public void ToggleIndex()
    {
        indexPanel.SetActive(!indexPanel.activeSelf);

        if (indexPanel.activeSelf && indexUI != null && GameManager.Instance.dragonIndex != null)
        {
            indexUI.ShowIndex(GameManager.Instance.dragonIndex);
        }
    }

    public void ToggleShower()
    {
        if (!GameManager.Instance.hasSoaped)
        {
            AlarmManager.Instance.ShowAlarm("비누칠부터 해야해요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!GameManager.Instance.hasBrushed)
        {
            AlarmManager.Instance.ShowAlarm("브러싱부터 해야해요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        showerImageObject.SetActive(!showerImageObject.activeSelf);

        if (showerImageObject.activeSelf)
        {
            GameManager.Instance.GetShower();

            DragItem showerDrag = showerImageObject.GetComponent<DragItem>();
            if (showerDrag != null)
            {
                showerDrag.SetCurrentItem(null);
            }
        }
        else
        {
            GameManager.Instance.isShowering = false;
        }

        soapPanel.SetActive(false);
        brushPanel.SetActive(false);

        GameManager.Instance.isSoaping = false;
        if (GameManager.Instance.soapItemImage != null)
            GameManager.Instance.soapItemImage.enabled = false;

        GameManager.Instance.isBrushing = false;
        if (GameManager.Instance.brushItemImage != null)
            GameManager.Instance.brushItemImage.enabled = false;
    }

    public void CloseSoap()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (soapPanel.activeSelf)
            soapPanel.SetActive(false);

        GameManager.Instance.isSoaping = false;
        if (GameManager.Instance.soapItemImage != null)
            GameManager.Instance.soapItemImage.enabled = false;
    }

    public void CloseBrush()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (brushPanel.activeSelf)
            brushPanel.SetActive(false);

        GameManager.Instance.isBrushing = false;
        if (GameManager.Instance.brushItemImage != null)
            GameManager.Instance.brushItemImage.enabled = false;
    }

    public void CloseIndex()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        if (indexPanel.activeSelf)
            indexPanel.SetActive(false);
    }

    private void ConfirmDragonName()
    {
        string inputName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            AlarmManager.Instance.ShowAlarm("이름을 입력해주세요!");
            return;
        }

        GameManager.Instance.dragonHealth.stats.dragonName = inputName;

        dragonNameInputPanel.SetActive(false);

        nameInputField.text = string.Empty;

        AlarmManager.Instance.ShowAlarm($"{inputName}! 앞으로 잘해보자~");

        if (TutorialManager.Instance.currentStep == 6 && TutorialManager.Instance.tutorialActive)
            TutorialManager.Instance.NextStep();
    }

    public void CloseStat()
    { 
        statPanel.SetActive(false);
    }

    private void AllMainPanelsOut()
    {
        menuPanel.SetActive(false);
        statPanel.SetActive(false);
        helpPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        feedPanel.SetActive(false);
        playPanel.SetActive(false);
        bathUi.SetActive(false);
        soapPanel.SetActive(false);
        brushPanel.SetActive(false);
        showerImageObject.SetActive(false);
        dragonNameInputPanel.SetActive(false);
        indexPanel.SetActive(false);
    }
}
