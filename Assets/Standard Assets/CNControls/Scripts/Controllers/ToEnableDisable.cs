using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToEnableDisable : MonoBehaviour {

	public GameObject[] ToEnable;
	public GameObject[] ToDisable;

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void OnPressed()
	{
		foreach (GameObject g in ToEnable) 
		{
			if (g)
				g.SetActive (true);
		}

		foreach (GameObject h in ToDisable) 
		{
			if (h)
				h.SetActive (false);
		}
		print ("Button called");
	}
}
