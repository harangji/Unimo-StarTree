using UnityEngine;
using UnityEngine.EventSystems;

public class TouchTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // ȭ���� ��ġ�ߴ���
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // ��ġ���� ��, ��ġ ��ġ�� UI ����� ture
            Debug.Log($" UI ���ΰ�? {EventSystem.current.IsPointerOverGameObject(touch.fingerId)}");
        }	
    }
}
