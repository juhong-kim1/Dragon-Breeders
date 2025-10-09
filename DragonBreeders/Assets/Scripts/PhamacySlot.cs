using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PharmacySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI purchaseButtonText;
    public Button purchaseButton;
    public TextMeshProUGUI itemDiscription;

    private IItem item;
    private int itemPrice;
    private StatusType targetStatus;

    public PlayerManager playerManager;

    private void Start()
    {
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnClickPurchaseButton);
        }

        Button iconButton = icon.GetComponent<Button>();
        if (iconButton != null)
        {
            iconButton.onClick.AddListener(OnClickItemImage);
        }
    }

    public void SetItem(IItem newItem, int price, StatusType cureStatus)
    {
        item = newItem;
        itemPrice = price;
        targetStatus = cureStatus;

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
            iconButton.onClick.AddListener(OnClickItemImage);
        }

        Debug.Log($"약국 슬롯 설정: {item.GetName()} / 가격: {price} / 치료: {cureStatus}");
    }

    public void ClearSlot()
    {
        item = null;
        if (icon != null) icon.enabled = false;
        if (itemName != null) itemName.text = "";
        if (priceText != null) priceText.text = "";
    }

    private void OnClickPurchaseButton()
    {
        if (item == null) return;

        DragonHealth dragon = GameManager.Instance.dragonHealth;

        if (dragon == null)
        {
            AlarmManager.Instance.ShowAlarm("치료할 드래곤이 없어요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (!dragon.status.HasStatus(targetStatus))
        {
            string statusName = GetStatusName(targetStatus);
            AlarmManager.Instance.ShowAlarm($"{statusName} 상태가 아니에요!");
            SoundManager.Instance.PlayErrorSound();
            return;
        }

        if (playerManager != null && playerManager.coin >= itemPrice)
        {
            playerManager.coin -= itemPrice;
            playerManager.UpdateCoinUI();

            dragon.status.RemoveStatus(targetStatus);

            string statusName = GetStatusName(targetStatus);
            AlarmManager.Instance.ShowAlarm($"{item.GetName()} 사용! {statusName} 치료 완료!");
            SoundManager.Instance.PlaySFX(SoundManager.Instance.successAudioClip);

            Debug.Log($"{item.GetName()} 구매 및 사용 완료, {statusName} 치료");
        }
        else
        {
            AlarmManager.Instance.ShowAlarm("코인이 부족합니다");
            SoundManager.Instance.PlayErrorSound();
            Debug.Log("코인 부족");
        }
    }

    public void OnClickItemImage()
    {
        if (item == null) return;
        if (itemDiscription != null)
        {
            itemDiscription.text = item.GetDescription();
        }
    }

    private string GetStatusName(StatusType status)
    {
        switch (status)
        {
            case StatusType.Disease: return "질병";
            case StatusType.Injury: return "부상";
            default: return "알 수 없음";
        }
    }
}