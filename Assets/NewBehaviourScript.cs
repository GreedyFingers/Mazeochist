using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

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
		
		
	}
}
