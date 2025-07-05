using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerHarvestFX_Lobby : MonoBehaviour
{
    private Transform targetTransform;
    public void SetUnimoTransform(Transform unimoTransform)
    {
        targetTransform = unimoTransform;
        StartCoroutine(followCoroutine());
    }
    private IEnumerator followCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            transform.position = targetTransform.position + 1f * Vector3.up;
            yield return null;
        }
    }
}
