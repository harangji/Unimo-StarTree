// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class StarHoneyManager : MonoBehaviour
// {
//     private SHoneyData starHoneys;
//     private bool hasInit = false;
//     private Action honeyChangeActions;
//
//     void Awake()
//     {
//         InitData();
//         StartCoroutine(saveCoroutine());
//     }
//     private void OnDisable()
//     {
//         SaveData();
//     }
//     public void InitData()
//     {
//         starHoneys = PlayDataManager.LoadData<SHoneyData>(typeof(SHoneyData).ToString(), out bool hasdata);
//         if (!hasdata)
//         {
//             starHoneys = new SHoneyData();
//             starHoneys.YellowHoney = 0d;
//             starHoneys.OrangeHoney = 0;
//             starHoneys.WhiteHoney = 0;
//             PlayDataManager.SaveData<SHoneyData>(starHoneys, typeof(SHoneyData).ToString());
//         }
//         hasInit = true;
//     }
//     public void SaveData()
//     {
//         PlayDataManager.SaveData<SHoneyData>(starHoneys, typeof(SHoneyData).ToString());
//     }
//     //Basic Methods
// #region
//     public void SubscribeAction(Action action)
//     {
//         honeyChangeActions += action;
//     }
//     public void AddYellow(double yellow)
//     {
//         if (!hasInit) { return; }
//         starHoneys.YellowHoney += yellow;
//         if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//     }
//     public void UseYellow(double yellow, out bool isUsable)
//     {
//         if (!hasInit) { isUsable = false; return; }
//         if (yellow > starHoneys.YellowHoney) { isUsable = false; return; }
//         else
//         {
//             isUsable = true;
//             starHoneys.YellowHoney -= yellow;
//             if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//         }
//     }
//     public double GetYellow()
//     {
//         if (!hasInit) { return -1d; }
//         return starHoneys.YellowHoney;
//     }
//     public void AddOrange(int orange)
//     {
//         if (!hasInit) { return; }
//         starHoneys.OrangeHoney += orange;
//         if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//     }
//     public void UseOrange(int orange, out bool isUsable)
//     {
//         if (!hasInit) { isUsable = false; return; }
//         if (orange > starHoneys.OrangeHoney) { isUsable = false; return; }
//         else
//         {
//             isUsable = true;
//             starHoneys.OrangeHoney -= orange;
//             if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//         }
//     }
//     public int GetOrange()
//     {
//         if (!hasInit) { return -1; }
//         return starHoneys.OrangeHoney;
//     }
//     public void AddWhite(int white)
//     {
//         if (!hasInit) { return; }
//         starHoneys.WhiteHoney += white;
//         if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//         SaveData();
//     }
//     public void UseWhite(int white, out bool isUsable)
//     {
//         if (!hasInit) { isUsable = false; return; }
//         if (white > starHoneys.YellowHoney) { isUsable = false; return; }
//         else
//         {
//             isUsable = true;
//             starHoneys.WhiteHoney -= white;
//             if (honeyChangeActions != null) { honeyChangeActions.Invoke(); }
//         }
//     }
//     public int GetWhite()
//     {
//         if (!hasInit) { return -1; }
//         return starHoneys.WhiteHoney;
//     }
// #endregion
//     private IEnumerator saveCoroutine()
//     {
//         WaitForSeconds wait = new WaitForSeconds(5f);
//         while (true)
//         {
//             SaveData();
//             yield return wait;
//         }
//     }
// }
// [System.Serializable]
// public class SHoneyData
// {
//     public double YellowHoney;
//     public int OrangeHoney;
//     public int WhiteHoney;
// }