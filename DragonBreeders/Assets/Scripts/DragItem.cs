using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("기본 드래그 설정")]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private IItem currentItem;

    [Header("놀아주기 설정")]
    public float playDuration = 3f;
    public float requiredDistance = 70f;
    public Slider playProgressSlider;

    private float playTimer = 0f;
    private bool isPlayingWithDragon = false;
    private Vector2 totalDragDistance = Vector2.zero;
    private Vector2 lastDragPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetCurrentItem(IItem item)
    {
        currentItem = item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((currentItem == null && !GameManager.Instance.isShowering) || !gameObject.activeInHierarchy) return;

        originalPosition = rectTransform.anchoredPosition;
        lastDragPosition = eventData.position;
        canvasGroup.blocksRaycasts = false;

        playTimer = 0f;
        totalDragDistance = Vector2.zero;
        isPlayingWithDragon = false;

        if (playProgressSlider != null)
            playProgressSlider.gameObject.SetActive(false);

        Debug.Log("드래그 시작");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        Vector2 dragDelta = currentPos - lastDragPosition;
        lastDragPosition = currentPos;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (IsInDragonArea(currentPos))
        {
            if (currentItem == null && GameManager.Instance.isShowering)
            {
                HandlePlayDrag(dragDelta);
            }
            else if (currentItem != null)
            {
                int itemType = currentItem.GetItemType();

                if (itemType == 6) // 놀이용품
                {
                    HandlePlayDrag(dragDelta);
                }
                else if (itemType == 4) // 비누
                {
                    HandlePlayDrag(dragDelta);
                }
                else if (itemType == 5) // 브러쉬
                {
                    HandlePlayDrag(dragDelta);
                }
                else if (itemType == 2) // 음식
                {
                    transform.localScale = Vector3.one * 1.2f;
                }
            }
        }
        else
        {
            if (isPlayingWithDragon)
            {
                CancelPlay();
            }
            transform.localScale = Vector3.one;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Vector2 screenPos = eventData.position;

        if (currentItem == null && GameManager.Instance.isShowering)
        {
            
        }

        else if (currentItem != null)
        {
            if (IsInDragonArea(screenPos))
            {
                if (currentItem.GetItemType() == 2)
                {
                    Debug.Log("드래곤 영역에 드롭 성공!");
                    GameManager.Instance.UseFoodItem(currentItem.GetID(), 1);
                }
            }
            else
            {
                AlarmManager.Instance.ShowAlarm("아이템 떨어졌어요!");
                Debug.Log("드래곤 영역 밖에 드롭됨");
            }
        }

        if (isPlayingWithDragon)
        {
            CancelPlay();
        }

        rectTransform.anchoredPosition = originalPosition;
        transform.localScale = Vector3.one;
    }

    private bool IsInDragonArea(Vector2 screenPos)
    {
        float dragonCenterX = Screen.width * 0.5f;
        float dragonCenterY = Screen.height * 0.5f;
        float dragonRange = Screen.width * 0.5f;

        Vector2 dragonCenter = new Vector2(dragonCenterX, dragonCenterY);
        float distance = Vector2.Distance(screenPos, dragonCenter);

        return distance < dragonRange;
    }

    private void HandlePlayDrag(Vector2 dragDelta)
    {
        if (!isPlayingWithDragon)
        {
            isPlayingWithDragon = true;
            playTimer = 0f;
            totalDragDistance = Vector2.zero;

            if (playProgressSlider != null)
            {
                playProgressSlider.gameObject.SetActive(true);
                playProgressSlider.value = 0f;
            }

            if (currentItem == null && GameManager.Instance.isShowering)
            {
                Debug.Log("샤워 시작!");
                GameManager.Instance.bathParticle.Play();
                SoundManager.Instance.PlaySFX(SoundManager.Instance.bathAudioClip);
            }
            else if (currentItem != null)
            {
                int itemType = currentItem.GetItemType();
                if (itemType == 6)
                {
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.playAudioClip);
                    Debug.Log("놀아주기 시작!");
                }
                else if (itemType == 4)
                {
                    Debug.Log("비누질 시작!");
                    GameManager.Instance.bathParticle.Play();
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.bathAudioClip);
                }
                else if (itemType == 5)
                {
                    Debug.Log("브러싱 시작!");
                    GameManager.Instance.bathParticle.Play();
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.bathAudioClip);
                }
            }
        }

        playTimer += Time.deltaTime;
        totalDragDistance += new Vector2(Mathf.Abs(dragDelta.x), Mathf.Abs(dragDelta.y));

        if (playProgressSlider != null)
        {
            bool shouldShowProgress = false;

            if (currentItem == null && GameManager.Instance.isShowering)
            {
                shouldShowProgress = true;
            }
            else if (currentItem != null)
            {
                int itemType = currentItem.GetItemType();
                if (itemType == 6 && GameManager.Instance.playItemImage.enabled)
                    shouldShowProgress = true;
                else if (itemType == 4 && GameManager.Instance.soapItemImage != null && GameManager.Instance.soapItemImage.enabled)
                    shouldShowProgress = true;
                else if (itemType == 5 && GameManager.Instance.brushItemImage != null && GameManager.Instance.brushItemImage.enabled)
                    shouldShowProgress = true;
            }

            if (shouldShowProgress)
                playProgressSlider.value = playTimer / playDuration;
        }

        transform.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * 10f) * 0.1f);

        if (playTimer >= playDuration && totalDragDistance.magnitude >= requiredDistance)
        {
            CompletePlay();
        }
    }

    private void CompletePlay()
    {
        GameManager.Instance.bathParticle.Stop();

        if (currentItem == null && GameManager.Instance.isShowering)
        {
            if (!GameManager.Instance.canBath)
            {
                AlarmManager.Instance.ShowAlarm("방금 목욕했어요!");
                CancelPlay();
                return;
            }
            GameManager.Instance.CompleteShower();
            Debug.Log("샤워 완료!");
            CancelPlay();
            return;
        }

        if (currentItem != null)
        {
            int itemType = currentItem.GetItemType();

            if (itemType == 6)
            {
                if (!GameManager.Instance.canPlay)
                {
                    AlarmManager.Instance.ShowAlarm("방금 놀아서 힘들어요!");
                    CancelPlay();
                    return;
                }
                GameManager.Instance.UsePlayItem(currentItem.GetID(), 1);
                Debug.Log("놀아주기 완료!");
            }
            else if (itemType == 4) // 비누
            {
                if (!GameManager.Instance.canBath)
                {
                    AlarmManager.Instance.ShowAlarm("방금 목욕했어요!");
                    CancelPlay();
                    return;
                }
                GameManager.Instance.UseSoapItem(currentItem.GetID(), 1);
                Debug.Log("비누질 완료!");
            }
            else if (itemType == 5) // 브러쉬
            {
                if (!GameManager.Instance.canBath)
                {
                    AlarmManager.Instance.ShowAlarm("방금 목욕했어요!");
                    CancelPlay();
                    return;
                }
                GameManager.Instance.UseBrushItem(currentItem.GetID(), 1);
                Debug.Log("브러싱 완료!");
            }
        }

        CancelPlay();
    }

    private void CancelPlay()
    {
        isPlayingWithDragon = false;
        playTimer = 0f;
        totalDragDistance = Vector2.zero;

        if (playProgressSlider != null)
            playProgressSlider.gameObject.SetActive(false);

        transform.localScale = Vector3.one;

        GameManager.Instance.bathParticle.Stop();

    }
}