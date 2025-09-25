// FeedItemDrag.cs - feedItemImage에 붙일 스크립트
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;

    private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private IItem currentItem;

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
        canvasGroup.blocksRaycasts = false; // 레이캐스트 차단 해제

        Debug.Log("feedItemImage 드래그 시작");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // 레이캐스트 차단 복원

        Debug.Log("OnEndDrag 호출됨");

        if (currentItem == null)
        {
            Debug.Log("currentItem이 null입니다");
            return;
        }

        Vector2 screenPos = eventData.position;
        Debug.Log($"드롭 위치: {screenPos}");


        float dragonCenterX = Screen.width * 0.5f;
        float dragonCenterY = Screen.height * 0.5f;
        float dragonRange = Screen.width * 0.3f;

        Vector2 dragonCenter = new Vector2(dragonCenterX, dragonCenterY);
        float distance = Vector2.Distance(screenPos, dragonCenter);

        Debug.Log($"드래곤 중심점: {dragonCenter}, 거리: {distance}, 범위: {dragonRange}");

        if (distance < dragonRange)
        {
            Debug.Log("드래곤 영역에 드롭 성공!");
            GameManager.Instance.UseFoodItem(currentItem.GetID(), 1);
        }
        else
        {
            Debug.Log("드래곤 영역 밖에 드롭됨");
        }

        // 원래 위치로 되돌리기
        rectTransform.anchoredPosition = originalPosition;
    }
}