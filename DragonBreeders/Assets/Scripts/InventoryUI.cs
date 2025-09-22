using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI 설정")]
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private int initialPoolSize = 30;

    [Header("데이터베이스")]
    [SerializeField] private ItemDatabase itemDatabase;

    // 슬롯 풀링
    private Queue<InventorySlot> slotPool = new Queue<InventorySlot>();
    private List<InventorySlot> activeSlots = new List<InventorySlot>();

    void Start()
    {
        InitializeSlotPool();
    }

    void InitializeSlotPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.gameObject.SetActive(false);
            slotPool.Enqueue(newSlot);
        }

        Debug.Log($"인벤토리 슬롯 풀 초기화: {initialPoolSize}개");
    }

    public void RefreshDisplay(Dictionary<int, int> inventoryItems)
    {
        ReturnAllSlotsToPool();

        var itemList = new List<KeyValuePair<int, int>>(inventoryItems);
        itemList.Sort((a, b) => a.Key.CompareTo(b.Key));

        foreach (var itemEntry in itemList)
        {
            int itemID = itemEntry.Key;
            int amount = itemEntry.Value;

            IItem itemData = itemDatabase.GetItem(itemID);
            if (itemData != null)
            {
                InventorySlot slot = GetSlotFromPool();
                slot.gameObject.SetActive(true);
                slot.SetItem(itemData, amount);

                slot.transform.SetAsLastSibling();

                activeSlots.Add(slot);
            }
        }
    }

    private InventorySlot GetSlotFromPool()
    {
        if (slotPool.Count == 0)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
            return newSlot;
        }

        return slotPool.Dequeue();
    }

    private void ReturnAllSlotsToPool()
    {
        for (int i = activeSlots.Count - 1; i >= 0; i--)
        {
            InventorySlot slot = activeSlots[i];
            slot.ClearSlot();
            slot.gameObject.SetActive(false);

            slot.transform.SetAsLastSibling();

            slotPool.Enqueue(slot);
        }
        activeSlots.Clear();
    }



    public void ClearDisplay()
    {
        ReturnAllSlotsToPool();
    }
}
