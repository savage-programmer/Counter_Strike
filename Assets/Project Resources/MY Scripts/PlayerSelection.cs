using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonCover;

public class PlayerSelection : MonoBehaviour {
    public static PlayerSelection Instance;
    public GameObject[] Players;
    public GameObject MiniMap, UICanvas, MainCamera;
	// Use this for initialization
	void Start () {

		
	}
     void Awake()

    {
        Instance = this;
        Debug.Log("playerselect");
        for (int i=0; i <Players.Length;i++)
        {
            Players[i].gameObject.SetActive(false);
        }
       int curr = PlayerPrefs.GetInt("SelectedTexture");
        int temp = 0;
        if(curr>=0 || curr <=3)
        {
            GlobalScripts.CurrPlayer = 0;
        }

        if (curr >= 4 || curr <= 6)
        {
            GlobalScripts.CurrPlayer = 1;
        }

        if (curr ==7)
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
        UICanvas.GetComponent<GameManager_GJ11>().player= Players[temp];
        //  MainCamera.GetComponent<ThirdPersonCamera>().SelectPlayer();


     


        // Player set in ObjectiveHandler, ThirdpersonCamera, minimap ,UICanvas
    }

    public void SetPlayer()
    {
        MissionController.instance.Player = Players[GlobalScripts.CurrPlayer];
       // MissionController.instance.Player = Players[3];
    }
    // Update is called once per frame
    void Update () {
		
	}
}
