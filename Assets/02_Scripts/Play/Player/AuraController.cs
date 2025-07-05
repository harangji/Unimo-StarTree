using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{
    private float growthperSec = 12f;
    private float originalGrowth = 12f;
    private Vector3 originalScale;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flower"))
        {
            if (other.TryGetComponent<FlowerController>(out var flower))
            {
                flower.AuraAffectFlower(growthperSec * Time.fixedDeltaTime);
            }
        }
    }
    public void InitAura(float range)
    {
        transform.localScale = range * Vector3.one;
        originalScale = transform.localScale;
        growthperSec = originalGrowth;
    }
    public void Shrink()
    {
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform, 0.5f * originalScale, 0.1f));
        growthperSec = 0.3f * originalGrowth;
    }
    public void Resume()
    {
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform, originalScale, 0.07f));
        growthperSec = originalGrowth;
    }
}
