using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI coinTextShop;
    public TextMeshProUGUI coinTextMap;

    public int coin;
    public int famePoint = 0;

    void Start()
    {
        coin = 3000;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinText.text = coin.ToString();
        coinTextShop.text = coin.ToString();
        coinTextMap.text = coin.ToString();
    }
}
