using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {
	
	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4,Screen.width/2, Screen.height/2);	
	
	void Start()
	{
		this.enabled = false;
		Screen.showCursor = false;
		
	}
	
	void Update()
	{
		
		
	}
	
	void OnGUI()
	{
		
		windowRect = GUI.Window(0,windowRect,DoMyWindow,"Congratulations!");
		Screen.showCursor = true;
		
	}
		
    void DoMyWindow(int windowID) 
	{
        if (GUI.Button(new Rect((windowRect.width/2)-(windowRect.width/16), (windowRect.height/2)-(windowRect.height/16), windowRect.width/8, windowRect.height/8), "OK"))
			Application.Quit();
        
    }
}
