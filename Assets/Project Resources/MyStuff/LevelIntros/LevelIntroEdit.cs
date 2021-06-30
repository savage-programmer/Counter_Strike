using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[System.Serializable]
public class CamDummyDelay1
{
	public Transform CamDummy;
	public float delay;
}
public class LevelIntroEdit : MonoBehaviour {

	public Camera MainCam;
	public LayerMask Before;
	public LayerMask After;

	public Camera Cam;
	SmoothFollow CamScript;
	public GameObject[] ToEnable;
	public CamDummyDelay1[] CamDummies;
	public bool DontRotate;
	public float delay;

	int index;
	float Cdelay;
//	public Text Description;
//	public string Description;

	void Start()
	{
		//Invoke ("EndIt",0);
	}


	void OnEnable()
	{
		if(MainCam)
			MainCam.cullingMask = After;

		index = 0;
		Cdelay = 0;
        MainCam.gameObject.SetActive(false);
	//	CamScript = Cam.GetComponent<SmoothFollow> ();
	//	if (CamDummies.Length > 0) 
	//	{
		//	CamScript.target = CamDummies [index].CamDummy;
	//	}
		//	if (DontRotate)
		//	Cam.transform.rotation = CamDummies [index].CamDummy.rotation;
		foreach (GameObject g in ToEnable)
			g.SetActive (true);

	//	CamDummies [index].CamDummy.gameObject.SetActive (true);
		Cam.gameObject.SetActive (true);

	//	index++;

	//	if (CamDummies.Length > 1) 
	//	{
		//	float tempdelay = 0;
		//	for (int i = 0; i < CamDummies.Length; i++) {
			//	tempdelay += CamDummies [i].delay;
			//	Cdelay += CamDummies [i].delay;
			//	Invoke ("SwitchDummy", Cdelay);
	//		}

		//	if (delay < tempdelay) {
		//		float offset = tempdelay - delay;
		//		delay = tempdelay + 1;
		//	}
	//	}
	//	Debug.Log (" End it play");
		Invoke ("EndIt",delay);
	}
	//void SwitchDummy()
	//{
	//	if (index >= CamDummies.Length)
	//		return;
	//	CamScript.target = CamDummies [index].CamDummy;
	//	if (DontRotate)
	//		Cam.transform.rotation = CamDummies [index].CamDummy.rotation;
	//	CamDummies [index].CamDummy.gameObject.SetActive (true);
	//	index++;

	//}
	public void EndIt()
	{
		if(MainCam)
			MainCam.cullingMask = Before;

        MainCam.gameObject.SetActive(true);
       
        //		Destroy (CamDummie);
        for (int i = 0 ;i<CamDummies.Length;i++) 
		{
			CamDummies [i].CamDummy.gameObject.SetActive (false);
		}

//		Destroy (Cam);
		Cam.gameObject.SetActive (false);
		foreach (GameObject g in ToEnable) {
			g.gameObject.SetActive (false);
		}

		Debug.Log (" Initiate Play= ");
		ObjectiveHandler.instance.InitObjeiveMech ();

//		if (GlobalScripts.CurrLevelIndex == 0) 
//		{
//			GameManager.instance.TutorialPlay ();
//		}
//		else
//		{
			GameManager_GJ11.instance.StartPlay ();
//		}
//		Destroy (this);
//		this.enabled = false;
		this.gameObject.SetActive(false);

	}
	// Use this for initialization
//	void Start () 
//	{
//		
//	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		
//	}
}
