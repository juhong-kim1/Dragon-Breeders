using UnityEngine;
using UnityEngine.UI;

public class HospitalWindow : GenericWindow
{
    public Button backButton;
    public Button PhamacyButton;

    public void OnClickBack()
    {
        manager.Open(Windows.Map);

    }

    public void OnClickPhamacy()
    {
        manager.Open(Windows.Pharmacy);

    }
}
