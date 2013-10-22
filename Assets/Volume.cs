using UnityEngine;
using System.Collections;

public class Volume : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 audio.volume =  GamePreferences.Volume;
	}
	
	// Update is called once per frame
	void Update () {
	//AudioListener.volume = GamePreferences.Volume;
	
	  audio.volume =  GamePreferences.Volume;
	}
}
