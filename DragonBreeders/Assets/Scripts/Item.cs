using UnityEngine;

public interface IItem
{
    int GetID();
    string GetName();
    int GetItemType();
    Sprite GetIcon();
    string GetDescription();
    int GetPrice();
}

public class Item : IItem
{
    public int itemID;
    public string itemName;
    public int itemType;
    public Sprite icon;
    public string description;
    public int price;

    public int GetID() => itemID;
    public string GetName() => itemName;
    public int GetItemType() => itemType;
    public Sprite GetIcon() => icon;
    public string GetDescription() => description;
    public int GetPrice() => price;
}
