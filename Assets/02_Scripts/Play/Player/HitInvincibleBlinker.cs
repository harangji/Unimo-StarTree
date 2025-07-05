using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInvincibleBlinker : MonoBehaviour
{
    [SerializeField] private List<Renderer> blinkTargets;
    private List<Material> blinkMaterials;

    private float blinkTime = 0.166667f;
    private float upBlinkCycle = 0.5f;
    private float downBlinkCycle;
    private float dampStartRemain;
    private float dampPower = 2f;

    // Start is called before the first frame update
    void Start()
    {
        blinkMaterials = new List<Material>();
        for (int i = 0; i < blinkTargets.Count; i++)
        {
            blinkMaterials.Add(blinkTargets[i].material);
        }
        dampStartRemain = 3f * blinkTime;
        downBlinkCycle = 1f - upBlinkCycle;

        FindAnyObjectByType<PlayerVisualController>().AddInvincibleBlinker(this);
    }

    public void StartBlink(float duration)
    {
        StartCoroutine(blinkCoroutine(duration));
    }
    private IEnumerator blinkCoroutine(float duration)
    {
        float lapseTime = 0f;
        while(lapseTime <= duration)
        {
            lapseTime += Time.deltaTime;
            float blink = calculateBlinkAmplitude(duration - lapseTime) * calculateBlinkSawTooth(lapseTime);
            for (int i = 0; i < blinkTargets.Count; i++)
            {
                blinkMaterials[i].SetFloat("_Invincible", blink);
            }
            yield return null;
        }
        for (int i = 0; i < blinkTargets.Count; i++)
        {
            blinkMaterials[i].SetFloat("_Invincible", 0f);
        }
        yield break;
    }
    private float calculateBlinkSawTooth(float lapse)
    {
        float ratio = lapse / blinkTime;
        ratio -= Mathf.Floor(ratio);
        if (ratio <= upBlinkCycle)
        {
            ratio /= upBlinkCycle;
        }
        else
        {
            ratio = (1f - ratio) / downBlinkCycle;
        }
        return ratio;
    }
    private float calculateBlinkAmplitude(float remain)
    {
        float ratio;
        if (remain < dampStartRemain)
        {
            ratio = Mathf.Pow(remain / dampStartRemain, dampPower);
        }
        else { ratio = 1f; }
        return ratio;
    }
}
