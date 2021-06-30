using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class privacypolicy : MonoBehaviour {

	public GameObject scrollview,crossbt;
	public GameObject privacyButton;

	// Use this for initialization
//	void Start () {
//		
//	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void OnExitButtonClicked()
	{
		scrollview.SetActive (false);
		crossbt.SetActive (false);
		privacyButton.SetActive (true);

	}


	public void OnPrivacyButtonClicked()
	{
		scrollview.SetActive (true);
		crossbt.SetActive (true);
		privacyButton.SetActive (false);
	}
}
