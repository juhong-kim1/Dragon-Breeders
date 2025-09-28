using System;
using System.Collections.Generic;

[Serializable]
public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

[Serializable]
public class SaveDataV1 : SaveData
{
    public int Coin { get; set; } = 0;
    public Dictionary<int, int> InventoryItems { get; set; } = new Dictionary<int, int>();
    public SaveDragonData CurrentDragon { get; set; } = null;
    public List<DragonEntry> DragonIndex { get; set; } = new List<DragonEntry>();
    public List<SaveEggData> EggVault { get; set; } = new List<SaveEggData>();
    public List<SaveShopData> ShopItems { get; set; } = new List<SaveShopData>();

    public List<int> activeStatusList = new List<int>();

    public Dictionary<int, float> statusTimers = new Dictionary<int, float>();
    public int TutorialStep { get; set; } = 0;
    public bool TutorialCompleted { get; set; } = false;
    public string LastSaveTime { get; set; } = "";
    public string LastShopResetTime { get; set; } = "";

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}

