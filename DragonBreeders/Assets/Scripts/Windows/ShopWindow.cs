using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : GenericWindow
{
    public Button backButton;

    public void OnClickBack()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickBack);

        manager.Open(Windows.Map);
    }
}
