using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
public class IDLELvManager : MonoBehaviour
{
    private static readonly double basicExpSTATIC = 50d;
    private static readonly double incExpSTATIC = 1.2d;
    private static readonly double incExpAfterLastTierSTATIC = 1.25d;
    private static readonly double investLargeExpRatioSTATIC = 0.400001d;

    [SerializeField] private bool isAuto = false;

    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private List<GameObject> expUpButtons;
    [SerializeField] private TextMeshProUGUI upListText;
    private double currentTotalExp;
    private IDLELvData lvData;
    private bool hasInit = false;
    private Action<int> IDLELvUpCheckAction;
    private List<Func<int, string>> IDLEUpListStrFunc;

    private StarHoneyManager starHoneyManager;

    void Awake()
    {
        initIDLE();
    }

    //public void InitData()
    //{
    //    lvData = PlayDataManager.LoadData<IDLELvData>(typeof(IDLELvData).ToString(), out bool hasdata);
    //    if (!hasdata)
    //    {
    //        lvData = new IDLELvData();
    //        PlayDataManager.SaveData<IDLELvData>(lvData, typeof(IDLELvData).ToString());
    //    }
    //    hasInit = true;
    //}
    //public void SaveData()
    //{
    //    PlayDataManager.SaveData<IDLELvData>(lvData, typeof(IDLELvData).ToString());
    //}
  
    public void SubscribeLvUpAction(Action<int> action)
    {
        IDLELvUpCheckAction += action; //Consider describe method would be needed or not
    }
    public void SubscribeUpListFunc(Func<int, string> func)
    {
        if (IDLEUpListStrFunc.Contains(func) == false) { IDLEUpListStrFunc.Add(func); }
    }
    private void initIDLE()
    {
        IDLEUpListStrFunc = new();
        starHoneyManager = FindAnyObjectByType<StarHoneyManager>();
        //InitData();
        lvData = new IDLELvData();
        currentTotalExp = calculateTotalExp();
    }
    private void upIDLELv()
    {
        lvData.Lv++;
        lvData.Exp -= currentTotalExp;
        currentTotalExp = calculateTotalExp();
        if (IDLELvUpCheckAction != null) { IDLELvUpCheckAction.Invoke(lvData.Lv); }
    }
  
    private double calculateTotalExp()
    {
        return basicExpSTATIC * Math.Pow(incExpSTATIC, lvData.Lv);
    }
   
 
    private double calculateProportion(double ratio)
    {
        return ratio * currentTotalExp;
    }
}

[System.Serializable]
public class IDLELvData
{
    public int Lv = 0;
    public double Exp = 0d;
}