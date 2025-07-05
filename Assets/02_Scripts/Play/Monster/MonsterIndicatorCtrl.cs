using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIndicatorCtrl : MonoBehaviour
{
    [SerializeField] protected GameObject indicatorObj;
    protected Renderer indicatorRender;
    protected Material indicatorMat;

    protected void Awake()
    {
        indicatorRender = indicatorObj.GetComponentInChildren<Renderer>();
        indicatorMat = indicatorRender.material;
        InitIndicator();
    }
    public Transform GetIndicatorTransform()
    {
        return indicatorObj.transform;
    }
    virtual public void InitIndicator()
    { }
    virtual public void ActivateIndicator()
    { indicatorObj.SetActive(true); }
    virtual public void DeactivateIndicator()
    { indicatorObj.SetActive(false); }
    virtual public void ControlIndicator(float ratio)
    { }
}

