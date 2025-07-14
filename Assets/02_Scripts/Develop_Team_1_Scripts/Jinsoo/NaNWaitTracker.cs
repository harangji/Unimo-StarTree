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
            // ���� Ʈ���̽��� �ֿܼ� ���
            Debug.LogError("?? NaN WaitForSeconds ȣ�� ��ġ:\n" + stackTrace);
            // (����) �����Ϳ��� �극��ũ
            Debug.Break();
        }
    }
}