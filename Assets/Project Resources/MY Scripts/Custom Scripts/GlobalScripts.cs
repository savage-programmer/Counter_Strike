﻿using UnityEngine;
using System.Collections;

public static class GlobalScripts  
{
	public static bool ShowMainMenuAdd = false;
 
 	public static int mainmenu_checkbt=0;
    public enum GameState { GameReady=0, GamePlaying=1 }           // GamePlaying: after ready game,play the game
    public static GameState gameState;
  
	public static float damage_player;

	public static bool HeadShot=false;//True if HIt bULLET oN hEAD
	public static int DimndCount=0;
	public static int CurrLevelIndex =4;
    public static int CurrPlayer = 0;
    public static bool currIndex = false;


}
