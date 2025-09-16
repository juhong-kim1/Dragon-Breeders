using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 드래그/포인터 이벤트용

public class EggSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image icon;
    public Egg egg;

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
        if (egg != null && egg.dragonPrefab != null)
        {
            GameObject parent = GameObject.Find("Dragon");
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

