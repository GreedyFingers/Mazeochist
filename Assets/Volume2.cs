using UnityEngine;
using System.Collections;


// sets the volume used for music but not for other sounds

public class Volume2 : MonoBehaviour {
	public static float hSliderValue   ;
	 public string MenuButtonIdentity;
	// Use this for initialization
	void Start () {
	 	
		hSliderValue = GamePreferences.Volume;
		GamePreferences.Volume= hSliderValue;    // sets the Volume variable equal to the sliders value

	// GetComponent<TextMesh>().text = ""+GamePreferences.Volume; // displays the volume
		 GetComponent<TextMesh>().text = ""+(Mathf.Round(GamePreferences.Volume*100f));
	
	}
	// Update is called once per frame
	void Update () {
		GamePreferences.Volume= hSliderValue; // sets the Volume variable equal to the sliders value
		
		
				// GetComponent<TextMesh>().text = ""+GamePreferences.Volume; // displays the volume
		
			 GetComponent<TextMesh>().text = ""+(Mathf.Round(GamePreferences.Volume*100f))/100f ;
		
		
		 audio.volume =  hSliderValue; // changes the volume to the value of the slider
		
		
	
		
		
		
		
	}


 void OnGUI() {
    
 



 

	
	// creates the slider to control the music volume	
hSliderValue = GUI.HorizontalSlider(new Rect(520, 450, 400, 35), hSliderValue, 0.0F, 1.0F);
		GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
		

		
		
		
		
	 
	
		
		
		
		
		
	
	 audio.volume =  hSliderValue; // sets the volume
	

	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	
	
	
	
	
