using UnityEngine;

public class EnvirementTextureSelection : MonoBehaviour {

    //	public bool UseAnimator;
    //	public Animator AnimC;
    //	public string TriggerOnChange;


    //public SkinnedMeshRenderer Mesh;
    public Material Ground, Wall, Ceiling, Door, Neutral, Door1,Door2, Marble;
    //public Material HeadMat;
    //public Material EyesMat;
    public Texture[] GroundTexture,GroundNormal,WallTexture, WallNormal,CeilingTexture, CeilingNormal,DoorTexture,DoorNormal,NeutralTexture,MarbleTexture;
    //public Texture[] GroundNormal, WallNormal, CeilingNormal, DoorNormal;
    //public Texture[] Heads;
    public Color[] GroundColor,GroundEmissionColor,WallColor,WallEmissionColor,CeilingColor,CeilingEmissionColor,DoorColor,DoorEmissionColor,NeutralColor, MarbleColor;

    void Awake()
	{
		//SkinMat = Mesh.materials [0];

		//HeadMat = Mesh.materials [1];
		//EyesMat = Mesh.materials [2];

		//int current = PlayerPrefs.GetInt ("SelectedTexture",0);

		//SkinMat.mainTexture = Skins [current];
    //    SkinMat.SetTexture("_MainTex", Skins[0]);
     //   SkinMat.SetTexture("_Albedo", Skins[0]);
        //	HeadMat.mainTexture = Heads [current];
        //EyesMat.color = Eyes [current];
    }
	// Use this for initialization
	void Start () 
	{
        //Ground.EnableKeyword("_NORMALMAP");
        //Wall.EnableKeyword("_NORMALMAP");
        //Ceiling.EnableKeyword("_NORMALMAP");
        //Door.EnableKeyword("_NORMALMAP");
        int current = 0;
        current = Random.Range(0,3);
    //    PlayerPrefs.SetInt("EnvSelectedTexture", current);

        Ground.SetTexture("_MainTex", GroundTexture[current]);
        Ground.SetTexture("_BumpMap", GroundNormal[current]);
        Ground.SetTexture("_EmissionMap", GroundTexture[current]);
        Ground.SetColor("_Color", GroundColor[current]);
        Ground.SetColor("_EmissionColor", GroundEmissionColor[current]);

        Wall.SetTexture("_MainTex", WallTexture[current]);
        Wall.SetTexture("_BumpMap", WallNormal[current]);
        Wall.SetTexture("_EmissionMap", WallTexture[current]);
        Wall.SetColor("_Color", WallColor[current]);
        Wall.SetColor("_EmissionColor", WallEmissionColor[current]);


        Ceiling.SetTexture("_MainTex", CeilingTexture[current]);
        Ceiling.SetTexture("_BumpMap", CeilingNormal[current]);
        Ceiling.SetTexture("_EmissionMap", CeilingTexture[current]);
        Ceiling.SetColor("_Color", CeilingColor[current]);
        Ceiling.SetColor("_EmissionColor", CeilingEmissionColor[current]);


        Door.SetTexture("_MainTex", DoorTexture[current]);
        Door.SetTexture("_BumpMap", DoorNormal[current]);
        Door.SetTexture("_EmissionMap", DoorTexture[current]);
        Door.SetColor("_Color", DoorColor[current]);
        Door.SetColor("_EmissionColor", DoorEmissionColor[current]);

        Door1.SetTexture("_MainTex", DoorTexture[current]);
        Door1.SetTexture("_BumpMap", DoorNormal[current]);
        Door1.SetTexture("_EmissionMap", DoorTexture[current]);
        Door1.SetColor("_Color", DoorColor[current]);
        Door1.SetColor("_EmissionColor", DoorEmissionColor[current]);

        Door2.SetTexture("_MainTex", DoorTexture[current]);
        Door2.SetTexture("_BumpMap", DoorNormal[current]);
        Door2.SetTexture("_EmissionMap", DoorTexture[current]);
        Door2.SetColor("_Color", DoorColor[current]);
        Door2.SetColor("_EmissionColor", DoorEmissionColor[current]);

        Neutral.SetTexture("_MainTex", NeutralTexture[current]);
        Neutral.SetColor("_Color", NeutralColor[current]);

        Marble.SetTexture("_MainTex", MarbleTexture[current]);
        Marble.SetColor("_Color", MarbleColor[current]);
        print("complete");
    }
	
