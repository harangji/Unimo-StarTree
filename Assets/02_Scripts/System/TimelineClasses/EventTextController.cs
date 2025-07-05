using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;
    [SerializeField] private List<string> textKeys;
    private int currentTxt;
    private float waitTime = 0f;
    private float autoNextTime = 4f;
    private float currentNextTime = 2f;

    private void Start()
    {
        currentNextTime = autoNextTime;
    }
    private void Update()
    {
        if (waitTime > 0f) { waitTime -=  Time.deltaTime; }
        if (currentNextTime > 0f) 
        { 
            currentNextTime -= Time.deltaTime; 
            if(!(currentNextTime > 0f))
            {
                CallNextText();
            }
        }
    }
    public void StartPrint()
    {
        textObj.gameObject.SetActive(true);
        CallNextText();
    }
    public void StopPrint()
    {
        textObj.gameObject.SetActive(false);
    }
    public int GetCurrentIdx()
    {
        return currentTxt;
    }
    public void CallNextText()
    {
        if (waitTime > 0f) { return; }
        if (currentTxt < textKeys.Count)
        {
            textObj.text = textKeys[currentTxt];
            currentTxt++;
            waitTime = 0.5f;
            currentNextTime = autoNextTime;
        }
    }
}
