using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Game : UI_Base
{
    public TextMeshProUGUI GameOneBest, GameTwoBest;
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.InGame);
        GameOneBest.text = "Best Score\n" + StringMethod.ToCurrencyString(Base_Manager.Data.UserData.BestScoreGameOne);
        GameTwoBest.text = "Best Score\n" + StringMethod.ToCurrencyString(Base_Manager.Data.UserData.BestScoreGameTwo);
        base.Start();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

    public void GoGameScene(int value)
    {
        WholeSceneController.Instance.ReadyNextScene(value);
        Base_Manager.Data.UserData.GamePlay++;
        Base_Manager.Data.UserData.GamePlaySum++;
        Pinous_Flower_Holder.FlowerHolder.Clear();
        //Base_Mng.ADS._interstitialCallback = () =>
        //{
           
        //};
        //Base_Mng.ADS.ShowInterstitialAds();
    }
}
