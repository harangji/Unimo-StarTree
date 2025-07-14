using UnityEngine;

public class NaNWaitTracker : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error
            && condition.Contains("float.NaN has been passed into WaitForSeconds"))
        {
            // 스택 트레이스를 콘솔에 출력
            Debug.LogError("?? NaN WaitForSeconds 호출 위치:\n" + stackTrace);
            // (선택) 에디터에서 브레이크
            Debug.Break();
        }
    }
}