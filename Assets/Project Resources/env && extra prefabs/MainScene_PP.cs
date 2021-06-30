using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainScene_PP : MonoBehaviour {

	public static MainScene_PP instance;
	public GameObject loading;
	public Doom_Tweener Tweener;
	public GameObject PlayPanel,ExitPanel,MainMenuPanel,LevelSelection,RateUsPanal;
	public GameObject PrivacySubPanel;
	public Button ContinueBtn;

	public GameObject Playbtn, RateBtn, Morebtn, ExitBtn, PrivacyBtn,sound_On,sound_Off;

	public static bool adsalternative = false;
	public Slider loadingslider;
	public Text loadingtext;
	bool loadcheck = false;

	public void Awake()
	{
		instance = this;
		if (GlobalScripts.CurrLevelIndex >4)
		{
			GlobalScripts.CurrLevelIndex = 0;
		} 
		AudioListener.pause = false;

//		MoPubAds.initBanner (MoPubAds._bannerAdUnitId , MoPubAdPosition.BottomCenter); // banner ini
//		//		MoPubAds.initBanner_smart (MoPubAds._smartbannerAdUnitId , MoPubAdPosition.TopLeft); // banner ini
//		MoPubAds.loadAd (MoPubAds._interstitialOnGpEndId);
//		MoPubAds.loadAd (MoPubAds._interstitialOnExit);
//		MoPubAds.loadAd (MoPubAds._interstitialOnSelectionId);
//		MoPubAds.initializeRewardVideo(MoPubAds._interstitialOnVideo);
//		MoPubAds.requestRewardVideo (MoPubAds._interstitialOnVideo);
		GlobalScripts.CurrLevelIndex = PlayerPrefs.GetInt ("CurrentMission", 0);
	}
	public void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
//			Exit.SetActive (true);
//			MoPubAds.showAd (MoPubAds._interstitialOnExit);

		}


		if (loadcheck == true) {

			loadingslider.value = loadingslider.value + 0.1f * Time.deltaTime;
			int temp = Mathf.RoundToInt(loadingslider.value * 100);
			loadingtext.text = temp.ToString () + "%";
			//Debug.Log ("loadingslider = "+ loadingslider.value);

		}

		if (loadingslider.value >= 1f) {
			loadcheck = false;
			LoadTheScene ();
            
        }


	}
	public void Start()
	{

        //		MoPubAds.showBanner ();	
        //  
        //		if (GlobalScripts.ShowMainMenuAdd) 
        //		{
        //			print ("main menu add shown: "+GlobalScripts.ShowMainMenuAdd);
        //			GlobalScripts.ShowMainMenuAdd = true;
        //			MoPubAds.showAd (MoPubAds._interstitialOnSelectionId);
        //		}
        //		else
        //			print ("main menu add shown: "+GlobalScripts.ShowMainMenuAdd);
        //		AGameUtils.initAnalytics ();
        
	}
	public void musicBtnOn()
	{
		AudioListener.pause = true;
		sound_On.SetActive (false);
		sound_Off.SetActive (true);

	}
	public void musicBtnOff()
	{
		AudioListener.pause = false;
		sound_On.SetActive (true);
		sound_Off.SetActive (false);
	}
	public void ChangePlayer()
	{
		MainMenuPanel.SetActive (false);
		PlayPanel.SetActive (true);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
	}

	public void OnBackToMain()
	{
		MainMenuPanel.SetActive (true);
		LevelSelection.SetActive (false);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
        
	}

	public void OnOkay()
	{
		MainMenuPanel.SetActive (true);
		PlayPanel.SetActive (false);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
	}

	public void OnPlayButtonClicked()
	{
//		if (GlobalScripts.CurrLevelIndex > 0) 
//		{
//			ContinueBtn.interactable = true;
//		}
//		else
//		{
//			ContinueBtn.interactable = false;
//		}
		MainMenuPanel.SetActive (false);
		LevelSelection.SetActive (true);
//		loading.SetActive (true);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
        //		Invoke("startScene", 1.0f);
        //		SceneManager.LoadScene(2);
       
    }

	public void OnRateButtonClicked()
	{
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
        RateUsPanal.SetActive(true);
	}

	public void OnMoreButtonClicked()
	{
        GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
        Application.OpenURL("https://play.google.com/store/apps/developer?id=itech01");
    }

	public void startScene()
	{
		SceneManager.LoadScene(1);
        
    }
	public void OnClickYes()
	{
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();	
		Application.Quit();
	}
	public void OnExit()
	{
		ExitPanel.SetActive (true);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
        


    }
	public void OnClickNo()
	{

		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
		ExitPanel.SetActive (false);

        //		if (AdsManagerMainMenu.big_bannerView != null) {
        //			AdsManagerMainMenu.instance.hide_Big_banner ();
        //		}
        

    }
//
//	public void wait(){
//		SceneManager.LoadScene(2);
//	}

	public void OnClickContinue()
	{
		if(loading)
			loading.SetActive (true);
        
        //Invoke("LoadTheScene",10f);
        GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();

        //		SceneManager.LoadScene(2);
        
    }

	public void OnClickNewGame()
	{
		if(loading)
			loading.SetActive (true);
       


        PlayerPrefs.SetInt ("CurrentMission", 0);
		GlobalScripts.CurrLevelIndex = 0;

		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
		//if (Tenlogiclocal.Ads_purchase) {
			SceneManager.LoadScene(1);
       

        //} else {
        //	//Invoke ("LoadTheScene", 10f);
        //	loadcheck = true;
        //}

        //SceneManager.LoadScene(2);
        
    }

	public void OnClickPrivacy()
	{
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
		PrivacySubPanel.SetActive (true);
		RateBtn.SetActive (false);
		Morebtn.SetActive (false);
		PrivacyBtn.SetActive (false);
		ExitBtn.SetActive (false);
		Playbtn.SetActive (false);
	}
	public void OnClickCross()
	{
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
		PrivacySubPanel.SetActive (false);
		RateBtn.SetActive (true);
		Morebtn.SetActive (true);
		PrivacyBtn.SetActive (true);
		ExitBtn.SetActive (true);
		Playbtn.SetActive (true);
	}
	public void OnReset()
	{
		PlayerPrefs.SetInt ("CurrentMission", 0);
		GlobalScripts.CurrLevelIndex = 0;

		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
	}
	public void OnOK()
	{
		if(loading)
			loading.SetActive (true);
		Tweener.LodingCircle ();
		MainMenuPanel.SetActive (false);
		LevelSelection.SetActive (false);
		GameObject.Find ("ButtonClickSound").GetComponent<AudioSource> ().Play ();
		//if (Tenlogiclocal.Ads_purchase) {
			SceneManager.LoadScene(1);
       

        //} else {
        //	//Invoke ("LoadTheScene", 10f);
        //	loadcheck = true;
        //}
    }
	void LoadTheScene()
	{
		SceneManager.LoadScene(1);
        
    }
}
