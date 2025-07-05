using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class PlayDataManager
{
    static private SPlayData playData;
    static public void InitData()
    {
        playData = DataLocalIOManager.LoadData<SPlayData>(typeof(SPlayData).ToString(), out bool hasfile);
        if (!hasfile)
        {
            playData = new SPlayData();
            playData.ActualData = new();
            SaveData<SPlayData>(playData, typeof(SPlayData).ToString());
        }
    }
    static public void SaveData<T>(T data, string dataKey) where T : class
    {
        if (playData == null || playData.ActualData == null) { InitData(); }
        string datastr = JsonConvert.SerializeObject(data);
        if (!playData.ActualData.ContainsKey(dataKey))
        {
            playData.ActualData.Add(dataKey, string.Empty);
        }
        playData.ActualData[dataKey] = datastr;
        DataLocalIOManager.SaveData<SPlayData>(playData, typeof(SPlayData).ToString());
    }
    static public T LoadData<T>(string dataKey, out bool hasdata) where T : class
    {
        if (playData == null || playData.ActualData == null) { InitData(); }
        if (!playData.ActualData.ContainsKey(dataKey))
        {
            hasdata = false;
            playData.ActualData.Add(dataKey, string.Empty);
            return default(T);
        }
        else
        {
            hasdata = true;
            T data = JsonConvert.DeserializeObject<T>(playData.ActualData[dataKey]);
            return data;
        }
    }
}
[System.Serializable]
public class SPlayData
{
    public Dictionary<string, string> ActualData;
}