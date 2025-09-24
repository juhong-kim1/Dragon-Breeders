using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RewardItem
{
    public int itemId;
    public int quantity;
    public ItemTableData itemData;

    public RewardItem(int itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
        this.itemData = DataTableManger.ItemTable.Get(itemId);
    }

    public override string ToString()
    {
        string itemName = itemData?.StringName ?? "Unknown Item";
        return $"{itemName} x{quantity}";
    }
}

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private int rewardItemCount = 3;

    public static RewardSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<RewardItem> GenerateRewards(int biome)
    {
        List<RewardItem> rewards = new List<RewardItem>();

        List<DropTableData> availableDrops = DataTableManger.DropTable.GetDropsByBiome(biome);

        if (availableDrops.Count == 0)
        {
            Debug.LogWarning($"바이옴 {biome}에서 드롭 가능한 아이템이 없습니다!");
            return rewards;
        }

        Debug.Log($"바이옴 {biome}에서 {availableDrops.Count}개 아이템 드롭 가능");

        var uniqueItems = availableDrops.GroupBy(drop => drop.ITEM_ID)
                                       .Select(group => group.First())
                                       .ToList();

        if (uniqueItems.Count < rewardItemCount)
        {
            Debug.LogWarning($"바이옴 {biome}에 서로 다른 아이템이 {uniqueItems.Count}개밖에 없습니다! (필요: {rewardItemCount}개)");
        }

        List<DropTableData> weightedList = CreateWeightedList(uniqueItems);
        List<int> usedItemIds = new List<int>();

        int itemsToGenerate = Mathf.Min(rewardItemCount, uniqueItems.Count);

        for (int i = 0; i < itemsToGenerate; i++)
        {
            DropTableData selectedDrop = SelectRandomDrop(weightedList, usedItemIds);

            if (selectedDrop == null)
            {
                Debug.LogWarning("더 이상 선택할 수 있는 아이템이 없습니다!");
                break;
            }

            int quantity = Random.Range(selectedDrop.MINDROP, selectedDrop.MAXDROP + 1);

            RewardItem reward = new RewardItem(selectedDrop.ITEM_ID, quantity);
            rewards.Add(reward);

            Debug.Log($"보상 {i + 1}: {reward}");

            usedItemIds.Add(selectedDrop.ITEM_ID);
        }

        return rewards;
    }
    private List<DropTableData> CreateWeightedList(List<DropTableData> drops)
    {
        List<DropTableData> weightedList = new List<DropTableData>();

        foreach (var drop in drops)
        {
            for (int i = 0; i < drop.DROP_RATE; i++)
            {
                weightedList.Add(drop);
            }
        }

        return weightedList;
    }

    private DropTableData SelectRandomDrop(List<DropTableData> weightedList, List<int> usedItemIds)
    {
        if (weightedList.Count == 0) return null;

        List<DropTableData> availableDrops = weightedList.Where(drop => !usedItemIds.Contains(drop.ITEM_ID)).ToList();

        if (availableDrops.Count == 0) return null;

        int randomIndex = Random.Range(0, availableDrops.Count);
        return availableDrops[randomIndex];
    }

    public void GiveRewardsToPlayer(List<RewardItem> rewards)
    {
        if (GameManager.Instance?.inventoryManager == null)
        {
            Debug.LogError("GameManager 또는 InventoryManager를 찾을 수 없습니다!");
            return;
        }

        foreach (var reward in rewards)
        {
            GameManager.Instance.inventoryManager.AddItem(reward.itemId, reward.quantity);
            Debug.Log($"플레이어가 {reward}를 획득했습니다!");
        }

        Debug.Log($"총 {rewards.Count}개의 보상 아이템이 인벤토리에 추가되었습니다.");
    }

    public void SetRewardCount(int count)
    {
        rewardItemCount = Mathf.Max(1, count);
    }
}
