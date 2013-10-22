using UnityEngine;
using System.Collections;

public class Volume2 : MonoBehaviour {
	public static float hSliderValue   = 1.0f;
	 public string MenuButtonIdentity;
	// Use this for initialization
	void Start () {
	 	GamePreferences.Volume= hSliderValue;

	 GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
	
	}
	
	// Update is called once per frame
	void Update () {
		GamePreferences.Volume= hSliderValue;
		
		
				 GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
		//	AudioListener.volume =  hSliderValue;
		 audio.volume =  hSliderValue;
		
		
	
		
		
		GamePreferences.Volume= hSliderValue;
		
	
	
				 GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
		
	}


 void OnGUI() {
    
 



 

	
		
hSliderValue = GUI.HorizontalSlider(new Rect(50, 25, 100, 30), hSliderValue, 0.0F, 1.0F);
		GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
		

		
		
		
		
	 
	
		
		
		
		
		
	
	 audio.volume =  hSliderValue;
	

	
	}
	
	
	
	
	
	
	
	
	
	
	 void OnMouseUp()
    {
   
		
	
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	  void OnMouseEnter()
    {
	
	
		
        renderer.material.color = Color.blue;
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	
	
	
	
	
	
	
	
