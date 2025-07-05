using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataConfigIOManager
{
    static private Dictionary<string, string> allConfigData = new();
    static private string configDataPath;

    static public void SaveData<T>(T data, string fileKey) where T : class
    {
        if (configDataPath == null || configDataPath.Length == 0) { initConfigData(); }
        T copyData = data;
        string configStr = JsonConvert.SerializeObject(copyData);
        if (!allConfigData.ContainsKey(fileKey)) { allConfigData.Add(fileKey, configStr); }
        else { allConfigData[fileKey] = configStr; }
        string crecordJson = JsonConvert.SerializeObject(allConfigData);
        File.WriteAllText(configDataPath, crecordJson);
    }
    static public T LoadData<T>(string fileKey, out bool hasdata) where T : class, new()
    {
        if (configDataPath == null || configDataPath.Length == 0) { initConfigData(); }
        if (!allConfigData.ContainsKey(fileKey)) 
        {
            T initData = new T();
            string configStr = JsonConvert.SerializeObject(initData);
            allConfigData.Add(fileKey, configStr);
            hasdata = false;

            return initData;
        }
        else
        {
            hasdata = true;
            return JsonConvert.DeserializeObject<T>(allConfigData[fileKey]);
        }
    }
    static private void initConfigData()
    {
        string datadir = Path.Combine(Application.persistentDataPath,"UnimoData");
        //Make folder for savefile
        if (!Directory.Exists(datadir))
            Directory.CreateDirectory(datadir);
        configDataPath = Path.Combine(datadir, "config.json");
        if (!File.Exists(configDataPath))
        {
            string crecordJson = JsonConvert.SerializeObject(allConfigData);
            File.WriteAllText(configDataPath, crecordJson);
        }
        string configJson = File.ReadAllText(configDataPath);
        allConfigData = JsonConvert.DeserializeObject<Dictionary<string, string>>(configJson);
    }
}
