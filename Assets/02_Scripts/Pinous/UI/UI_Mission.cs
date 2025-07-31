using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Mission_State { Daily, Achievements } //Daily : 미션, Achievements : 업적

public class UI_Mission : UI_Base
{
    public Mission_State m_State;
    public Mission_Base[] m_Mission;
    public Mission_Base[] m_TrophyBase;
    public Transform Content;

    public GameObject DailyObject, TrophyObject;
    public Image DailyImage, TrophyImage;
    public Outline DailyBar, TrophyBar;

    public List<Dictionary<string, object>> Data = new List<Dictionary<string, object>>();
    public override void Start()
    {
        Data = CSVReader.Read("Mission");

        GetMission(Mission_State.Daily);
        base.Start();
    }

    public void GetButton(bool Daily)
    {
        DailyObject.SetActive(Daily);
        TrophyObject.SetActive(!Daily);
        DailyBar.enabled = Daily;
        TrophyBar.enabled = !Daily;
        GetMission(Daily ? Mission_State.Daily : Mission_State.Achievements); //미션, 업적 판별
    }

    

    public void GetMission(Mission_State state)
    {
        int a = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i]["Type"].ToString() == state.ToString())
            {
                if (state == Mission_State.Daily)
                {
                    var reward = Data[i]["Reward"].ToString().Split("_");
                    m_Mission[i].Init(
                        Data[i]["Style"].ToString(), 
                        int.Parse(Data[i]["Count"].ToString()), 
                        int.Parse(reward[1]), 
                        stateCheck(reward[0]));
                }
                else if (state == Mission_State.Achievements)
                {
                    string rewardRaw = Data[i]["Reward"].ToString();
                    var rewardSplit = rewardRaw.Split('_');

                    if (rewardSplit.Length == 2)
                    {
                        // 별꿀 보상: B_50, O_500
                        int rewardAmount = int.Parse(rewardSplit[1]);
                        Asset_State rewardType = stateCheck(rewardSplit[0]);
                        m_TrophyBase[a].Init(
                            Data[i]["Style"].ToString(),
                            int.Parse(Data[i]["Count"].ToString()),
                            a,
                            rewardType,
                            rewardAmount
                        );
                    }
                    else
                    {
                        // 캐릭터/장비 보상: DoomDoom, GomGom 등
                        m_TrophyBase[a].Init(
                            Data[i]["Style"].ToString(),
                            int.Parse(Data[i]["Count"].ToString()),
                            a,
                            Asset_State.Blue // or 무시됨
                        );
                    }

                    a++;
                }
            }
        }
    }

    private Asset_State stateCheck(string temp)
    {
        switch(temp)
        {
            case "Y": return Asset_State.Yellow;
            case "O": return Asset_State.Red;
            case "B": return Asset_State.Blue;
        }
        return Asset_State.Yellow;
    }

}
