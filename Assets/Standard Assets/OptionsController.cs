

using UnityEngine;
using System.Collections;

public class OptionsController : MonoBehaviour {

    public string MenuButtonIdentity;
	
	
	
	  void OnMouseEnter()
    {
	
	// when mouse hovers over turn text blue
		
        renderer.material.color = Color.blue; 
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white; // when mouse hovers over turn text white
    }
	
}









/*
using UnityEngine;
using System.Collections;

public class OptionsController : MonoBehaviour {

    public string MenuButtonIdentity;
	
	
	
	  void OnMouseEnter()
    {
	
	
		
        renderer.material.color = Color.blue;
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
	
	
	
	
	
	
	
	
	
	
	
    void Start()
    {
        switch (MenuButtonIdentity)
        {
            case "SoundButton":
                switch (GamePreferences.Sound)
                {
                    case true:
                        GetComponent<TextMesh>().text = "ON";
                        break;
                    
                    case false:
                        GetComponent<TextMesh>().text = "OFF";
                        break;
                }
                break;

           
           
        }
    }

   
    void OnMouseUp()
    {
        switch (MenuButtonIdentity)
        {
            case "SoundButton":
                 audio.Play();
			GamePreferences.Sound = !GamePreferences.Sound;
                if (GamePreferences.Sound)
                    GetComponent<TextMesh>().text = "ON";
                else
                    GetComponent<TextMesh>().text = "OFF";
                    break;
            
            
            
         
        }
    }






    void Update()
    {
        switch (MenuButtonIdentity)
        {
            case "SoundButton":
                switch (GamePreferences.Sound)
                {
                    case true:
                        GetComponent<TextMesh>().text = "ON";
                        break;
                    
                    case false:
                        GetComponent<TextMesh>().text = "OFF";
                        break;
                }
                break;

           
           
        }
    }





}

*/