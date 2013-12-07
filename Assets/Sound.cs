using UnityEngine;
using System.Collections;



// controls slider used to adjust volume of all non music sounds

public class Sound : MonoBehaviour {
	public static float hSliderValue2;
	 public string MenuButtonIdentity;
	// Use this for initialization
	void Start () {
	 	
		
		// sets the game preference variable sound equal to the sliders value
		hSliderValue2 = GamePreferences.Sound;
		
		GamePreferences.Sound= hSliderValue2;   

	 GetComponent<TextMesh>().text = ""+(Mathf.Round(GamePreferences.Sound*100f))/100f ; // displays the volume level
	
	}
	
	// Update is called once per frame
	void Update () {
		
		

		 audio.volume =  hSliderValue2; // sets volume equal to the sliders value
	
		
		
		GamePreferences.Sound= hSliderValue2;
		
	
		 GetComponent<TextMesh>().text = ""+(Mathf.Round(GamePreferences.Sound*100f))/100f ;
		
		//GetComponent<TextMesh>().text = ""+GamePreferences.Sound;
		
	}


 void OnGUI() {

	
		// creates the slider
hSliderValue2 = GUI.HorizontalSlider(new Rect(560, 335, 400, 35), hSliderValue2, 0.0F, 1.0F);
		GetComponent<TextMesh>().text = ""+GamePreferences.Sound; // displays the value of the slider
		

	
		
		
		
		
		
	
	 audio.volume =  hSliderValue2; // set volume level
	

	
	}
	
	
	
	
	
	
	
	
	
	
	 void OnMouseUp()
    {
   
		
	
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	