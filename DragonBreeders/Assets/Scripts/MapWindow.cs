using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapWindow : GenericWindow
{
    public Button backButton;
    public Button menuButton;
    public Button statButton;
    public Button helpButton;
    public Button trainingButton;

    public Button[] locationButtons = new Button[5];
    public Button[] difficultyButtons = new Button[3];

    private int locationIndex;

    public GameObject eggVaultWindowObject;
    public GameObject mainWindowObject;
    public TutorialManager tutorialManager;
    public GameManager gameManager;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject trainingLocationPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject trainingStartPanel;
    [SerializeField] private Image difficultyPanelImage;
    [SerializeField] private Image trainingStartPanelImage;

    private void Start()
    {
        menuButton.onClick.AddListener(ToggleMenu);
        statButton.onClick.AddListener(ToggleStat);
        helpButton.onClick.AddListener(ToggleHelp);
        trainingButton.onClick.AddListener(ToggleTraining);

        for (int i = 0; i < locationButtons.Length; ++i)
        {
            int index = i;

            locationIndex = i;
            locationButtons[i].onClick.AddListener(() => ToggleLocation(index));
        }

        for (int i = 0; i < difficultyButtons.Length; ++i)
        {
            difficultyButtons[i].onClick.AddListener(() => ToggleDifficulty(locationIndex));
        }

        //desertButton.onClick.AddListener(ToggleLocation);

        menuPanel.SetActive(false);
        statPanel.SetActive(false);
        helpPanel.SetActive(false);
        trainingLocationPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        trainingStartPanel.SetActive(false);
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

    public void OnClickTrainingStart()
    {
        manager.Open(Windows.EggVault);
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

    public void OnClickShop()
    {
        manager.Open(Windows.Shop);
    }

    private void ToggleTraining()
    { 
        trainingLocationPanel.SetActive(!trainingLocationPanel.activeSelf);
    }

    private void ToggleLocation(int index)
    {
        difficultyPanel.SetActive(true);

        switch (index)
        {
            case 0:
                difficultyPanelImage.color = new Color32(222,250,118,255);
                break;
            case 1:
                difficultyPanelImage.color = new Color32(30, 140, 254, 255);
                break;
            case 2:
                difficultyPanelImage.color = new Color32(58, 203, 92, 255);
                break;
            case 3:
                difficultyPanelImage.color = new Color32(118, 223, 79, 255);
                break;
            case 4:
                difficultyPanelImage.color = new Color32(230, 220, 230, 255);
                break;
        }
    }

    private void ToggleDifficulty(int index)
    {
        trainingStartPanel.SetActive(true);

        switch (index)
        {
            case 0:
                trainingStartPanelImage.color = new Color32(222, 250, 118, 255);
                break;
            case 1:
                trainingStartPanelImage.color = new Color32(30, 140, 254, 255);
                break;
            case 2:
                trainingStartPanelImage.color = new Color32(58, 203, 92, 255);
                break;
            case 3:
                trainingStartPanelImage.color = new Color32(118, 223, 79, 255);
                break;
            case 4:
                trainingStartPanelImage.color = new Color32(230, 220, 230, 255);
                break;
        }
    }
}
