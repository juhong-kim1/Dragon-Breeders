using UnityEngine.SceneManagement;
using UnityEngine;
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
    }

    public void OnClickStart()
    {
        manager.Open(Windows.Start);
        GameManager.Instance.alarmPanel.gameObject.SetActive(false);
    }

    public void OnClickBack()
    {
        manager.Open(Windows.Game);

        if (tutorialManager != null && GameManager.Instance.dragonHealth != null)
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
        GameManager.Instance.MoveBattleScene();

        SoundManager.Instance.PlayBGM(SoundManager.Instance.BattleBGM);

        if(TutorialManager.Instance.currentStep == 17)
        TutorialManager.Instance.NextStep();
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
        if (GameManager.Instance.dragonHealth == null)
        {
            AlarmManager.Instance.ShowAlarm("È¥ÀÚ¼± ½Î¿ï ¼ö ¾ø¾î¿ä!");
            return;
        }

        trainingLocationPanel.SetActive(!trainingLocationPanel.activeSelf);
    }

    private void ToggleLocation(int index)
    {
        GameManager.Instance.TrainingPlace = (TrainingPlace)index + 1;
        locationIndex = index;
        difficultyPanel.SetActive(true);
        difficultyPanelImage.color = locationColors[index];

        if(TutorialManager.Instance.currentStep == 16)
        TutorialManager.Instance.NextStep();
    }

    private void ToggleDifficulty(int index)
    {
        GameManager.Instance.Difficulty = (Difficulty)index + 1;

        trainingStartPanel.SetActive(true);
        trainingStartPanelImage.color = locationColors[locationIndex];
    }

    private void ToggleXButton()
    {
        trainingStartPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        trainingLocationPanel.SetActive(false);
    }
}