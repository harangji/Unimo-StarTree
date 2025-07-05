using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinous_Flower_Holder : MonoBehaviour
{
    public static Pinous_Flower_Holder instance = null;
    public static List<Transform> FlowerHolder = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}
