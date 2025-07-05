using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageExtenstions
{
    public static Color32 HSVInterp(this Color32 oriColor, Color32 targetColor, float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        float ho, so, vo;
        Color.RGBToHSV(oriColor, out ho, out so, out vo);
        float hi, si, vi;
        Color.RGBToHSV(targetColor, out hi, out si, out vi);
        float hf, sf, vf;
        hf = (1 - ratio) * ho + ratio * hi;
        sf = (1 - ratio) * so + ratio * si;
        vf = (1 - ratio) * vo + ratio * vi;

        Color32 color = Color.HSVToRGB(hf, sf, vf);
        color.a = 255;
        return color;
    }
}
