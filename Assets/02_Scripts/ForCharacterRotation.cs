using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForCharacterRotation : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float rotationSpeed = 0.1f; // �巡�׿� ���� ȸ�� �ӵ�

    private bool isDragging = false;
    private Vector2 lastDragPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        // �巡�� ���� ��ġ ����
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

        // �巡�� ���� ���
        float dragDelta = currentDragPosition.x - lastDragPosition.x;

        // �巡�� ���⿡ ���� ȸ��
        Costume_Finder.instance.transform.Rotate(0, -dragDelta * rotationSpeed, 0);

        // ���� �巡�� ��ġ�� ������ ��ġ�� ������Ʈ
        lastDragPosition = currentDragPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false; // �巡�� ����
    }
}