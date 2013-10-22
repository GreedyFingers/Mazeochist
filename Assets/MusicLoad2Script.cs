using UnityEngine;
using System.Collections;





// used to start and stop the main game music
public class MusicLoad2Script : MonoBehaviour {





	// Use this for initialization
	void Start () {
		
	DontDestroyOnLoad(this.gameObject); // don't destroy when new scene is loaded
		
	  Application.LoadLevel(5);    // go to main game scene
	}
	
	

	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
		if(  (Application.loadedLevel)== 1) // if exits main game than destroy and stop music
		{
			
	Destroy(this.gameObject);		
		
			
		}
		
		
	}
}
