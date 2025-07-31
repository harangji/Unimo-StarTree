using JetBrains.Annotations;
using TMPro;
using UnityEngine;

/// <summary>
/// 게임 종료 후 게임 클리어 UI가 나타났을 때 실행되는 Class (GameOver 시에는 실행되지 않음)
/// </summary>
public class GetRewardScript : MonoBehaviour
{
    // public TextMeshProUGUI[] texts;
    public TextMeshProUGUI YellowText, RedText;
    // public TextMeshProUGUI ScoreText;

    [CanBeNull] public GameObject ADSButton;

    private double yellow, red;
    private int stage;
    
    private void Start()
    {
        if(ADSButton != null) ADSButton.SetActive(true);
        
        stage = StageLoader.CurrentStageNumber;
        
        if (Base_Manager.Data.UserData.RewardedStages.Contains(stage))
        {
            Debug.Log($"[Stage {stage}] 보상은 이미 지급되었습니다.");
            return;
        }

        // Base_Manager.Data.UserData.Red += double.Parse(texts[0].text.ToString());
        // Base_Manager.Data.UserData.Yellow += StringMethod.ToCurrencyDouble(texts[1].text);
        //
        // YellowText.text = texts[1].text;
        // RedText.text = texts[0].text;
        
        yellow = RewardCalculator.GetYellowReward(stage);
        red = RewardCalculator.GetRedReward(stage);

        Base_Manager.Data.UserData.Yellow += yellow;
        Base_Manager.Data.UserData.Red += red;
        Base_Manager.Data.UserData.RewardedStages.Add(stage);
        
        Debug.Log($"[Stage {stage}] Yellow: {yellow}, Red: {red} 지급 완료");
        
        YellowText.text = yellow.ToString();
        RedText.text = red.ToString();
        
        Debug.Log("RewardedStages ::: " + string.Join(", ", Base_Manager.Data.UserData.RewardedStages));
        
        Base_Manager.Data.SaveUserData();
        Debug.Log("저장 완료");
    }
    
    /// <summary>
    /// 게임 클리어 UI의 광고 시청 후 2배 보상 버튼 클릭 시 실행 (동일 보상을 한 번 더 지급함으로써 2배 획득 처리)
    /// </summary>
    public void GetRewardDoubleScore()
    {
        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            Base_Manager.Data.UserData.RePlay++;
            Base_Manager.Data.UserData.Yellow += yellow;
            Base_Manager.Data.UserData.Red += red;
            
            YellowText.text = (yellow * 2).ToString();
            RedText.text = (red * 2).ToString();
            
            // Base_Manager.Data.UserData.Red += double.Parse(texts[0].text);
            // var yellow = StringMethod.ToCurrencyDouble(texts[1].text);
            // Base_Manager.Data.UserData.Yellow += yellow;

            // RedText.text = (double.Parse(texts[0].text) * 2).ToString();
            // YellowText.text = StringMethod.ToCurrencyString(yellow * 2);

            ADSButton.SetActive(false);
        });
    }
}
