using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI itemName;
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

        if (icon != null && item != null)
        {
            icon.sprite = item.GetIcon();
            icon.enabled = true;

            itemName.text = item.GetName();
            itemName.enabled = true;
        }

        UpdateAmountText();
        Debug.Log($"½½·Ô¿¡ ¾ÆÀÌÅÛ ¼³Á¤: {item.GetName()} x{amount}");
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
        Debug.Log($"½½·Ô {gameObject.name}, item: {item}");
        return item == null;
    }
    public bool IsSameItem(IItem other) => !IsEmpty() && item.GetID() == other.GetID();
}
