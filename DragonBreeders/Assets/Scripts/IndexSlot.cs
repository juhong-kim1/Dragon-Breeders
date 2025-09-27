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

    public void SetEntry(DragonEntry entry)
    {
        customNameText.text = entry.customName;
        speciesNameText.text = entry.speciesName;
        releaseDateText.text = entry.releaseDate;

        staminaText.text = $"ÃÖÁ¾Ã¼·Â: {entry.finalStamina}";
        intimacyText.text = $"ÃÖÁ¾Ä£¹Ðµµ: {entry.finalIntimacy}";
        cleanText.text = $"ÃÖÁ¾Ã»°áµµ: {entry.finalClean}";

        trainingWinCountText.text = $"ÈÆ·Ã½Â¸®È½¼ö: {entry.trainingWinCount}";
        trainingLoseCountText.text = $"ÈÆ·ÃÆÐ¹èÈ½¼ö: {entry.trainingLoseCount}";
        playCountText.text = $"°°ÀÌ ³í È½¼ö: {entry.playCount}";
        feedCountText.text = $"¸ÔÀÌ ÁØ È½¼ö: {entry.feedCount}";
        bathCountText.text = $"¸ñ¿åÇÑ È½¼ö: {entry.bathCount}";
        restCountText.text = $"ÈÞ½ÄÇÑ È½¼ö: {entry.restCount}";
        passOutCountText.text = $"±âÀýÇÑ È½¼ö: {entry.passOutCount}";
    }
}
