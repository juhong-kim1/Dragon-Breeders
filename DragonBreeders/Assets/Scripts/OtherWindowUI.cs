using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherWindowUI : MonoBehaviour
{
    public TextMeshProUGUI growthText;
    public TextMeshProUGUI currentStamina;
    public TextMeshProUGUI maxStamina;
    public TextMeshProUGUI currentFatigue;
    public TextMeshProUGUI maxFatigue;
    public TextMeshProUGUI currentHungry;
    public TextMeshProUGUI maxHungry;
    public TextMeshProUGUI currentIntimacy;
    public TextMeshProUGUI maxIntimacy;
    public TextMeshProUGUI currentClean;
    public TextMeshProUGUI maxClean;
    public TextMeshProUGUI currentExperience;
    public TextMeshProUGUI maxExperience;

    public Slider staminaSlider;
    public Slider fatigueSlider;
    public Slider hungrySlider;
    public Slider intimacySlider;
    public Slider cleanSlider;
    public Slider experienceSlider;

    public void UpdateStats(DragonHealth dragon)
    {
        if (dragon == null) return;
        var stats = dragon.stats;

        growthText.text = $"{dragon.currentGrowthText}";

        currentStamina.text = $"{(int)stats.stamina}";
        currentFatigue.text = $"{(int)stats.fatigue}";
        currentHungry.text = $"{(int)stats.hunger}";
        currentIntimacy.text = $"{(int)stats.intimacy}";
        currentClean.text = $"{(int)stats.clean}";
        currentExperience.text = $"{(int)stats.experience}";

        maxStamina.text = $"{(int)stats.maxStamina}";
        maxFatigue.text = $"{(int)stats.maxFatigue}";
        maxHungry.text = $"{(int)stats.maxHunger}";
        maxIntimacy.text = $"{(int)stats.maxIntimacy}";
        maxClean.text = $"{(int)stats.maxClean}";
        maxExperience.text = $"{(int)stats.experienceMax}";

        staminaSlider.value = Mathf.Clamp01(stats.stamina / stats.maxStamina);
        fatigueSlider.value = Mathf.Clamp01(stats.fatigue / stats.maxFatigue);
        hungrySlider.value = Mathf.Clamp01(stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01(stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01(stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);
    }
}
