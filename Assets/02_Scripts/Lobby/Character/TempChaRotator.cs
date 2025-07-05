using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempChaRotator : MonoBehaviour
{
    [SerializeField] private Vector2 chaCenter = new Vector2(535f, 1570f);
    private Vector2 centerPos;
    private float centerAngle = 180f;
    private float rotateSense = 1f;
    private bool isRotate = false;

    [SerializeField] private bool test_IsKeyboard = false;
    // Update is called once per frame
    void Update()
    {
        if (test_IsKeyboard)
        {
            float rotAngle = 180f * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
            {
                Quaternion quat = Quaternion.Euler(0f, rotAngle, 0f);
                transform.rotation *= quat;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Quaternion quat = Quaternion.Euler(0f, -rotAngle, 0f);
                transform.rotation *= quat;
            }
        }
        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            centerPos = touch.position;
            centerAngle = transform.rotation.eulerAngles.y;
            isRotate = isinRange(centerPos);
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            if (!isRotate) { return; }
            float angle = convertToAngleInc(touch.position);
            transform.rotation = Quaternion.Euler(0f, centerAngle + angle, 0f);
        }
    }
    private bool isinRange(Vector2 center)
    {
        if (Mathf.Abs(center.x - chaCenter.x) <= 300f && Mathf.Abs(center.y - chaCenter.y) <= 300f)
        {
            return true;
        }
        else { return false; }
    }
    private float convertToAngleInc(Vector2 inputPos)
    {
        Vector2 diff = inputPos - centerPos;

        return -rotateSense * diff.x;
    }
}
