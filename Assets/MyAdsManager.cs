using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class MyAdsManager : MonoBehaviour
{
    public string gameId = "1234567";
   public bool testMode = true;
    public static MyAdsManager instance;
    public string appID= "ca-app-pub-7209911560969800~9985938486";
    private BannerView bannerView;
    public string bannerID= "ca-app-pub-3940256099942544/6300978111";
    private InterstitialAd fullScreenAd;
    public string fullScreenAdID= "ca-app-pub-3940256099942544/1033173712";

    private RewardBasedVideoAd rewardAd;
    public string rewardedAdID="";

    // Start is called before the first frame update
    void Awake()
    {
        if(instance== null){
            instance=this;
        }
        else{
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
      //  UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        
    }
    private void Start(){
        MyAdsManager.instance.RequestBanner();
        RequestFullScreenAd();
        rewardAd= RewardBasedVideoAd.Instance;
        RequestRewardedAd();

        rewardAd.OnAdLoaded+=HandleRewardBasedVideoLoaded;
        rewardAd.OnAdFailedToLoad+=HandleRewardBasedVideoFailedToLoad;
        rewardAd.OnAdRewarded+=HandleRewardBasedVideoRewarded;
        rewardAd.OnAdClosed+=HandleRewardBasedVideoClosed;

        Advertisement.Initialize(gameId, testMode);

        
    }

    public void RequestBanner(){
        bannerView=new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request= new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        bannerView.Show();
    }

   public void HideBanner(){
       bannerView.Hide();
   }
   public void RequestFullScreenAd(){
       fullScreenAd=new InterstitialAd(fullScreenAdID);
       AdRequest request=new AdRequest.Builder().Build();
       fullScreenAd.LoadAd(request);
       
   }

   public void ShowFullScreenAd(){
      if(fullScreenAd.IsLoaded()){

          fullScreenAd.Show();

      } 
      else{
          Advertisement.Show();
          Debug.Log("full screen ad not loaded");
      }
        RequestFullScreenAd();
   }

   public void RequestRewardedAd(){
       AdRequest request=new AdRequest.Builder().Build();
       rewardAd.LoadAd(request, rewardedAdID);

   }
   public void ShowRewardedAd(){
       if(rewardAd.IsLoaded()){
           rewardAd.Show();
       }
       else {
          Advertisement.Show();
           Debug.Log("rewarded ad not loaded");
       }

   }

   public void HandleRewardBasedVideoLoaded(object sender, EventArgs args ){

       Debug.Log("rewarded video ad loaded successfully");

   }
    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args ){

       Debug.Log("failed to load rewarded video ad" +args.Message);

   }
     public void HandleRewardBasedVideoRewarded(object sender, Reward args ){
         string type=args.Type;
         double amount=args.Amount;
         Debug.Log("You have been Rewarded with "+ amount.ToString() + "" + type);
        // MenuManager.instance.recieveRewardDlg.SetActive(true);
        // MenuManager.instance.watchVideoDlg.SetActive(false);
        // MenuManager.instance.TestFunction();
        // show reward video panel here
        // then disable other panels
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
     // MenuManager.Instance.coinTextMenu.text=" "+PlayerPrefs.GetInt("Coins");
    }
    public void HandleRewardBasedVideoClosed(object sender, EventArgs args ){
         Debug.Log("reward video has closed");
         RequestRewardedAd();

   }

}
