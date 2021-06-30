using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSelection : MonoBehaviour {

	public bool UseAnimator;
	public Animator AnimC;
	public string TriggerOnChange;

    public GameObject[] Players;
	//public SkinnedMeshRenderer Mesh;
	public Material Player1Skin,Player1Head,Player2,Player3,Player4;
	//public Material HeadMat;
	//public Material EyesMat;
	public Texture[] Player1SkinTex,Player1HeadTex;
	public Texture[] Player2Tex,Player3Tex,Player4Tex;
	//public Color[] Player1EyesColor;

	void Awake()
	{
        //SkinMat = Mesh.materials [0];
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].SetActive(false);
        }
        Players[0].SetActive(true);
        //HeadMat = Mesh.materials [1];
        //EyesMat = Mesh.materials [2];

        int current = PlayerPrefs.GetInt ("SelectedTexture",0);

        //SkinMat.mainTexture = Skins [current];
        Player1Skin.EnableKeyword("_DETAIL_MULX2");
        Player1Head.EnableKeyword("_DETAIL_MULX2");
        Player2.EnableKeyword("_DETAIL_MULX2");
        Player3.EnableKeyword("_DETAIL_MULX2");
        Player4.EnableKeyword("_DETAIL_MULX2");
       

        //  Player1Skin.SetTexture("_DetailAlbedoMap", Skins[0]);
        Player1Skin.SetTexture("_MainTex", Player1SkinTex[0]);
        Player1Head.SetTexture("_MainTex", Player1HeadTex[0]);
      //  Player1Eyes.SetColor("_Color", Player1EyesColor[0]);
        // SkinMat.SetTexture("_SecondaryTexture", Skins[0]);
        //	HeadMat.mainTexture = Heads [current];
        //EyesMat.color = Eyes [current];
    }
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
	public void Next()
	{
        //SkinMat.EnableKeyword("_DETAIL_MULX2");
        int current = PlayerPrefs.GetInt ("SelectedTexture",0);
		++current;
		if (current >= 9) 
		{
			current = 0;
		}

		PlayerPrefs.SetInt ("SelectedTexture",current);
        Debug.Log(current);
        if (current == 0)
        {
            for(int i=0; i<Players.Length;i++)
            {
                Players[i].SetActive(false);
            }
            Players[0].SetActive(true);

            Player1Skin.SetTexture("_MainTex", Player1SkinTex[0]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[0]);
          //  SkinMat.SetTexture("_MainTex", Skins[0]);
        //   SkinMat.SetTexture("_DetailAlbedoMap", Skins[0]);
        }
        if (current == 1)
        {
            Player1Skin.SetTexture("_MainTex", Player1SkinTex[1]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[1]);

            //          SkinMat.SetTexture("_MainTex", Skins[1]);
            //          SkinMat.SetTexture("_DetailAlbedoMap", Skins[1]);
        }
        if (current == 2)
        {
            Player1Skin.SetTexture("_MainTex", Player1SkinTex[2]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[2]);

            //           SkinMat.SetTexture("_MainTex", Skins[2]);
            //          SkinMat.SetTexture("_DetailAlbedoMap", Skins[2]);
        }

        if (current == 3)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[0].SetActive(true);

            Player1Skin.SetTexture("_MainTex", Player1SkinTex[3]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[3]);

           
        }


        if (current == 4)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[0]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[0]);

        }


        if (current ==5)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[1]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[1]);

        }

        if (current == 6)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[2]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[2]);
        }


        if (current == 7)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[2].SetActive(true);

            Player3.SetTexture("_MainTex", Player3Tex[0]);
            Player3.SetTexture("_DetailAlbedoMap", Player3Tex[0]);
        }


        if (current == 8)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[3].SetActive(true);

            Player4.SetTexture("_MainTex", Player4Tex[0]);
            Player4.SetTexture("_DetailAlbedoMap", Player4Tex[0]);
        }

    }
	public void Prev()
	{
      //  SkinMat.EnableKeyword("_DETAIL_MULX2");
        int current = PlayerPrefs.GetInt ("SelectedTexture",0);
		--current;
		if (current < 0) 
		{
            current = 8;
    
		}
        Debug.Log(current);
        PlayerPrefs.SetInt ("SelectedTexture",current);

        if (current == 0)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[0].SetActive(true);

            Player1Skin.SetTexture("_MainTex", Player1SkinTex[0]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[0]);
            //  SkinMat.SetTexture("_MainTex", Skins[0]);
            //   SkinMat.SetTexture("_DetailAlbedoMap", Skins[0]);
        }
        if (current == 1)
        {
            Player1Skin.SetTexture("_MainTex", Player1SkinTex[1]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[1]);

            //          SkinMat.SetTexture("_MainTex", Skins[1]);
            //          SkinMat.SetTexture("_DetailAlbedoMap", Skins[1]);
        }
        if (current == 2)
        {
            Player1Skin.SetTexture("_MainTex", Player1SkinTex[2]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[2]);

            //           SkinMat.SetTexture("_MainTex", Skins[2]);
            //          SkinMat.SetTexture("_DetailAlbedoMap", Skins[2]);
        }

        if (current == 3)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[0].SetActive(true);

            Player1Skin.SetTexture("_MainTex", Player1SkinTex[3]);
            Player1Head.SetTexture("_MainTex", Player1HeadTex[3]);


        }


        if (current == 4)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[0]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[0]);

        }


        if (current == 5)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[1]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[1]);

        }

        if (current == 6)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[1].SetActive(true);

            Player2.SetTexture("_MainTex", Player2Tex[2]);
            Player2.SetTexture("_DetailAlbedoMap", Player2Tex[2]);
        }


        if (current == 7)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[2].SetActive(true);

            Player3.SetTexture("_MainTex", Player3Tex[0]);
            Player3.SetTexture("_DetailAlbedoMap", Player3Tex[0]);
        }


        if (current == 8)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }
            Players[3].SetActive(true);

            Player4.SetTexture("_MainTex", Player4Tex[0]);
            Player4.SetTexture("_DetailAlbedoMap", Player4Tex[0]);
        }
    }
	public void OnComplete()
	{
		print ("col");
	}
}
