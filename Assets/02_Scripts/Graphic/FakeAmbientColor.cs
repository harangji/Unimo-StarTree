using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeAmbientColor : MonoBehaviour
{
    public Color FillLightColor = new Color32(168, 134, 245, 255);
    public Color KeyLightColor = new Color32(186,191,147,255);
    // Start is called before the first frame update
    void Start()
    {
        setAmbientColor();
    }
    private void OnValidate()
    {
        setAmbientColor();
    }
    private void setAmbientColor()
    {
        Shader.SetGlobalColor("_FillLightColor", FillLightColor);
        Shader.SetGlobalColor("_KeyLightColor", KeyLightColor);
    }
}
