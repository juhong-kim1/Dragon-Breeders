using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndexUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject indexPanel;
    public Transform contentParent;
    public GameObject dragonSlotPrefab;

    private DragonIndex currentIndex;

    private void Start()
    {
        indexPanel.SetActive(false);
    }

    public void ShowIndex(DragonIndex index)
    {
        Debug.Log("ShowIndex 호출됨");
        currentIndex = index;
        indexPanel.SetActive(true);
        RefreshIndex();
    }

    public void CloseIndex()
    {
        indexPanel.SetActive(false);
    }

    private void RefreshIndex()
    {
        if (currentIndex == null) return;

        Debug.Log($"RefreshIndex 호출됨, 기존 자식 수: {contentParent.childCount}");

        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        List<DragonEntry> entries = currentIndex.GetAllEntries();
        foreach (DragonEntry entry in entries)
        {
            GameObject slot = Instantiate(dragonSlotPrefab, contentParent);
            IndexSlot slotComponent = slot.GetComponent<IndexSlot>();
            if (slotComponent != null)
            {
                slotComponent.SetEntry(entry);
            }
        }
    }
}
