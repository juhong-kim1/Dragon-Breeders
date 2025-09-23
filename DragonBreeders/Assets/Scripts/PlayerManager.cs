using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI coinTextShop;

    public int coin;
    public int famePoint = 0;

    void Start()
    {
        coin = 10000;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinText.text = coin.ToString();
        coinTextShop.text = coin.ToString();
    }
}
