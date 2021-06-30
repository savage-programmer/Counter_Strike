using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour 
{
	public GameObject[] LevelLocks;
	public GameObject[] Levels;

	public bool IsMissionBased;
	public GameObject[] Missions;
	public GameObject[] MissionLocks;
	public GameObject[] LevelsContainer;
	public int[] LevelsMissionWise;

	int UnlockedLevels;
	int UnlockedMissions;

	public GameObject BackMissions;
	public GameObject BackMain;

	void OnEnable()
	{
		BackMissions.SetActive (false);
		BackMain.SetActive (true);
		Start_ ();
	}

	// Use this for initialization
	void Start_ () 
	{
		UnlockedLevels = PlayerPrefs.GetInt ("Unlocked",0);
		if(UnlockedLevels>=Levels.Length)
		{
			PlayerPrefs.SetInt ("Unlocked",Levels.Length-1);

		}
		int i = 0;
		if (IsMissionBased) {
			UnlockedMissions = 0;
			for (i = 0; i < Missions.Length; i++) {
				if (UnlockedLevels > LevelsMissionWise [i]) {
					UnlockedMissions++;
				}
//				else if (UnlockedLevels = LevelsMissionWise [i]) 
//				{
//					
//				}
			}
			for (i = 0; i < Missions.Length; i++) {
				if (i <= UnlockedMissions) {
					MissionLocks [i].SetActive (false);
					Missions [i].GetComponent<UnityEngine.UI.Button> ().interactable = true;
				} else {
					MissionLocks [i].SetActive (true);
					Missions [i].GetComponent<UnityEngine.UI.Button> ().interactable = false;
				}
			}

			for (i = 0; i < LevelsContainer.Length; i++) {
				LevelsContainer [i].SetActive (false);
			}
			for (i = 0; i < Missions.Length; i++) {
				Missions [i].SetActive (true);
			}
		}
		else
		{
			for (i = 0; i < Levels.Length; i++) 
			{
				Levels [i].SetActive (true);
			}
		}

		for (i = 0; i < Levels.Length; i++) 
		{
			if (i <= UnlockedLevels) 
			{
				print ("called: "+i);
				LevelLocks [i].SetActive (false);
				Levels [i].GetComponent<UnityEngine.UI.Button> ().interactable = true;
			}
			else
			{
				LevelLocks [i].SetActive (true);
				Levels [i].GetComponent<UnityEngine.UI.Button> ().interactable = false;
			}
		}
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		
//	}


	public void SelectLevel(int index)
	{
		GlobalScripts.CurrLevelIndex = index;
		MainScene_PP.instance.OnOK ();
	}
	public void SelectMission(int index)
	{
		BackMissions.SetActive (true);
		BackMain.SetActive (false);

		LevelsContainer [index].SetActive (true);

		for (int i = 0; i < Missions.Length; i++)
		{
			Missions [i].SetActive (false);
		}
	}
	public void OnBackToMissions()
	{
//		int i = 0;
//		for (i = 0; i < LevelsContainer.Length; i++)
//			LevelsContainer [i].SetActive (false);
		BackMissions.SetActive (false);
		BackMain.SetActive (true);
		Start_ ();
	}
}
