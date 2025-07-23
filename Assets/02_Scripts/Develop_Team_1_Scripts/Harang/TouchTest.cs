using UnityEngine;
using UnityEngine.EventSystems;

public class TouchTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 화면을 터치했는지
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // 터치했을 때, 터치 위치가 UI 위라면 ture
            Debug.Log($" UI 위인가? {EventSystem.current.IsPointerOverGameObject(touch.fingerId)}");
        }	
    }
}
