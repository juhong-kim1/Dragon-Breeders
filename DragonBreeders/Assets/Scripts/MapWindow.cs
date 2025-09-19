using UnityEngine;
using UnityEngine.UI;

public class MapWindow : GenericWindow
{
    public Button backButton;
    public Button menuButton;
    public Button statButton;
    public Button helpButton;

    public GameObject eggVaultWindowObject;
    public GameObject mainWindowObject;
    public TutorialManager tutorialManager;
    public GameManager gameManager;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject helpPanel;

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        statButton.onClick.AddListener(ToggleStat);
        helpButton.onClick.AddListener(ToggleHelp);
        menuPanel.SetActive(false);
        statPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        manager.Open(Windows.Start);
        gameManager.alarmPanel.gameObject.SetActive(false);
    }

    public void OnClickBack()
    {
        manager.Open(Windows.Game);

        if (tutorialManager != null && gameManager.dragonHealth != null)
            tutorialManager.OnWindowOpened(mainWindowObject);
    }

    public void OnClickEgg()
    {
        manager.Open(Windows.EggVault);

        if (tutorialManager != null)
            tutorialManager.OnWindowOpened(eggVaultWindowObject);
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
}
