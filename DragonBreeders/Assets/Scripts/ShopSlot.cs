using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI purchaseButtonText;
    public IItem item;
    public Button purchaseButton;
    public TextMeshProUGUI itemDiscription;

    public Inventory inventory;
    public PlayerManager playerManager;

    public void Start()
    {
        purchaseButton.onClick.AddListener(onClickPurchaseButton);
    }

    public void SetItem(IItem newItem, int price)
    {
        item = newItem;

        if (icon != null && item != null)
        {
            icon.sprite = item.GetIcon();
            icon.enabled = true;

            itemName.text = item.GetName();
            itemName.enabled = true;

            priceText.text = price.ToString();
        }

        Debug.Log($"상점 슬롯 설정: {item.GetName()} / 가격: {price}");
    }

    public void ClearSlot()
    {
        item = null;

        if (icon != null) icon.enabled = false;
        if (itemName != null) itemName.text = "";
        if (priceText != null) priceText.text = "";
    }

    public void onClickPurchaseButton()
    {
        if (item == null) return;

        if (playerManager.TrySpendCoin(item.GetPrice()))
        {
            inventory.AddItem(item);
            AlarmManager.Instance.ShowAlarm($"{item.GetName()} 구매완료!");
            ClearSlot();
            purchaseButtonText.text = "구매 완료";
            Debug.Log("구매 완료");
        }
        else
        {
            AlarmManager.Instance.ShowAlarm($"코인이 부족합니다");
            Debug.Log("코인 부족");
        }
    }

    public void OnClickItemImages()
    { 
        if (item == null) return;

        itemDiscription.text = item.GetDescription();
    }
}