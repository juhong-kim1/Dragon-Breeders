using UnityEngine;

[System.Serializable]
public class SaveEggData
{
    public string eggName;
    public string iconName;
    public int speciesType;
    public int elementType;

    public SaveEggData() { }

    public SaveEggData(Egg egg)
    {
        if (egg != null)
        {
            eggName = egg.eggName;
            iconName = egg.icon != null ? egg.icon.name : "";
            speciesType = egg.speciesType;
            elementType = egg.elementType;
        }
    }

    public Egg CreateEgg(GameManager gameManager)
    {
        if (gameManager == null) return null;

        GameObject dragonPrefab = GetDragonPrefab(gameManager);
        Sprite eggIcon = GetEggIcon(gameManager);

        if (dragonPrefab == null || eggIcon == null)
        {
            Debug.LogError($"알 복원 실패: Prefab={dragonPrefab != null}, Icon={eggIcon != null}");
            return null;
        }

        return new Egg
        {
            eggName = eggName,
            icon = eggIcon,
            dragonPrefab = dragonPrefab,
            speciesType = speciesType,
            elementType = elementType
        };
    }

    private GameObject GetDragonPrefab(GameManager gameManager)
    {
        if (gameManager?.dragonPrefabs == null) return null;

        int index = ((elementType - 1) * 4) + (speciesType - 1);

        if (index >= 0 && index < gameManager.dragonPrefabs.Length)
        {
            return gameManager.dragonPrefabs[index];
        }

        return null;
    }

    private Sprite GetEggIcon(GameManager gameManager)
    {
        if (gameManager?.icon == null) return null;

        foreach (var sprite in gameManager.icon)
        {
            if (sprite != null && sprite.name == iconName)
            {
                return sprite;
            }
        }

        int iconIndex = elementType - 1;
        if (iconIndex >= 0 && iconIndex < gameManager.icon.Length)
        {
            return gameManager.icon[iconIndex];
        }

        return null;
    }
}
