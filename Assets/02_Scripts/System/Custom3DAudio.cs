using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom3DAudio : MonoBehaviour
{
    static private Transform listenerSTATIC;
    [SerializeField] private float ratio3D = 0f;
    [SerializeField] private float minDist = 2.5f;
    [SerializeField] private float maxDist = 10f;
    private AudioSource audioSource;
    private float maxVolume;
    private void Start()
    {
        if (ratio3D < 0.01f) { Destroy(gameObject.GetComponent<Custom3DAudio>()); }
        if (TryGetComponent<AudioSource>(out var audiosource))
        {
            audioSource = audiosource;
            maxVolume = audioSource.volume;
            setVolume();
            StartCoroutine(volumeControlCoroutine());
        }
        else
        {
            Destroy(gameObject.GetComponent<Custom3DAudio>());
        }
    }
    static public void SetListenerPos(Transform listener)
    {
        listenerSTATIC = listener;
    }
    public void SetMaxVolume(float newmax)
    {
        maxVolume = newmax;
    }
    private void setVolume()
    {
        float dist = (listenerSTATIC.position - transform.position).magnitude;
        float ratio = ratio3D * calculate3DDistAttenuation(dist) + (1 - ratio3D);
        float newvolume = ratio * maxVolume;
        audioSource.volume = newvolume;
        audioSource.volume = Sound_Manager.instance._audioSources[1].volume;
    }
    private float calculate3DDistAttenuation(float dist)
    {
        if(dist < minDist) { return 1f; }
        if(dist > maxDist) { return 0f; }
        return Mathf.Clamp01((maxDist - dist) / (maxDist - minDist));
    }
    private IEnumerator volumeControlCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while(true)
        {
            setVolume();
            yield return wait;
        }
    }
}
