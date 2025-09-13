using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public Button startButton;


    public void OnClickStart()
    {
        manager.Open(Windows.Game);
    }
}
