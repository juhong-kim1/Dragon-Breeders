using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots;

    public void AddItem(IItem itemData, int amount = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].IsEmpty())
            {
                slots[i].SetItem(itemData);
                Debug.Log($"아이템 추가 성공: {itemData.GetName()} x{amount} → Slot {i}");
                return;
            }
        }
        Debug.Log("인벤토리가 가득 찼습니다!");
    }
}
