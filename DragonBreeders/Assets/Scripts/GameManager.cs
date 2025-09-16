using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;

    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI figureText;
    public TextMeshProUGUI hungryText;
    public TextMeshProUGUI intimacyText;
    public TextMeshProUGUI cleanText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI growthStateText;
    public TextMeshProUGUI currentStatusText;

    public TextMeshProUGUI[] mapStatTexts;

    public float exploreTimer = 0f;

    public bool canExplore = true;

    public void Update()
    {
        UpdateStatText();

        if (!canExplore)
        {
            exploreTimer += Time.deltaTime;
            var trainingData = DataTableManger.NurtureTable?.Get(50000);

            //Debug.Log(trainingData.TIME);

            if (trainingData != null && exploreTimer >= trainingData.TIME)
            {
                canExplore = true;
                exploreTimer = 0f;
            }
        }



    }

    public void UpdateStatText()
    {
        if (dragonHealth == null) return;

        var stats = dragonHealth.stats;

        if (staminaText) staminaText.text = $"Stamina: {stats.stamina:F0}/{stats.maxStamina:F0}";
        if (hungryText) hungryText.text = $"Hunger: {stats.hunger:F0}";
        if (intimacyText) intimacyText.text = $"Intimacy: {stats.intimacy:F0}";
        if (cleanText) cleanText.text = $"Clean: {stats.clean:F0}";
        if (figureText) figureText.text = $"Fatigue: {stats.fatigue:F0}";
        if (experienceText) experienceText.text = $"Exp: {stats.experience:F0}/{stats.experienceMax:F0}";
        if (growthStateText) growthStateText.text = $"Stage: {dragonHealth.currentGrowth}";
        if (currentStatusText) currentStatusText.text = $"Status: {dragonHealth.currentStatuses}";

        UpdateMapUI(stats);
    }

    private void UpdateMapUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 8)
        {
            mapStatTexts[0].text = $"Stamina: {stats.stamina:F0}";
            mapStatTexts[1].text = $"Hunger: {stats.hunger:F0}";
            mapStatTexts[2].text = $"Intimacy: {stats.intimacy:F0}";
            mapStatTexts[3].text = $"Clean: {stats.clean:F0}";
            mapStatTexts[4].text = $"Fatigue: {stats.fatigue:F0}";
            mapStatTexts[5].text = $"Exp: {stats.experience:F0}/{stats.experienceMax:F0}";
            mapStatTexts[6].text = $"Stage: {dragonHealth.currentGrowth}";
            mapStatTexts[7].text = $"Status: {dragonHealth.currentStatuses}";
        }
    }

    public void OnClickFeed()
    {
        if (dragonHealth.isPassOut) return;

        var data = DataTableManger.NurtureTable.Get(50300);
        if (data == null) return;

        int hungerRecovery = dragonHealth.stats.maxHunger * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Hunger, hungerRecovery);
    }

    public void OnClickBath()
    {
        if (dragonHealth.isPassOut) return;

        var data = DataTableManger.NurtureTable.Get(50400);
        if (data == null) return;

        int cleanRecovery = dragonHealth.stats.maxClean * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Clean, cleanRecovery);
    }

    public void OnClickPlay()
    {
        if (dragonHealth.isPassOut) return;

        var data = DataTableManger.NurtureTable.Get(50500);
        if (data == null) return;

 
        int intimacyRecovery = dragonHealth.stats.maxIntimacy * data.REC_PERCENT / 100;
        dragonHealth.stats.ChangeStat(StatType.Intimacy, intimacyRecovery);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);
    }

    public void OnClickRest()
    {
        if (dragonHealth.isPassOut)
        {
            dragonHealth.Recover();
        }
        else
        {
            var data = DataTableManger.NurtureTable.Get(50200);
            if (data == null) return;

            int fatigueRecovery = dragonHealth.stats.maxFatigue * data.REC_PERCENT / 100;
            dragonHealth.stats.ChangeStat(StatType.Fatigue, -fatigueRecovery);
        }
    }

    public void OnClickTrain()
    {
        if (dragonHealth.isPassOut) { Debug.Log("기절 중, 훈련 불가"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) { Debug.Log("유아기에선 훈련 불가"); return; }

        var data = DataTableManger.NurtureTable.Get(50014);
        if (data == null) return;

        dragonHealth.stats.ChangeStat(StatType.Experience, data.EXPGROWTH);
        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        if (Random.Range(0f, 100f) < data.RATE_INJURY)
        {
            StatusType[] injuries = { StatusType.Scratches, StatusType.Bleeding, StatusType.Fracture };
            StatusType randomInjury = injuries[Random.Range(0, injuries.Length)];
            dragonHealth.status.AddStatus(randomInjury);
        }
    }

    public void OnClickExplore()
    {
        if (dragonHealth.isPassOut) { Debug.Log("기절 중, 탐험 불가"); return; }
        if (!canExplore) { Debug.Log("탐험 쿨 진행 중"); return; }
        if (dragonHealth.currentGrowth == DragonGrowthState.Infancy) {Debug.Log("유아기에선 탐험 불가"); return; }


        var data = DataTableManger.NurtureTable.Get(50000);
        if (data == null) return;

        Debug.Log("탐험 시작");

        dragonHealth.stats.ChangeStat(StatType.Clean, -data.DEPLETE_HYG);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, data.RECEIVE_FTG);

        if (Random.Range(0f, 100f) < data.RATE_DISEASE)
        {
            StatusType[] diseases = { StatusType.Cold, StatusType.FoodPoisoning, StatusType.HighFever, StatusType.Infection };
            StatusType randomDisease = diseases[Random.Range(0, diseases.Length)];
            dragonHealth.status.AddStatus(randomDisease);
        }

        canExplore = false;
        exploreTimer = 0f;
    }
}