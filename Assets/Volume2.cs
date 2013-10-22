using UnityEngine;
using System.Collections;


// sets the volume used for music but not for other sounds

public class Volume2 : MonoBehaviour {
	public static float hSliderValue   = 1.0f;
	 public string MenuButtonIdentity;
	// Use this for initialization
	void Start () {
	 	GamePreferences.Volume= hSliderValue;    // sets the Volume variable equal to the sliders value

	 GetComponent<TextMesh>().text = ""+GamePreferences.Volume; // displays the volume
	
	}
	
	// Update is called once per frame
	void Update () {
		GamePreferences.Volume= hSliderValue; // sets the Volume variable equal to the sliders value
		
		
				 GetComponent<TextMesh>().text = ""+GamePreferences.Volume; // displays the volume
		
		 audio.volume =  hSliderValue; // changes the volume to the value of the slider
		
		
	
		
		
		
		
	}


 void OnGUI() {
    
 



 

	
	// creates the slider to control the music volume	
hSliderValue = GUI.HorizontalSlider(new Rect(50, 25, 100, 30), hSliderValue, 0.0F, 1.0F);
		GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
		

		
		
		
		
	 
	
		
		
		
		
		
	
	 audio.volume =  hSliderValue; // sets the volume
	

	
	}
	
	
	
	
	
	
	
	
	
	
	 void OnMouseUp()
    {
   
		
	
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	  void OnMouseEnter()
    {
	
	
		
    //    renderer.material.color = Color.blue;
    }

    void OnMouseExit()
    {
      //  renderer.material.color = Color.white;
    }
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	
	
	
	
	
	
	
	
