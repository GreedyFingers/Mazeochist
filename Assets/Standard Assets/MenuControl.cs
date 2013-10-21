using UnityEngine;
using System.Collections;








public class MenuControl : MonoBehaviour 
{
	
	
	
	void Start () {
	
	
		
	     
			
	
	}
	
    public string MenuButtonIdentity;

    void OnMouseEnter()
    {
	
	
		
        renderer.material.color = Color.blue;
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    void OnMouseUp()
    {
        switch (MenuButtonIdentity)
        {
            case "PlayButton":
          	audio.Play();
			light.intensity =4;
			Application.LoadLevel(2);
                break;
            case "HelpButton":
                audio.Play();
			
			Application.LoadLevel(4);
              
			
			
			
			
			
			
			
			
			break;
            case "OptionsButton":
              
		audio.Play();
			Application.LoadLevel(3);
                break;
            case "QuitButton":
              audio.Play();
			Application.Quit();
                break;
        
		case "Back":
            audio.Play();
			Application.LoadLevel(1);
                break;
           
		
         
			
			
			
			
		}
    }
}


