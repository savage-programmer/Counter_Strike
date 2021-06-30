using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public static MissionController instance;

    [Header("              Player Selection          ")]
    public GameObject[] Players;
    public GameObject MiniMap, UICanvas, MainCamera;


    public ObjectiveHandler MH;
    public GameObject[] EnemyinLevels;
    public GameObject[] IntroLevels;
    public GameObject Player;
    public string[] Levels;
    public Text missionstatement;
    public GameObject position, sound_On, sound_Off;
    void Awake()
    {
        instance = this;

        for (int j = 0; j < Players.Length; j++)
        {
            Players[j].gameObject.SetActive(false);
        }
        int curr = PlayerPrefs.GetInt("SelectedTexture");
        //   Debug.Log("Selected Player");
        //    Debug.Log(PlayerPrefs.GetInt("SelectedTexture"));
        int temp = 0;
        if (curr >= 0 && curr <= 3)
        {
            GlobalScripts.CurrPlayer = 0;
        }

        if (curr >= 4 && curr <= 6)
        {
            GlobalScripts.CurrPlayer = 1;
        }

        if (curr == 7)
        {
            GlobalScripts.CurrPlayer = 2;
        }

        if (curr == 8)
        {
            GlobalScripts.CurrPlayer = 3;
        }

        temp = GlobalScripts.CurrPlayer;
        //  temp = 3;

        Players[temp].SetActive(true);

        MiniMap.GetComponent<bl_MiniMap>().m_Target = Players[temp];
        UICanvas.GetComponent<GameManager_GJ11>().player = Players[temp];
        Player = Players[temp];
        //  MainCamera.GetComponent<ThirdPersonCamera>().SelectPlayer();





        // Player set in ObjectiveHandler, ThirdpersonCamera, minimap ,UICanvas










        int i = 0;
        GameObject[] Temp = new GameObject[EnemyinLevels.Length];

        for (i = 0; i < EnemyinLevels.Length; i++)
        {
            Temp[i] = EnemyinLevels[i];
        }

        for (i = 0; i < EnemyinLevels.Length; i++)
        {
            print("enemy lenth" + EnemyinLevels.Length);
            EnemyinLevels[i] = Temp[MH.LevelsOrder[i]];
            print("orders levels" + Temp[MH.LevelsOrder[i]] + EnemyinLevels[i]);
        }

        Temp = new GameObject[IntroLevels.Length];
        for (i = 0; i < IntroLevels.Length; i++)
        {
            Temp[i] = IntroLevels[i];
        }

        for (i = 0; i < IntroLevels.Length; i++)
        {
            IntroLevels[i] = Temp[MH.LevelsOrder[i]];
        }

        string[] sTemp = new string[Levels.Length];
        for (i = 0; i < Levels.Length; i++)
        {
            sTemp[i] = Levels[i];
        }

        for (i = 0; i < IntroLevels.Length; i++)
        {
            print("intro levels" + IntroLevels.Length);
            Levels[i] = sTemp[MH.LevelsOrder[i]];
        }


    }
    // Use this for initialization
    void Start()
    {
        selectlevelstart();
        selectlevelIntro();
        
        //	Player.GetComponent<CharacterMotor> ().SetLookTarget();
        if (GlobalScripts.CurrLevelIndex == 1)
        {
            //	Player.gameObject.transform.position = position.gameObject.transform.position;
            //	Player.gameObject.transform.rotation = position.gameObject.transform.rotation;
            //camra.gameObject.transform.rotation = position.gameObject.transform.rotation;
        }
    }
    public void musicBtnOn()
    {
        AudioListener.pause = true;
        sound_On.SetActive(false);
        sound_Off.SetActive(true);

    }
    public void musicBtnOff()
    {
        AudioListener.pause = false;
        sound_On.SetActive(true);
        sound_Off.SetActive(false);
    }
    public void selectlevelIntro()
    {
        
        if (GlobalScripts.CurrLevelIndex - 1 >= 0)
            IntroLevels[GlobalScripts.CurrLevelIndex - 1].SetActive(false);

        IntroLevels[GlobalScripts.CurrLevelIndex].SetActive(true);
        ObjectiveHandler.instance.PreObjective();
        Time.timeScale = 1;
    }
    public void selectlevel()
    {
        
        EnemyinLevels[GlobalScripts.CurrLevelIndex].SetActive(true);
        print(EnemyinLevels[GlobalScripts.CurrLevelIndex]);
    }

    public void selectlevelstart()
    {
        MyAdsManager.instance.ShowFullScreenAd();
        if (GlobalScripts.CurrLevelIndex < Levels.Length)
        {
            missionstatement.text = Levels[GlobalScripts.CurrLevelIndex];
        }
        //		if (GlobalScripts.CurrLevelIndex == 0)
        //		{
        //			missionstatement.text = "Rescue one injured citizen who is rolled over by a gangster car.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 1)
        //		{
        //			missionstatement.text = "There is an accident between gangster & citizen car. Rescue the citizen to hospital and fight the gangster.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 2)
        //		{
        //			missionstatement.text = "Robbers have looted all the money and trying to get away.Stop them";
        //		} 
        //		else if (GlobalScripts.CurrLevelIndex == 3) 
        //		{
        //			missionstatement.text = "Two Cars had an Accident.Rescue the girl";
        //		}else if (GlobalScripts.CurrLevelIndex == 4)
        //		{
        //			missionstatement.text = "Rescue the man fallen from a building on fire.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 5) 
        //		{
        //			missionstatement.text = "Find and Beat that purse snatcher";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 6)
        //		{
        //			missionstatement.text = "Show your web bullet skills.Catch them all.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 7)
        //		{
        //			missionstatement.text = "Get to the roof top and beat those terrorists.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 8)
        //		{
        //			missionstatement.text = "3 citizens are injured in a car accident. Rescue them to hospital.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 9)
        //		{
        //			missionstatement.text = "2 gangsters are going to rob a business man by his car. Get there and kill them on time";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 10)
        //		{
        //			missionstatement.text = "Identify the bomb in briefcase and take it to roof top to avoid destruction.";
        //		}
        //		else if (GlobalScripts.CurrLevelIndex == 11)
        //		{
        //			missionstatement.text = "Kill the gangsters who planned the briefcase bomb attack.";
        //		}
    }
    public string GetMissionText(int Mission)
    {
        string s = string.Empty;
        switch (Random.Range(0, 3))
        {
            case 0:
                s = "Good Work !";
                break;
            case 1:
                s = "Nice Work!";
                break;
            case 2:
                s = "Nice Work!";
                break;
        }
        //		if (Mission == 0)
        //		{
        //			s = "Rescue one injured citizen who is rolled over by a gangster car.";
        //		} else if (Mission == 1) {
        //			s = "There is an accident between gangster & citizen car. Rescue the citizen to hospital and fight the gangster.";
        //		} else if (Mission == 2) {
        //			s = "Robbers have looted all the money and trying to get away.Stop them.";
        //		} else if (Mission == 3) {
        //			s = "Two Cars had an Accident.Rescue the girl";
        //		}else if (Mission == 4) {
        //			s = "Rescue the man fallen from a building on fire.";
        //		}else if (Mission == 5) {
        //			s = "Find and Beat that purse snatcher";
        //		}else if (Mission == 6) {
        //			s = "Show your web bullet skills.Catch them all.";
        //		}else if (Mission == 7) {
        //			s = "Get to the roof top and beat those terrorists.";
        //		}
        return s;
    }
    //	// Update is called once per frame
    //	void Update () {
    //		
    //	}
}
