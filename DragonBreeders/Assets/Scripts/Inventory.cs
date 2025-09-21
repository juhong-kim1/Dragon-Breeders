using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public InventorySlot slotPrefab;
    public Transform slotParent;

    public void AddItem(IItem itemData, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.item.GetID() == itemData.GetID())
            {
                slot.AddAmount(amount);
                return;
            }
        }
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemData);
                Debug.Log($"아이템 추가 성공: {itemData.GetName()} → 기존 슬롯");
                return;
            }
        }

        InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
        newSlot.SetItem(itemData);
        slots.Add(newSlot);
        Debug.Log($"아이템 추가 성공: {itemData.GetName()} → 새 슬롯 생성");
    }
}