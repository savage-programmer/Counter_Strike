using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class Doom_Tweener : MonoBehaviour {
	public Image ImageCircle;
	public bool OnStart = false;
	public bool OnMainMenu = false;
	public GameObject Play,RateUs,MoreApp,PrivacyPolicy,SelectPlayer,Name;
	public AudioSource source,source1,source2;

	// Use this for initialization
	IEnumerator Start () {


		if (OnStart) {

			if (ImageCircle) {

				ImageCircle.DOColor (RandomColor (), 1.5f).SetEase (Ease.Linear).Pause ();
				ImageCircle.DOFillAmount (0, 1.5f).SetEase (Ease.Linear).SetLoops (-1, LoopType.Yoyo)
					.OnStepComplete (() => {
						ImageCircle.fillClockwise = !ImageCircle.fillClockwise;
						ImageCircle.DOColor (RandomColor (), 1.5f).SetEase (Ease.Linear);
					});
			}
		}

		if (OnMainMenu) {
			
			if (Play) {
				//Play.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, 2f).SetDelay (5);
				Play.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, 2f).SetDelay (5).SetEase (Ease.OutBounce).OnStepComplete (() => {
					Play.GetComponent<RectTransform> ().DOScale (new Vector2 (1.2f, 1.2f), .5f).SetLoops (-1, LoopType.Yoyo);
				});
			}

			//source.PlayDelayed (5f);

			if (Name)
				Name.GetComponent<RectTransform> ().DOScale (Vector2.one, 1f).SetDelay (1);

			if (RateUs)
				RateUs.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, .5f).SetDelay (2f);

			if (MoreApp)
				MoreApp.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, .5f).SetDelay (2.5f);

			if (PrivacyPolicy)
				PrivacyPolicy.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, .5f).SetDelay (3f);

	
			if (SelectPlayer)
				SelectPlayer.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, .5f).SetDelay (3.5f);

			yield return new WaitForSeconds(1);
			source.Play ();
			yield return new WaitForSeconds(1);
			source.Play ();
			yield return new WaitForSeconds(.5f);
			source.Play ();
			yield return new WaitForSeconds(.5f);
			source.Play ();
			yield return new WaitForSeconds(.5f);
			source.Play ();
			yield return new WaitForSeconds(1.5f);
			source1.Play ();
			yield return new WaitForSeconds(.5f);
			source2.Play ();
		}
		yield return new WaitForSeconds(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}






	Color RandomColor()
	{
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
	}
	public void LodingCircle()
	{


		if (ImageCircle) {

			ImageCircle.DOColor (RandomColor (), 1.5f).SetEase (Ease.Linear).Pause ();
			ImageCircle.DOFillAmount (0, 1.5f).SetEase (Ease.Linear).SetLoops (-1, LoopType.Yoyo)
				.OnStepComplete (() => {
					ImageCircle.fillClockwise = !ImageCircle.fillClockwise;
					ImageCircle.DOColor (RandomColor (), 1.5f).SetEase (Ease.Linear);
				});
		}
	}


}
