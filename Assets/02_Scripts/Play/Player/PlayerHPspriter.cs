using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPspriter : MonoBehaviour
{
    [SerializeField] private List<GameObject> images;

    public void renewImages(int number)
    {
        if (number < 0) { number = 0; }
        for (int i = 0; i < images.Count; i++)
        {
            if (i >= number) { images[i].SetActive(false); }
            else { images[i].SetActive(true); }
        }
    }
}
