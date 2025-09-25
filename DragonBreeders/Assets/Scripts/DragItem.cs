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
    public float requiredDistance = 100f;
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
        if (currentItem == null || !gameObject.activeInHierarchy) return;

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
        if (currentItem == null) return;

        Vector2 currentPos = eventData.position;
        Vector2 dragDelta = currentPos - lastDragPosition;
        lastDragPosition = currentPos;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;


        if (IsInDragonArea(currentPos))
        {
            if (currentItem.GetItemType() == 6)
            {
                HandlePlayDrag(dragDelta);
            }
            else if (currentItem.GetItemType() == 2)
            {
                transform.localScale = Vector3.one * 1.2f;
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

        if (currentItem == null)
        {
            Debug.Log("currentItem이 null입니다");
            return;
        }

        Vector2 screenPos = eventData.position;

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
            AlarmManager.Instance.ShowAlarm("음식 떨어졌어요!");
            Debug.Log("드래곤 영역 밖에 드롭됨");
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
        float dragonRange = Screen.width * 0.3f;

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

            Debug.Log("놀아주기 시작!");
        }

        playTimer += Time.deltaTime;
        totalDragDistance += new Vector2(Mathf.Abs(dragDelta.x), Mathf.Abs(dragDelta.y));


        if (playProgressSlider != null && GameManager.Instance.playItemImage.enabled)
        {
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
        if (!GameManager.Instance.canPlay)
        {
            AlarmManager.Instance.ShowAlarm("방금 놀아서 힘들어요!");
            CancelPlay();
            return;
        }

        GameManager.Instance.UsePlayItem(currentItem.GetID(), 1);

        Debug.Log("놀아주기 완료!");


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
    }
}