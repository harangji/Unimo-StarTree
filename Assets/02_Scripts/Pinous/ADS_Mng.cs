using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS_Mng
{
    public enum AdsStateType
    {
        None,
        Failed,
        Success
    }

    bool TEST_MODE = false;

    InterstitialAd _interstitialAd;
    RewardedAd _rewardedAd;
    BannerView _bannerView;
    Action _rewardedCallback;
    public Action _interstitialCallback;
    AdRequest _adRequest;
    public Rect bannerRect;
    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareAds();
        BannerView();
    }

    // 실제 출시하기 전에 테스트로 사용하는 ID들.
    // 출시 전에 실제 ID 박으면 정지 사유가 되니 조심하자.
    const string TEST_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    const string TEST_ANDROID_INTERSTITIAL = "ca-app-pub-3940256099942544/1033173712";
    const string TEST_ANDROID_REWARDED = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_ANDROID_BANNER = "ca-app-pub-3940256099942544/6300978111";
    const string TEST_IOS_INTERSTITIAL = "ca-app-pub-3940256099942544/4411468910";
    const string TEST_IOS_REWARDED = "ca-app-pub-3940256099942544/1712485313";
    const string TEST_IOS_BANNER = "ca-app-pub-3940256099942544/2934735716";
    // 실제 출시용 ID들

    public void PrepareAds()
    {
#if UNITY_ANDROID
        string interstitial = "ca-app-pub-6288959995887297/7488940996"; // Android_Interstitial
        string rewarded = "ca-app-pub-6288959995887297/1982497285"; // Android_Rewarded
        string interstitialTest = TEST_ANDROID_INTERSTITIAL;
        string rewardedTest = TEST_ANDROID_REWARDED;
#else
        string interstitial = "ca-app-pub-6288959995887297/6624824574"; // IOS_Interstitial
        string rewarded = "ca-app-pub-6288959995887297/1352316595"; // IOS_Rewarded
		string interstitialTest = TEST_IOS_INTERSTITIAL;
		string rewardedTest = TEST_IOS_REWARDED;
#endif

        // create our request used to load the ad.
        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(TEST_MODE ? interstitialTest : interstitial, _adRequest,
          OnAdLoadCallback);
        RewardedAd.Load(TEST_MODE ? rewardedTest : rewarded, _adRequest, OnAdRewardCallback);
    }

    public void BannerView()
    {
        if (Base_Mng.Data.data.ADSBuy == true) return;

        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }

        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        string bannerADS = "";
#if UNITY_ANDROID
        bannerADS = "ca-app-pub-6288959995887297/3310885236";
#elif UNITY_IOS
        bannerADS = "ca-app-pub-6288959995887297/8935191510";
#endif

        _bannerView = new BannerView(TEST_MODE ? TEST_ANDROID_BANNER : bannerADS, adaptiveSize, AdPosition.Bottom);
        _bannerView.LoadAd(_adRequest);
    }

    public void BannerDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }

    private void OnAdLoadCallback(InterstitialAd ad, LoadAdError error)
    {
        // if error is not null, the load request failed.
        if (error != null || ad == null)
        {
            Debug.LogError("interstitial ad failed to load an ad " +
                           "with error : " + error);
            return;
        }

        Debug.Log("Interstitial ad loaded with response : "
                  + ad.GetResponseInfo());

        _interstitialAd = ad;
        RegisterReloadHandler(_interstitialAd);
    }

    private void OnAdRewardCallback(RewardedAd ad, LoadAdError error)
    {
        // if error is not null, the load request failed.
        if (error != null || ad == null)
        {
            Debug.LogError("Rewarded ad failed to load an ad " +
                           "with error : " + error);
            return;
        }

        Debug.Log("Rewarded ad loaded with response : "
                  + ad.GetResponseInfo());
        _rewardedAd = ad;

        RegisterEventHandlers(_rewardedAd);
    }

    private void RegisterReloadHandler(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");
            //if(_interstitialCallback != null)
            //{
            //    _interstitialCallback?.Invoke();
            //    _interstitialCallback = null;
            //}
            // Reload the ad so that we can show another as soon as possible.
            PrepareAds();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            PrepareAds();
        };
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            PrepareAds();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            PrepareAds();
        };
        ad.OnAdPaid += (AdValue adValue) =>
        {
            //Base_Mng.instance.Coroutine_Starter(GetReward());
        };
    }

    IEnumerator GetReward()
    {
        yield return new WaitForSeconds(0.3f);

        _rewardedCallback?.Invoke();
        Base_Mng.Data.data.ADS++;
        Base_Mng.Data.data.ADSNoneReset++;

        _rewardedCallback = null;
    }

    public void ShowInterstitialAds()
    {

        if (Base_Mng.Data.data.ADSBuy || Base_Mng.Data.data.ADS_Inter_Buy)
        {
            if (_interstitialCallback != null)
            {
                _interstitialCallback?.Invoke();
                _interstitialCallback = null;
            }
            return;
        }

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
            _interstitialAd.Show();
        else
        {
            Canvas_Holder.instance.Get_Toast("NoADS");
            PrepareAds();
        }
    }

    public void ShowRewardedAds(Action rewardedCallback)
    {
        const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
        _rewardedCallback = rewardedCallback;

        if (Base_Mng.Data.data.ADSBuy)
        {
            _rewardedCallback?.Invoke();
            Base_Mng.Data.data.ADS++;
            Base_Mng.Data.data.ADSNoneReset++;
            PlayerPrefs.SetInt("GetRewardCount", 2);
            _rewardedCallback = null;
            return;
        }

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
            _rewardedAd.Show((Reward reward) =>
            {
                Base_Mng.instance.Coroutine_Starter(GetReward());
            });
        else
        {
            Canvas_Holder.instance.Get_Toast("NoADS");
            PrepareAds();
        }
    }
}
