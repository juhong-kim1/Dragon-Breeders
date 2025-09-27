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

    public void OnClickStart()
    {
        manager.Open(Windows.Start);
        gameManager.alarmPanel.gameObject.SetActive(false);

    }

    public void OnClickMap()
    {
        manager.Open(Windows.Map);

        if (tutorialManager != null)
            tutorialManager.OnWindowOpened(mapWindowObject);
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    private void ToggleStat()
    {
        statPanel.SetActive(!statPanel.activeSelf);
        if (TutorialManager.Instance.currentStep == 8)
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
        if (InventoryPanel.activeSelf)
            InventoryPanel.SetActive(false);
    }

    private void ToggleFeed()
    { 
        feedPanel.SetActive(!feedPanel.activeSelf);
        
        if(TutorialManager.Instance.currentStep == 9)
        TutorialManager.Instance.NextStep();

        ClosePlay();

        if (feedPanel.activeSelf)
        {
            GameManager.Instance.GetFeed();
        }
    }

    public void CloseFeed()
    {
        if (feedPanel.activeSelf)
            feedPanel.SetActive(false);

        GameManager.Instance.isFeeding = false;
        GameManager.Instance.feedItemImage.enabled = false;
    }

    public void TogglePlay()
    { 
        playPanel.SetActive(!playPanel.activeSelf);
        CloseFeed();

        if (playPanel.activeSelf)
        { 
            GameManager.Instance.GetPlay();
        }
    }

    public void ClosePlay()
    {
        if (playPanel.activeSelf)
            playPanel.SetActive(false);

        GameManager.Instance.isPlaying = false;
        GameManager.Instance.playItemImage.enabled = false;
    }

    public void ToggleBath()
    { 
        downUi.SetActive(false);
        bathUi.SetActive(true);
    }

    public void CloseBath()
    {
        downUi.SetActive(true);
        bathUi.SetActive(false);

        soapPanel.SetActive(false);
        brushPanel.SetActive(false);
    }
    public void ToggleSoap()
    {
        soapPanel.SetActive(!soapPanel.activeSelf);
        CloseBrush();

        if (soapPanel.activeSelf)
        {
            GameManager.Instance.GetSoap();
        }
    }

    public void ToggleBrush()
    {
        brushPanel.SetActive(!brushPanel.activeSelf);
        CloseSoap();

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

        GameManager.Instance.isSoaping = false;
        if (GameManager.Instance.soapItemImage != null)
            GameManager.Instance.soapItemImage.enabled = false;

        GameManager.Instance.isBrushing = false;
        if (GameManager.Instance.brushItemImage != null)
            GameManager.Instance.brushItemImage.enabled = false;
    }

    public void CloseSoap()
    {
        if (soapPanel.activeSelf)
            soapPanel.SetActive(false);

        GameManager.Instance.isSoaping = false;
        if (GameManager.Instance.soapItemImage != null)
            GameManager.Instance.soapItemImage.enabled = false;
    }

    public void CloseBrush()
    {
        if (brushPanel.activeSelf)
            brushPanel.SetActive(false);

        GameManager.Instance.isBrushing = false;
        if (GameManager.Instance.brushItemImage != null)
            GameManager.Instance.brushItemImage.enabled = false;
    }

    public void CloseIndex()
    {
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

        AlarmManager.Instance.ShowAlarm($"{inputName}! 앞으로 잘해보자~");
    }
}
