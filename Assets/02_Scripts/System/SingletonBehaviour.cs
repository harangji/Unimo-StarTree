using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singletone parent class. Call .Instance to get singletone instance, and return in awake when _notWantedObj is true
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour //Generic singletone parent class
{
    private static object Lock = new object();
    protected static T _instance; // instance of singletone object
    public static bool IsNotWantedObj = false;
    public static T Instance // read only property for singletone
    {
        get
        {
            lock (Lock) // Thread-safe property
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T; // Assign singletone instance
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        set
        {
            lock (Lock)
            {
                if (_instance != null)
                {
                    if (_instance != value)
                    {
                        IsNotWantedObj = true;
                        Destroy(value.gameObject);
                    }
                    return;
                }
                else
                {
                    _instance = value;
                    DontDestroyOnLoad(value.gameObject);
                }
            }
        }
    }
    protected void Awake() // use first generated object as singletone
    {
        Instance = gameObject.GetComponent<T>();
    }
}
