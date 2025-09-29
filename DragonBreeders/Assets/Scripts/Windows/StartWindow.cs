using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public Button startButton;

    public GameManager gameManager;

    private void OnEnable()
    {
        TutorialManager.Instance.tutorialPanel.SetActive(false);
    }

    public void OnClickStart()
    {
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
}
