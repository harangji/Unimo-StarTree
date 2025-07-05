using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandAnimator : MonoBehaviour
{
    [SerializeField] private Animator[] animators;

    private void Awake()
    {
        for(int i = 0; i< animators.Length; i++)
        {
            animators[i].transform.localScale = Vector3.zero;
        }
        this.gameObject.SetActive(false);
    }
    public void DefaultInitalize()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            var go = animators[i].transform;
            animators[i].enabled = false;
            go.localScale = Vector3.one;
        }
    }
    public void Initalize()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].transform.localScale = new Vector3(0, 0, 0);
        }

        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("isSCALE");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
