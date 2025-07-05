using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDirectionalLight : MonoBehaviour
{
    public Vector3 LightDirection = new Vector3(-0.5f, 0f, 0.5f);
    private Vector3 normLightDirection;
    // Start is called before the first frame update
    void Start()
    {
        setLightDirection();
    }
    private void OnValidate()
    {
        setLightDirection();
    }
    private void setLightDirection()
    {
        normLightDirection = LightDirection.normalized;
        Shader.SetGlobalVector("_InvertedDirLight", normLightDirection);
    }
    public void NormalizeLightVector()
    {
        LightDirection.Normalize();
        setLightDirection();
    }
}
