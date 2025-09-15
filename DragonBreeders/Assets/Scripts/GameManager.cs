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

    public float timer = 0f;

    public void Update()
    {
        timer += Time.deltaTime;

        UpdateStatText();

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
        if (mapStatTexts.Length >= 7)
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

    public void OnClickRest()
    {
        if (dragonHealth.isPassOut)
        {
            dragonHealth.Recover();
        }
        else
        {
            dragonHealth.StartResting();
        }
    }

    public void OnClickFeed()
    {
        if (!dragonHealth.isPassOut)
        {
            dragonHealth.stats.ChangeStat(StatType.Hunger, 30);
            dragonHealth.stats.ChangeStat(StatType.Intimacy, 10);
        }
    }

    public void OnClickPlay()
    {
        if (!dragonHealth.isPassOut)
        {
            dragonHealth.stats.ChangeStat(StatType.Hunger, -10);
            dragonHealth.stats.ChangeStat(StatType.Intimacy, 20);
        }

    }

    public void OnClickTrain()
    {
        if (dragonHealth.isPassOut)
            return;

        dragonHealth.stats.ChangeStat(StatType.Hunger, -10);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, 20);
        dragonHealth.stats.ChangeStat(StatType.Stamina, 20);

        CheckInjury(StatusType.Scratches);
        CheckInjury(StatusType.Bleeding);
        CheckInjury(StatusType.Fracture);
    }

    private void CheckInjury(StatusType status)
    {
        var debuffData = dragonHealth.status.GetDebuffData(status);
        if (debuffData != null)
        {
            float random = UnityEngine.Random.Range(0f, 100f);
            if (random < debuffData.TRIGGER_RATE)
            {
                dragonHealth.status.AddStatus(status);
            }
        }
    }

    public void OnClickEXP()
    {
        if (dragonHealth.isPassOut)
            return;

        dragonHealth.stats.ChangeStat(StatType.Hunger, -20);
        dragonHealth.stats.ChangeStat(StatType.Fatigue, 30);
        dragonHealth.stats.ChangeStat(StatType.Stamina, 10);

        CheckInjury(StatusType.Cold);
        CheckInjury(StatusType.FoodPoisoning);
        CheckInjury(StatusType.HighFever);
        CheckInjury(StatusType.Infection);
    }


}
