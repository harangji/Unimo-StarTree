using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    
    public float MinPos;
    public float MaxPos;
    private void Start()
    {
        transform.position = pos();

        StartCoroutine(MovementPos());
    }

    Vector3 pos()
    {
        return new Vector3(transform.position.x, Random.Range(MinPos, MaxPos), transform.position.z);
    }

    IEnumerator MovementPos()
    {
        float percent = 0;
        float current = 0.0f;
        float start = transform.position.y;
        float end = pos().y;
        float LerpTimer = Random.Range(5.0f, 10.0f);
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / LerpTimer;
            float LerpPos = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(transform.position.x, LerpPos, transform.position.z);
            yield return null;
        }

        StartCoroutine(MovementPos());
    }
}
