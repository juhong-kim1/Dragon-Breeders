using System.Text;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DragonHealth dragonHealth;
    public StringBuilder stringBuilder;

    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI fullText;
    public TextMeshProUGUI IntimacyText;
    public TextMeshProUGUI CleanText;

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
        IntimacyText.text = $"Intimacy : {dragonHealth.intimacy}";
        CleanText.text = $"Clean : {dragonHealth.clean}";

    }
}
