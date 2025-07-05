using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskOut : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim.SetTrigger("maskout");
    }
}
