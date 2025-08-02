using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController_Unique : FlowerController
{
    [SerializeField] private float maxGrowth = 20f;
    private float currentGrowth;
    private bool isGrowing;
    private float timeAfterActive = 0.08f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAfterActive > 0f)
        {
            timeAfterActive -= Time.deltaTime;
            if (timeAfterActive <= 0f) { isGrowing = false; visual.SetFlowerActivate(isGrowing); }
        }
    }
    override public void AuraAffectFlower(float affection)
    {
        if (!isGrowing) { isGrowing = true; visual.SetFlowerActivate(isGrowing); }
        timeAfterActive = 0.15f;
        currentGrowth += affection;
        (visual as FlowerFXController_ST001).AffectFlowerFX(currentGrowth / maxGrowth);
        if (currentGrowth >= maxGrowth) 
        { 
            completeBloom();
        }
    }
}
