using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class permissionDialogue : MonoBehaviour {

	public string privacyURL = "http://tekbash.blogspot.com/";
	public string sceneName = "init";

	[Space(10)]
	public string playerprefs = "result_gdpr";
	private string alreadyAsked = "firstTime";

	public Text privacyText;
	[Space(10)]	[TextArea(5,10)]
	public string consentMessageAccepted = "Great. We hope you enjoy" +
		"your personalized ad experience.";
	[Space(10)]	[TextArea(5,10)]
	public string consentMessageDisagreed = "Ad Networks won't collect your data through this app for personalized advertising. If you consent yo Ad Networks personalizing your advertising experience in a different app, we will still collect your data through that app.";

	[Space(10)]
	public GameObject closeButton;
	public GameObject consentButtonsPanel;

	// Use this for initialization
	void Awake () {
		if (PlayerPrefs.GetInt (alreadyAsked) == 1) {
			Application.LoadLevel (sceneName);
		}
	}
		
	public void openUrl()
	{
		Application.OpenURL (privacyURL);
	}

	public void consent(bool val)
	{
		if(val) {
			PlayerPrefs.SetInt (playerprefs, 1);
			privacyText.text = consentMessageAccepted;
//			StartApp.StartAppWrapper.setUserConsent ("ACCESS_FINE_LOCATION", System.DateTime.Now.Millisecond,true);
//			StartApp.StartAppWrapper.setUserConsent ("ACCESS_COARSE_LOCATION", System.DateTime.Now.Millisecond,true);
//			StartApp.StartAppWrapper.setUserConsent ("EULA", System.DateTime.Now.Millisecond,true);

		}
		else {
			PlayerPrefs.SetInt (playerprefs, 0);
			privacyText.text = consentMessageDisagreed;
//			StartApp.StartAppWrapper.setUserConsent ("ACCESS_FINE_LOCATION", System.DateTime.Now.Millisecond,false);
//			StartApp.StartAppWrapper.setUserConsent ("ACCESS_COARSE_LOCATION", System.DateTime.Now.Millisecond,false);
//			StartApp.StartAppWrapper.setUserConsent ("EULA", System.DateTime.Now.Millisecond,false);
		}

		closeButton.SetActive (true);
		consentButtonsPanel.SetActive (false);

		PlayerPrefs.SetInt (alreadyAsked, 1);
		PlayerPrefs.Save ();
	}

	public void loadLevel()
	{
		Application.LoadLevel (sceneName);
	}
		
	public void cancel()
	{
		Application.Quit ();
	}
}
