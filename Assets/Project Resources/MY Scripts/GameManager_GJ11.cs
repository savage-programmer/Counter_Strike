using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityStandardAssets.Vehicles.Car;
 public enum GameStates
{
	None,
	Start,
	Paused
};
public class GameManager_GJ11 : MonoBehaviour
{
	public static GameManager_GJ11 instance;
//	[HideInInspector]
	public GameStates State;

	public GameObject PlayButton;
	public GameObject CAdd;
	public float TimeDelay;
	bool GamePlayed;
	float time;
	public GameObject Missionhandler;
	public GameObject obj1;
	public GameObject obj2;
	public Transform position1; 

	public GameObject GameControls,player;
	public Canvas MobileControls;
    public LevelIntro onstart;
	public int CountDeadAI;
	public Text Failed;
	public Text OnSuccesTxt;
	public Text GamePlayStartTxt;

	public Text MC;
	void Awake()
	{
		instance = this;
        State = GameStates.Paused;
        //Instantiate (Env, position.transform.position, position.transform.rotation);
    }
	// Use this for initialization
	void Start () 
	{
 
        //	if(Camera.main.GetComponent<AudioSource> () != null)
        //		Camera.main.GetComponent<AudioSource> ().Stop ();
        //GameManager.instance.StartPlay ();
        //ObjectiveHandler.instance.InitObjeiveMech ();
        //		Time.timeScale = 1;
        //	PlayButton.SetActive (true);

        State = GameStates.Paused;

	}
	public GameObject Tutorial;
	public void TutorialPlay()
	{
		Tutorial.SetActive (true);
	}
	public void StartPlay()
	{
		print("start play");
	//	PlayButton.SetActive (true);

		if (GlobalScripts.CurrLevelIndex==0)
			PlayButton.SetActive (false);
//		Time.timeScale = 0f;
		State = GameStates.Start;


	}
//	 Update is called once per frame
//	void Update () 
//	{
//	}
	public void closeCAdds()
	{
		if(GameDialogs_GJ11.instance.mobileControls != null)
			GameDialogs_GJ11.instance.mobileControls.enabled = (true);
		GameControls.SetActive (true);
		CAdd.SetActive (false);

		GameDialogs_GJ11.instance.PauseBtn.SetActive (true);
		Time.timeScale = 1f;
	}
	public void ShowAltAdds()
	{
		GameDialogs_GJ11.instance.ShowAdd ();
	}
	public void StartGame()
	{
		StartTheGamePlay ();
	}
	void StartTheGamePlay()
	{
		Time.timeScale = 1f;
		GameDialogs_GJ11.instance.ShowUI ();
		Missionhandler.GetComponent<MissionController> ().selectlevel ();
		GamePlayed = true;
		State = GameStates.Start;
		//Missionhandler.gameObject.GetComponent<ObjectiveHandler>().SwitchPlayerPosition (position1);

		if (GlobalScripts.CurrLevelIndex ==2)
		{
			player.GetComponent<ThirdPersonCover.CharacterHealth> ().Health = 1500f;
			player.GetComponent<ThirdPersonCover.CharacterHealth>().MaxHealth = 1500f;
			print ("health = 1500");
		}
		if (GlobalScripts.CurrLevelIndex ==4)

		{       player.GetComponent<ThirdPersonCover.CharacterHealth> ().Health = 3500f;
			player.GetComponent<ThirdPersonCover.CharacterHealth>().MaxHealth  = 3500f;
			print ("health = 4000");
		}

        if (GlobalScripts.CurrLevelIndex == 1)
        {
            player.GetComponent<ThirdPersonCover.CharacterHealth>().Health = 3000f;
            player.GetComponent<ThirdPersonCover.CharacterHealth>().MaxHealth = 3000f;
            player.GetComponent<ThirdPersonCover.CharacterHealth>().Regeneration = 5f;
            print("health = 2000");
        }

    }
	public void OnFailAfter(float time)
	{
		Invoke ("OnFail",time);
	}
	public void OnFail()
	{
		MobileControls.enabled =  (false);
		 
		GameDialogs_GJ11.instance.Dia_Failed ();

		State = GameStates.Paused;
	}
	public void OnSuccess()
	{
		State = GameStates.Paused;

		 

		OnSuccesTxt.text = Missionhandler.GetComponent<MissionController> ().GetMissionText(GlobalScripts.CurrLevelIndex + 1);
		if(PlayerPrefs.GetInt ("Unlocked",0)<= GlobalScripts.CurrLevelIndex)
			PlayerPrefs.SetInt ("Unlocked",PlayerPrefs.GetInt ("Unlocked",0)+1);
		GameDialogs_GJ11.instance.Dia_Success ();
	}
	public void OnSuccessAfter()
	{
		MobileControls.enabled =  (false);
		GameDialogs_GJ11.instance.PauseBtn.SetActive (false);
		if (GlobalScripts.CurrLevelIndex == 2 && PlayerPrefs.GetInt ("RateSeen", 0) < 2) {
			if (PlayerPrefs.GetInt ("Rated", 0) == 1) {
				Invoke ("OnSuccess", 5f);
			} else {
				print ("rateShow");

				Invoke ("OnSuccessRate", 5f);

			}
		} else if (GlobalScripts.CurrLevelIndex == 5 && PlayerPrefs.GetInt ("RateSeen", 0) < 2) {
			if (PlayerPrefs.GetInt ("Rated", 0) == 1) {
				Invoke ("OnSuccess", 5f);
			} else {
				Invoke ("OnSuccessRate", 5f);

			}
		}
		else 
		{
//			DebugTxt.text = "on success after 5";
			Invoke ("OnSuccess", 5f);
		}

	}
	public GameObject ContinueBtn;
	/// <summary>
	/// Raises the success after event.
	/// </summary>
	/// <param name="NoNext">If set to <c>true</c> no next.</param>
	public void OnSuccessAfter(bool NoNext)
	{
		if (NoNext) 
		{
			GlobalScripts.CurrLevelIndex = GlobalScripts.CurrLevelIndex - 1;
//			PlayerPrefs.SetInt ("CurrentMission", GlobalScripts.CurrLevelIndex);
			ContinueBtn.SetActive (false);
		}

		MobileControls.enabled =  (false);
		GameDialogs_GJ11.instance.PauseBtn.SetActive (false);
		//		Invoke ("OnSuccess", time);
//		if (GlobalScripts.CurrLevelIndex == 2 && PlayerPrefs.GetInt ("RateSeen", 0) < 2) {
//			if (PlayerPrefs.GetInt ("Rated", 0) == 1) {
//				Invoke ("OnSuccess", 5f);
//			} else {
//				print ("rateShow");
//
//				Invoke ("OnSuccessRate", 5f);
//
//			}
//		} else if (GlobalScripts.CurrLevelIndex == 5 && PlayerPrefs.GetInt ("RateSeen", 0) < 2) {
//			if (PlayerPrefs.GetInt ("Rated", 0) == 1) {
//				Invoke ("OnSuccess", 5f);
//			} else {
//				Invoke ("OnSuccessRate", 5f);
//
//			}
//		}
//		else 
//		{
			//			DebugTxt.text = "on success after 5";
			Invoke ("OnSuccess", 5f);
//		}

	}
	public void OnRated()
	{
		GameDialogs_GJ11.rateUsFlag = true;
		 
		GameDialogs_GJ11.instance.RateUsPanel.SetActive (false);
		PlayerPrefs.SetInt ("Rated",1);
 		GameDialogs_GJ11.instance.Dia_Success ();
		//		SceneManager.LoadScene (1);
	}
	public void OnLater()
	{
		 
		GameDialogs_GJ11.instance.RateUsPanel.SetActive (false);
//		GameDialogs.instance.Dia_Success ();
		OnSuccess();
		PlayerPrefs.SetInt ("RateSeen",PlayerPrefs.GetInt("RateSeen",0)+1);
	}
	public void OnSuccessRate()
	{
		 
		GameDialogs_GJ11.instance.Dia_Rate ();
	}
	public void OnTapPause()
	{
		GameControls.SetActive (false);
		MobileControls.enabled =  (false);
		Time.timeScale = 0;
		State = GameStates.Paused;
		GameDialogs_GJ11.instance.Dia_Paused ();
	}
	public void OnTapResume()
	{
		Time.timeScale = 1;
		GameDialogs_GJ11.instance.Dia_Resume ();
		State = GameStates.Start;
 

	}
	public void OnTapRestart()
	{
		Time.timeScale = 1;
		GameDialogs_GJ11.instance.Btn_Restart ();
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	public void RestartOnFail()
	{
//		if(!GameObject.FindObjectOfType<ThirdPersonCover.PlayerController> ().GetComponent<ThirdPersonCover.CharacterMotor>().IsAlive)
//		{
//		if (ObjectiveHandler.instance.currentLevel == 0 && ObjectiveHandler.instance.Progress == 0)
//		{
//			
//		}
//		else
		if (ObjectiveHandler.instance.currentLevel == 0 && ObjectiveHandler.instance.Progress == 1) 
		{
			print ("here");
			GameDialogs_GJ11.instance.Btn_Restart ();
			MobileControls.enabled =  (false);
			GameDialogs_GJ11.instance.Dia_Restart_Custom ();
			GameObject.FindObjectOfType<ThirdPersonCover.PlayerController> ().GetComponent<ThirdPersonCover.CharacterMotor> ().ReSpawn ();
			Time.timeScale = 1;
			State = GameStates.Start;
		}
		else if (ObjectiveHandler.instance.currentLevel == 0 && ObjectiveHandler.instance.Progress == 2)
		{
			GameDialogs_GJ11.instance.Btn_Restart ();
			MobileControls.enabled =  (false);
			GameDialogs_GJ11.instance.Dia_Restart_Custom ();
			GameObject.FindObjectOfType<ThirdPersonCover.PlayerController> ().GetComponent<ThirdPersonCover.CharacterMotor> ().ReSpawn ();
			Time.timeScale = 1;
			State = GameStates.Start;
		}
		else
		{
			Time.timeScale = 1;
			GameDialogs_GJ11.instance.Btn_Restart ();
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
	}
	public void OnTapRestartAfterSuccess()
	{
		GlobalScripts.CurrLevelIndex = GlobalScripts.CurrLevelIndex - 1;
		PlayerPrefs.SetInt ("CurrentMission", GlobalScripts.CurrLevelIndex);

		Time.timeScale = 1;
		GameDialogs_GJ11.instance.Btn_Restart ();
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	public void OnTapMain ()
	{
		GameDialogs_GJ11.instance.Btn_Main ();

		SceneManager.LoadScene(0);
		Time.timeScale = 1f;
	}
	public void OnTapNext ()
	{
		print ("next_0");
		//GameDialogs.instance.Btn_Next ();
		//if (GlobalScripts.CurrLevelIndex >= MissionController.instance.EnemyinLevels.Length)
		if (GlobalScripts.CurrLevelIndex >4)
		{
			GlobalScripts.CurrLevelIndex = 0;
		} 

		if (GlobalScripts.CurrLevelIndex ==1)
		{
			player.GetComponent<ThirdPersonCover.CharacterHealth> ().Health = 3000f;
			player.GetComponent<ThirdPersonCover.CharacterHealth>().MaxHealth = 3000f;
            player.GetComponent<ThirdPersonCover.CharacterHealth>().Regeneration = 5f;
            print ("health = 2000");
		}
		if (GlobalScripts.CurrLevelIndex ==4)
			
		{       player.GetComponent<ThirdPersonCover.CharacterHealth> ().Health = 3000f;
			player.GetComponent<ThirdPersonCover.CharacterHealth>().MaxHealth  = 3000f;
			print ("health = 2500");
		} 
		Time.timeScale = 0;
		print ("next_1");
//		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		GameDialogs_GJ11.instance.successPanel.SetActive (false);
		GameObject.FindObjectOfType<ThirdPersonCover.PlayerController> ().GetComponent<ThirdPersonCover.CharacterHealth> ().Health = 
			GameObject.FindObjectOfType<ThirdPersonCover.PlayerController> ().GetComponent<ThirdPersonCover.CharacterHealth> ().MaxHealth;
	     Missionhandler.GetComponent<MissionController> ().selectlevel ();
		CountDeadAI = 0;

		Missionhandler.GetComponent<MissionController> ().selectlevelIntro();
		Missionhandler.GetComponent<MissionController> ().selectlevelstart ();
		print ("next_2");
		GameObject.FindGameObjectWithTag("camcontrol").GetComponent<camcontroller>().currentLevel=GlobalScripts.CurrLevelIndex;
		if (GlobalScripts.CurrLevelIndex == 0) {
			GameObject.FindWithTag ("camcontrol").GetComponent<camcontroller> ().swichposition();

		}

        //onstart.EndIt ();
         SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt ("InventoryStoredToLoad", 1);
	}
}
