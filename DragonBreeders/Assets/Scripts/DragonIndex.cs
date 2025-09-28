using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DragonEntry
{
    public string customName;
    public string speciesName;
    public string releaseDate;

    public float finalStamina;
    public float finalFatigue;
    public float finalHunger;
    public float finalIntimacy;
    public float finalClean;

    public int trainingWinCount;
    public int trainingLoseCount;
    public int playCount;
    public int bathCount;
    public int feedCount;
    public int restCount;
    public int passOutCount;

    public DragonEntry() { }

    public DragonEntry(DragonHealth dragon)
    {
        customName = dragon.stats.dragonName;
        speciesName = dragon.stats.dragonSpecies;
        releaseDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        finalStamina = dragon.stats.stamina;
        finalFatigue = dragon.stats.fatigue;
        finalHunger = dragon.stats.hunger;
        finalIntimacy = dragon.stats.intimacy;
        finalClean = dragon.stats.clean;

        var gameManager = GameManager.Instance;

        trainingWinCount = gameManager.trainingWinCount;
        trainingLoseCount = gameManager.trainingLoseCount;
        playCount = gameManager.playCount;
        bathCount = gameManager.bathCount;
        feedCount = gameManager.feedCount;
        restCount = gameManager.restCount;
        passOutCount = gameManager.passOutCount;
    }
}

public class DragonIndex : MonoBehaviour
{
    [SerializeField] private List<DragonEntry> indexedDragons = new List<DragonEntry>();

    public void RegisterDragon(DragonHealth dragon)
    {
        DragonEntry newEntry = new DragonEntry(dragon);
        indexedDragons.Add(newEntry);

        AlarmManager.Instance.ShowAlarm($"{newEntry.customName}이 도감에 등록되었습니다!");
        Debug.Log($"도감 등록: {newEntry.customName} ({newEntry.speciesName})");
    }

    public List<DragonEntry> GetAllEntries()
    {
        return indexedDragons;
    }

    public int GetIndexCount()
    {
        return indexedDragons.Count;
    }

    public void LoadEntries(List<DragonEntry> loadedEntries)
    {
        indexedDragons.Clear();
        indexedDragons.AddRange(loadedEntries);
        Debug.Log($"도감 로드 완료: {loadedEntries.Count}마리의 드래곤");
    }
}
