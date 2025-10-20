using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private int initialPoolSize = 30;

    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private InventoryManager inventoryManager;

    private Queue<InventorySlot> slotPool = new Queue<InventorySlot>();
    private List<InventorySlot> activeSlots = new List<InventorySlot>();

    void Start()
    {
        InitializeSlotPool();
    }

    void OnEnable()
    {
        if (inventoryManager != null)
        {
            RefreshDisplay(inventoryManager.GetAllItems());
        }
    }

    void InitializeSlotPool()
    {
        for (int i = 0; i < 6; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.gameObject.SetActive(true);
            newSlot.ClearSlot();
            activeSlots.Add(newSlot);
        }

        for (int i = 6; i < initialPoolSize; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.gameObject.SetActive(false);
            slotPool.Enqueue(newSlot);
        }

        Debug.Log($"인벤토리 초기화: 기본 6개 슬롯 + 풀 {initialPoolSize - 6}개");
    }

    public void RefreshDisplay(Dictionary<int, int> inventoryItems)
    {
        RefreshDisplay(inventoryItems, -1);
    }

    public void RefreshDisplay(Dictionary<int, int> inventoryItems, int filterItemType)
    {
        ReturnExtraSlotsToPool();

        var itemList = new List<KeyValuePair<int, int>>(inventoryItems);

        if (filterItemType >= 0)
        {
            List<KeyValuePair<int, int>> filteredList = new List<KeyValuePair<int, int>>();

            foreach (var item in itemList)
            {
                var itemData = itemDatabase.GetItemTableData(item.Key);
                if (itemData != null && itemData.ITEM_TYPE == filterItemType)
                {
                    filteredList.Add(item);
                }
            }

            itemList = filteredList;
        }

        itemList.Sort((a, b) => a.Key.CompareTo(b.Key));

        int itemIndex = 0;

        for (int i = 0; i < 6; i++)
        {
            if (i < activeSlots.Count)
            {
                if (itemIndex < itemList.Count)
                {
                    var itemEntry = itemList[itemIndex];
                    IItem itemData = itemDatabase.GetItem(itemEntry.Key);

                    if (itemData != null)
                    {
                        activeSlots[i].SetItem(itemData, itemEntry.Value);
                        itemIndex++;
                    }
                }
                else
                {
                    activeSlots[i].ClearSlot();
                }
            }
        }

        while (itemIndex < itemList.Count)
        {
            var itemEntry = itemList[itemIndex];
            IItem itemData = itemDatabase.GetItem(itemEntry.Key);

            if (itemData != null)
            {
                InventorySlot slot = GetSlotFromPool();
                slot.gameObject.SetActive(true);
                slot.SetItem(itemData, itemEntry.Value);
                slot.transform.SetAsLastSibling();
                activeSlots.Add(slot);
            }
            itemIndex++;
        }

        //Debug.Log($"인벤토리 UI 새로고침: {itemList.Count}개 아이템, {activeSlots.Count}개 슬롯");
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

    private void ReturnExtraSlotsToPool()
    {
        for (int i = activeSlots.Count - 1; i >= 6; i--)
        {
            InventorySlot slot = activeSlots[i];
            slot.ClearSlot();
            slot.gameObject.SetActive(false);
            slot.transform.SetAsLastSibling();
            slotPool.Enqueue(slot);
            activeSlots.RemoveAt(i);
        }
    }

    public void ClearDisplay()
    {
        for (int i = 0; i < activeSlots.Count; i++)
        {
            if (i < 6)
            {
                activeSlots[i].ClearSlot();
            }
            else
            {
                activeSlots[i].ClearSlot();
                activeSlots[i].gameObject.SetActive(false);
                slotPool.Enqueue(activeSlots[i]);
            }
        }

        if (activeSlots.Count > 6)
        {
            activeSlots.RemoveRange(6, activeSlots.Count - 6);
        }

        Debug.Log("인벤토리 UI 모든 슬롯 정리");
    }

    public int GetDisplayedItemCount()
    {
        int count = 0;
        foreach (var slot in activeSlots)
        {
            if (!slot.IsEmpty())
                count++;
        }
        return count;
    }

    public List<InventorySlot> GetActiveSlots()
    {
        return new List<InventorySlot>(activeSlots);
    }
}