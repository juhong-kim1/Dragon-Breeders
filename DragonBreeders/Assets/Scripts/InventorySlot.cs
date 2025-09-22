using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Components")]
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI amountText;
    public GameObject amountParent;

    private IItem currentItem;
    private int currentAmount;

    public void SetItem(IItem item, int amount = 1)
    {
        currentItem = item;
        currentAmount = amount;

        if (item != null)
        {
            // 아이콘 설정
            if (icon != null)
            {
                icon.sprite = item.GetIcon();
                icon.enabled = true;
            }

            // 이름 설정
            if (itemName != null)
            {
                itemName.text = item.GetName();
                itemName.enabled = true;
            }

            // 수량 설정
            UpdateAmountDisplay();
        }
    }

    private void UpdateAmountDisplay()
    {
        if (amountParent != null)
        {
            amountParent.SetActive(currentAmount > 1);
        }

        if (amountText != null)
        {
            amountText.text = currentAmount.ToString();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentAmount = 0;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (itemName != null)
        {
            itemName.text = "";
            itemName.enabled = false;
        }

        if (amountParent != null)
        {
            amountParent.SetActive(false);
        }
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public IItem GetItem()
    {
        return currentItem;
    }

    public int GetAmount()
    {
        return currentAmount;
    }
}
