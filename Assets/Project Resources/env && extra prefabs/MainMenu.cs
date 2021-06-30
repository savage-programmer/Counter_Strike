using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GssAdSdk;
public class MainMenu :MonoBehaviour //GSSNetworkHandlerDelegate
{

    AndroidJavaClass exitClass;
    AndroidJavaObject exitClassObject;
    public GameObject dialogexit;
    public GameObject play, ContinueBtn, feedback,exitbt;
	public GameObject playerobj1;
    public static bool ads = false;
    public AudioClip buttonSound;
	public static bool mainmenufirst=true;
	public static bool adsalternative = true;
	bool Exitadd;

	public Text gamename;

	void Awake(){
		//GoogleAnalytics.instance.SendlogForView (GameAnalytics_Views.Main_Menu);
//		///TenlogixAds.loadStartAppSplash ("MainMenu");
//		MoPubAds.initBanner (MoPubAds._bannerAdUnitId , MoPubAdPosition.BottomCenter); // banner ini
////		MoPubAds.initBanner_smart (MoPubAds._smartbannerAdUnitId , MoPubAdPosition.TopLeft); // banner ini
//		MoPubAds.loadAd (MoPubAds._interstitialOnGpEndId);
//		MoPubAds.loadAd (MoPubAds._interstitialOnExit);
//		MoPubAds.loadAd (MoPubAds._interstitialOnSelectionId);
//		MoPubAds.initializeRewardVideo(MoPubAds._interstitialOnVideo);
//		MoPubAds.requestRewardVideo (MoPubAds._interstitialOnVideo);
	
	}

    public void Play()
    {
		
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
		if(mainmenufirst){
			mainmenufirst = false;
//			MoPubAds.showRewardVideo (MoPubAds._interstitialOnVideo);
		}
        Application.LoadLevel(Application.loadedLevel + 1);
    }

	public void Continue()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
		if(mainmenufirst){
			mainmenufirst = false;
//			MoPubAds.showRewardVideo (MoPubAds._interstitialOnVideo);
		}
		Application.LoadLevel(Application.loadedLevel + 1);
	}

     public void FeedBack()
    {

//		AGameUtils.SendFeedbackMail(); // rate us
    }


	public void Exit()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
//		MoPubAds.showAd (MoPubAds._interstitialOnExit);
		dialogexit.SetActive (true);			
		play.SetActive (false);
		feedback.SetActive(false);
		exitbt.SetActive (false);
//		playerobj2.SetActive (false);
		playerobj1.SetActive (false);
	}
    
      public void ButtonYes()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
        Application.Quit();
    }
    
    	
        public void ButtonNo()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
//        Debug.Log("quit");
        dialogexit.SetActive(false);     
        play.SetActive(true);
        feedback.SetActive(true);
		exitbt.SetActive (true);
//		playerobj2.SetActive (true);
		playerobj1.SetActive (true);
    }
  
   
		void Start ()
		{
//		gamename.text = "" + AGameUtils.PRODUCT_NAME;

		//gssadsdk
		/*
		if(!TenlogixAds.tenlogixAdsSdk_initialized)
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			NetworkHandler networkobj = new NetworkHandler(this);
			TenlogixAds.setConfig(false,"com.MARAK.RMARAK","RMARAK","1",networkobj,TenlogixAds.ScreenOrientation_Landscape);
		}
		*/
//		MoPubAds.showBanner ();	
//
//		MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
////           

		Exitadd = false;

      

		}
		void Update ()
		{

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
            GetComponent<AudioSource>().PlayOneShot(buttonSound);
                 if (!ads)
                {
//                    TenlogixAds.getExitAd("MainMenu");
					
                    ads = true;
                }

                else
                {
				
             dialogexit.SetActive (true);			
			play.SetActive (false);
            feedback.SetActive(false);
			exitbt.SetActive (false);
            ads = true;
                }
            
			
		}
			
		}

	//gssadsdk
	/*
	public override void NetworkCallFailure (string errorMsg)
	{
//		TenlogixAds.init ();
	}
	public override void NetworkCallSuccess (string data)
	{
		//gssadsdk
//		TenlogixAds.init (data);
	}
*/
       
}

