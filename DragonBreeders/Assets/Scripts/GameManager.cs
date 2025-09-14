using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;

    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI figureText;
    public TextMeshProUGUI hungryText;
    public TextMeshProUGUI intimacyText;
    public TextMeshProUGUI cleanText;

    public TextMeshProUGUI[] mapStatTexts;



    public void Update()
    {
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


        UpdateMapUI(stats);

    }

    private void UpdateMapUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 6)
        {
            mapStatTexts[0].text = $"Stamina: {stats.stamina:F0}";
            mapStatTexts[2].text = $"Hunger: {stats.hunger:F0}";
            mapStatTexts[3].text = $"Intimacy: {stats.intimacy:F0}";
            mapStatTexts[4].text = $"Clean: {stats.clean:F0}";
            mapStatTexts[5].text = $"Fatigue: {stats.fatigue:F0}";
        }
    }

    public void OnClickReset()
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
        if (dragonHealth.hasTriggerPassOut)
        {
            dragonHealth.stats.ChangeStat(StatType.Hunger, 30);
        }
    }

    public void OnClickPlay()
    { }
}
