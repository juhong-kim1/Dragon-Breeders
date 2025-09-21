using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public IItem item;

    void Awake()
    {
        item = null;
        if (icon != null)
            icon.enabled = false;
    }

    public void SetItem(IItem newItem)
    {
        item = newItem;
        if (icon != null && item != null)
        {
            icon.sprite = item.GetIcon();
            icon.enabled = true;
            Debug.Log($"아이템 슬롯 아이콘 설정 완료: {item.GetName()}");
        }
    }

    public void ClearItem()
    {
        item = null;
        if (icon != null)
            icon.enabled = false;
    }

    public bool IsEmpty()
    {
        return item == null;
    }
}
