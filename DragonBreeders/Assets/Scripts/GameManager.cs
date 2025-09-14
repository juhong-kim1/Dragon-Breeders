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

        if (staminaText) staminaText.text = $"체력: {stats.stamina:F0}/{stats.maxStamina:F0}";
        if (hungryText) hungryText.text = $"배고픔: {stats.hunger:F0}";
        if (intimacyText) intimacyText.text = $"친밀도: {stats.intimacy:F0}";
        if (cleanText) cleanText.text = $"청결: {stats.clean:F0}";
        if (figureText) figureText.text = $"피로도: {stats.fatigue:F0}";


        UpdateMapUI(stats);

    }

    private void UpdateMapUI(DragonStats stats)
    {
        if (mapStatTexts.Length >= 6)
        {
            mapStatTexts[0].text = $"체력: {stats.stamina:F0}";
            mapStatTexts[2].text = $"배고픔: {stats.hunger:F0}";
            mapStatTexts[3].text = $"친밀도: {stats.intimacy:F0}";
            mapStatTexts[4].text = $"청결: {stats.clean:F0}";
            mapStatTexts[5].text = $"피로도: {stats.fatigue:F0}";
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
