using UnityEngine;
using UnityEngine.UI;

public class HomeWindow : GenericWindow
{
    public Button backButton;

    public void OnClickBack()
    {
        manager.Open(Windows.Game);
        
    }
}
