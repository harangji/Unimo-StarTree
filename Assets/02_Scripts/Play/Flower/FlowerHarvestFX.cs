using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerHarvestFX : MonoBehaviour
{
    static private Transform targetTransform;
    private void Start()
    {
        if (targetTransform == null) { targetTransform = PlaySystemRefStorage.playerStatManager.transform; }
        StartCoroutine(followCoroutine());
    }
    private IEnumerator followCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            transform.position = targetTransform.position + 0.5f*Vector3.up;
            yield return null;
        }
    }
}
