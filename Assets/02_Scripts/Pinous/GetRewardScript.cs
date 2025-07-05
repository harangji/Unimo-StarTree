using TMPro;
using UnityEngine;

public class GetRewardScript : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    public TextMeshProUGUI YellowText, RedText;
    public TextMeshProUGUI ScoreText;

    public GameObject ADSButton;
    private void Start()
    {
        ADSButton.SetActive(true);

        Base_Mng.Data.data.Red += double.Parse(texts[0].text.ToString());
        Base_Mng.Data.data.Yellow += StringMethod.ToCurrencyDouble(texts[1].text);

        YellowText.text = texts[1].text;
        RedText.text = texts[0].text;
    }

    public void GetRewardDoubleScore()
    {
        Base_Mng.ADS.ShowRewardedAds(() =>
        {
            Base_Mng.Data.data.Red += double.Parse(texts[0].text);
            Base_Mng.Data.data.RePlay++;
            var yellow = StringMethod.ToCurrencyDouble(texts[1].text);
            Base_Mng.Data.data.Yellow += yellow;

            RedText.text = (double.Parse(texts[0].text) * 2).ToString();
            YellowText.text = StringMethod.ToCurrencyString(yellow * 2);

            ADSButton.SetActive(false);
        });
    }
}
