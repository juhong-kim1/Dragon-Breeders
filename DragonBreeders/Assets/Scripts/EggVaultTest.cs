using UnityEngine;

public class EggVaultTest : MonoBehaviour
{
    public EggVault vault;
    public Sprite windEggIcon;
    public GameObject dragonPrefab;

    void Start()
    {
        foreach (var slot in vault.slots)
        {
            slot.ClearEgg();
        }

        Egg firstEgg = new Egg
        {
            eggName = "Wind Egg",
            icon = windEggIcon,
            dragonPrefab = dragonPrefab,
            speciesType = 2,
            elementType = 4
        };

        vault.AddEgg(firstEgg);
    }
}



