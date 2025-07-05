using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    Transform[] buttons = new Transform[12];

    private void Start()
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            buttons[i] = transform.GetChild(i);
        }

        StartCoroutine(buttonRandomKey(Random.Range(0, buttons.Length)));
    }

    IEnumerator buttonRandomKey(int value)
    {
        Transform T = buttons[value];

        float current = 0;
        float percent = 0;
        float start = T.transform.localPosition.y;
        float end = T.transform.localPosition.y - 0.1f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpPos = Mathf.Lerp(start, end, percent);
            T.transform.localPosition = new Vector3(T.transform.localPosition.x, LerpPos, T.transform.localPosition.z);
            yield return null;
        }

        current = 0;
        percent = 0;
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpPos = Mathf.Lerp(end, start, percent);
            T.transform.localPosition = new Vector3(T.transform.localPosition.x, LerpPos, T.transform.localPosition.z);
            yield return null;
        }

        StartCoroutine(buttonRandomKey(Random.Range(0, buttons.Length)));
    }
}
