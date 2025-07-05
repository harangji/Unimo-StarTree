using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSustainer : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
