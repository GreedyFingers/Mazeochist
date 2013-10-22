using UnityEngine;
using System.Collections;




// keeps intro music playing when scenes change
//and stops it when it is no longer needed
public class DestroyMusicScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	DontDestroyOnLoad(this.gameObject); // don't destroy object when new scene loads
		
	  Application.LoadLevel(1); // load main menu scene
	}
	
	

	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
		// at start of main game destroy this object and stop music
		
		if(  (Application.loadedLevel)== 2) 
		{
			
	Destroy(this.gameObject);		
		
			
		}
		
		
		
		
		
		
		// destroy this object and exit program 
		if( GamePreferences.Quit ==1){
			Destroy(this.gameObject);	
			
			Application.Quit();
			
		}
		
		
		
		
		 // at start of main game destroy this object and stop music
		if(  (Application.loadedLevel)== 5) 
		{
			
	Destroy(this.gameObject);		
		
			
		}
		
		
		
		
		
		
		
		
		
		
		
		
	}
}
