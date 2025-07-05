using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    [SerializeField] private float speed = 7f;

    // Update is called once per frame
    void Update()
    {
        float haxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");
        Vector2 vec = new Vector2(haxis, vaxis);
        moveObj(vec);
    }
    private void moveObj(Vector2 inputDir)
    {
        Vector3 dir = new Vector3(inputDir.x, 0f, inputDir.y);
        transform.position += speed * Time.deltaTime * dir;
    }
}
