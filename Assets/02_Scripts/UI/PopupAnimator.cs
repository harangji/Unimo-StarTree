using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimator : MonoBehaviour
{
    [SerializeField] private float disappearTime = 0.5f;
    private Animator animator;
    private void OnEnable()
    {
        if (animator == null) 
        { animator = GetComponent<Animator>(); }
        animator.SetBool("isappear", true);
    }
    public void PopdownUI()
    {
        animator.SetBool("isappear", false);
        StartCoroutine(CoroutineExtensions.DelayedActionCall(()=> { gameObject.SetActive(false); }, disappearTime));
    }
}
