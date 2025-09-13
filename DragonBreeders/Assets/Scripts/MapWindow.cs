using UnityEngine;
using UnityEngine.UI;

public class MapWindow : GenericWindow
{
    public Button backButton;
    public void OnClickBack()
    {
        manager.Open(Windows.Game);
    }
}
