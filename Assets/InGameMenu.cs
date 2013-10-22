using UnityEngine;
using System.Collections;

/// <Basic Description>
/// InGameMenu: Creates GUI window and displays it after collision
/// </Basic Description>
/// <Dependencies>
/// Is told when to display GUI by FSMPlayer
/// </Dependencies>
/// <Interfaces>
/// This class interfaces with the player directly by the user selecting "ok"
/// </Interfaces>
/// <FSM Dependencies>
/// (none yet)
/// </FSM Dependencies>
public class InGameMenu : MonoBehaviour {
	
	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4,Screen.width/2, Screen.height/2);	

	///Input: (none)
	///Output: (none)
	///Called From: Called upon instantiation of the object
	///Calls: (none)	
	void Start()
	{
		this.enabled = false;
		Screen.showCursor = false;
		
	}
	
	void Update()
	{
		
		
	}

	//Input: (none)
	//Output: (none)
	//Called From: 
	//Calls:
	void OnGUI()
	{
		
		windowRect = GUI.Window(0,windowRect,DoMyWindow,"Congratulations!");
		Screen.showCursor = true;
		
	}
	
	//Input: (none)
	//Output: (none)
	//Called From: OnGUI()
	//Calls: (none)
    void DoMyWindow(int windowID) 
	{
        if (GUI.Button(new Rect((windowRect.width/2)-(windowRect.width/16), (windowRect.height/2)-(windowRect.height/16), windowRect.width/8, windowRect.height/8), "OK"))
			Application.Quit();
        
    }
}
