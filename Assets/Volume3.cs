using UnityEngine;
using System.Collections;


// script used to set volume of all non music sounds
public class Volume3 : MonoBehaviour {

	


	// Use this for initialization
	void Start () {
	 audio.volume =  GamePreferences.Sound;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	  audio.volume =  GamePreferences.Sound; // sets volume
	
	}
}




