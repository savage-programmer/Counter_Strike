using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageManager : MonoBehaviour
{
    public AudioClip[] HitSound;
	public GameObject Effect,heath_bar, plbar;
    public int HP = 200;
	public static int plHP = 8000;
	bool checkonetime;
	double dmgvalue,dmgvalue_all,dmgvalue_tank;

//	public GameObject dinosaur;

    private void Start()
    {
		if (gameObject.tag == "Player") {
			HP = 40000;

		} else if (gameObject.tag == "tank") {
			HP = 500;
		} else {
			HP = 400;
		}
		checkonetime = true;
    }

    public void ApplyDamage(int damage)
    {
		if(HP<0)
		return;
		dmgvalue = damage * 0.000125;
		dmgvalue_tank = damage * 0.002;
		dmgvalue_all = damage * 0.0025;
        if (HitSound.Length > 0)
        {
            AudioSource.PlayClipAtPoint(HitSound[Random.Range(0, HitSound.Length)], transform.position);
        }
		if (gameObject.tag == "Player") {
			print ("pl_Hitmm" + plHP);
			plHP -= damage;
			float fillvalue =(float) dmgvalue;
			plbar.GetComponent<Image> ().fillAmount -= fillvalue;
			//print ("pl_Hit" + plHP);
			//pl1.GetComponent<DamageManager>.

		}

		if (gameObject.tag == "tank" || gameObject.tag == "car1" || gameObject.tag == "enemy" || gameObject.tag == "turret") {
			if (gameObject.tag == "tank") {

				float fillvalue2 = (float)dmgvalue_tank;
				heath_bar.gameObject.transform.localScale -= new Vector3 (fillvalue2, 0, 0);
				HP -= damage;	
			} else {
				float fillvalue1 = (float)dmgvalue_all;
				heath_bar.gameObject.transform.localScale -= new Vector3 (fillvalue1, 0, 0);
				HP -= damage;
			}
		}
		if (plHP <= 0&&checkonetime)
		{
			Dead();
		}
		if (HP <= 0&&checkonetime)
		{
			Dead();
		}
    }

	void OnEnable() {


		checkonetime = true;
	}

    private void Dead()
    {
		
		//if (dinosaur.name == "dinosaur"){
//			dinosaur.GetComponent<Animation> ();
//			dinosaur.GetComponent<Animation>().Play ("die");
        // Instantiate(Effect, transform.position, transform.rotation);
		//}
		if (gameObject.tag == "Player") {

			Destroy (gameObject, 1.5f);
			GameObject.FindWithTag("MainCamera").GetComponent<GameDialogs_GJ11>().Dia_Failed();
			Debug.Log ("attacked!!!");

			} else {
			checkonetime = false;
			GameObject.FindWithTag ("controls").GetComponent<RocketColisionChecker> ().show_damgaecar (this.gameObject);

		}

//		if (gameObject.name == "robo") {
//			gameObject.SetActive (true);
//		}

		//		this.gameObject.SetActive (false);
//        Destroy(this.gameObject);
    }

}
