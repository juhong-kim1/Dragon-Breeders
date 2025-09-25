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

    public GameObject mapWindowObject;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private GameObject feedPanel;
    [SerializeField] private GameObject playPanel;

    [SerializeField] private InventoryManager inventoryManager;
    public TutorialManager tutorialManager;
    public GameManager gameManager;

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        statButton.onClick.AddListener(ToggleStat);
        helpButton.onClick.AddListener(ToggleHelp);
        inventoryButton.onClick.AddListener(ToggleInventory);
        feedButton.onClick.AddListener(ToggleFeed);
        playButton.onClick.AddListener(TogglePlay);
        menuPanel.SetActive(false);
        statPanel.SetActive(false);
        helpPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        feedPanel.SetActive(false);
        playPanel.SetActive(false);
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
}
