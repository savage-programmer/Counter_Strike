using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonCover;

public class Pickupcontrol : MonoBehaviour {
    public GameObject Pickup;
    public GameObject []SoldierPrefebs;
    public Transform Position1, Position2;
    bool Check = true;
    public GameObject[] SoldierInstance;
   // Transform PathManager;
   // public PathManager []path;
   // static bool Changepath = true;
    // Use this for initialization
    void Start() {
         

        if (Pickup == null)
        {
            Pickup = this.gameObject;
        }

        if (Position1 == null)
        {
            Position1 = transform.GetChild(0);
        }

        if (Position2 == null)
        {
            Position2 = transform.GetChild(1);
        }
       

        SoldierInstance[0] = Instantiate(SoldierPrefebs[Random.Range(0, SoldierPrefebs.Length)], Position1.position, Position1.rotation, Position1.transform);
        SoldierInstance[1] = Instantiate(SoldierPrefebs[Random.Range(0, SoldierPrefebs.Length)], Position2.position, Position2.rotation, Position2.transform);
        SoldierInstance[0].gameObject.GetComponent<AIController>().MaxStandDistance = 50;
        SoldierInstance[0].gameObject.GetComponent<AIController>().MinStandDistance = 5;
        SoldierInstance[1].gameObject.GetComponent<AIController>().MaxStandDistance = 50;
        SoldierInstance[1].gameObject.GetComponent<AIController>().MinStandDistance = 5;

     //   PathManager = GameObject.FindGameObjectWithTag("PathManager").transform;
      //  path[0] = PathManager.GetChild(0).GetComponent<PathManager>();
     //   path[1] = PathManager.GetChild(1).GetComponent<PathManager>();

        //if (Changepath==true)
        //{
        //    GetComponent<hoMove>().SetPath(path[0]);
        //    Changepath = false;
        //}

        //if (Changepath == false)
        //{
        //    GetComponent<hoMove>().SetPath(path[1]);
        //    Changepath = true;
        //}

    }
	
	// Update is called once per frame
	void Update () {
        if(SoldierInstance[0]==null && SoldierInstance[1]==null && Check)
        {
            Check = false;
            Destroy(this.gameObject, 5f);
            Debug.Log("pickup destroyed");

        }
		
	}
}
