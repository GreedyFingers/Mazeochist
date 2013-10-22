using UnityEngine;
using System.Collections;

public class Volume3 : MonoBehaviour {

	


	// Use this for initialization
	void Start () {
	 audio.volume =  GamePreferences.Sound;
	}
	
	// Update is called once per frame
	void Update () {
//	AudioListener.volume = GamePreferences.Sound;
	  audio.volume =  GamePreferences.Sound;
	
	}
}




