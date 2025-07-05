using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { collider.enabled = true; }, 1.1f));
    }
    virtual public void UseItem()
    { Destroy(gameObject); }
}
