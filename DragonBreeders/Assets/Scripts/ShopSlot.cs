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

    public int itemPrice;

    public InventoryManager inventoryManager;
    public PlayerManager playerManager;

    public bool isPurchased = false;

    public void Start()
    {
        purchaseButton.onClick.AddListener(onClickPurchaseButton);

        //Button iconButton = icon.GetComponent<Button>();
        //if (iconButton != null)
        //{
        //    iconButton.onClick.AddListener(OnClickItemImages);
        //}
    }

    public void SetItem(IItem newItem, int price)
    {
        item = newItem;
        itemPrice = price;

        if (icon != null && item != null)
        {
            icon.sprite = item.GetIcon();
            icon.enabled = true;

            itemName.text = item.GetName();
            itemName.enabled = true;

            priceText.text = price.ToString();
        }

        Button iconButton = icon.GetComponent<Button>();
        if (iconButton != null)
        {
            iconButton.onClick.RemoveAllListeners();
            iconButton.onClick.AddListener(OnClickItemImages);
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
        if (item == null || isPurchased) return;

        if (playerManager != null && playerManager.coin >= itemPrice)
        {
            playerManager.coin -= itemPrice;
            playerManager.UpdateCoinUI();
            inventoryManager.AddItem(item.GetID(), 1);
            AlarmManager.Instance.ShowAlarm($"{item.GetName()} 구매완료!");
            SetPurchased();
            Debug.Log($"구매 완료: {item.GetName()}, 남은 코인: {playerManager.coin}");
        }
        else
        {
            AlarmManager.Instance.ShowAlarm("코인이 부족합니다");
            Debug.Log("코인 부족");
        }
    }

    public void OnClickItemImages()
    { 
        if (item == null) return;

        itemDiscription.text = item.GetDescription();
    }

    public void SetPurchased()
    {
        isPurchased = true;
        if (purchaseButtonText != null)
        {
            purchaseButtonText.text = "구매 완료";
        }
        Debug.Log($"SetPurchased 호출됨: {item?.GetName()}, isPurchased = {isPurchased}");
    }
}