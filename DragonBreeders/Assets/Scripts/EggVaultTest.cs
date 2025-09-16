using UnityEngine;

public class EggVaultTest : MonoBehaviour
{
    public EggVault vault;
    public Sprite grassEggIcon;
    public GameObject dragonPrefab;

    void Start()
    {
        // 슬롯 강제 초기화
        foreach (var slot in vault.slots)
        {
            slot.ClearEgg();
        }

        Egg firstEgg = new Egg
        {
            eggName = "Grass Egg",
            icon = grassEggIcon,
            dragonPrefab = dragonPrefab
        };

        vault.AddEgg(firstEgg);
    }
}



