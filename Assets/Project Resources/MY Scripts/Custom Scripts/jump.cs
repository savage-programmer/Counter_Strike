using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour {

    public Animator introplayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Jump trigger");
        if(other.gameObject.name== "jumptrigger")
        {
            introplayer.Play("JumpBig",0);
            Debug.Log("Jump inner trigger");
        }
    }

}
