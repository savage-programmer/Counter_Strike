using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcontroller : MonoBehaviour {

	// Use this for initialization
	public GameObject Maincam,introcam,campoint; 
	public GameObject[] camposition;
	public int num=0,currentLevel;
	void Start () {
		currentLevel = GlobalScripts.CurrLevelIndex;
		swichposition();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void swichposition()
	{
		
//		introcam.transform.position = Vector3.Lerp (introcam.transform.position, camposition[num].transform.position, 3 * Time.deltaTime);
//		introcam.transform.rotation = Quaternion.Lerp (introcam.transform.rotation, camposition[num].transform.rotation, 3 * Time.deltaTime);
		//introcam.transform.LookAt (this.gameObject.transform);	
		GameObject.FindWithTag("UI").GetComponent<GameManager_GJ11>().PlayButton.SetActive(false);

		if (num >= camposition.Length) {
			Maincam.SetActive (true);
			introcam.SetActive (false);
			num = 0;
			GameObject.FindWithTag("UI").GetComponent<GameManager_GJ11>().PlayButton.SetActive(true);
		   Time.timeScale = 0;
			return;
		}
		introcam.GetComponent<SmoothFollow> ().target = camposition [num].transform;
		num=num+1;
		Invoke ("swichposition", 0f);
	}


	public void lastcam()
	{
		introcam.SetActive (true);
		introcam.GetComponent<SmoothFollow> ().target =campoint.transform;
	}

}
