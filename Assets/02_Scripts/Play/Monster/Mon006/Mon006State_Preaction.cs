using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon006State_Preaction : MonsterState_Preaction
{
    [SerializeField] private Mon006State_Action aState;
    [SerializeField] private GameObject chargeFX;
    [SerializeField] private List<ParticleSystem> chargeFXPSystem;
    [SerializeField] private Custom3DAudio chargeSFX3D;
    [SerializeField] private float chargeTime = 1.5f;
    private Mon006Indicator indicatorCtrl;
    private float lapseTime = 0;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        setFXDuration(chargeTime);
        chargeFX.SetActive(true);
        indicatorCtrl = controller.indicatorCtrl as Mon006Indicator;
        indicatorCtrl.SetIndicatorLength(indicatorCtrl.GetIndicatorTransform().position.y/controller.transform.localScale.x);
        indicatorCtrl.SetIndicatorWidth(aState.GetLWidth());
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { invokeActionState(); }, chargeTime));
    }
    public override void UpdateAction()
    {
        chargeSFX3D.transform.position = calcProjectedSFXPos();
        chargeSFX3D.SetMaxVolume(calcSFXVolume());
        if (lapseTime < chargeTime)
        {
            lapseTime += Time.deltaTime;
            controlIndi(lapseTime / chargeTime);
        }
    }
    private void controlIndi(float ratio)
    {
        indicatorCtrl.ControlIndicator(ratio);
    }
    private void setFXDuration(float duration)
    {
        foreach(var ps in chargeFXPSystem)
        {
            ps.Stop();
            ParticleSystem.MainModule main = ps.main;
            main.duration = duration+0.2f;
            ps.Play();
        }
    }
    private Vector3 calcProjectedSFXPos()
    {
        Vector3 toPlayer = controller.playerTransform.position - controller.transform.position;
        Vector3 toSFX = Vector3.Dot(toPlayer, controller.transform.forward) * controller.transform.forward;
        toSFX += controller.transform.position;
        return toSFX;
    }
    private float calcSFXVolume()
    {
        if (lapseTime / chargeTime >0.8f)
        {
            return 4f * (1f - lapseTime / chargeTime);
        }
        else { return 0.8f; }
    }
}
