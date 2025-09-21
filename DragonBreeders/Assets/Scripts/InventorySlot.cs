using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI itemName;
    public GameObject amountParent;
    public IItem item;
    private int amount;

    void Awake()
    {
        ClearItem();
    }

    public void SetItem(IItem newItem, int newAmount = 1)
    {
        item = newItem;
        amount = newAmount;

        if (icon != null && item != null && amountParent != null)
        {
            icon.sprite = item.GetIcon();
            icon.enabled = true;

            itemName.text = item.GetName();
            itemName.enabled = true;

            amountParent.SetActive(true);
        }

        Debug.Log($"슬롯에 아이템 설정: ID={item.GetID()}, 이름={item.GetName()}, 스프라이트={icon.sprite?.name}, 수량={amount}");

        UpdateAmountText();
        Debug.Log($"슬롯에 아이템 설정: {item.GetName()} x{amount}");
    }

    public void AddAmount(int add)
    {
        amount += add;
        UpdateAmountText();
    }

    public int GetAmount() => amount;

    public void ClearItem()
    {
        item = null;
        amount = 0;
        if (icon != null) icon.enabled = false;
        if (amountParent != null) amountParent.SetActive(false);
        UpdateAmountText();
    }

    private void UpdateAmountText()
    {
        if (amountText != null)
        {
            amountText.text = amount > 0 ? amount.ToString() : "";
        }
    }

    public bool IsEmpty()
    {
        Debug.Log($"슬롯 {gameObject.name}, item: {item}");
        return item == null;
    }
    public bool IsSameItem(IItem other) => !IsEmpty() && item.GetID() == other.GetID();
}
