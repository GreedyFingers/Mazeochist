using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	public static float hSliderValue2   = 1.0f;
	 public string MenuButtonIdentity;
	// Use this for initialization
	void Start () {
	 	GamePreferences.Sound= hSliderValue2;
//static var volume : float;
	 GetComponent<TextMesh>().text = ""+GamePreferences.Sound;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		

			//AudioListener.volume =  hSliderValue2;
		 audio.volume =  hSliderValue2;
	
		
		
		GamePreferences.Sound= hSliderValue2;
		
	
	
				 GetComponent<TextMesh>().text = ""+GamePreferences.Sound;
		
	}


 void OnGUI() {

	
		
hSliderValue2 = GUI.HorizontalSlider(new Rect(100, 50, 100, 30), hSliderValue2, 0.0F, 1.0F);
		GetComponent<TextMesh>().text = ""+GamePreferences.Sound;
		

	
		
		
		
		
		
	//	AudioListener.volume =  hSliderValue2;
	 audio.volume =  hSliderValue2;
	

	
	}
	
	
	
	
	
	
	
	
	
	
	 void OnMouseUp()
    {
   
		// GetComponent<TextMesh>().text = ""+GamePreferences.Volume;
	
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	