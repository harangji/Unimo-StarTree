using UnityEngine;

public static class MyDebug
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public static void Log(object message)
    {
        Debug.Log($"<color=red>{message}</color>");
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
#else
    public static void Log(object message) { }
    public static void LogWarning(object message) { }
    public static void LogError(object message) { }
#endif
}