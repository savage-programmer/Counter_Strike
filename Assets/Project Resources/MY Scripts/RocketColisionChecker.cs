using UnityEngine;
using System.Collections;


public class RocketColisionChecker : MonoBehaviour {
	public static RocketColisionChecker instance;
	public GameObject bulletCarExplosion;
	public GameObject dummyCar;
	public GameObject bulletImpactFX;
	public GameObject[] Mission_OBJ,Explosives,Assosiated_Obj;
	public AudioClip expsound;
	 int curLevel;
	GameObject temp;
	// Use this for initialization
	void Start () {
		curLevel = GlobalScripts.CurrLevelIndex;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision colinfo){
		if(colinfo.gameObject.tag=="TrafficCar"){

//
//			if (colinfo.gameObject.name == "enemy") {
//
//				RoboTransformControler.carshitcount++;
//			
//					GetComponent<hoMove> ().enabled = false;
//
//				GetComponent<Animation>().Play ("die");
//				Destroy (gameObject, 2f);
//			}

			//colinfo.gameObject.SetActive (false);

		Instantiate (bulletCarExplosion,colinfo.transform.position,Quaternion.identity);


			colinfo.gameObject.SetActive (false);

			GameObject go =  Instantiate (dummyCar,colinfo.transform.position,colinfo.transform.rotation) as GameObject;
			Transform[] allChildren = go.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren) 
			{
				child.transform.parent = null;child.transform.parent = null;
				child.transform.parent = null;child.transform.parent = null;
				child.transform.parent = null;child.transform.parent = null;
				child.gameObject.AddComponent<Rigidbody> ();
				child.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3(Random.Range(-10f,10f), Random.Range(0f,10f),Random.Range(-10f,10f)), ForceMode.Impulse); 
				Destroy (child.gameObject,3f);
			}
		}
	}

	public void show_damgaecar(GameObject colinfo){

		curLevel = GlobalScripts.CurrLevelIndex;
		
//		if (colinfo.gameObject.tag == "enemy") {
//
//
//			Instantiate (bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
//			colinfo.gameObject.SetActive (false);
//		} else if (colinfo.gameObject.tag == "tank") {
//
//		
//			Instantiate (bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
//			colinfo.gameObject.SetActive (false);
//		     }
// 			
//		else if (colinfo.gameObject.tag == "turret") {
//
//
//			Instantiate (bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
//			colinfo.gameObject.SetActive (false);
//		}

//		else {	
		Instantiate (bulletCarExplosion, colinfo.transform.position, Quaternion.identity);
		if(GameObject.FindWithTag("UI") != null && GameObject.FindWithTag("UI").GetComponent<AudioSource>()!=null)
			GameObject.FindWithTag("UI").GetComponent<AudioSource>().PlayOneShot(expsound);
		
		//GetComponent<AudioSource>().PlayOneShot(lvlComplete);
		colinfo.gameObject.SetActive (false);
			

		//if (curLevel = 0) {
		if (colinfo.gameObject.tag == "MissionObj") {
			ObjectiveHandler.instance.Progressed ();
			Instantiate (bulletCarExplosion,Mission_OBJ[curLevel].transform.position, Quaternion.identity);
			if(GameObject.FindWithTag("UI") != null && GameObject.FindWithTag("UI").GetComponent<AudioSource>()!=null)
				GameObject.FindWithTag("UI").GetComponent<AudioSource>().PlayOneShot(expsound);
			
			Mission_OBJ[curLevel].gameObject.SetActive (false);

			Debug.Log("Mission OBj Destroyed ");
		}


		else if (colinfo.gameObject.tag == "explosive") {

			for(int i=0; i<Explosives.Length; i++)
			{
				temp = Explosives [i];
				if (temp.GetInstanceID() == colinfo.GetInstanceID()) {
					Instantiate (bulletCarExplosion,  Assosiated_Obj[i].transform.position, Quaternion.identity);
					if(GameObject.FindWithTag("UI") != null && GameObject.FindWithTag("UI").GetComponent<AudioSource>()!=null)
						GameObject.FindWithTag("UI").GetComponent<AudioSource>().PlayOneShot(expsound);
					
					Assosiated_Obj[i].gameObject.SetActive (false);
					Debug.Log("Obj Destroyed");
				}
				}

		
		}




//			GameObject go = Instantiate (dummyCar, colinfo.transform.position, colinfo.transform.rotation) as GameObject;
//			Transform[] allChildren = go.GetComponentsInChildren<Transform> ();
//			foreach (Transform child in allChildren) {
//				child.transform.parent = null;
//				child.transform.parent = null;
//				child.transform.parent = null;
//				child.transform.parent = null;
//				child.transform.parent = null;
//				child.transform.parent = null;
//				child.gameObject.AddComponent<Rigidbody> ();
//				child.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (-10f, 10f), Random.Range (0f, 10f), Random.Range (-10f, 10f)), ForceMode.Impulse); 
//				Destroy (child.gameObject, 3f);
//			}
		}

//	public void die(){
//		
//
//			dinosaur.GetComponent<Animation>().Play ("die");
//		Destroy (gameObject, 3f);
//
	}

