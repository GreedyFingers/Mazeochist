using UnityEngine;
using System.Collections;

public class DestroyMusicScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	DontDestroyOnLoad(this.gameObject);
		
	  Application.LoadLevel(1);
	}
	
	

	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
		if(  (Application.loadedLevel)== 2)
		{
			
	Destroy(this.gameObject);		
		
			
		}
		if( GamePreferences.Quit ==1){
			Destroy(this.gameObject);	
			
			Application.Quit();
			
		}
		
		if(  (Application.loadedLevel)== 5)
		{
			
	Destroy(this.gameObject);		
		
			
		}
		
		
		
		
		
		
		
		
		
		
		
		
	}
}
