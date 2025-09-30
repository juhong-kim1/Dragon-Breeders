using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndexSlot : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI customNameText;
    public TextMeshProUGUI speciesNameText;
    public TextMeshProUGUI releaseDateText;

    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI intimacyText;
    public TextMeshProUGUI cleanText;

    public TextMeshProUGUI trainingWinCountText;
    public TextMeshProUGUI trainingLoseCountText;
    public TextMeshProUGUI playCountText;
    public TextMeshProUGUI feedCountText;
    public TextMeshProUGUI bathCountText;
    public TextMeshProUGUI restCountText;
    public TextMeshProUGUI passOutCountText;

    public Image dragonImage;

    public void SetEntry(DragonEntry entry)
    {
        customNameText.text = entry.customName;
        speciesNameText.text = entry.speciesName;
        releaseDateText.text = entry.releaseDate;

        staminaText.text = $"ÃÖÁ¾Ã¼·Â: {entry.finalStamina}";
        intimacyText.text = $"ÃÖÁ¾Ä£¹Ğµµ: {entry.finalIntimacy}";
        cleanText.text = $"ÃÖÁ¾Ã»°áµµ: {entry.finalClean}";

        trainingWinCountText.text = $"ÈÆ·Ã½Â¸®È½¼ö: {entry.trainingWinCount}";
        trainingLoseCountText.text = $"ÈÆ·ÃÆĞ¹èÈ½¼ö: {entry.trainingLoseCount}";
        playCountText.text = $"°°ÀÌ ³í È½¼ö: {entry.playCount}";
        feedCountText.text = $"¸ÔÀÌ ÁØ È½¼ö: {entry.feedCount}";
        bathCountText.text = $"¸ñ¿åÇÑ È½¼ö: {entry.bathCount}";
        restCountText.text = $"ÈŞ½ÄÇÑ È½¼ö: {entry.restCount}";
        passOutCountText.text = $"±âÀıÇÑ È½¼ö: {entry.passOutCount}";

        FindSprite(entry);
    }

    private void FindSprite(DragonEntry entry)
    {
        int index = ((entry.species - 1) * 4) + (entry.elements - 1);

        Debug.Log($"µå·¡°ï: {entry.customName}, species={entry.species}, elements={entry.elements}, °è»êµÈ ÀÎµ¦½º={index}");

        if (index >= 0 && index < GameManager.Instance.dragonImages.Length)
        {
            if (dragonImage != null)
            {
                dragonImage.sprite = GameManager.Instance.dragonImages[index];
                dragonImage.enabled = true;
                Debug.Log($"ÇÒ´çµÈ ½ºÇÁ¶óÀÌÆ®: {GameManager.Instance.dragonImages[index].name}");
            }
        }
        else
        {
            Debug.LogError($"ÀÎµ¦½º ¹üÀ§ ÃÊ°ú!");
        }
    }
}
