using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerComboController : MonoBehaviour
{
    //private readonly float comboTime = 3.1f;
    [SerializeField] private float scoreAddatStnd = 5f;
    [SerializeField] private float timeBonusatStnd = 1f;
    [SerializeField] private float parameterStandard = 195f;
    private int comboAccum = 0;
    private float lossperStun = 0.1f;
    private FlowerComboVisualCtrl visualCtrl;
    // Start is called before the first frame update
    void Start()
    {
        visualCtrl = GetComponent<FlowerComboVisualCtrl>();
        visualCtrl.SetSaturationStandard(parameterStandard);
    }
    public void AddCombo()
    {
        ++comboAccum;
        visualCtrl.SetComboVisual(comboAccum);
    }
    public void LossCombo(float stunduration)
    {
        int loss = Mathf.Max(1, (int)(lossperStun * stunduration * comboAccum));
        comboAccum -= loss;
        if (comboAccum < 0) { comboAccum = 0; }
        visualCtrl.SetComboVisual(comboAccum);
    }
    public float GetScoreBonus()
    {
        float ratio = comboAccum / parameterStandard;
        return 1f + scoreAddatStnd * (0.5f * ratio + 0.5f * ratio * ratio);
    }
    public float GetTimeBonus()
    {
        float ratio = comboAccum / parameterStandard;
        return 1f + timeBonusatStnd * (0.7f * ratio + 0.3f * Mathf.Pow(ratio, 2.5f));
    }
    public int GetComboAccum() 
    {
        return comboAccum;
    }
}
