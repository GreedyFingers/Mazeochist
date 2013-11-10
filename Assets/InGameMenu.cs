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
	
	public enum WINDOW_TYPE {GAME_WON, GAME_LOST, GAME_PAUSED};	
	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4,Screen.width/2, Screen.height/2);	
	private WINDOW_TYPE _currentWindow; 
	
	public WINDOW_TYPE CurrentWindow
	{
		get{return _currentWindow;}
		set{_currentWindow = value;}
	}
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
		switch(_currentWindow)
		{
			case(WINDOW_TYPE.GAME_WON):
			{
				windowRect = GUI.Window(0,windowRect,DoMyWindow,"Congratulations!");
				break;
			}
			case(WINDOW_TYPE.GAME_LOST):
			{
				windowRect = GUI.Window(0,windowRect,DoMyWindow,"Too bad!");			
				break;
			}
			case(WINDOW_TYPE.GAME_PAUSED):
			{
				windowRect = GUI.Window(0,windowRect,DoMyWindow,"Game paused");
				break;
			}
		Screen.showCursor = true;
		}
	}
	
	//Input: (none)
	//Output: (none)
	//Called From: OnGUI()
	//Calls: (none)
    void DoMyWindow(int windowID) 
	{
		if(_currentWindow != WINDOW_TYPE.GAME_PAUSED)
        	if (GUI.Button(new Rect((windowRect.width/2)-(windowRect.width/16), (windowRect.height/2)-(windowRect.height/16), windowRect.width/8, windowRect.height/8), "OK"))
				Application.Quit();
        
    }
}
