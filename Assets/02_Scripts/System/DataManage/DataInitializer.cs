using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private string IV;
    // Start is called before the first frame update
    void Awake()
    {
        Cryptographer.InitCryp(key, IV);
        PlayDataManager.InitData();
    }
}
