using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public Button stopButton;

    public GameObject stopPanel;

    private void Start()
    {
        stopButton.onClick.AddListener(() => ToggleStopButton());
        stopPanel.gameObject.SetActive(false);
    }

    public void OnClickQuitOut()
    {
        GameManager.Instance.MoveSceneOnOff();


        SceneManager.UnloadSceneAsync("BattleScene");
    }


    public void ToggleStopButton()
    {
        stopPanel.SetActive(!stopPanel.activeSelf);
    }


}
