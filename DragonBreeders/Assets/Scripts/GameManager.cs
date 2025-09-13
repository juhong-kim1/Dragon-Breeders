using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;
    public StringBuilder stringBuilder;

    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI fullText;
    public TextMeshProUGUI intimacyText;
    public TextMeshProUGUI cleanText;

    public TextMeshProUGUI mapStaminaText;
    public TextMeshProUGUI mapVitalityText;
    public TextMeshProUGUI mapFullText;
    public TextMeshProUGUI mapIntimacyText;
    public TextMeshProUGUI mapCleanText;

    private DragonGrowthState state;

    public void Start()
    {
        state = dragonHealth.currentGrowth;
    }



    public void Update()
    {
        UpdateStatText();

    }

    public void UpdateStatText()
    {
        staminaText.text = $"Stamina : {dragonHealth.stamina}";
        vitalityText.text = $"Vitality : {dragonHealth.vitality}";
        fullText.text = $"Full : {dragonHealth.full}";
        intimacyText.text = $"Intimacy : {dragonHealth.intimacy}";
        cleanText.text = $"Clean : {dragonHealth.clean}";

        mapStaminaText.text = $"Stamina : {dragonHealth.stamina}";
        mapVitalityText.text = $"Vitality : {dragonHealth.vitality}";
        mapFullText.text = $"Full : {dragonHealth.full}";
        mapIntimacyText.text = $"Intimacy : {dragonHealth.intimacy}";
        mapCleanText.text = $"Clean : {dragonHealth.clean}";

    }

    public void OnClickReset()
    {
        if (dragonHealth.isPassOut)
        {
            dragonHealth.isPassOut = false;
            dragonHealth.hasTriggerPassOut = false;
            dragonHealth.animator.Rebind();
        }

        dragonHealth.vitality += 50;
    }

    public void OnClickFeed()
    {
        if (dragonHealth.isPassOut)
            return;

        dragonHealth.full += 50;
    }
}
