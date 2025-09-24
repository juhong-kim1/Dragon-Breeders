using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public Button startButton;

    public GameManager gameManager;


    public void OnClickStart()
    {
        manager.Open(Windows.Game);
        gameManager.alarmPanel.gameObject.SetActive(true);
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
