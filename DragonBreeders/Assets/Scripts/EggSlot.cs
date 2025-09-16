using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // �巡��/������ �̺�Ʈ��

public class EggSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image icon;
    public Egg egg;

    private bool isPressing = false;
    private float pressTime = 0f;
    public float holdDuration = 5f; // 5��

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
                Debug.LogWarning("Dragon ������Ʈ�� ���� �����ϴ�!");
                parent = null;
            }

            GameObject newDragon = Instantiate(
                egg.dragonPrefab,
                Camera.main.transform.position + Camera.main.transform.forward * 2f,
                Quaternion.identity
            );

            if (parent != null)
                newDragon.transform.SetParent(parent.transform);

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

