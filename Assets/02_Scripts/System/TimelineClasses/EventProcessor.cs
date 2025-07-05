using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventProcessor : MonoBehaviour
{
    [SerializeField] private PlayableDirector masterPlayable;
    [SerializeField] private EventTextController textCtrl;
    [SerializeField] private List<int> pauseTxts;
    private int currentQueue = 0;
    private bool isPrintEventTxt = false;
    private bool waitTxtEnd = false;

    private void Update()
    {
        if(waitTxtEnd)
        {
            CheckTxtGoal();
        }
    }
    public void CallNextText()
    {
        if (isPrintEventTxt) { textCtrl.CallNextText(); }
    }
    public void StartPrint()
    {
        isPrintEventTxt = true;
        textCtrl.StartPrint();
    }
    public void StopPrint()
    {
        currentQueue++;
        isPrintEventTxt = false;
        textCtrl.StopPrint();
    }
    public void CheckTxtGoal()
    {
        if (textCtrl.GetCurrentIdx() <= pauseTxts[currentQueue])
        {
            masterPlayable.Pause();
            waitTxtEnd = true;
        }
        else
        {
            waitTxtEnd = false;
            restartEvent();
        }
    }
    private void restartEvent()
    {
        masterPlayable.Play();
    }
}
