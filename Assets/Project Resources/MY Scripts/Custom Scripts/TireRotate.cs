using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireRotate : MonoBehaviour {


    // Use this for initialization
    public float speed = 5f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(speed*Time.deltaTime, 0f, 0f);
	}
}
