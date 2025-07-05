using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFXController_ST001 : FlowerFXController
{
    [SerializeField] private string bloomAnimClipname = "anim_FLO001_Blossom";

    private float lastRatio;
    private float followSpeed = 0.05f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
    public void AffectFlowerFX(float growth)
    {
        if (isinBlossom())
        {
            float ratio = Mathf.Clamp01(growth);
            calculateNewLastRatio(ratio);
            jumpToTime(bloomAnimClipname, lastRatio);
        }
    }
    override public void TriggerHarvestFX(float hLvRatio)
    {
        base.TriggerHarvestFX(hLvRatio);
        jumpToTime(bloomAnimClipname, 1f);
        anim.SetBool("isharvested", true);
        anim.SetLayerWeight(1, 0);
    }
    override public void SetFlowerActivate(bool activate)
    {
        anim.SetBool("isactive", activate);
    }
    private bool isinBlossom()
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName(bloomAnimClipname))
        {
            return true;
        }
        return false;
    }
    private void jumpToTime(string name, float nTime)
    {
        anim.Play(name, 1, nTime);
    }
    private void calculateNewLastRatio(float ratio)
    {
        lastRatio = followSpeed * ratio + (1 - followSpeed) * lastRatio;
    }
    override protected IEnumerator harvestFXCoroutine()
    {
        yield return new WaitForSeconds(0.28f);
        float normTime = 0f;
        while (normTime <= 0.6666f)
        {
            normTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            yield return null;
        }
        harvestSFX.gameObject.SetActive(true);
        harvestVFX.SetActive(true);
        harvestVFX.transform.SetParent(null, true);
        harvestVFX.transform.rotation = Quaternion.Euler(-30f, 180f, 0f);
        yield break;
    }
}
