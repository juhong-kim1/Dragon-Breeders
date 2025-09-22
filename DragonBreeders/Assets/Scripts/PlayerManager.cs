using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerManager : MonoBehaviour
{
    public int coin = 1000;
    public Inventory inventory;

    public int famePoint;
    public TextMeshProUGUI coinTextInventory;
    public TextMeshProUGUI coinTextShop;

    private void Start()
    {
        coin = 10000;
        UpdateCoinUI();
    }

    public bool TrySpendCoin(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    public void UpdateCoinUI()
    {
        coinTextInventory.text = coin.ToString();
        coinTextShop.text = coin.ToString();
    }
}
