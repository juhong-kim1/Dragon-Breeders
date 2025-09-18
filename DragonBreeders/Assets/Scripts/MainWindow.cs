using UnityEngine;
using UnityEngine.UI;

public class MainWindow : GenericWindow
{
    public Button mapButton;
    public Button menuButton;
    public Button statButton;
    public Button helpButton;

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
    }

    public void OnClickMap()
    {
        manager.Open(Windows.Map);
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
