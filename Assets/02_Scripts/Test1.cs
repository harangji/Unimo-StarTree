using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float scalev = (Time.timeScale > 0.5f) ? 0f : 1f;
            Time.timeScale = scalev;
        }
    }
}
