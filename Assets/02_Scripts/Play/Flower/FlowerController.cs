using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    //static protected FlowerComboController comboCtrlSTATIC;
    static public HarvestLevelController HarvestLvCtrlSTATIC { private get; set; }
    protected FlowerGenerator flowerGenerator;

    [SerializeField] protected float scoreGain = 10f;
    [SerializeField] protected int resourceIdx = 0;
    [SerializeField] protected float destroyWaitTime = 2f;

    protected FlowerFXController visual;
    protected void Start()
    {
        visual = GetComponent<FlowerFXController>();
    }
    public void InitFlower(FlowerGenerator generator)
    {
        flowerGenerator = generator;
        if (flowerGenerator != null) { flowerGenerator.AllFlowers.Add(this); }
        StartCoroutine(CoroutineExtensions.DelayedActionCall(ActivateFlower, 0.5f));
    }
    virtual public void AuraAffectFlower(float affection)
    { }
    protected void completeBloom()
    {
        DeactivateFlower();
        visual.TriggerHarvestFX(HarvestLvCtrlSTATIC.GetLvSatuRatio()); //Change so that use lv satu ratio
        PlaySystemRefStorage.scoreManager.AddBloomScore(resourceIdx, HarvestLvCtrlSTATIC.GetScoreBonus() * scoreGain);
        if (flowerGenerator != null) 
        { 
            flowerGenerator.AllFlowers.Remove(this); 
            flowerGenerator.GatherFlower(); 
        }
        //comboCtrlSTATIC.AddCombo();
    }
    virtual protected void ActivateFlower()
    {
        GetComponent<Collider>().enabled = true;
    }
    virtual protected void DeactivateFlower()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, destroyWaitTime);
    }
}
