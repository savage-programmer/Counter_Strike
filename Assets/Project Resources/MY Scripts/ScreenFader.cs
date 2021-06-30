using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

 //   public Image FadeImg; // dy default in every scene in fade img will be black and its alpha will be max.
     float fadeSpeed ;
	SpriteRenderer sr;
	Image FadeImg;

    bool DoFade, UnDoFade;
	
	// Use this for initialization
    void Start()

    {   
	
		if (this.gameObject.GetComponent (typeof(UnityEngine.UI.Image)) != null) {
				FadeImg = this.gameObject.GetComponent<UnityEngine.UI.Image> ();

		} else if (this.gameObject.GetComponent (typeof(SpriteRenderer)) != null) {
				sr = this.gameObject.GetComponent<SpriteRenderer> ();

		} 
		//sr = this.gameObject.GetComponent<SpriteRenderer> ();
        // when we start ka scene fade out automatically run and after succful run it will disabled automatically.
        DoFade = false;  // use to fade the screen 
        UnDoFade = true; // use to unfade the screen
         
	}
	
	// Update is called once per frame
	void Update () {
        if (DoFade)
        { 
			if(this.gameObject.GetComponent (typeof(SpriteRenderer)) != null){
				sr.color=Color.Lerp(sr.color,Color.clear,fadeSpeed*Time.unscaledDeltaTime);
			}else if(this.gameObject.GetComponent (typeof(UnityEngine.UI.Image)) != null){
				FadeImg.color=Color.Lerp(FadeImg.color,Color.clear,fadeSpeed*Time.unscaledDeltaTime);				
			}
            // Lerp the colour of the image between itself and black.
//            FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
//            if (FadeImg.color == Color.clear)
//            {
//                FadeImg.enabled = false;
//            }
//			if (sr.color == Color.clear)
//			{
//				//sr.enabled = false;
//			}
        }
        if (UnDoFade)
        {    
			//sr.color=Color.Lerp(sr.color,Color.white,fadeSpeed*Time.unscaledDeltaTime);
			if(this.gameObject.GetComponent (typeof(SpriteRenderer)) != null){
					sr.color=Color.Lerp(sr.color,Color.white,fadeSpeed*Time.unscaledDeltaTime);
			}else if(this.gameObject.GetComponent (typeof(UnityEngine.UI.Image)) != null){
					FadeImg.color=Color.Lerp(FadeImg.color,Color.white,fadeSpeed*Time.unscaledDeltaTime);				
			}
            // Lerp the colour of the image between itself and transparent.
           // FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
//			if (sr.color == Color.clear)
//			{
//				//sr.enabled = false;
//			}
//            if (FadeImg.color == Color.clear)
//            {
//                FadeImg.enabled = false;
//            }
        }
	}

	public  void FadeToClear(float setfadespeed)
    {
		fadeSpeed = setfadespeed;
        DoFade = false;
        UnDoFade = true;
		//FadeImg.enabled = true;
		//sr.enabled = true;
    }

	public void FadeToBlack(float setfadespeed)
    {
		fadeSpeed = setfadespeed;
        DoFade = true;
        UnDoFade = false;
	//	FadeImg.enabled = true;
	//	sr.enabled = true;
      
    }


}
