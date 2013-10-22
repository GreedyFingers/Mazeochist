using UnityEngine;
using System.Collections;

public class MusicLoad2Script : MonoBehaviour {





	// Use this for initialization
	void Start () {
		
	DontDestroyOnLoad(this.gameObject);
		
	  Application.LoadLevel(5);
	}
	
	

	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
		if(  (Application.loadedLevel)== 1)
		{
			
	Destroy(this.gameObject);		
		
			
		}
		
		
	}
}
