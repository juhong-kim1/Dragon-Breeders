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
}


