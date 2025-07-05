using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioListener : MonoBehaviour
{
    void Awake()
    {
        Custom3DAudio.SetListenerPos(transform);
    }
}
