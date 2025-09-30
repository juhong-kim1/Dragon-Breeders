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
        GameManager.Instance.soapItemImage.enabled = false;
        GameManager.Instance.brushItemImage.enabled = false;

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

        int itemType = currentItem.GetItemType();

        SoundManager.Instance.PlaySFX(SoundManager.Instance.uiClickItem);

        if (GameManager.Instance.isFeeding && itemType == 2)
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

        if (GameManager.Instance.isPlaying && itemType == 6)
        {
            GameManager.Instance.playItemDiscription.text = currentItem.GetDescription();

            if (!GameManager.Instance.canPlay)
            {
                AlarmManager.Instance.ShowAlarm("방금 놀았어요!");
                return;
            }

            GameManager.Instance.playItemImage.enabled = true;
            GameManager.Instance.playItemImage.sprite = currentItem.GetIcon();

            DragItem playDrag = GameManager.Instance.playItemImage.GetComponent<DragItem>();
            if (playDrag != null)
            {
                playDrag.SetCurrentItem(currentItem);
            }
        }

        if (GameManager.Instance.isSoaping && itemType == 4)
        {
            if (!GameManager.Instance.hasSoaped)
            {
                GameManager.Instance.soapItemDiscription.text = currentItem.GetDescription();
                GameManager.Instance.soapItemImage.enabled = true;
                GameManager.Instance.soapItemImage.sprite = currentItem.GetIcon();
                DragItem soapDrag = GameManager.Instance.soapItemImage.GetComponent<DragItem>();
                if (soapDrag != null)
                {
                    soapDrag.SetCurrentItem(currentItem);
                }
            }
            else
            {
                AlarmManager.Instance.ShowAlarm("이미 비누를 사용했어요!");
            }
        }

        if (GameManager.Instance.isBrushing && itemType == 5)
        {
            if (!GameManager.Instance.hasSoaped)
            {
                AlarmManager.Instance.ShowAlarm("먼저 비누를 사용해주세요!");
            }
            else if (!GameManager.Instance.hasBrushed)
            {
                GameManager.Instance.brushItemDiscription.text = currentItem.GetDescription();
                GameManager.Instance.brushItemImage.enabled = true;
                GameManager.Instance.brushItemImage.sprite = currentItem.GetIcon();
                DragItem brushDrag = GameManager.Instance.brushItemImage.GetComponent<DragItem>();
                if (brushDrag != null)
                {
                    brushDrag.SetCurrentItem(currentItem);
                }
            }
            else
            {
                AlarmManager.Instance.ShowAlarm("이미 브러쉬를 사용했어요!");
            }
        }
    }
}
