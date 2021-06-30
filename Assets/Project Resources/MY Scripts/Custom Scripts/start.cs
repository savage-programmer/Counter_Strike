using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour {
   public GameObject prefebnew;
	// Use this for initialization
	void Start () {
        Instantiate(prefebnew, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
