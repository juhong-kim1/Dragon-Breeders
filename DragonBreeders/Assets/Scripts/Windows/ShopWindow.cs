using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : GenericWindow
{
    public Button backButton;

    public void OnClickBack()
    {
        manager.Open(Windows.Map);
    }
}
