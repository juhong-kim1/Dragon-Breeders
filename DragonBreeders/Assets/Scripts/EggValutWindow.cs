using UnityEngine;
using UnityEngine.UI;

public class EggVaultWindow : GenericWindow
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button backButton;

    public void OnClickBack()
    {
        manager.Open(Windows.Home);
    }
}
