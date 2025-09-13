using UnityEngine;
using UnityEngine.UI;

public class MainWindow : GenericWindow
{
    public Button mapButton;


    public void OnClickMap()
    {
        manager.Open(Windows.Map);
    }
}
