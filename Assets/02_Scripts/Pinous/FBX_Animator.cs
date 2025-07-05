using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBX_Animator : MonoBehaviour
{
    public float speed;
    public enum FBX_STATE
    {
        CD,
        TeaBag,
        PinWheel,
        AudioMixer
    }

    private void Start()
    {
        GetStartAnimator();
    }

    public FBX_STATE m_State;

    void GetStartAnimator()
    {
        switch(m_State)
        {
            case FBX_STATE.TeaBag: StartCoroutine(Teabag_Coroutine()); break;
            case FBX_STATE.AudioMixer: StartCoroutine(AudioMixerAnim()); break;
        }
    }

    private void Update()
    {
        if(m_State == FBX_STATE.CD)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * speed);
        }
        if(m_State == FBX_STATE.PinWheel)
        {
            transform.Rotate(Vector3.back, Time.deltaTime * speed);
        }
    }
   

    private IEnumerator Teabag_Coroutine()
    {
        float current = 0.0f;
        float percent = 0;
        float start = 11.0f;
        float end = 13.0f;
        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / speed;
            transform.localPosition = new Vector3(transform.localPosition.x,
                Mathf.Lerp(start, end, percent), transform.localPosition.z);
            yield return null;
        }
        current = 0.0f;
        percent = 0.0f;
        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / speed;
            transform.localPosition = new Vector3(transform.localPosition.x,
         Mathf.Lerp(end, start, percent), transform.localPosition.z);
            yield return null;
        }
        StartCoroutine(Teabag_Coroutine());
    }

    private IEnumerator AudioMixerAnim()
    {
        float current = 0.0f;
        float percent = 0;
        float start = transform.localPosition.z;
        float end = Random.Range(-1.0f, 0.7f);
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / speed;
            transform.localPosition = new Vector3(transform.localPosition.x,
                transform.localPosition.y, Mathf.Lerp(start, end, percent));

            yield return null;
        }
        current = 0.0f;
        percent = 0.0f;
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / speed;
            transform.localPosition = new Vector3(transform.localPosition.x,
         transform.localPosition.y, Mathf.Lerp(end, start, percent));
            yield return null;
        }
        StartCoroutine(AudioMixerAnim());
    }

}
