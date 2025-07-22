using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아우라 내부(해당 스크립트)에서 아우라 크기 조절이라 꽃 피우는 속도까지 설정하는 코드 이미 있음. 
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
    
    //CSV로부터 오라 크기(range)를 능동적으로 받아서 조절,
    //AuraController, 해당 스크립트에서 실질적인 크기를 담당
    //해당 로직을 작성하는 것이 책임 분리(SRP)와 응집도 측면에서 가장 적절.
    
    public void InitAura(float range, float auraStrength)
    {
        transform.localScale = range * Vector3.one;
        originalScale = transform.localScale;

        originalGrowth = 12f * auraStrength; // 기본 성장 속도 × 배율
        growthperSec = originalGrowth;
        Debug.Log($"[AuraController] 아우라 초기화됨 → Range: {range}, Scale: {transform.localScale}, Growth: {growthperSec}");
    }
    
    public void SetAuraRange(float newRange)
    {
        Vector3 targetScale = newRange * Vector3.one;
        originalScale = targetScale;
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform, targetScale, 0.1f));

        Debug.Log($"[AuraController] 실시간 아우라 크기 변경됨 → {newRange}");
    }

    
    //원본 코드. 정현식
    //public void InitAura(float range)
    //{
    //    transform.localScale = range * Vector3.one;
    //    originalScale = transform.localScale;
    //    growthperSec = originalGrowth;
    //}
    
    
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
