using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] protected MonGenCalculator monGenCalculator;
    
    [SerializeField] protected GameObject monsterPattern1;
    [SerializeField] protected GameObject monsterPattern2;
    [SerializeField] protected GameObject monsterPattern3;

    protected float timetoNextGen;
    protected float timetoNextBigWave;
    protected float currentBigWaveDuration;
    protected bool isBigWave;
    protected float bigWaveLapseTime = 0f;
    protected Transform playerTransform;
    protected bool isPaused = false;
    protected bool isExtreme = false;
    protected Coroutine specialPatternCoroutine;
    // Start is called before the first frame update
    protected void OnEnable()
    {
        PlayerMover mover= FindAnyObjectByType<PlayerMover>();
        if (mover != null ) { playerTransform = mover.transform; }
        monGenCalculator.InitTime();
        isPaused = true;
        timetoNextGen = 0f;
        timetoNextBigWave = monGenCalculator.calculateBigWaveTime();
        currentBigWaveDuration = monGenCalculator.calculateBigWaveDuration();
        specialPatternCoroutine = StartCoroutine(startPatternCoroutine());
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isPaused) { return; }

        float timediff = Time.deltaTime;
        monGenCalculator.AddLapseTime(timediff);
        timetoNextGen -= timediff/monGenCalculator.calculateBigWaveWeight(Mathf.Clamp01(bigWaveLapseTime / currentBigWaveDuration));
        timetoNextBigWave -= timediff;
        if (timetoNextGen < 0f)
        {
            generateEnemy();
            timetoNextGen = monGenCalculator.calculateBasicGenTime();
        }
        if (timetoNextBigWave < 0f && !isBigWave)
        {
            StartCoroutine(callBigWaveCoroutine());
        }
    }
    public void PauseGenerate(bool isstopEx)
    {
        isPaused = true;
        if (isstopEx)
        {
            if (isExtreme || specialPatternCoroutine != null) { StopCoroutine(specialPatternCoroutine); }
        }
    }
    public void ResumeGenerator()
    {
        isPaused = false;
    }
    public void TriggerExPattern()
    {
        PauseGenerate(false);
        specialPatternCoroutine = StartCoroutine(exPatternCoroutine());
    }
    protected virtual MonsterController generateEnemy()
    {
        Vector3 pos = findGenPosition();
        Quaternion quat = setGenRotation(pos);
        MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        controller.InitEnemy(playerTransform);

        return controller;
    }
    virtual protected Vector3 findGenPosition()
    {
        return Vector3.zero;
    }
    protected virtual Quaternion setGenRotation(Vector3 genPos)
    {
        Quaternion rot = Quaternion.identity;
        return rot;
    }
    protected IEnumerator callBigWaveCoroutine()
    {
        isBigWave = true;
        bigWaveLapseTime = 0f;
        while (bigWaveLapseTime < currentBigWaveDuration)
        {
            bigWaveLapseTime += Time.deltaTime;
            yield return null;
        }
        timetoNextBigWave = monGenCalculator.calculateBigWaveTime();
        currentBigWaveDuration = monGenCalculator.calculateBigWaveDuration();
        isBigWave = false;
        yield break;
    }
    virtual protected IEnumerator startPatternCoroutine()
    {
        isPaused = false;
        yield break;
    }
    virtual protected IEnumerator exPatternCoroutine()
    {
        isPaused = false;
        isExtreme = false;
        yield break;
    }
}
