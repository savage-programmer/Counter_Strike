using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;

public class StarRateUs : MonoBehaviour {
	public GameObject RateUsPanal;
	public GameObject[] Stars;
    public bool IsMainManu=false;
	public  Text text;
	public GameObject Laterbtn;
	// Use this for initialization
	void Start () {
		for (int i = 0; i <= 4; i++)
		{
			Stars[i].SetActive (false);
		}
		text.gameObject.SetActive(false);
		Laterbtn.SetActive (true);
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnEnable()

	{
		for (int i = 0; i <= 4; i++)
		{
			Stars[i].SetActive (false);
		}
		text.gameObject.SetActive(false);
		Laterbtn.SetActive (true);
	}
	public void GivenStar(int Star)
	{
		
		if (Star <= 3) 
		{
			for (int i = 0; i < Star; i++) 
			{
				Stars [i].SetActive (true);	
			}


			Laterbtn.SetActive (false);
			text.gameObject.SetActive (true);
			text.text = " Thank You ";


			Invoke ("ClosePanal", 2);
		}

		if (Star > 3) 
		{
			for (int i = 0; i < Star; i++) 
			{
				Stars [i].SetActive (true);	
			}

		
			Laterbtn.SetActive (false);
			text.gameObject.SetActive (true);
			text.text = " Thank You for Feed Back  ";
			Invoke ("ClosePanalRated", 2);

			
		}
			
		
	}

	public void ClosePanal()
	{
        if (IsMainManu)
        {
            RateUsPanal.SetActive(false);
        }
        else
        {
            GameManager_GJ11.instance.OnLater();
            RateUsPanal.SetActive(false);
        }
    }

    public void ClosePanalRated()
    {
        if (IsMainManu)
        {
 
            RateUsPanal.SetActive(false);
        }
        else
        {
            GameManager_GJ11.instance.OnRated();
            RateUsPanal.SetActive(false);
        }
       

    }
   
}


