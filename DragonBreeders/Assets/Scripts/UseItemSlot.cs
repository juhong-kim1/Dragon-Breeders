using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseItemSlot : MonoBehaviour
{
    [Header("UI Components")]
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI amountText;
    public GameObject amountParent;
    public TextMeshProUGUI itemDiscription;

    private IItem currentItem;
    private int currentAmount;

    public void SetItem(IItem item, int amount = 1)
    {
        currentItem = item;
        currentAmount = amount;

        if (item != null)
        {
            if (icon != null)
            {
                icon.sprite = item.GetIcon();
                icon.enabled = true;
            }

            if (itemName != null)
            {
                itemName.text = item.GetName();
                itemName.enabled = true;
            }

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

    public void OnClickItemImages()
    {
        if (currentItem == null) return;

        itemDiscription.text = currentItem.GetDescription();
    }
}
