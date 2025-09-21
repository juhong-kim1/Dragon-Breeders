using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public InventorySlot slotPrefab;
    public Transform slotParent;
    public int initialSlots = 6;

    void Start()
    {
        for (int i = 0; i < initialSlots; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.ClearItem();
            slots.Add(newSlot);
        }
    }
    public void AddItem(IItem itemData, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.item.GetID() == itemData.GetID())
            {
                slot.AddAmount(amount);
                Debug.Log($"아이템 수량 증가: {itemData.GetName()} x{amount}");
                return;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemData, amount);
                Debug.Log($"아이템 추가: {itemData.GetName()} → 기존 슬롯");
                return;
            }
        }

        InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
        newSlot.SetItem(itemData, amount);
        slots.Add(newSlot);
        Debug.Log($"아이템 추가: {itemData.GetName()} → 새 슬롯 생성");
    }

    public int GetAmountByID(int itemID)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.item.GetID() == itemID)
            {
                return slot.GetAmount();
            }
        }
        return 0;
    }
}