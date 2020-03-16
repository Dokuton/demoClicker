using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleAdManager : MonoBehaviour
{
    public static GoogleAdManager Instance = null;

    private const string REWARDED_VIDEO = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd _rewardedAd;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Init()
    {
        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
    }

    public void CreateAd()
    {
        AdRequest request = new AdRequest.Builder().Build();

        _rewardedAd = new RewardedAd(REWARDED_VIDEO);
        Init();

        _rewardedAd.LoadAd(request);
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        if (_rewardedAd.IsLoaded())
            _rewardedAd.Show();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        GameManager.Instance.ContinueAfterAd();
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }
}
