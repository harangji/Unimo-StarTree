using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestLevelController : MonoBehaviour
{
    public bool isLeveling { get; set; } = false;

    private int harvestLevel = 0;
    private float currentExp = 0f;
    private float lossperStun = 0.1f;
    private float minLoss = 0.05f;
    private HarvestLevelVisualCtrl visualCtrl;
    [SerializeField] private float nextLvExp = 1.5f;
    [SerializeField] private float scoreAddatSatu = 5f;
    [SerializeField] private float timeBonusatSatu = 1f;
    [SerializeField] private int saturationLvStandard = 20;

    private void Awake()
    {
        PlaySystemRefStorage.harvestLvController = this;
        FlowerController.HarvestLvCtrlSTATIC = this;
        visualCtrl = GetComponent<HarvestLevelVisualCtrl>();
        visualCtrl.SetSaturationStandard(saturationLvStandard);
    }
    // Update is called once per frame
    void Update()
    {
        visualCtrl.SetExpGauge(Mathf.Clamp01(currentExp / nextLvExp));
    }
    public void AddExp(float exp)
    {
        currentExp += exp;
        if (currentExp > nextLvExp) { levelUp(); }
    }
    public float GetLvSatuRatio()
    {
        return (float)harvestLevel / saturationLvStandard;
    }
    public float GetScoreBonus()
    {
        float ratio = (float)harvestLevel / saturationLvStandard;
        return 1f + scoreAddatSatu * ratio;
    }
    public float GetTimeBonus()
    {
        float ratio = (float)harvestLevel / saturationLvStandard;
        return 1f + timeBonusatSatu * Mathf.Pow(ratio, 1f);
    }
    public void LossExp(float stunduration)
    {
        currentExp -= Mathf.Max(minLoss, lossperStun * stunduration) * nextLvExp;
        if(currentExp < 0f) { currentExp = 0f; }
    }
    private void levelUp()
    {
        currentExp -= nextLvExp;
        harvestLevel++;
        visualCtrl.SetLvVisual(harvestLevel);
        nextLvExp *= 1.06f;
    }
}
