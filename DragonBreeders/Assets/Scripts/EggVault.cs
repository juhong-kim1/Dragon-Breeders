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
                Debug.Log($"�� �߰� ����: {eggData.eggName} �� Slot {i}");
                return;
            }
        }
        Debug.Log("�����Ұ� ���� á���ϴ�!");
    }
}


