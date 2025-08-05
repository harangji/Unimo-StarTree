using JetBrains.Annotations;
using TMPro;
using UnityEngine;

/// <summary>
/// ���� ���� �� ���� Ŭ���� UI�� ��Ÿ���� �� ����Ǵ� Class (GameOver �ÿ��� ������� ����)
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
            Debug.Log($"[Stage {stage}] ������ �̹� ���޵Ǿ����ϴ�.");
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
        
        Debug.Log($"[Stage {stage}] Yellow: {yellow}, Red: {red} ���� �Ϸ�");

        YellowText.text = StringMethod.ToCurrencyString(yellow);
        RedText.text = StringMethod.ToCurrencyString(red);
        
        Debug.Log("RewardedStages ::: " + string.Join(", ", Base_Manager.Data.UserData.RewardedStages));
        
        Base_Manager.Data.SaveUserData();
        Debug.Log("���� �Ϸ�");
    }
    
    /// <summary>
    /// ���� Ŭ���� UI�� ���� ��û �� 2�� ���� ��ư Ŭ�� �� ���� (���� ������ �� �� �� ���������ν� 2�� ȹ�� ó��)
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
