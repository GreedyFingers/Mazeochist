using UnityEngine;
using System.Collections;






// controls functions of main , help , and options menu

public class MenuControl : MonoBehaviour 
{
	
	
	
	void Start () {
	
	
		
	     
			
	
	}
	
    public string MenuButtonIdentity;

   
	
	
	// when mouse hovers over turn text to blue
	void OnMouseEnter() 
    {
	
	
		
        renderer.material.color = Color.blue;
    }

    // when mouse leaves turn text to white
	void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    void OnMouseUp()
    {
        switch (MenuButtonIdentity)
        {
            case "PlayButton": // if clicks on play then playsclick sound and loads main game
          	audio.Play();
			
	
			Application.LoadLevel(2);
                break;
           
		
		// if clicks on Help then play click sound and load Help Menu
		
		case "HelpButton":
                
			audio.Play();
			
			Application.LoadLevel(4);
              
			
		
			
			break;
           
		
		
		
		
			
		// if clicks on Options then play click sound and load options Menu
		
		
		case "OptionsButton":
              
		audio.Play();
			
			Application.LoadLevel(3);
                break;
            case "QuitButton":
              audio.Play();
			GamePreferences.Quit =1;
			
			
			
			
			
				
		// if clicks on Quit then play click sound and Quit
			
			
			Application.Quit();
                break;
        
		case "Back":
           audio.Play();
		
			Application.LoadLevel(1);
            
			break;
           
		
         
			  
			
			
			
		}
    }
}