	// Update is called once per frame
//	void Update () {
//		
//	}
	public void Next()
	{
		int current = PlayerPrefs.GetInt ("EnvSelectedTexture");
		++current;
		if (current >= GroundTexture.Length) 
		{
			current = 0;
		}

		PlayerPrefs.SetInt ("EnvSelectedTexture",current);
        Debug.Log(current);
        if (current == 0)
        {
            //Ground.SetTexture("_MainTex", GroundTexture[current]);
            //Ground.SetTexture("_NORMALMAP", GroundNormal[current]);
            //Ground.SetColor("_Color", GroundColor[current]);

            //Wall.SetTexture("_MainTex", WallTexture[current]);
            //Wall.SetTexture("_NORMALMAP", WallNormal[current]);
            //Wall.SetColor("_Color", WallColor[current]);


            //Ceiling.SetTexture("_MainTex", CeilingTexture[current]);
            //Ceiling.SetTexture("_NORMALMAP", CeilingNormal[current]);
            //Ceiling.SetColor("_Color", CeilingColor[current]);


            //Door.SetTexture("_MainTex", DoorTexture[current]);
            //Door.SetTexture("_NORMALMAP", DoorNormal[current]);
            //Door.SetColor("_Color", DoorColor[current]);

            //Neutral.SetTexture("_MainTex", NeutralTexture[current]);
            //Neutral.SetColor("_Color", NeutralColor[current]);

            //    SkinMat.SetTexture("_MainTex", Skins[current]);
            //   SkinMat.SetTexture("_Albedo", Skins[0]);
        }
        if (current == 1)
        {
        //    SkinMat.SetTexture("_MainTex", Skins[1]);
        //    SkinMat.SetTexture("_Albedo", Skins[1]);
        }
        if (current == 2)
        {
         //   SkinMat.SetTexture("_MainTex", Skins[2]);
        //    SkinMat.SetTexture("_Albedo", Skins[2]);
        }
        //	HeadMat.mainTexture = Heads [current];
        //EyesMat.color = Eyes [current];
    }
	public void Prev()
	{
		int current = PlayerPrefs.GetInt ("EnvSelectedTexture");
		--current;
		if (current < 0) 
		{
			current =  GroundTexture.Length-1;
		}
        Debug.Log(current);
        PlayerPrefs.SetInt ("EnvSelectedTexture",current);






        //Ground.SetTexture("_MainTex", GroundTexture[current]);
        //Ground.SetTexture("_NORMALMAP", GroundNormal[current]);
        //Ground.SetColor("_Color", GroundColor[current]);

        //Wall.SetTexture("_MainTex", WallTexture[current]);
        //Wall.SetTexture("_NORMALMAP", WallNormal[current]);
        //Wall.SetColor("_Color", WallColor[current]);


        //Ceiling.SetTexture("_MainTex", CeilingTexture[current]);
        //Ceiling.SetTexture("_NORMALMAP", CeilingNormal[current]);
        //Ceiling.SetColor("_Color", CeilingColor[current]);


        //Door.SetTexture("_MainTex", DoorTexture[current]);
        //Door.SetTexture("_NORMALMAP", DoorNormal[current]);
        //Door.SetColor("_Color", DoorColor[current]);

        //Neutral.SetTexture("_MainTex", NeutralTexture[current]);
        //Neutral.SetColor("_Color", NeutralColor[current]);








        if (current == 0)
        {
        //    SkinMat.SetTexture("_MainTex", Skins[0]);
         //   SkinMat.SetTexture("_Albedo", Skins[0]);
        }
        if (current == 1)
        {
        //    SkinMat.SetTexture("_MainTex", Skins[1]);
        //    SkinMat.SetTexture("_Albedo", Skins[1]);
        }
        if (current == 2)
        {
         //   SkinMat.SetTexture("_MainTex", Skins[2]);
         //   SkinMat.SetTexture("_Albedo", Skins[2]);
        }
        //HeadMat.mainTexture = Heads [current];
        //HeadMat.mainTexture.color
        //EyesMat.color = Eyes [current];
    }
	public void OnComplete()
	{
		print ("col");
	}
}
