using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimTrigger : MonoBehaviour
{
    [SerializeField] private List<string> triggerList;
    [SerializeField] private float triggerTime = 3f;
    private Animator charAnim;

    // Start is called before the first frame update
    void Start()
    {
        charAnim = GetComponent<Animator>();
        StartCoroutine(triggerCoroutine());
    }

    private IEnumerator triggerCoroutine()
    {
        while(true)
        {
            yield return randomWait();
            int idx = Random.Range(0, triggerList.Count);
            charAnim.SetTrigger(triggerList[idx]);
        }
    }
    private WaitForSeconds randomWait()
    {
        float randTime = triggerTime * Random.Range(1f, 1.7f);
        return new WaitForSeconds(randTime);
    }
}
