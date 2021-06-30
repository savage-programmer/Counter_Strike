using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		
		if (collision.collider.gameObject.CompareTag ("Player")) 
		{
			ObjectiveHandler.instance.Progressed ();
			Debug.Log ("checkpoint");
			this.gameObject.SetActive(false);
		}
	}
}
