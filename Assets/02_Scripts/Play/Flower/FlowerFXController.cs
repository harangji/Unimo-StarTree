using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFXController : MonoBehaviour
{
    [SerializeField] protected GameObject harvestVFX;
    [SerializeField] protected AudioSource harvestSFX;

    protected Animator anim;
    protected float randAngle = 20f;
    protected float randScale = 0.12f;
    protected float iniSFXPitch = 0.94387f;
    protected float lastSFXPitch = 1.0293f;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        float angle = randAngle * Random.Range(-1f, 1f);
        float scale = randScale * Random.Range(-1f, 1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        transform.localScale = (1 + scale) * Vector3.one;
    }
    
    virtual public void TriggerHarvestFX(float hLvRatio)
    {
        setSFXPitch(hLvRatio);
        StartCoroutine(harvestFXCoroutine());
    }
    virtual public void SetFlowerActivate(bool activate)
    { }
    protected void setSFXPitch(float hLvRatio)
    {
        float pitch = 1;
        if (hLvRatio < 1f)
        {
            pitch = (1f - hLvRatio) * iniSFXPitch + hLvRatio * lastSFXPitch;
        }
        else
        {
            pitch = lastSFXPitch;
        }
        //if (hLvRatio < 0.25f) 
        //{
        //    float ratio = Mathf.Clamp01(hLvRatio);
        //    pitch = (1- ratio) * iniSFXPitch + ratio; 
        //}
        //else if (hLvRatio < 1f) 
        //{
        //    float ratio = Mathf.Clamp01((float)(hLvRatio - 0.25f) / 0.75f);
        //    pitch = (1 - ratio) + ratio * lastSFXPitch;
        //}
        //else { pitch = lastSFXPitch; }
        harvestSFX.pitch = pitch;
    }
    virtual protected IEnumerator harvestFXCoroutine()
    {
        harvestSFX.gameObject.SetActive(true);
        harvestVFX.SetActive(true);
        harvestVFX.transform.SetParent(null, true);
        yield break;
    }
}
