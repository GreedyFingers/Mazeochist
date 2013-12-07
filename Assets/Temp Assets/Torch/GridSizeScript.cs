



using UnityEngine;
using System.Collections;


// sets the volume used for music but not for other sounds

public class  GridSizeScript: MonoBehaviour {
	
	 public string MenuButtonIdentity;
	
	void Start () {
		
	
 	
		

		
	
	}
	
	// Update is called once per frame
	void Update () {       
	
		
			
		
		
		
		
		
	}

	
	
	void OnMouseEnter() 
    {
	
	
		
        renderer.material.color = Color.blue;
    }

    // when mouse leaves turn text to white
	void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
	
	
	
	
	
	
	
	
	
	

 	void  OnMouseUp(){
    
 



 

	    if( GetComponent<TextMesh>().text == "LARGE"){
			
				 GetComponent<TextMesh>().text = "S"; 
			
			
				
					playerPrefs._intGridSize = 17;
				   playerPrefs._enemyStartTime = 25;
	               playerPrefs._enemySpeed=6;
				
				
				
			}     
			
			
			
			if( GetComponent<TextMesh>().text == "SMALL"){
			
				 GetComponent<TextMesh>().text = "LARGE"; 
			
				
				   playerPrefs._intGridSize = 23;
			
				   playerPrefs._enemyStartTime = 60;
	               playerPrefs._enemySpeed=7;
				
				
				
				
				
				
			}
			
			
			  if( GetComponent<TextMesh>().text == "S"){
			
				 GetComponent<TextMesh>().text = "SMALL"; 
			
				
			}
			
			
			
			
			
			
			
			audio.Play();
			
			
              
			
		
			
			
		

		
		
		
		
	 
	
		
		
		
		
		
	
	
	

	
	}
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
           
           
        }
	
	
	
	
	
	
	
	
	
