using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInerthiaCtrl : MonoBehaviour
{
    private Test_PlayerMover mover;
    private Transform camTransform;
    private float serialTouchTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Test_PlayerMover>();
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) { return; }
        Touch touch = Input.GetTouch(0);
        if (touch.position.y < 120) { return; }
        if (touch.phase == TouchPhase.Began)
        {
            Vector3 touchedpos = new Vector3(touch.position.x, touch.position.y, 50);
            Vector3 pos = Camera.main.ScreenToWorldPoint(touchedpos);
            mover.AddForceByPosition(calculateMapPos(pos));
            serialTouchTime = 0.2f;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            serialTouchTime -= Time.deltaTime;
            if (serialTouchTime < 0)
            {
                Vector3 touchedpos = new Vector3(touch.position.x, touch.position.y, 50);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchedpos);
                mover.AddForceByPosition(calculateMapPos(pos));
                serialTouchTime = 0.2f;
            }
        }
    }
    private Vector3 calculateMapPos(Vector3 touchPos)
    {
        Vector3 vecToCam = (camTransform.position - touchPos).normalized;
        float tValue = -touchPos.y / vecToCam.y;
        Vector3 mapPos = touchPos + tValue * vecToCam;
        return mapPos;
    }
}
