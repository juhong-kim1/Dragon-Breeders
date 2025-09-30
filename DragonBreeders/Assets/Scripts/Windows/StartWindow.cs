using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public Button startButton;
    public GameObject settingPanel;
    public Button xButton;
    public Button settingButton;

    public GameManager gameManager;

    private void OnEnable()
    {
        TutorialManager.Instance.tutorialPanel.SetActive(false);
    }

    private void Start()
    {
        settingPanel.SetActive(false);
        settingButton.onClick.AddListener(OnClickSetting);
        xButton.onClick.AddListener(CloseSetting);
    }

    public void OnClickStart()
    {
        if (!TutorialManager.Instance.isTutorialClear)
        {
            TutorialManager.Instance.tutorialPanel.SetActive(true);
        }

        Debug.Log($"튜토리얼클리어 {TutorialManager.Instance.isTutorialClear}");
        Debug.Log($"튜토리얼액티브 {TutorialManager.Instance.tutorialActive}");

        manager.Open(Windows.Game);
        gameManager.alarmPanel.gameObject.SetActive(true);

        SoundManager.Instance.RandomMainBGMPlay();
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }

    public void OnClickSetting()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    private void CloseSetting()
    { 
        settingPanel.SetActive(false);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);
    }
}
