using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI coinText;

    public int coin;
    public int famePoint = 0;

    void Start()
    {
        coin = 1000;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        int coinAmount = playerInventory.GetAmountByID(5070001);
        coinText.text = coinAmount.ToString();
    }
}
