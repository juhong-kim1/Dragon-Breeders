using UnityEngine;

public class EggVault : MonoBehaviour
{
    public EggSlot[] slots;

    public void AddEgg(Egg eggData)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].IsEmpty())
            {
                slots[i].SetEgg(eggData);
                Debug.Log($"알 추가 성공: {eggData.eggName} → Slot {i}");
                return;
            }
        }
        Debug.Log("보관소가 가득 찼습니다!");
    }

    public bool IsEmpty()
    {
        foreach (var slot in slots)
        {
            if (slot != null && !slot.IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public void AddRandomEggIfEmpty()
    {
        if (IsEmpty())
        {
            GameManager.Instance.OnClickEggCheat();
            Debug.Log("보관소가 비어서 랜덤 알 추가됨");
        }
    }
}


