using UnityEngine;
using UnityEngine.UI;

public class HomeWindow : GenericWindow
{
    public Button backButton;
    public Button EggButton;

    public void OnClickBack()
    {
        manager.Open(Windows.Map);
        
    }

    public void OnClickEgg()
    {
        manager.Open(Windows.EggVault);
    }
}
