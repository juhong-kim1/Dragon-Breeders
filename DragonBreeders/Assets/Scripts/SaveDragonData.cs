using UnityEngine;

[System.Serializable]
public class SaveDragonData
{
    public DragonStats stats;

    public int currentSpeciesType;
    public int currentElementType;
    public DragonGrowthState currentGrowth;
    public bool isPassOut;

    public int trainingWinCount;
    public int trainingLoseCount;
    public int playCount;
    public int bathCount;
    public int feedCount;
    public int restCount;
    public int passOutCount;

    public SaveDragonData() { }

    public SaveDragonData(DragonHealth dragonHealth, GameManager gameManager)
    {
        if (dragonHealth != null)
        {
            stats = new DragonStats();
            stats.stamina = dragonHealth.stats.stamina;
            stats.fatigue = dragonHealth.stats.fatigue;
            stats.hunger = dragonHealth.stats.hunger;
            stats.intimacy = dragonHealth.stats.intimacy;
            stats.clean = dragonHealth.stats.clean;
            stats.experience = dragonHealth.stats.experience;
            stats.maxStamina = dragonHealth.stats.maxStamina;
            stats.maxFatigue = dragonHealth.stats.maxFatigue;
            stats.maxHunger = dragonHealth.stats.maxHunger;
            stats.maxIntimacy = dragonHealth.stats.maxIntimacy;
            stats.maxClean = dragonHealth.stats.maxClean;
            stats.experienceMax = dragonHealth.stats.experienceMax;
            stats.dragonName = dragonHealth.stats.dragonName;
            stats.dragonSpecies = dragonHealth.stats.dragonSpecies;
            

            currentSpeciesType = dragonHealth.currentSpeciesType;
            currentElementType = dragonHealth.currentElementType;
            currentGrowth = dragonHealth.currentGrowth;
            isPassOut = dragonHealth.isPassOut;
        }

        if (gameManager != null)
        {
            trainingWinCount = gameManager.trainingWinCount;
            trainingLoseCount = gameManager.trainingLoseCount;
            playCount = gameManager.playCount;
            bathCount = gameManager.bathCount;
            feedCount = gameManager.feedCount;
            restCount = gameManager.restCount;
            passOutCount = gameManager.passOutCount;
        }
    }

    public DragonHealth CreateDragon(GameManager gameManager)
    {
        GameObject dragonParent = GameObject.FindWithTag("Dragon");
        if (dragonParent == null) return null;
        

        GameObject dragonPrefab = GetDragonPrefab(gameManager);
        if (dragonPrefab == null) return null;


        GameObject newDragon = Object.Instantiate(dragonPrefab, dragonParent.transform);
        DragonHealth dragonHealth = newDragon.GetComponent<DragonHealth>();

        if (dragonHealth == null)
        {
            Object.Destroy(newDragon);
            return null;
        }

        ApplyDataToDragon(dragonHealth, gameManager);

        return dragonHealth;
    }

    public void ApplyDataToDragon(DragonHealth dragonHealth, GameManager gameManager)
    {
        if (dragonHealth == null) return;

        dragonHealth.isLoadedFromSave = true;

        dragonHealth.currentSpeciesType = currentSpeciesType;
        dragonHealth.currentElementType = currentElementType;
        dragonHealth.currentGrowth = currentGrowth;
        dragonHealth.isPassOut = isPassOut;

        if (stats != null && dragonHealth.stats != null)
        {
            dragonHealth.stats.stamina = stats.stamina;
            dragonHealth.stats.fatigue = stats.fatigue;
            dragonHealth.stats.hunger = stats.hunger;
            dragonHealth.stats.intimacy = stats.intimacy;
            dragonHealth.stats.clean = stats.clean;
            dragonHealth.stats.experience = stats.experience;
            dragonHealth.stats.maxStamina = stats.maxStamina;
            dragonHealth.stats.maxFatigue = stats.maxFatigue;
            dragonHealth.stats.maxHunger = stats.maxHunger;
            dragonHealth.stats.maxIntimacy = stats.maxIntimacy;
            dragonHealth.stats.maxClean = stats.maxClean;
            dragonHealth.stats.experienceMax = stats.experienceMax;
            dragonHealth.stats.dragonName = stats.dragonName;
            dragonHealth.stats.dragonSpecies = stats.dragonSpecies;
        }

        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                dragonHealth.currentGrowthText = "유아기";
                break;
            case DragonGrowthState.GrowingUp:
                dragonHealth.currentGrowthText = "성장기";
                break;
            case DragonGrowthState.Maturity:
                dragonHealth.currentGrowthText = "성숙기";
                break;
            case DragonGrowthState.Adult:
                dragonHealth.currentGrowthText = "성인";
                break;
        }

        if (gameManager != null)
        {
            gameManager.trainingWinCount = trainingWinCount;
            gameManager.trainingLoseCount = trainingLoseCount;
            gameManager.playCount = playCount;
            gameManager.bathCount = bathCount;
            gameManager.feedCount = feedCount;
            gameManager.restCount = restCount;
            gameManager.passOutCount = passOutCount;
        }

        if (currentGrowth == DragonGrowthState.Adult && gameManager != null)
        {
            gameManager.releaseButton.gameObject.SetActive(true);
        }

        EggSlot.isDragonActive = true;

        dragonHealth.ApplyTableData();

        Debug.Log($"드래곤 데이터 복원 완료: {stats?.dragonName}, 성장단계: {currentGrowth}, 경험치: {stats?.experience}/{stats?.experienceMax}");
    }

    private GameObject GetDragonPrefab(GameManager gameManager)
    {
        if (gameManager?.dragonPrefabs == null) return null;

        int index = ((currentSpeciesType - 1) * 4) + (currentElementType - 1);

        if (index >= 0 && index < gameManager.dragonPrefabs.Length)
        {
            return gameManager.dragonPrefabs[index];
        }

        return null;
    }
}
