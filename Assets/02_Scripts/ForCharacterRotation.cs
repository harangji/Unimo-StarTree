using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForCharacterRotation : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float rotationSpeed = 0.1f; // 드래그에 따른 회전 속도

    private bool isDragging = false;
    private Vector2 lastDragPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        // 드래그 시작 위치 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out lastDragPosition
        );
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentDragPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out currentDragPosition
        );

        // 드래그 방향 계산
        float dragDelta = currentDragPosition.x - lastDragPosition.x;

        // 드래그 방향에 따라 회전
        Costume_Finder.instance.transform.Rotate(0, -dragDelta * rotationSpeed, 0);

        // 현재 드래그 위치를 마지막 위치로 업데이트
        lastDragPosition = currentDragPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false; // 드래그 종료
    }
}