using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ShopSlot[] shopSlots = new ShopSlot[6];
    private List<ShopTableData> allItems;

    public TextMeshProUGUI resetTimerText;

    private void Start()
    {
        if (ShouldResetShop())
        {
            ResetShop();
        }
        else if (SaveLoadManager.Data?.ShopItems?.Count > 0)
        {
            LoadShopData(SaveLoadManager.Data.ShopItems);
        }
        else
        {
            PopulateShop();
        }
    }

    private void Update()
    {
        UpdateResetTimer();
    }

    private void UpdateResetTimer()
    {
        if (resetTimerText == null) return;

        if (string.IsNullOrEmpty(SaveLoadManager.Data.LastShopResetTime))
        {
            resetTimerText.text = "00:00";
            return;
        }

        long lastResetTimeBinary = System.Convert.ToInt64(SaveLoadManager.Data.LastShopResetTime);
        System.DateTime lastResetTime = System.DateTime.FromBinary(lastResetTimeBinary);
        System.DateTime nextResetTime = lastResetTime.AddMinutes(15);
        System.DateTime currentTime = System.DateTime.Now;

        if (currentTime >= nextResetTime)
        {
            resetTimerText.text = "00:00";
            ResetShop();
        }
        else
        {
            System.TimeSpan timeLeft = nextResetTime - currentTime;
            int minutes = (int)timeLeft.TotalMinutes;
            int seconds = timeLeft.Seconds;
            resetTimerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    private void PopulateShop()
    {
        allItems = DataTableManger.ShopTable.GetShopItems();

        List<ShopTableData> randomItems = GetRandomItems(6);

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i >= randomItems.Count)
            {
                shopSlots[i].gameObject.SetActive(false);
                continue;
            }

            var shopData = randomItems[i];
            var runtimeItem = CreateRuntimeItem(shopData.ITEM_ID_SHOP, shopData.PRICE);
            shopSlots[i].SetItem(runtimeItem, shopData.PRICE);
        }
    }

    private List<ShopTableData> GetRandomItems(int count)
    {
        List<ShopTableData> copy = new List<ShopTableData>(allItems);
        List<ShopTableData> result = new List<ShopTableData>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }

    private IItem CreateRuntimeItem(int itemId, int price)
    {
        var itemData = DataTableManger.ItemTable.Get(itemId);
        if (itemData == null)
        {
            Debug.LogError($"ItemTable에 ID {itemId} 없음");
            return null;
        }

        Item runtimeItem = new Item
        {
            itemID = itemData.ITEM_ID,
            itemName = itemData.StringName,
            icon = itemData.SpriteIcon,
            description = itemData.StringDescription,
            itemType = itemData.ITEM_TYPE,
            price = price,
        };

        return runtimeItem;
    }

    public void SaveShopData(List<SaveShopData> shopData)
    {
        shopData.Clear();
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (shopSlots[i].item != null)
            {
                Debug.Log($"저장 - 슬롯 {i}: {shopSlots[i].item.GetName()}, 구매됨: {shopSlots[i].isPurchased}");
                shopData.Add(new SaveShopData(shopSlots[i].item, shopSlots[i].itemPrice, shopSlots[i].isPurchased));
            }
            else
            {
                Debug.Log($"저장 - 슬롯 {i}: 빈 슬롯");
            }
        }
    }

    public void LoadShopData(List<SaveShopData> shopData)
    {
        Debug.Log($"상점 로드 시작: {shopData.Count}개 아이템");

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i >= shopData.Count)
            {
                shopSlots[i].gameObject.SetActive(false);
                continue;
            }

            var saveData = shopData[i];
            Debug.Log($"로드 - 슬롯 {i}: ID {saveData.itemId}, 가격 {saveData.price}, 구매됨: {saveData.isPurchased}");

            var runtimeItem = CreateRuntimeItem(saveData.itemId, saveData.price);

            if (runtimeItem != null)
            {
                shopSlots[i].SetItem(runtimeItem, saveData.price);

                if (saveData.isPurchased)
                {
                    shopSlots[i].SetPurchased();
                    Debug.Log($"슬롯 {i} 구매 완료 상태로 설정");
                }
            }
        }
    }

    private bool ShouldResetShop()
    {
        if (string.IsNullOrEmpty(SaveLoadManager.Data.LastShopResetTime))
            return true;

        long lastResetTimeBinary = System.Convert.ToInt64(SaveLoadManager.Data.LastShopResetTime);
        System.DateTime lastResetTime = System.DateTime.FromBinary(lastResetTimeBinary);
        System.DateTime currentTime = System.DateTime.Now;

        double hoursPassed = (currentTime - lastResetTime).TotalHours;

        return hoursPassed >= 1.0;
    }

    private void ResetShop()
    {
        SaveLoadManager.Data.LastShopResetTime = System.DateTime.Now.ToBinary().ToString();
        SaveLoadManager.Data.ShopItems.Clear();

        foreach (var slot in shopSlots)
        {
            if (slot != null)
            {
                slot.isPurchased = false;
                slot.purchaseButtonText.text = "구매";
                slot.ClearSlot();
            }
        }

        PopulateShop();
        Debug.Log("상점이 리셋되었습니다!");
    }
}
