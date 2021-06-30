
var fadeSpeed:float=1;

var beginTintAlpha:float=0.5;
var myColor:Color;

function Start () {


}
function OnEnable()
{
beginTintAlpha=.33;
}

function Update () {

beginTintAlpha-=Time.deltaTime*fadeSpeed;

GetComponent.<Renderer>().material.SetColor ("_TintColor", Color(myColor.r, myColor.g, myColor.b ,beginTintAlpha));

}