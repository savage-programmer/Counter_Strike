using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// MoPubAds Callings\
/// 
/// Banner : MoPubAds.showBanner(); /// MoPubAds.hideBanner ();
/// 
/// Selection Add: MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);
/// 
/// GP end : MoPubAds.showAd(MoPubAds._interstitialOnGpEndId);
/// 
/// Reward Video: MoPubAds.showRewardVideo(MoPubAds._interstitialOnVideo);
/// 
/// as Templates
public class GameDialogs_GJ11 : MonoBehaviour
{
	public static GameDialogs_GJ11 instance;
	public static bool rateUsFlag;
    public GameObject successPanel, failedPanel, pausedPanel, RateUsPanel, GameControls;
	public static bool isGameOver;
    public static bool ads = false;
	public GameObject PauseBtn;
	public Canvas mobileControls;
    public AudioClip lvlComplete, lvlFail,buttonSound;  
    bool diaCalled;
	private bool levelStart;


	void Awake() 
	{
		instance = this;
	}

	void Start ()
	{
		isGameOver = false;
		diaCalled = false;
		levelStart = false;
		rateUsFlag = false;
	}

	public void ClearUI()
	{
		if(mobileControls != null)
			mobileControls.enabled =  (false);
		PauseBtn.SetActive (false);
		if (GameControls != null) 
		{
			GameControls.SetActive (false);
		}
	}

	public void ShowUI()
	{
		if(mobileControls != null)
			mobileControls.enabled =  (true);
		PauseBtn.SetActive (true);
		if (GameControls != null) 
		{
			GameControls.SetActive (true);
		}
	}

	public void PlayGame()
	{
	//	MoPubAds.showAd(MoPubAds._interstitialOnGpEndId);
	}
	public void HideBanner()
	{
//		MoPubAds.hideBanner ();
	}

	public void ShowAdd ()
	{
        //		MoPubAds.showBanner();
        //
        //		if (MainMenu.adsalternative) 
        //		{
        //			MainMenu.adsalternative = false;
        //			MoPubAds.showAd(MoPubAds._interstitialOnSelectionId);	
        ////		} 
        ////		else 
        //		{
        //			MainMenu.adsalternative = true;
 
        //	}

        //		PauseBtn.SetActive (false);
    }

	public void Dia_Success ()
	{
//		if (!diaCalled) 
//		{
//			diaCalled = true;
			isGameOver = true;
			ClearUI ();
			GetComponent<AudioSource>().PlayOneShot(lvlComplete);
//			GameManager.instance.DebugTxt.text = "on success dialog";
			successPanel.SetActive (true);

			GlobalScripts.CurrLevelIndex = GlobalScripts.CurrLevelIndex + 1;
			PlayerPrefs.SetInt ("CurrentMission", GlobalScripts.CurrLevelIndex);

			//			GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().clip = lvlComplete;
			//            GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().Play();
			if ((GlobalScripts.CurrLevelIndex) <= PlayerPrefs.GetInt ("Tank1"))
			{
				PlayerPrefs.SetInt ("Tank1", GlobalScripts.CurrLevelIndex + 1);
			}
//			if (PlayerPrefs.GetInt ("LevelOpen") < SceneManager.GetActiveScene().buildIndex+1)
//			{
//				PlayerPrefs.SetInt ("LevelOpen",SceneManager.GetActiveScene().buildIndex);
//			}

			PauseBtn.SetActive(false);

		if (GameDialogs_GJ11.rateUsFlag)
			{
			GameDialogs_GJ11.rateUsFlag = false;
			}
			else
			{
				ShowAdd ();
			}
			Time.timeScale = 0;
//		}
	}

	public void Dia_Resume()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
		PauseBtn.SetActive(true);
		ShowUI ();

		if(GameControls != null)
			GameControls.SetActive(true);

		pausedPanel.SetActive (false);
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
 
	}
	public void Dia_Restart_Custom()
	{
		if(Camera.main.GetComponent<AudioSource> ()!=null)
			Camera.main.GetComponent<AudioSource> ().Stop ();
		GetComponent<AudioSource>().Stop();
		failedPanel.SetActive (false);
		if(GameControls != null)
			GameControls.SetActive(true);
		ShowUI ();
		PauseBtn.SetActive (true);

		Time.timeScale = 1;
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
 	}
	public void Dia_Failed ()
	{
		if(Camera.main.GetComponent<AudioSource> ()!=null)
			Camera.main.GetComponent<AudioSource> ().Stop ();
		if (!diaCalled) 
		{
			diaCalled = true;
			isGameOver = true;
			GetComponent<AudioSource>().PlayOneShot (lvlFail);

			if(GameControls != null)
				GameControls.SetActive(false);
			ClearUI();
			failedPanel.SetActive (true);

			ShowAdd ();

			PauseBtn.SetActive (false);
			Time.timeScale = 0;

		}
//		MoPubAds.showBanner();
	}

	public void Dia_Paused ()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
		Time.timeScale = 0;
		if(GameControls != null)
			GameControls.SetActive(false);
		pausedPanel.SetActive (true);
		ClearUI ();

     }


	public void Dia_TimesUp ()
	{
		PlayerPrefs.SetInt ("InventoryStoredToLoad", 0);
		StopAllCoroutines ();
		Time.timeScale=0;

		if(GameControls != null)
			GameControls.SetActive(false);

		PauseBtn.SetActive(false);
		ClearUI ();
		ShowAdd ();
		failedPanel.SetActive (true);
	}
	public void Dia_Rate ()
	{
		RateUsPanel.SetActive (true);

	}
	//---------------------------------------------------------------
	public void Btn_Main ()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
 	}

	public void Btn_Restart ()
	{
		PlayerPrefs.SetInt ("InventoryStoredToLoad", 0);

		//		MoPubAds.hideBanner ();
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
		Time.timeScale = 1;  
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
 	}

	public void Btn_Next ()
	{
		GetComponent<AudioSource>().PlayOneShot(buttonSound);
		Time.timeScale = 1;
		PauseBtn.SetActive(true); 
		successPanel.SetActive (false);
		ShowUI ();
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
 	}

	public void Btn_Resume ()
	{
         GetComponent<AudioSource>().PlayOneShot(buttonSound);
		PauseBtn.SetActive(true);    
		Time.timeScale = 1;
		ShowUI ();
//		if (AdsManagerMainMenu.big_bannerView != null) {
//			AdsManagerMainMenu.instance.hide_Big_banner ();
//		}
		
		if(GameObject.FindWithTag("MainCamera") != null && GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>()!=null)
			GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().Play();
		//		MoPubAds.hideBanner ();
	}
}
