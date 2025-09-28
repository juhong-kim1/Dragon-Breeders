using UnityEngine;

[System.Serializable]
public class SaveShopData
{
    public int itemId;
    public int price;
    public bool isPurchased;

    public SaveShopData() { }

    public SaveShopData(IItem item, int price, bool purchased = false)
    {
        itemId = item?.GetID() ?? 0;
        this.price = price;
        isPurchased = purchased;
    }
}
