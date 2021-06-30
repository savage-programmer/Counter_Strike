using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour {
    public GameObject Door;
    public Transform OpenPosition, ClosePosition;
    public bool Open = false, close = true;

	// Use this for initialization
	void Start () {
		if(Door==null)
        {
            Door=transform.GetChild(0).gameObject;
        }
        if(OpenPosition==null)
        {

            OpenPosition = transform.GetChild(1).transform;
        }

        if (ClosePosition == null)
        {

            ClosePosition = transform.GetChild(2).transform;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(Open==true && close == false)
        {
            Door.transform.position = Vector3.Lerp(Door.transform.position, OpenPosition.position, .1f);
           // Debug.Log("Opening");
            if(Door.transform.position==OpenPosition.position)
            {
                Open = false;
                Debug.Log("Door Open");
            }
            
        }
        if (Open == false && close == true)
        {
          //  Debug.Log("Closing");
            //  Door.transform.Translate(ClosePosition);
            Door.transform.position = Vector3.Lerp(Door.transform.position, ClosePosition.position, .1f);
            //    Door.transform.position = Door.transform.Translate(OpenPosition);
            if (Door.transform.position == ClosePosition.position)
            {
                close = false;
                Debug.Log("Door close");
            }
        }

    }


    public void OpenDoor()
    {
        Open = true;
        close = false;
        Invoke("Checkforupdate", 3f);

    }

    public void CloseDoor()
    {

        Open = false;
        close = true;
        Invoke("Checkforupdate1", 3f);
    }

    void Checkforupdate()
    {
        Open = false;
       
    }

    void Checkforupdate1()
    {
       
        close = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloseDoor();
        }
    }

}
