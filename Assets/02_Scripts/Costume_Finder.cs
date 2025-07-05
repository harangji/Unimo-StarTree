using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Costume_Finder : MonoBehaviour
{
    public static Costume_Finder instance = null;
    public bool GetStartHandler;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    Vector2 startPos;
    Vector2 endPos;
    public float speed;
   
    public void ReturnCamera()
    {
        GetStartHandler = false;
        transform.rotation = Quaternion.Euler(0,180.0f, 0.0f);
    }
}
