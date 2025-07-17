using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 남은 플레이 시간 관리를 담당
// 시간 감소, 게이지 갱신, 일시정지, 시간 초과 처리, 난이도 증가 곡선 적용
public class PlayTimeManager : MonoBehaviour
{
    public float LapseTime { get; private set; } = 0f;
    [SerializeField] private bool isInfinite = false;
    [SerializeField] private float maxTime;
    [SerializeField] private float reduceIncTime = 120f;
    private float remainTime;
    private float minReduce = 1f;
    private bool isPaused = true;
    private ItemGenerator itemGenerator;
    [SerializeField] private TimeGaugeController timerGauge;
    
    private bool mbTimerStopped = false;
    
    // 외부에서 제한 시간을 설정할 수 있게 함
    public void SetStageTimeLimit(float time)
    {
        maxTime = time;
        remainTime = time;
        timerGauge.SetGauge(remainTime / maxTime);
    }
    
    public void StopTimer()
    {
        mbTimerStopped = true;
    }
    
    void Awake()
    {
        PlaySystemRefStorage.playTimeManager = this;
        itemGenerator = FindAnyObjectByType<ItemGenerator>();
        remainTime = maxTime;
        timerGauge.SetGauge(remainTime / maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || mbTimerStopped) { return; }
        LapseTime += Time.deltaTime;
        itemGenerator.DecreaseTick(Time.deltaTime);
        ChangeTimer(-Time.deltaTime);
    }
    
    // void Update()
    // {
    //     if (isPaused) { return; }
    //     LapseTime += Time.deltaTime;
    //     float rate = calcReduceRate(LapseTime);
    //     itemGenerator.DecreaseTick(Time.deltaTime * rate);
    //     ChangeTimer(-Time.deltaTime * rate);
    // }
    
    public void InitTimer()
    {
        remainTime = maxTime;
        isPaused = true;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ToggleTimer(); }, PlayProcessController.InitTimeSTATIC));
    }
    public void ToggleTimer()
    {
        isPaused = !isPaused;
    }
    public void ChangeTimer(float tchange)
    {
        if (isInfinite) { return; }
        remainTime += tchange;
        if (remainTime > maxTime) 
        {
            remainTime = maxTime; 
        }
        if (remainTime < 0f) 
        { 
            remainTime = 0f;
            timeUp();
        }
        timerGauge.SetGauge(remainTime / maxTime);
    }
    public float GetMaxTime()
    {
        return maxTime;
    }
    public float GetRemainTimeRatio()
    {
        return remainTime / maxTime;
    }
    private float calcReduceRate(float lapse)
    {
        float ratio = lapse / reduceIncTime;
        float rate = minReduce * ((0.7f * ratio * ratio + 0.3f * Mathf.Pow(ratio,3.2f)) + (1+Mathf.Exp(0.78f*ratio))/2);
        return rate;
    }
    private void timeUp()
    {
        isPaused = true;
        PlaySystemRefStorage.playProcessController.GameOver();
    }
}
