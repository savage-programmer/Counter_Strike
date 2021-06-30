using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsManager : MonoBehaviour {

	public GameObject pistol,MachineGun,ShotGun;
	public static int Count1, Count2, Count3;

	// Use this for initialization
	void Start () {
		
		if (GlobalScripts.CurrLevelIndex == 1) {
			Count1 = 10;
			Count2 = 10;
			Count3 = 10;
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void count()
	{

	}

}
