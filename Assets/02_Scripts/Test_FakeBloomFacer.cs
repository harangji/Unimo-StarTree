using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FakeBloomFacer : MonoBehaviour
{
    static private Transform camPosSTATIC;
    private void Start()
    {
        if (camPosSTATIC == null) { camPosSTATIC = Camera.main.transform; }
    }
    // Update is called once per frame
    void Update()
    {
        transform.forward = camPosSTATIC.position - transform.position;
    }
}
