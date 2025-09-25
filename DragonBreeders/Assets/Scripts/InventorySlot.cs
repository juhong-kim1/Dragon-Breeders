using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Components")]
    public Image icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI amountText;
    public GameObject amountParent;

    public Button itemButton;

    private IItem currentItem;
    private int currentAmount;

    private void Start()
    {
        itemButton = GetComponent<Button>();

        itemButton.onClick.AddListener(OnClickItemImages);

        GameManager.Instance.feedItemImage.enabled = false;
        GameManager.Instance.playItemImage.enabled = false;
    }

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

        if (GameManager.Instance.isFeeding)
        {
            GameManager.Instance.feedItemDiscription.text = currentItem.GetDescription();
            GameManager.Instance.feedItemImage.enabled = true;
            GameManager.Instance.feedItemImage.sprite = currentItem.GetIcon();

            DragItem feedDrag = GameManager.Instance.feedItemImage.GetComponent<DragItem>();
            if (feedDrag != null)
            {
                feedDrag.SetCurrentItem(currentItem);
            }
        }

        if (GameManager.Instance.isPlaying)
        {
            GameManager.Instance.playItemDiscription.text = currentItem.GetDescription();
            GameManager.Instance.playItemImage.enabled = true;
            GameManager.Instance.playItemImage.sprite = currentItem.GetIcon();

            DragItem playDrag = GameManager.Instance.playItemImage.GetComponent<DragItem>();
            if (playDrag != null)
            {
                playDrag.SetCurrentItem(currentItem);
            }
        }
    }
}
