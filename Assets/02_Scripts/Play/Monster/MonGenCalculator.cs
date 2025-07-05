using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonGenCalculatorSObj", menuName = "Scriptable Object/MonGenCalc", order = int.MaxValue)]
public class MonGenCalculator : ScriptableObject
{
    private float generatorTime = 0f;
    [SerializeField] private float minimumGentime = 1f; //Basic spawn time is controlled using sigmoid decay
    [SerializeField] private float genDecayingTime = 100f;
    [SerializeField] private float defaultGentime = 4.5f;

    [SerializeField] private float bigWaveGenMaxReduce = 0.3f;
    //private float bigWaveReduceGraphPow = 2.5f;
    //private float modAtPeak = 0.21317037564f;
    [SerializeField] private float minimumBigWavetime = 25f; //BigWave call time is controlled using linear decay
    [SerializeField] private float bigWaveDecreasingTime = 240f;
    [SerializeField] private float defaultBigWavetime = 50f;
    [SerializeField] private float minimumBigWaveDuration = 15f; //BigWave duration is controlled using linear decay proportional to BigWave call time changes
    [SerializeField] private float defaultBigWaveDuration = 25f;

    public void InitTime()
    {
        generatorTime = 0f;
    }
    public void AddLapseTime(float time)
    {
        generatorTime += time;
    }
    public float calculateBasicGenTime()
    {
        float basicTime = minimumGentime + 1.367f * (defaultGentime - minimumGentime) / (1 + Mathf.Exp((generatorTime - genDecayingTime) / genDecayingTime));
        
        basicTime *= Random.Range(0.75f, 1.25f);
        return basicTime;
    }
    public float calculateBigWaveTime()
    {
        float ratio = Mathf.Clamp01(generatorTime / bigWaveDecreasingTime);
        float bigWtime = ratio * minimumBigWavetime + (1 - ratio) * defaultBigWavetime;
        bigWtime *= Random.Range(0.9f, 1.1f);
        return bigWtime;
    }
    public float calculateBigWaveDuration()
    {
        float ratio = Mathf.Clamp01(generatorTime / bigWaveDecreasingTime);
        float bigWduration = ratio * minimumBigWaveDuration + (1 - ratio) * defaultBigWaveDuration;
        bigWduration *= Random.Range(0.9f, 1.1f);
        return bigWduration;
    }
    public float calculateBigWaveWeight(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        //float weight = Mathf.Pow(ratio, bigWaveReduceGraphPow) * (ratio * ratio - 1);
        //weight = ((1 - bigWaveGenMaxReduce) / modAtPeak) * (weight + modAtPeak) + bigWaveGenMaxReduce;

        float weight = Mathf.Abs((ratio - 0.5f) / 0.5f);
        weight = (1 - bigWaveGenMaxReduce) * Mathf.Pow(weight, 3f) + bigWaveGenMaxReduce;
        return weight;
    }
    public float GetGenTime()
    {
        return generatorTime;
    }
}
