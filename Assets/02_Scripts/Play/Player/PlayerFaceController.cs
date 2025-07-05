using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceController : MonoBehaviour
{
    private Animator faceAnim;
    private float blinkTime = 4f;
    private float blinkDeviation = 2.5f;
    private float closeDuration = 0.15f;
    private float serialBlinkTime = 0.15f;
    private bool blockBlink = true;

    // Start is called before the first frame update
    void Start()
    {
        faceAnim = GetComponent<Animator>();
        StartCoroutine(blinkCoroutine());
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ToggleBlink(false); }, PlayProcessController.InitTimeSTATIC));
    }

    public void ToggleBlink(bool isstop)
    {
        blockBlink = isstop;
    }
    private float calcNextBlink(ref int serial)
    {
        if (serial == 0 && Random.Range(0,1f) < 0.25f) 
        {
            serial++;
        }
        float x = Random.Range(-2f, 2f);
        float gauss = blinkDeviation * Mathf.Exp(-x * x);
        gauss *= -1f;
        gauss += blinkDeviation;
        if (gauss < -blinkTime + 2f * closeDuration) { gauss = 2f * closeDuration; }
        return blinkTime + gauss;
    }
    private void blinkEye()
    {
        if (blockBlink) { return; }
        faceAnim.SetTrigger("blink");
    }
    private IEnumerator blinkCoroutine()
    {
        WaitForSeconds blinkWait = new WaitForSeconds(closeDuration);
        WaitForSeconds serialWait = new WaitForSeconds(serialBlinkTime);
        int serial = 0;
        while (true)
        {
            serial = 0;
            yield return new WaitForSeconds(calcNextBlink(ref serial));
            blinkEye();
            yield return blinkWait;
            if (serial > 0)
            {
                yield return serialWait;
                blinkEye();
                yield return blinkWait;
            }
        }
    }
}
