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

        growthText.text = $"{dragon.currentGrowth}";

        currentStamina.text = $"{stats.stamina}";
        currentFatigue.text = $"{stats.fatigue}";
        currentHungry.text = $"{stats.hunger}";
        currentIntimacy.text = $"{stats.intimacy}";
        currentClean.text = $"{stats.clean}";
        currentExperience.text = $"{stats.experience}";

        maxStamina.text = $"{stats.maxStamina}";
        maxFatigue.text = $"{stats.maxFatigue}";
        maxHungry.text = $"{stats.maxHunger}";
        maxIntimacy.text = $"{stats.maxIntimacy}";
        maxClean.text = $"{stats.maxClean}";
        maxExperience.text = $"{stats.experienceMax}";

        staminaSlider.value = Mathf.Clamp01((float)stats.stamina / stats.maxStamina);
        fatigueSlider.value = Mathf.Clamp01((float)stats.fatigue / stats.maxFatigue);
        hungrySlider.value = Mathf.Clamp01((float)stats.hunger / stats.maxHunger);
        intimacySlider.value = Mathf.Clamp01((float)stats.intimacy / stats.maxIntimacy);
        cleanSlider.value = Mathf.Clamp01((float)stats.clean / stats.maxClean);
        experienceSlider.value = Mathf.Clamp01(stats.experience / stats.experienceMax);
    }
}
