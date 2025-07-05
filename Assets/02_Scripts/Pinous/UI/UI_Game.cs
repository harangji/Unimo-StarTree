using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Game : UI_Base
{
    public TextMeshProUGUI GameOneBest, GameTwoBest;
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.InGame);
        GameOneBest.text = "Best Score\n" + StringMethod.ToCurrencyString(Base_Mng.Data.data.BestScoreGameOne);
        GameTwoBest.text = "Best Score\n" + StringMethod.ToCurrencyString(Base_Mng.Data.data.BestScoreGameTwo);
        base.Start();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

    public void GoGameScene(int value)
    {
        WholeSceneController.Instance.ReadyNextScene(value);
        Base_Mng.Data.data.GamePlay++;
        Pinous_Flower_Holder.FlowerHolder.Clear();
        //Base_Mng.ADS._interstitialCallback = () =>
        //{
           
        //};
        //Base_Mng.ADS.ShowInterstitialAds();
    }
}
