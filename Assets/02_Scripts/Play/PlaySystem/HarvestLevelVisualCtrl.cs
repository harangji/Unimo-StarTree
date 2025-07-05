using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HarvestLevelVisualCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private RectTransform expGaugeRect;
    [SerializeField] private Renderer harvestLvVFX;
    private Material harvestLvMat;
    private float gaugeWidth = 30f;
    private float gaugeHeight = 10f;
    private float visualSaturation = 20;
    // Start is called before the first frame update
    void Start()
    {
        harvestLvMat = harvestLvVFX.material;
        gaugeWidth = expGaugeRect.rect.width;
        gaugeHeight = expGaugeRect.rect.height;
        SetExpGauge(0f);
        SetLvVisual(0);
    }
    public void SetLvVisual(int lv)
    {
        lvText.text = lv.ToString();
        float ratio = Mathf.Pow(Mathf.Clamp01(lv / visualSaturation),0.5f);
        float auraVis = (1f - ratio) * 0.01f + ratio;
        StartCoroutine(setLvVisualCoroutine(auraVis));
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(lvText.transform, 1.25f * Vector3.one, 0.2f));
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(lvText.transform, Vector3.one, 0.2f)); }, 0.2f));
    }
    public void SetExpGauge(float expratio)
    {
        expGaugeRect.sizeDelta = new Vector2(expratio * gaugeWidth, gaugeHeight);
    }
    public void SetSaturationStandard(float satu)
    {
        visualSaturation = satu;
    }
    private IEnumerator setLvVisualCoroutine(float target)
    {
        float lapse = 0f;
        float start = harvestLvMat.GetFloat("_InnerScale");
        while (lapse <= 0.15f)
        {
            float ratio = Mathf.Clamp01(lapse / 0.15f);
            float value = (1f- ratio) * start + ratio * target;
            harvestLvMat.SetFloat("_InnerScale", value);
            lapse += Time.deltaTime;
            yield return null;
        }
        harvestLvMat.SetFloat("_InnerScale", target);
        yield break;
    }
}
