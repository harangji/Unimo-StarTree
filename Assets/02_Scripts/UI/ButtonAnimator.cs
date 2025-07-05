using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    static public bool isPressingSTATIC { get; set; }
    static private ButtonAnimator lastButtonSTATIC;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (animator != null) { animator.SetTrigger("pressed"); }
        isPressingSTATIC = true;
        lastButtonSTATIC = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (lastButtonSTATIC == this && isPressingSTATIC == true)
        {
            if (animator != null) { animator.SetTrigger("pressed"); }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null) { animator.SetTrigger("normal"); }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (animator != null) { animator.SetTrigger("normal"); }
    }
}
