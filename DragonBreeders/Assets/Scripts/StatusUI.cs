using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [Header("Status Icons")]
    public Image[] statusIconSlots;
    public Sprite[] statusSprites;

    private Dictionary<StatusType, int> statusToSpriteIndex = new Dictionary<StatusType, int>
    {
        { StatusType.Disease, 0 },
        { StatusType.Injury, 1 },
        { StatusType.Dirty, 2 },
        { StatusType.Hungry, 3 },
        { StatusType.Fatigue, 4 },
        { StatusType.PassOut, 5 }
    };

    private void Start()
    {
        ClearAllIcons();
    }

    public void UpdateStatusIcons(DragonStatus status)
    {
        if (status == null)
        {
            ClearAllIcons();
            return;
        }

        List<StatusType> activeStatuses = status.GetActiveStatuses();

        ClearAllIcons();

        for (int i = 0; i < activeStatuses.Count && i < statusIconSlots.Length; i++)
        {
            StatusType statusType = activeStatuses[i];

            if (statusToSpriteIndex.TryGetValue(statusType, out int spriteIndex))
            {
                if (spriteIndex < statusSprites.Length && statusSprites[spriteIndex] != null)
                {
                    statusIconSlots[i].sprite = statusSprites[spriteIndex];
                    statusIconSlots[i].enabled = true;
                }
            }
        }
    }

    private void ClearAllIcons()
    {
        foreach (var icon in statusIconSlots)
        {
            if (icon != null)
            {
                icon.enabled = false;
            }
        }
    }
}