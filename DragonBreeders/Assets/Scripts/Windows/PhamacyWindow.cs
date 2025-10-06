using UnityEngine;
using UnityEngine.UI;

public class PhamacyWindow : GenericWindow
{
    public Button BackButton;

    private void Start()
    {
        BackButton.onClick.AddListener(ToggleBack);
    }

    private void ToggleBack()
    {
        manager.Open(Windows.Hospital);
    }
}
