using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowerComboVisualCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Renderer havestVFXInner;
    private Material harvestMat;
    private float oriInScale = 0.3f;
    private float visualSaturation = 200;
    //private int lastAccum = 0;
    private void Start()
    {
        harvestMat = havestVFXInner.material;
        harvestMat.SetFloat("_InnerScale", oriInScale);
    }
    public void SetComboVisual(int accum)
    {
        comboText.text = accum.ToString();
        float ratio = Mathf.Pow(Mathf.Clamp01(accum / visualSaturation),0.7f);
        harvestMat.SetFloat("_InnerScale", (1f-ratio) * oriInScale + ratio);
    }
    public void SetSaturationStandard(float satu)
    {
        visualSaturation = satu;
    }
}
