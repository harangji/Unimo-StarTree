using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    // Update is called once per frame

    void Update()
    {
        if (ButtonAnimator.isPressingSTATIC == false) { return; }
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0)) { ButtonAnimator.isPressingSTATIC = false; }
#else
        if (Input.touchCount == 0)
        {
            ButtonAnimator.isPressingSTATIC = false;
        }
#endif
    }
}
