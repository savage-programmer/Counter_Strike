using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvMover : MonoBehaviour {

    [Header ("******** SPEED **********")]
    public float Speed = 5;
    float XPosition;
    Vector3 pos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
        pos = transform.position;
        if(pos.x >= 1000 )
        {
            pos.x = -1000;
            transform.position = pos;

        }
        
      
        
    }
}
