using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �ƿ�� ����(�ش� ��ũ��Ʈ)���� �ƿ�� ũ�� �����̶� �� �ǿ�� �ӵ����� �����ϴ� �ڵ� �̹� ����. 
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
    
    //CSV�κ��� ���� ũ��(range)�� �ɵ������� �޾Ƽ� ����,
    //AuraController, �ش� ��ũ��Ʈ���� �������� ũ�⸦ ���
    //�ش� ������ �ۼ��ϴ� ���� å�� �и�(SRP)�� ������ ���鿡�� ���� ����.
    
    public void InitAura(float range, float auraStrength)
    {
        transform.localScale = range * Vector3.one;
        originalScale = transform.localScale;

        originalGrowth = 12f * auraStrength; // �⺻ ���� �ӵ� �� ����
        growthperSec = originalGrowth;
        Debug.Log($"[AuraController] �ƿ�� �ʱ�ȭ�� �� Range: {range}, Scale: {transform.localScale}, Growth: {growthperSec}");
    }
    
    public void SetAuraRange(float newRange)
    {
        Vector3 targetScale = newRange * Vector3.one;
        originalScale = targetScale;
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform, targetScale, 0.1f));

        Debug.Log($"[AuraController] �ǽð� �ƿ�� ũ�� ����� �� {newRange}");
    }

    
    //���� �ڵ�. ������
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
