using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecordManager : MonoBehaviour
{
    private SGRecordData gameRecords;
    private bool hasInit = false;
    // Start is called before the first frame update
    void Awake()
    {
        InitData();
    }
    public void InitData()
    {
        gameRecords = PlayDataManager.LoadData<SGRecordData>(typeof(SGRecordData).ToString(), out bool hasdata);
        if (!hasdata)
        {
            gameRecords = new SGRecordData();
            gameRecords.Records = new();
            PlayDataManager.SaveData<SGRecordData>(gameRecords, typeof(SGRecordData).ToString());
        }
        hasInit = true;
    }
    public void SaveData()
    {
        PlayDataManager.SaveData<SGRecordData>(gameRecords, typeof(SGRecordData).ToString());
    }
    public void SetBestRecord(double score, int gameidx)
    {
        if (!hasInit) { return; }
        while(gameidx >= gameRecords.Records.Count)
        {
            gameRecords.Records.Add(0f);
        }
        gameRecords.Records[gameidx] = score;
        switch(gameidx)
        {
            case 0: Base_Mng.Data.data.BestScoreGameOne = score; break;
            case 1: Base_Mng.Data.data.BestScoreGameTwo = score; break;
        }
        SaveData();
    }
    public double GetBestRecord(int gameidx)
    {
        if (!hasInit) { return -1f; }
        while (gameidx >= gameRecords.Records.Count)
        {
            gameRecords.Records.Add(0f);
        }
        return gameRecords.Records[gameidx];
    }
}
[System.Serializable]
public class SGRecordData
{
    public List<double> Records;
}