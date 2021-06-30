using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gdpr : MonoBehaviour
{
    private string policyKey = "policy";

    void Start()
    {
        var accepted = PlayerPrefs.GetInt(policyKey, 0) ==1;

        if (accepted)
            return;

        SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
           SetTermsOfServiceLink("").
           SetPrivacyPolicyLink("https://sites.google.com/view/itch01/home"),
           onMenuClosed);
    }

    private void onMenuClosed()
    {
        Debug.LogWarning("Policy accepted");
        PlayerPrefs.SetInt(policyKey, 1);
    }
}
