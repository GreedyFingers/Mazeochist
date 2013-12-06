



using UnityEngine;
using System.Collections;


// sets the volume used for music but not for other sounds

public class  GridSizeScript: MonoBehaviour {
	public static float hSliderValue   = 0;
	 public string MenuButtonIdentity;
	public int CtoI =0;
	// Use this for initialization
	void Start () {
		
	CtoI = (int)(Mathf.Round(hSliderValue+2)) ;    
 	
		

		
		playerPrefs._intGridSize= CtoI;
	 GetComponent<TextMesh>().text = ""+playerPrefs._intGridSize; 
		 //GetComponent<TextMesh>().text ="" + GameObject.Find ("playerPrefs").GetComponent<playerPrefs>();
	}
	
	// Update is called once per frame
	void Update () {        // sets gridsize
	
		
			
		CtoI = (int)(Mathf.Round(hSliderValue+2)) ;    
 	
		playerPrefs._intGridSize= CtoI;
	//		
		GetComponent<TextMesh>().text = ""+playerPrefs._intGridSize; 
		// display gridsize
		
		
	
	//	(Mathf.Round(GamePreferences.Volume * 100f)) / 100f;
		//yourFloat = Mathf.Round(yourFloat * 100f) / 100f;
		
		
		
		
	}


 void OnGUI() {
    
 



 

	
	// creates the slider to control the gridsize
hSliderValue = GUI.HorizontalSlider(new Rect(500, 200, 400, 35), hSliderValue, 0f, 97f);
		
		
			//	GetComponent<TextMesh>().text = ""+playerPrefs._intGridSize; // displays the volume
		
		
		
		
	 
	
		
		
		
		
		
	
	
	

	
	}
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	
	
	
	
	
	
	
	
