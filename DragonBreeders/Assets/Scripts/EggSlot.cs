using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EggSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static readonly string dragonTag = "Dragon";


    public Image icon;
    public Egg egg;

    public static bool isDragonActive = false;

    [SerializeField] private GameManager gameManager;

    private bool isPressing = false;
    private float pressTime = 0f;
    public float holdDuration = 5f; // 5초

    void Awake()
    {
        egg = null;
        if (icon != null)
            icon.enabled = false;
    }

    void Update()
    {
        if (isPressing && egg != null)
        {
            pressTime += Time.deltaTime;
            if (pressTime >= holdDuration)
            {
                HatchEgg();
                isPressing = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (egg != null)
        {
            isPressing = true;
            pressTime = 0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false;
        pressTime = 0f;
    }

    private void HatchEgg()
    {
        if (isDragonActive)
        {
            Debug.Log("이미 드래곤을 사육중입니다.");

            return;
        }


        if (egg != null && egg.dragonPrefab != null)
        {
            GameObject parent = GameObject.FindWithTag(dragonTag);
            if (parent == null)
            {
                Debug.LogWarning("Dragon 오브젝트가 씬에 없습니다!");
                parent = null;
            }

            GameObject newDragon = Instantiate(
                egg.dragonPrefab,
               parent.transform
            );

            if (parent != null)
                newDragon.transform.SetParent(parent.transform);

            DragonHealth newHealth = newDragon.GetComponent<DragonHealth>();
            if (newHealth != null)
            {
                if (gameManager != null)
                {
                    gameManager.dragonHealth = newHealth;
                }
            }

            isDragonActive = true;

            ClearEgg();
        }
    }

    public void SetEgg(Egg newEgg)
    {
        egg = newEgg;
        if (icon != null && egg != null)
        {
            icon.sprite = egg.icon;
            icon.enabled = true;

            Debug.Log($"아이콘 활성화 완료, enabled: {icon.enabled}");
        }
    }

    public void ClearEgg()
    {
        egg = null;
        if (icon != null)
            icon.enabled = false;
    }

    public bool IsEmpty()
    {
        return egg == null;
    }
}

