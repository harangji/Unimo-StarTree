using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AnimwithMove : MonoBehaviour
{
    public float speed = 7f;
    public bool isTimeLimit = false;
    public float limitTime = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
        limitTime -= Time.deltaTime;
        if (isTimeLimit && limitTime <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
